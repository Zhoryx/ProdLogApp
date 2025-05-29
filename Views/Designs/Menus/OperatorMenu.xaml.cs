using System;
using System.Collections.Generic;
using System.Windows;
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
            _presenter = new OperatorMenuPresenter(this, new DatabaseService());
            _presenter.LoadDailyProductions(); // Load the daily production list on startup
        }

        // Handles the action when the "Add" button is clicked
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            _presenter.OpenProductionForm(_presenter.GetProductionList(), -1);
        }

        // Handles the action when the "Modify" button is clicked
        private void Modify_Click(object sender, RoutedEventArgs e)
        {
            if (Productions.SelectedIndex >= 0)
            {
                _presenter.OpenProductionForm(_presenter.GetProductionList(), Productions.SelectedIndex);
            }
        }

        // Handles the action when the "Confirm" button is clicked
        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            _presenter.SavePartProductions();
        }

        // Displays a message to the user
        public void ShowMessage(string message)
        {
            MessageBox.Show(message, "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // Updates the production list in the view
        public void UpdateProductionList(List<Production> productions)
        {
            Productions.ItemsSource = productions;
        }

        private void Disconnect_Click(object sender, RoutedEventArgs e)
        {
            NavigateToLogin();
        }

        public void NavigateToLogin()
        {
            Login login = new Login(_databaseService);
            login.Show();
            this.Close();
        }


       

    }
}
