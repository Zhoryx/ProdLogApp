using ProdLogApp.Interfaces;
using ProdLogApp.Models;
using ProdLogApp.Presenters;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace ProdLogApp.Views.Designs.Prompts
{
    public partial class PromptPosition : Window, IPromptPositionView
    {
        private readonly PromptPositionPresenter _presenter;
        private readonly User _activeUser;
        private readonly IDatabaseService _databaseService;
        private Position _selectedPosition;

        public PromptPosition(User activeUser, IDatabaseService databaseService)
        {
            InitializeComponent();
            _activeUser = activeUser;
            _databaseService = databaseService ?? throw new ArgumentNullException(nameof(databaseService));
            _presenter = new PromptPositionPresenter(this, _databaseService); // ✅ El presenter ya carga los puestos
        }

        public void ObtenerPuestoSeleccionado(out int puestoId, out string descripcion)
        {
            puestoId = _selectedPosition?.PuestoId ?? 0;
            descripcion = _selectedPosition?.Nombre ?? "Sin selección";
        }

        public void MostrarPuestos(List<Position> puestos)
        {
            PositionList.ItemsSource = puestos;
            EmptyMessage.Visibility = puestos.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        private void PositionList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PositionList.SelectedItem is Position seleccion)
            {
                _selectedPosition = seleccion;
            }
        }

        private void searchbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            _presenter.FiltrarPorNombre(searchbox.Text);
        }

        private void searchboxid_TextChanged(object sender, TextChangedEventArgs e)
        {
            _presenter.FiltrarPorCodigo(searchboxid.Text);
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedPosition != null)
            {
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("Por favor selecciona un puesto antes de confirmar.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void GridViewColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            if (sender is GridViewColumnHeader header && header.Tag is string sortBy)
            {
                _presenter.OrdenarPor(sortBy);
            }
        }
    }
}
