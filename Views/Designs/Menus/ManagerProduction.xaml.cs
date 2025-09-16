using ProdLogApp.Interfaces;
using ProdLogApp.Models;      // User, ParteHeaderItem, Page<T>
using ProdLogApp.Services;    // IDatabaseService
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Xceed.Wpf.Toolkit.Primitives;

namespace ProdLogApp.Views
{
    public partial class ManagerProduction : Window
    {
        // ---- Estado de paginación ----
        private int _currentPage = 1;
        private int _pageSize = 100;
        private int _total = 0;
        private int _totalPages = 1;

        // ---- Dependencias ----
        private readonly IDatabaseService _databaseService;
        private readonly User _activeUser;

        public ManagerProduction(User activeUser, IDatabaseService databaseService)
        {
            InitializeComponent();

            _activeUser = activeUser ?? throw new ArgumentNullException(nameof(activeUser));
            _databaseService = databaseService ?? throw new ArgumentNullException(nameof(databaseService));

            // (Opcional) Mostrar usuario en el título
            this.Title = $"Menu - Producciones (Usuario: {_activeUser.Name ?? _activeUser.ToString()})";

            // Defaults visibles
            FechaHastaPicker.SelectedDate = DateTime.Today;
            FechaDesdePicker.SelectedDate = DateTime.Today.AddDays(-30);
            _pageSize = 100; // coincide con SelectedIndex=1 del ComboBox

            // Carga inicial cuando la vista ya está construida
            Loaded += async (_, __) => await LoadPageAsync();
        }

        // ==================== Handlers UI ====================

        private async void btnFiltrar_Click(object sender, RoutedEventArgs e)
        {
            _currentPage = 1;
            await LoadPageAsync();
        }

        private async void cbPageSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbPageSize?.SelectedItem is ComboBoxItem item &&
                int.TryParse(item.Content?.ToString(), out var size))
            {
                _pageSize = size;
                _currentPage = 1;
                await LoadPageAsync();
            }
        }

        private async void btnFirst_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage == 1) return;
            _currentPage = 1;
            await LoadPageAsync();
        }

        private async void btnPrev_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage <= 1) return;
            _currentPage--;
            await LoadPageAsync();
        }

        private async void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage >= _totalPages) return;
            _currentPage++;
            await LoadPageAsync();
        }

        private async void btnLast_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage >= _totalPages) return;
            _currentPage = _totalPages;
            await LoadPageAsync();
        }

        private async void Producciones_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (Producciones?.SelectedItem is not ParteHeaderItem parte)
                return;

          
            int usuarioId = parte.UserId;        
            DateTime fecha = parte.FechaParte.Date;

            var op = new OperatorMenu(
                _databaseService,
                usuarioId: usuarioId,
                fecha: fecha,
                operarioNombre: parte.Operario,    
                asManagerModal: true
            );

            op.Owner = this; 
            op.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            bool? ok = op.ShowDialog();
            if (ok == true)
            {
                
                await LoadPageAsync();
            }
        }

        private void Volver(object sender, RoutedEventArgs e)
        {
            Close();
        }

        // ==================== Núcleo de carga ====================

        private (DateTime from, DateTime to, string operarioLike) ReadFiltersSafe()
        {
            // Defaults: últimos 30 días
            var from = (FechaDesdePicker?.SelectedDate ?? DateTime.Today.AddDays(-30)).Date;
            var to = (FechaHastaPicker?.SelectedDate ?? DateTime.Today).Date;

            // Si el usuario invierte fechas, las corregimos
            if (from > to) (from, to) = (to, from);

            var operarioLike = string.IsNullOrWhiteSpace(OperarioAutoComplete?.Text)
                ? null
                : OperarioAutoComplete.Text.Trim();

            return (from, to, operarioLike);
        }

        private void ApplyPagerUi()
        {
            if (!IsLoaded) return;

            _totalPages = (_total <= 0 || _pageSize <= 0)
                ? 1
                : (int)Math.Ceiling(_total / (double)_pageSize);

            if (txtPageInfo != null)
                txtPageInfo.Text = $"Página {_currentPage} de {_totalPages} — Total: {_total}";

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
             //Defensa extra: no avanzar si el servicio no está
            if (_databaseService == null)
            {
                //messagebox.show("servicio de base de datos no inicializado.", "error", messageboxbutton.ok, messageboximage.error);
                return;
            }

            try
            {
                ToggleUi(false);

                var (from, to, operarioLike) = ReadFiltersSafe();

                // Llamada al servicio (paginado servidor)
                var page = await _databaseService.GetParteHeadersPageAsync(from, to, _currentPage, _pageSize, operarioLike);
                if (page == null)
                {
                    MessageBox.Show("No se pudo obtener la página de datos (resultado nulo).", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                _total = page.Total;
                BindList(page.Items ?? new List<ParteHeaderItem>());
                ApplyPagerUi();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
