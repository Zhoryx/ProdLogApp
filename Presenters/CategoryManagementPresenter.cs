using System;
using System.Windows;
using ProdLogApp.Views.Interfaces;

namespace ProdLogApp.Presenters
{
    public class CategoryManagementPresenter
    {
        private readonly ICategoryManagementView _view;

        public CategoryManagementPresenter(ICategoryManagementView view)
        {
            _view = view;

            // Subscribe events from the view
            _view.OnAddCategory += AddCategory;
            _view.OnDeleteCategory += DeleteCategory;
            _view.OnModifyCategory += ModifyCategory;
            _view.OnReturn += ReturnToMenu;
        }

        // Handles adding a new category
        private void AddCategory() => MessageBox.Show("Add category...");

        // Handles deleting a category
        private void DeleteCategory() => MessageBox.Show("Delete category...");

        // Handles modifying an existing category
        private void ModifyCategory() => MessageBox.Show("Modify category...");

        // Navigates back to the main menu
        private void ReturnToMenu() => _view.NavigateToMenu();
    }
}
