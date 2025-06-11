using ProdLogApp.Models;
using ProdLogApp.Views;

namespace ProdLogApp.Presenters;
public class PasswordRequestPresenter
{
    private readonly PasswordRequest _view;

    public PasswordRequestPresenter(PasswordRequest view)
    {
        _view = view;
    }

    public void ValidatePassword(User activeUser, string password)
    {
        if (User.PasswordValidate(activeUser, password))
        {
            _view.ShowAdminMenu(activeUser);
        }
        else
        {
            _view.ShowMessage("Contraseña incorrecta, intenta de nuevo.");
        }
    }
}
