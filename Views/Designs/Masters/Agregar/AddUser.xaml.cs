using ProdLogApp.Interfaces;
using ProdLogApp.Models;
using System;
using System.Windows;
using System.Windows.Controls;

namespace ProdLogApp.Views
{
    public partial class AddUser : Window
    {
        private readonly IDatabaseService _databaseService;
        private readonly User _editingUser;
        private readonly bool _isEdit;

        // ========= Helpers para tolerar ambos XAML =========
        // Nombre
        private TextBox NombreTB => (TextBox)FindName("NombreTextBox");

        // DNI (acepta "DniTextBox" o el viejo "TextBox")
        private TextBox DniTB =>
            (TextBox)(FindName("DniTextBox") ?? FindName("TextBox"));

        // Gerencial (acepta "EsGerencialCheckBox" o el viejo "CheckBox")
        private CheckBox GerencialCB =>
            (CheckBox)(FindName("EsGerencialCheckBox") ?? FindName("CheckBox"));

        // Password: puede ser PasswordBox ("PasswordGerenteBox") o TextBox ("TextBoxPasswordGerente")
        private string PasswordValue
        {
            get
            {
                var pw = FindName("PasswordGerenteBox") as PasswordBox;
                if (pw != null) return pw.Password?.Trim() ?? string.Empty;
                var tb = FindName("TextBoxPasswordGerente") as TextBox;
                return tb?.Text?.Trim() ?? string.Empty;
            }
            set
            {
                var pw = FindName("PasswordGerenteBox") as PasswordBox;
                if (pw != null) { pw.Password = value ?? string.Empty; return; }
                var tb = FindName("TextBoxPasswordGerente") as TextBox;
                if (tb != null) tb.Text = value ?? string.Empty;
            }
        }

        public AddUser(IDatabaseService databaseService, User user = null)
        {
            _databaseService = databaseService ?? throw new ArgumentNullException(nameof(databaseService));
            _editingUser = user;

            InitializeComponent();

            // Botón por si existe (no rompe si no está)
            try { (FindName("btnConfirmar") as Button)!.IsDefault = true; } catch { /* ignore */ }

            if (_editingUser != null)
            {
                _isEdit = true;
                Title = "Modificar Usuario";
                try { (FindName("btnConfirmar") as Button)!.Content = "Guardar cambios"; } catch { /* ignore */ }

                // Prefill (usa los nombres reales que haya en tu XAML)
                if (NombreTB != null) NombreTB.Text = _editingUser.Name ?? string.Empty;
                if (DniTB != null) DniTB.Text = _editingUser.Dni ?? string.Empty;
                if (GerencialCB != null) GerencialCB.IsChecked = _editingUser.IsAdmin;
                PasswordValue = string.Empty; // por seguridad no se muestra
            }
            else
            {
                Title = "Agregar Usuario";
                if (GerencialCB != null) GerencialCB.IsChecked = false;
                PasswordValue = string.Empty;
            }
        }

        private async void Confirmar_Click(object sender, RoutedEventArgs e)
        {
            // Lee valores desde los controles que existan
            string nombre = NombreTB?.Text?.Trim();
            string dni = DniTB?.Text?.Trim();
            bool esGer = GerencialCB?.IsChecked == true;
            string pass = PasswordValue; // hoy no se persiste (no hay columna), queda listo para cuando la agregues

            // Validaciones mínimas
            if (string.IsNullOrWhiteSpace(nombre))
            {
                MessageBox.Show("Ingresá el nombre.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(dni))
            {
                MessageBox.Show("Ingresá el DNI.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            // Si querés exigir pass para gerenciales al crear, descomentá:
            // if (!_isEdit && esGer && string.IsNullOrWhiteSpace(pass))
            // {
            //     MessageBox.Show("Ingresá la contraseña para la cuenta gerencial.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
            //     return;
            // }

            try
            {
                if (_isEdit && _editingUser != null)
                {
                    var toUpdate = new User
                    {
                        Id = _editingUser.Id,
                        Name = nombre,
                        Dni = dni,
                        IsAdmin = esGer,
                        // si no cambias fecha de ingreso en edición, conservá la anterior:
                        Fingreso = _editingUser.Fingreso == default ? DateOnly.FromDateTime(DateTime.Today) : _editingUser.Fingreso,
                        Active = _editingUser.Active
                    };

                    await _databaseService.UpdateUser(toUpdate);
                    MessageBox.Show("Usuario actualizado correctamente.", "OK", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    var toCreate = new User
                    {
                        Name = nombre,
                        Dni = dni,
                        IsAdmin = esGer,
                        Fingreso = DateOnly.FromDateTime(DateTime.Today),
                        Active = true
                    };

                    await _databaseService.AddUser(toCreate);
                    MessageBox.Show("Usuario guardado correctamente.", "OK", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar el usuario:\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e) => Close();

        // (No se usa aquí; lo dejo por si lo llamás desde otro lado)
        private bool IsValidTimeFormat(string timeInput)
        {
            return TimeSpan.TryParseExact(timeInput, "hh\\:mm", null, out _);
        }
    }
}
