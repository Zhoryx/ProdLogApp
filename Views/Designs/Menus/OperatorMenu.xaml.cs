using ProdLogApp.Interfaces;
using ProdLogApp.Models;
using ProdLogApp.Presenters;
using ProdLogApp.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ProdLogApp.Views
{
    public partial class OperatorMenu : Window, IOperatorMenuView
    {
        private readonly OperatorMenuPresenter _presenter;
        private readonly IDatabaseService _databaseService;

        private readonly bool _isManagerModal = false;
        private readonly int? _usuarioOverride = null;
        private readonly DateTime? _fechaOverride = null;
        private readonly string _operarioNombre = null;

        // >>> Implementación de la interfaz (evento requerido)
        public event Action<Production> OnProductionDoubleClick;

        // Modo operario (como ya lo tenías)
        public OperatorMenu()
        {
            InitializeComponent();
            _databaseService = new DatabaseService();
            _presenter = new OperatorMenuPresenter(this, _databaseService);
            Loaded += async (_, __) => await SafeLoadAsync();
        }

        // Modo gerente (popup)
        public OperatorMenu(IDatabaseService databaseService, int usuarioId, DateTime fecha, string operarioNombre = null, bool asManagerModal = true)
        {
            InitializeComponent();
            _databaseService = databaseService ?? new DatabaseService();
            _presenter = new OperatorMenuPresenter(this, _databaseService);

            _isManagerModal = asManagerModal;
            _usuarioOverride = usuarioId;
            _fechaOverride = fecha.Date;
            _operarioNombre = operarioNombre;

            Loaded += async (_, __) => await SafeLoadAsync();
        }

        private async Task SafeLoadAsync()
        {
            try
            {
                SetUiEnabled(false);

                if (_usuarioOverride.HasValue && _fechaOverride.HasValue)
                {
                    // --- Popup gerente ---
                    await _presenter.LoadDailyProductionsAsync(_usuarioOverride.Value, _fechaOverride.Value);
                    this.Title = "Detalle de Parte (Gerencia)";
                    txtFechaHoy.Text = $"Operario: {_operarioNombre ?? _usuarioOverride.ToString()}  |  Fecha: {_fechaOverride.Value:dd/MM/yyyy}";

                    if (btnDisconnect != null)
                    {
                        btnDisconnect.Content = "Volver";
                        btnDisconnect.ToolTip = "Cerrar y volver a la lista de partes";
                    }

                    if (btnConfirm != null)
                    {
                        btnConfirm.Content = "Guardar y cerrar";
                        btnConfirm.ToolTip = "Guardar cambios y volver a la lista";
                    }
                }
                else
                {
                    // --- Uso normal del operario ---
                    txtFechaHoy.Text = $"Hoy: {DateTime.Today:dd/MM/yyyy}";
                    await _presenter.LoadDailyProductionsForActiveUserAsync(DateTime.Today);

                    if (btnDisconnect != null)
                    {
                        btnDisconnect.Content = "Desconectar";
                        btnDisconnect.ToolTip = "Cerrar sesión y volver al login";
                    }
                    if (btnConfirm != null)
                    {
                        btnConfirm.Content = "Confirmar";
                        btnConfirm.ToolTip = "Confirmar la producción del día";
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMessage($"Error al iniciar: {ex.Message}");
            }
            finally
            {
                SetUiEnabled(true);
            }
        }

        private async Task ReloadContextAsync()
        {
            if (_usuarioOverride.HasValue && _fechaOverride.HasValue)
                await _presenter.LoadDailyProductionsAsync(_usuarioOverride.Value, _fechaOverride.Value);
            else
                await _presenter.LoadDailyProductionsForActiveUserAsync(DateTime.Today);
        }

        private void SetUiEnabled(bool enabled)
        {
            btnConfirm.IsEnabled = enabled;
            btnAdd.IsEnabled = enabled;
            btnModify.IsEnabled = enabled;
            btnDelete.IsEnabled = enabled;
        }

        private async void Add_Click(object sender, RoutedEventArgs e)
        {
            if (_presenter.OpenProductionFormForActiveUser())
                await ReloadContextAsync();
        }

        private async void Modify_Click(object sender, RoutedEventArgs e)
        {
            if (Productions_list.SelectedItem is Production selected)
            {
                if (_presenter.OpenProductionFormForActiveUser(selected))
                    await ReloadContextAsync();
            }
            else
            {
                ShowMessage("Seleccione un registro para modificar.");
            }
        }

        private async void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (Productions_list.SelectedItem is Production selected)
            {
                var confirm = MessageBox.Show(
                    "¿Eliminar la producción seleccionada?",
                    "Confirmar",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (confirm == MessageBoxResult.Yes)
                {
                    try
                    {
                        SetUiEnabled(false);
                        await _presenter.DeleteProductionAsync(selected);
                        await ReloadContextAsync(); // asegura consistencia
                    }
                    catch (Exception ex)
                    {
                        ShowMessage($"Error al eliminar: {ex.Message}");
                    }
                    finally
                    {
                        SetUiEnabled(true);
                    }
                }
            }
            else
            {
                ShowMessage("Seleccione un registro para eliminar.");
            }
        }

        private void ReturnToLogin()
        {
            var login = new Login(_databaseService);
            Application.Current.MainWindow = login;
            login.Show();
            Close();
        }

        // >>> Ahora async + await al guardar
        private async void Confirm_Click(object sender, RoutedEventArgs e)
        {
            var ask = MessageBox.Show(
                _isManagerModal
                    ? "¿Desea guardar/confirmar los cambios de este parte y cerrar?"
                    : "¿Desea confirmar la producción del día y volver al Login?",
                "Confirmar",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question,
                MessageBoxResult.No);

            if (ask != MessageBoxResult.Yes)
            {
                ShowMessage("Operación cancelada. Podés seguir trabajando.");
                return;
            }

            try
            {
                SetUiEnabled(false);

                await _presenter.SavePartProductionsForCurrentContextAsync();

                if (_isManagerModal)
                {
                    this.DialogResult = true;
                    this.Close();
                }
                else
                {
                    ReturnToLogin();
                }
            }
            catch (Exception ex)
            {
                ShowMessage($"Error al confirmar: {ex.Message}");
            }
            finally
            {
                SetUiEnabled(true);
            }
        }

        // --- Implementaciones de IOperatorMenuView ---

        public void ShowMessage(string message)
        {
            MessageBox.Show(message, "Información", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void UpdateProductionList(List<Production> productions)
        {
            Productions_list.ItemsSource = null;
            Productions_list.ItemsSource = productions;
        }

        public void AddProductionToList(Production production)
        {
            var currentList = Productions_list.ItemsSource as List<Production>;
            if (currentList != null)
            {
                currentList.Add(production);
                Productions_list.Items.Refresh();
            }
            else
            {
                UpdateProductionList(new List<Production> { production });
            }
        }

        public Production GetProductionInput()
        {
            var form = new ProductionForm(
                UserSession.GetInstance().ActiveUser,
                _databaseService
            );

            if (form.ShowDialog() == true)
                return form.ProduccionCreada;

            return null;
        }

        // Doble click por item del ListView (enganchado vía ItemContainerStyle EventSetter)
        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListViewItem lvi && lvi.DataContext is Production prod)
            {
                OnProductionDoubleClick?.Invoke(prod);  // lo toma el Presenter y abre editar
                e.Handled = true;
            }
        }

        private void Disconnect_Click(object sender, RoutedEventArgs e)
        {
            if (_isManagerModal)
            {
                this.DialogResult = false;
                this.Close();
            }
            else
            {
                ReturnToLogin();
            }
        }
    }
}
