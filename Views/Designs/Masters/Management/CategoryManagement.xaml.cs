using ProdLogApp.Interfaces;
using ProdLogApp.Services;
using ProdLogApp.Views;
using System.Windows;
using System.Windows.Controls;


namespace ProdLogApp.Views
{
    public partial class CategoryManagement : Window, ICategoryManagementView
    {
        private readonly CategoryManagementPresenter _presenter;
        private readonly IDatabaseService _databaseService;
        public event Action OnAddCategory;
        public event Action OnModifyCategory;
        public event Action OnToggleCategoryStatus;

        public CategoryManagement(IDatabaseService databaseService)
        {
            InitializeComponent();
            _databaseService = databaseService ?? throw new ArgumentNullException(nameof(databaseService));
            _presenter = new CategoryManagementPresenter(this, _databaseService); // ✅ Ahora pasa ambos parámetros
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

        private void ToggleCategoryStatus_Click(object sender, RoutedEventArgs e) => OnToggleCategoryStatus?.Invoke();

        private void AddCategory_Click(object sender, RoutedEventArgs e) => OnAddCategory?.Invoke();
        private void ModifyCategory_Click(object sender, RoutedEventArgs e) => OnModifyCategory?.Invoke();
        private void ReturnToMenu_Click(object sender, RoutedEventArgs e) => Close();
    }
}
