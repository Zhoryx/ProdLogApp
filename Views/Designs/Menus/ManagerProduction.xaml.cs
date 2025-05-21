using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ProdLogApp.Models;

namespace ProdLogApp.Views
{
    /// <summary>
    /// Lógica de interacción para MenuProduccionGerente.xaml
    /// </summary>
    public partial class ManagerProduction : Window
    {
        private readonly User _activeUser;
        public ManagerProduction(User activeUser)
        {
            InitializeComponent();
            _activeUser = activeUser;
        }

        // Evento cuando se cambia la selección en el ListView
        private void Producciones_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Aquí puedes manejar la lógica cuando se seleccione una producción en la lista
        }

        // Evento cuando se cambia la selección en los filtros
        private void Buscar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AplicarFiltros();
        }

        // Se corrige el nombre para que siga la convención PascalCase
        private void AplicarFiltro(object sender, RoutedEventArgs e)
        {
            AplicarFiltros();
        }

        // Evento para actualizar los filtros cuando el usuario escribe en el cuadro de búsqueda
        private void BuscarProductoTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            AplicarFiltros();
        }

        // Evento para aplicar los filtros dinámicamente
        private void AplicarFiltros_Click(object sender, RoutedEventArgs e)
        {
            AplicarFiltros();
        }

        // Método centralizado para aplicar los filtros
        private void AplicarFiltros()
        {
            // Aquí podrías conectar la lógica del presenter y actualizar la lista en la vista.
            // Ejemplo de obtención de valores:
            //string operario = BuscarOperarioTextBox.Text;
            DateTime? fechaDesde = FechaDesdePicker.SelectedDate;
            DateTime? fechaHasta = FechaHastaPicker.SelectedDate;
            //string producto = BuscarProductoTextBox.Text;

            // Llamar al método que aplicará los filtros en la lista de producciones
            // _presenter.AplicarFiltros(operario, fechaDesde, fechaHasta, producto);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
