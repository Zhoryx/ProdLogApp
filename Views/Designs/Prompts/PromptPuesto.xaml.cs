using ProdLogApp.Interfaces;
using ProdLogApp.Models;
using ProdLogApp.Presenters;
using ProdLogApp.Presenters.Prompts_PopUps;
using ProdLogApp.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace ProdLogApp.Views.Designs.Prompts
{
    public partial class PromptPuesto : Window, IPromptPuestoVista
    {
        public event Action OnConfirmarSeleccion;
        public event Action OnCancelar;

        private readonly PromptPuestoPresenter _presenter;
        private List<Puesto> _puestos = new();
        private Puesto _seleccionado;

        public PromptPuesto()
        {
            InitializeComponent();

            var proveedor = new ProveedorConexionMySql("ProdLogDb");
            var servicio = new ServicioPuestosMySql(proveedor);
            _presenter = new PromptPuestoPresenter(this, servicio);

            // Carga inicial (única)
            Loaded += async (_, __) => await _presenter.CargarPuestosAsync();
        }

        // ==========================
        //  Implementación de la vista
        // ==========================
        public void CargarPuestos(List<Puesto> puestos)
        {
            _puestos = puestos ?? new List<Puesto>();
            PositionList.ItemsSource = _puestos;

            EmptyMessage.Visibility = (_puestos.Count == 0)
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        public Puesto ObtenerPuestoSeleccionado() => _seleccionado;

        public void MostrarMensaje(string mensaje)
            => MessageBox.Show(mensaje, "Información", MessageBoxButton.OK, MessageBoxImage.Information);

        public void Cerrar() => Close();

        // ==========================
        //  Handlers UI
        // ==========================
        private void searchbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string filtro = (sender as TextBox)?.Text?.Trim() ?? "";
            var filtrados = _puestos
                .Where(p => !string.IsNullOrEmpty(p.Nombre) &&
                            p.Nombre.Contains(filtro, StringComparison.OrdinalIgnoreCase))
                .ToList();

            PositionList.ItemsSource = filtrados;
            EmptyMessage.Visibility = (filtrados.Count == 0) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void searchboxid_TextChanged(object sender, TextChangedEventArgs e)
        {
            string filtro = (sender as TextBox)?.Text?.Trim() ?? "";
            var filtrados = _puestos
                .Where(p => p.PuestoId.ToString().Contains(filtro))
                .ToList();

            PositionList.ItemsSource = filtrados;
            EmptyMessage.Visibility = (filtrados.Count == 0) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void PositionList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _seleccionado = PositionList.SelectedItem as Puesto;
        }

        private void GridViewColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            var header = sender as GridViewColumnHeader;
            var sortBy = header?.Tag?.ToString();
            if (string.IsNullOrWhiteSpace(sortBy)) return;

            _presenter.OrdenarPor(sortBy);
        }

        private void Position_list_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (PositionList.SelectedItem is Puesto sel)
            {
                _seleccionado = sel;
                DialogResult = true;
                Close();
            }
        }

        private void Position_list_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && PositionList.SelectedItem is Puesto sel)
            {
                _seleccionado = sel;
                DialogResult = true;
                Close();
                e.Handled = true;
            }
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (PositionList.SelectedItem is Puesto sel)
            {
                _seleccionado = sel;
                DialogResult = true;
                Close();
            }
            else
            {
                MostrarMensaje("Seleccioná un puesto antes de confirmar.");
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
