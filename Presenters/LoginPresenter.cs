using ProdLogApp.Models;
using ProdLogApp.Views.Interfaces;

namespace ProdLogApp.Presenters
{
    public class LoginPresenter
    {
        private readonly ILoginView _view;

        public LoginPresenter(ILoginView view)
        {
            _view = view;
        }

        public void ValidateLogin()
        {
            User user = User.GetByDni(_view.Dni);

            if (user != null)
            {
                if (user.IsAdmin)
                {
                    _view.ShowAdminWindow(user); 
                }
                else
                {
                    _view.ShowMainWindow(user); 
                }
            }
            else
            {
                _view.ShowMessage("Usuario no encontrado, revise su DNI");
            }
        }
    }
}
