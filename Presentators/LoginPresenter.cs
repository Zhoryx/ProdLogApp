using LoginApp.Model;
using LoginApp.View;

namespace LoginApp.Presenter
{
    public class LoginPresenter
    {
        private readonly ILoginView view;
        private readonly UserModel userModel;

        public LoginPresenter(ILoginView view)
        {
            this.view = view;
            this.userModel = new UserModel();
        }

        public void ValidateLogin()
        {
            string username = view.Username;
            string password = view.Password;

            if (userModel.Login(username, password))
            {
                view.ShowMessage("Inicio de sesión exitoso.");
            }
            else
            {
                view.ShowMessage("Usuario o contraseña incorrectos.");
            }
        }
    }
}
