using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using ProdLogApp.Interfaces;
using ProdLogApp.Models;
using ProdLogApp.Presenters;
using ProdLogApp.Services;

namespace ProdLogApp.Views
{
    public partial class OperatorMenu : Window, IOperatorMenuView
    {
        private readonly OperatorMenuPresenter _presenter;
        private readonly IDatabaseService _databaseService;


        public OperatorMenu()
        {
            InitializeComponent();

            _databaseService = new DatabaseService(); // Reemplaza si usás DI
            _presenter = new OperatorMenuPresenter(this, _databaseService);

            Loaded += async (_, __) => await SafeLoadAsync();
        }

        private async Task SafeLoadAsync()
        {
            try
            {
                txtFechaHoy.Text = $"Hoy: {DateTime.Today:dd/MM/yyyy}";

                SetUiEnabled(false);
                await _presenter.LoadDailyProductionsForActiveUserAsync(DateTime.Today);
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
            {
                await _presenter.LoadDailyProductionsForActiveUserAsync(DateTime.Today);
            }
        }

        private async void Modify_Click(object sender, RoutedEventArgs e)
        {
            if (Productions_list.SelectedItem is Production selected)
            {
                if (_presenter.OpenProductionFormForActiveUser(selected))
                {
                    await _presenter.LoadDailyProductionsForActiveUserAsync(DateTime.Today);
                }
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
                        // Opcional: recargar todo desde DB para asegurar consistencia
                        await _presenter.LoadDailyProductionsForActiveUserAsync(DateTime.Today);
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

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            var ask = MessageBox.Show(
                "¿Desea confirmar la producción del día y volver al Login?",
                "Confirmar",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question,
                MessageBoxResult.No);

            if (ask != MessageBoxResult.Yes)
            {
                ShowMessage("Operación cancelada. Podés seguir cargando producciones.");
                return;
            }

            try
            {
                SetUiEnabled(false);
                ReturnToLogin();
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



        private void Disconnect_Click(object sender, RoutedEventArgs e)
        {
            ReturnToLogin();
        }


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
            // Recuperar la lista que se está mostrando
            var currentList = Productions_list.ItemsSource as List<Production>;
            if (currentList != null)
            {
                currentList.Add(production);
                Productions_list.Items.Refresh(); // Refresca la UI
            }
            else
            {
                // Si no hay lista, crear una nueva con ese elemento
                UpdateProductionList(new List<Production> { production });
            }
        }

        public Production GetProductionInput()
        {
            var form = new ProductionForm(
                UserSession.GetInstance().ActiveUser,
                _databaseService // reutilizar!
            );

            if (form.ShowDialog() == true)
                return form.ProduccionCreada;

            return null;
        }




    }
}
