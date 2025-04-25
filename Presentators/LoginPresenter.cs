using ProdLogApp.Views;

namespace ProdLogApp
{
    public class LoginPresenter
    {
        private readonly Login _view;
        private readonly LoginModel _model;

        public LoginPresenter(Login view)
        {
            _view = view;
            _model = new LoginModel();
        }

        public LoginPresenter(Login view, LoginModel model)
        {
            _view = view;
            _model = model;
        }

        public void ValidarIngreso()
        {
           
        }
    }
}