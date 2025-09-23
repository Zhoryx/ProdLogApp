using ProdLogApp.Interfaces;
using ProdLogApp.Models;
using ProdLogApp.Presenters;
using ProdLogApp.Services;
using ProdLogApp.Views;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace ProdLogApp.Views
{
    public partial class UserManagement : Window, IUserManagementView
    {
        private readonly UserManagementPresenter _presenter;
        private readonly User _activeUser;
        private readonly IDatabaseService _databaseService;
        public event Action OnAddUser;
        public event Action OnToggleUserStatus;
        public event Action OnModifyUser;
        public event Action OnReturn;
        private GridViewColumnHeader _lastHeaderClicked = null;
        private ListSortDirection _lastDirection = ListSortDirection.Ascending;

        


        public UserManagement(User activeUser, IDatabaseService databaseService)
        {
            InitializeComponent();
            _activeUser = activeUser;
            _databaseService = databaseService ?? throw new ArgumentNullException(nameof(databaseService));

            _presenter = new UserManagementPresenter(_databaseService,this);
        }

        public void ShowMessage(string msg)
        {
            MessageBox.Show(msg, "Información", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void ShowUsers(List<User> users)
        {
            Users_list.ItemsSource = users ?? new List<User>();
        }

        public User SelectedUser()
        {
            return Users_list.SelectedItem as User;
        }

        public bool NewUser()
        {
            AddUser addWindow = new AddUser(_databaseService);
            return addWindow.ShowDialog() == true;
        }

        public void ModifyUser(User user)
        {
            AddUser modifyWindow = new AddUser(_databaseService, user);
            modifyWindow.ShowDialog();
        }

        private void Users_list_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected_user = Users_list.SelectedItem as User;
            if (selected_user != null)
            {
                Console.WriteLine($"Usuario seleccionado: {selected_user.Name}");
            }
        }


        // Method to navigate back to the main menu
        public void NavigateToMenu()
        {
            ManagerMenu menu = new ManagerMenu(_activeUser);
            menu.Show();
            this.Close();
        }

        private void Sort(string sortBy, ListSortDirection direction)
        {
            ICollectionView dataView = CollectionViewSource.GetDefaultView(Users_list.ItemsSource);
            if (dataView == null) return;

            dataView.SortDescriptions.Clear();
            dataView.SortDescriptions.Add(new SortDescription(sortBy, direction));
            dataView.Refresh();
        }

        private void GridViewColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            var headerClicked = sender as GridViewColumnHeader;
            var sortBy = headerClicked?.Tag?.ToString();
            if (string.IsNullOrEmpty(sortBy)) return;

            ListSortDirection direction;

            if (headerClicked != _lastHeaderClicked)
            {
                direction = ListSortDirection.Ascending;
            }
            else
            {
                direction = _lastDirection == ListSortDirection.Ascending
                    ? ListSortDirection.Descending
                    : ListSortDirection.Ascending;
            }

            Sort(sortBy, direction);
            _lastHeaderClicked = headerClicked;
            _lastDirection = direction;
        }

        private void Users_list_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Evitar abrir si el doble click fue en el fondo vacío del ListView
            var clickedElement = e.OriginalSource as DependencyObject;
            var container = ItemsControl.ContainerFromElement(Users_list, clickedElement) as ListViewItem;

            if (container == null)
                return; // doble click fuera de un item

            if (Users_list?.SelectedItem is User)
            {
                OnModifyUser?.Invoke(); // reusar el flujo existente del Presenter
            }
        }

        private void Users_list_KeyDown(object sender, KeyEventArgs e)
        {
            // Habilitar Enter/F2 para editar
            if ((e.Key == Key.Enter || e.Key == Key.F2) && Users_list?.SelectedItem is User)
            {
                OnModifyUser?.Invoke();
                e.Handled = true;
            }
        }


        private void ReturnToMenu_Click(object sender, RoutedEventArgs e)
        {
            NavigateToMenu();
        }

        private void ToggleUserStatus_Click(object sender, RoutedEventArgs e) => OnToggleUserStatus?.Invoke();
        private void AddUser_Click(object sender, RoutedEventArgs e) => OnAddUser?.Invoke();
        private void ModifyUser_Click(object sender, RoutedEventArgs e) => OnModifyUser?.Invoke();
        
    }
}

