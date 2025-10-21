// ProduccionesGerente.xaml.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ProdLogApp.Models;
using ProdLogApp.Servicios;

namespace ProdLogApp.Views
{
    public partial class ProduccionesGerente : Window
    {
        private int _currentPage = 1;
        private int _pageSize = 100;
        private int _total = 0;
        private int _totalPages = 1;

        private IServicioProducciones _svcProducciones;  // <-- ya no readonly
        private readonly Usuario _activeUser;

        // Fallback lazy: si por alguna razón vino null, se crea aquí y listo
        private IServicioProducciones SvcProducciones =>
            _svcProducciones ??= new ServicioProduccionesMySql(new ProveedorConexionMySql("ProdLogDb"));

        // (opcional) ctor sin params para evitar instanciaciones “raras”
        public ProduccionesGerente() : this(UserSession.GetInstance()?.ActiveUser, null) { }

        public ProduccionesGerente(Usuario activeUser, IServicioProducciones svcProducciones)
        {
            InitializeComponent();

            _activeUser = activeUser ?? throw new ArgumentNullException(nameof(activeUser));
            _svcProducciones = svcProducciones; // puede venir null -> SvcProducciones lo resuelve

            Title = $"Menu - Producciones (Usuario: {_activeUser.Nombre ?? _activeUser.ToString()})";
            FechaHastaPicker.SelectedDate = DateTime.Today;
            FechaDesdePicker.SelectedDate = DateTime.Today.AddDays(-30);
            _pageSize = 100;

            Loaded += async (_, __) => await LoadPageAsync();
        }

        // --- handlers (sin cambios de comportamiento) ---
        private async void btnFiltrar_Click(object sender, RoutedEventArgs e) { _currentPage = 1; await LoadPageAsync(); }
        private async void cbPageSize_SelectionChanged(object s, SelectionChangedEventArgs e)
        {
            if (cbPageSize?.SelectedItem is ComboBoxItem it && int.TryParse(it.Content?.ToString(), out var size))
            { _pageSize = size; _currentPage = 1; await LoadPageAsync(); }
        }
        private async void btnFirst_Click(object s, RoutedEventArgs e) { if (_currentPage == 1) return; _currentPage = 1; await LoadPageAsync(); }
        private async void btnPrev_Click(object s, RoutedEventArgs e) { if (_currentPage <= 1) return; _currentPage--; await LoadPageAsync(); }
        private async void btnNext_Click(object s, RoutedEventArgs e) { if (_currentPage >= _totalPages) return; _currentPage++; await LoadPageAsync(); }
        private async void btnLast_Click(object s, RoutedEventArgs e) { if (_currentPage >= _totalPages) return; _currentPage = _totalPages; await LoadPageAsync(); }

        private async void Producciones_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (Producciones?.SelectedItem is not ParteHeaderItem parte) return;

            var prov = new ProveedorConexionMySql("ProdLogDb");
            var op = new OperadorMenu(
                proveedorConexion: prov,
                usuarioId: parte.UserId,
                fecha: parte.FechaParte.Date,
                operarioNombre: parte.Operario,
                asManagerModal: true)
            {
                Owner = this,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
            op.ShowDialog();
            await LoadPageAsync();
        }

        private void Volver(object sender, RoutedEventArgs e) => Close();

        private (DateTime from, DateTime to, string operarioLike) ReadFiltersSafe()
        {
            var from = (FechaDesdePicker?.SelectedDate ?? DateTime.Today.AddDays(-30)).Date;
            var to = (FechaHastaPicker?.SelectedDate ?? DateTime.Today).Date;
            if (from > to) (from, to) = (to, from);

            var like = string.IsNullOrWhiteSpace(OperarioAutoComplete?.Text) ? null : OperarioAutoComplete.Text.Trim();
            return (from, to, like);
        }

        private void ApplyPagerUi()
        {
            if (!IsLoaded) return;
            _totalPages = (_total <= 0 || _pageSize <= 0) ? 1 : (int)Math.Ceiling(_total / (double)_pageSize);
            if (txtPageInfo != null) txtPageInfo.Text = $"Página {_currentPage} de {_totalPages} — Total: {_total}";
            if (btnFirst != null) btnFirst.IsEnabled = _currentPage > 1;
            if (btnPrev != null) btnPrev.IsEnabled = _currentPage > 1;
            if (btnNext != null) btnNext.IsEnabled = _currentPage < _totalPages;
            if (btnLast != null) btnLast.IsEnabled = _currentPage < _totalPages;
        }

        private void BindList(IList<ParteHeaderItem> items)
        {
            if (Producciones == null) return;
            Producciones.ItemsSource = null;
            Producciones.ItemsSource = items;
        }

        private async Task LoadPageAsync()
        {
            try
            {
                ToggleUi(false);

                var (from, to, operarioLike) = ReadFiltersSafe();

                var page = await SvcProducciones.ObtenerCabecerasPaginadasAsync(
                    desde: from, hasta: to, page: _currentPage, pageSize: _pageSize, operarioLike: operarioLike);

                _total = page?.Total ?? 0;
                BindList(page?.Items ?? new List<ParteHeaderItem>());
                ApplyPagerUi();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al listar producciones: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                ToggleUi(true);
            }
        }

        private void ToggleUi(bool enabled)
        {
            if (!IsLoaded) return;
            if (btnFiltrar != null) btnFiltrar.IsEnabled = enabled;
            if (cbPageSize != null) cbPageSize.IsEnabled = enabled;
            if (Producciones != null) Producciones.IsEnabled = enabled;
            if (btnFirst != null) btnFirst.IsEnabled = enabled && _currentPage > 1;
            if (btnPrev != null) btnPrev.IsEnabled = enabled && _currentPage > 1;
            if (btnNext != null) btnNext.IsEnabled = enabled && _currentPage < _totalPages;
            if (btnLast != null) btnLast.IsEnabled = enabled && _currentPage < _totalPages;
        }
    }
}
