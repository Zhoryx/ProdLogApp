using System;
using System.Windows;
using ProdLogApp.Models;
using ProdLogApp.Presenters;


namespace ProdLogApp.Views
{
    public partial class ManagerMenu : Window, IManagerMenuView
    {
        private ManagerMenuPresenter _presenter;

        public event Action OnAbrirPartesDiarios;
        public event Action OnMaestroProducto;
        public event Action OnMaestroCategoria;
        public event Action OnMaestroPuesto;
        public event Action OnMaestroUsuario;
        public event Action OnDesconectar;

        public ManagerMenu(User activeUser)
        {
            InitializeComponent();
            _presenter = new ManagerMenuPresenter(this, activeUser); 
        }

        private void AbrirPartesDiarios(object sender, RoutedEventArgs e) => OnAbrirPartesDiarios?.Invoke();
        private void MaestroProducto(object sender, RoutedEventArgs e) => OnMaestroProducto?.Invoke();
        private void MaestroCategoria(object sender, RoutedEventArgs e) => OnMaestroCategoria?.Invoke();
        private void MaestroPuesto(object sender, RoutedEventArgs e) => OnMaestroPuesto?.Invoke();
        private void MaestroUsuario(object sender, RoutedEventArgs e) => OnMaestroUsuario?.Invoke();
        private void Desconectar(object sender, RoutedEventArgs e) => OnDesconectar?.Invoke();

        public void CerrarVentana() => this.Close();
    }
}
