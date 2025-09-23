// Interfaces/IAddUserView.cs
using ProdLogApp.Models;

namespace ProdLogApp.Interfaces
{
    public interface IAddUserView
    {
        // Lectura/escritura de campos (mantengo tus x:Name)
        string Nombre { get; set; }
        string DNI { get; set; }
        bool EsGerencial { get; set; }
        string PasswordGerencial { get; set; }

        // Modo edición vs alta
        void SetEditMode(bool editing);

        // UX
        void ShowInfo(string msg);
        void ShowWarn(string msg);
        void CloseDialogOK();     // cierra con DialogResult = true
        void CloseDialogCancel(); // opcional

        // Señales (eventos) de la vista
        event Action OnConfirm;
        event Action OnCancel;

        // (Opcional) Pre-cargar si viene un User
        void LoadUser(User user);
    }
}
