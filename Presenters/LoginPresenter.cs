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
            User activeUser = null;

            _view.ShowMainWindow(activeUser);
            //User activeUser = User.GetByDni(_view.Dni);

            //if (activeUser != null)
            //{
            //    if (activeUser.IsAdmin)
            //    {
            //        _view.ShowAdminWindow(activeUser); 
            //    }
            //    else
            //    {
            //        _view.ShowMainWindow(activeUser); 
            //    }
            //}
            //else
            //{
            //    _view.ShowMessage("Usuario no encontrado, revise su DNI");
            //}
        }
    }
}
