using ProdLogApp.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProdLogApp.Presenters
{
    private readonly OperatorMenuPresenter _view;
    internal class OperatorMenuPresenter
    {
        public OperatorMenuPresenter(OperatorMenuPresenter view)
        {
            _view = view;
        }
    }
}
