using ProdLogApp.Interfaces;
using ProdLogApp.Models;
using ProdLogApp.Services;
using ProdLogApp.Views;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;


namespace ProdLogApp.Views
{
    public partial class CategoryManagement : Window, ICategoryManagementView
    {
        private readonly CategoryManagementPresenter _presenter;
        private readonly IDatabaseService _databaseService;
        private readonly User _activeUser;
        public event Action OnAddCategory;
        public event Action OnModifyCategory;
        public event Action OnToggleCategoryStatus;
        private GridViewColumnHeader _lastHeaderClicked = null;
        private ListSortDirection _lastDirection = ListSortDirection.Ascending;

        public CategoryManagement( User activeUser, IDatabaseService databaseService)
        {
            InitializeComponent();
            _databaseService = databaseService ?? throw new ArgumentNullException(nameof(databaseService));
            _presenter = new CategoryManagementPresenter(_databaseService,this);
            _activeUser = activeUser ?? throw new ArgumentNullException(nameof(activeUser));
        }

        public void MostrarMensaje(string mensaje)
        {
            MessageBox.Show(mensaje, "Información", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void MostrarCategorias(List<Categoria> categorias)
        {
            CategoryList.ItemsSource = categorias ?? new List<Categoria>();
        }

        public Categoria ObtenerCategoriaSeleccionada()
        {
            return CategoryList.SelectedItem as Categoria;
        }

        public void AbrirVentanaAgregarCategoria()
        {
            AddCategory ventanaAgregar = new AddCategory(_databaseService);
            ventanaAgregar.ShowDialog();
        }

        public void AbrirVentanaModificarCategoria(Categoria categoria)
        {
            AddCategory ventanaModificar = new AddCategory(_databaseService, categoria);
            ventanaModificar.ShowDialog();
        }

        private void CategoryList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var categoriaSeleccionada = CategoryList.SelectedItem as Categoria;
            if (categoriaSeleccionada != null)
            {
                Console.WriteLine($"Categoría seleccionada: {categoriaSeleccionada.Nombre}");
            }
        }
            public void NavigateToMenu()
            {
            ManagerMenu menu = new ManagerMenu(_activeUser);
            menu.Show();
            Close();
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

        private void Sort(string sortBy, ListSortDirection direction)
        {
            ICollectionView dataView = CollectionViewSource.GetDefaultView(CategoryList.ItemsSource);
            if (dataView == null) return;

            dataView.SortDescriptions.Clear();
            dataView.SortDescriptions.Add(new SortDescription(sortBy, direction));
            dataView.Refresh();
        }


        private void Categories_list_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
          
            var clicked = e.OriginalSource as DependencyObject;
            var container = ItemsControl.ContainerFromElement(CategoryList, clicked) as ListViewItem;
            if (container == null) return;

            if (CategoryList?.SelectedItem is Categoria)
                OnModifyCategory?.Invoke();
        }

        private void Categories_list_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.Enter || e.Key == Key.F2) && CategoryList?.SelectedItem is Categoria)
            {
                OnModifyCategory?.Invoke();
                e.Handled = true;
            }
        }

        private void ToggleCategoryStatus_Click(object sender, RoutedEventArgs e) => OnToggleCategoryStatus?.Invoke();

        private void AddCategory_Click(object sender, RoutedEventArgs e) => OnAddCategory?.Invoke();
        private void ModifyCategory_Click(object sender, RoutedEventArgs e) => OnModifyCategory?.Invoke();
        private void ReturnToMenu_Click(object sender, RoutedEventArgs e) => NavigateToMenu();
    }
}
