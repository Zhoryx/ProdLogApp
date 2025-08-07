using ProdLogApp.Interfaces;
using System;
using System.Windows.Controls;

public class AddProductionUserPresenter
{
    private readonly IAddProductionUserView _view;

    public AddProductionUserPresenter(IAddProductionUserView view)
    {
        _view = view;
    }

    public void ValidarHora(TextBox campo, string input)
    {
        input = input.Trim();

        if (string.IsNullOrWhiteSpace(input))
        {
            _view.MostrarError(campo, "La hora no puede estar vacía.");
            return;
        }

        if (TimeSpan.TryParseExact(input, "hh\\:mm", null, out TimeSpan parsed))
        {
            _view.ActualizarHora(campo, parsed.ToString(@"hh\:mm"));
            _view.LimpiarError(campo);
            return;
        }

        if (int.TryParse(input, out int valor))
        {
            string digits = valor.ToString();

            if (digits.Length <= 2)
            {
                int minutes = valor;
                if (minutes >= 0 && minutes < 60)
                {
                    _view.ActualizarHora(campo, $"00:{minutes:D2}");
                    _view.LimpiarError(campo);
                    return;
                }
            }
            else if (digits.Length == 3 || digits.Length == 4)
            {
                string horaStr = digits.PadLeft(4, '0');
                string horas = horaStr.Substring(0, 2);
                string minutos = horaStr.Substring(2, 2);

                if (int.TryParse(horas, out int h) && int.TryParse(minutos, out int m)
                    && h >= 0 && h < 24 && m >= 0 && m < 60)
                {
                    _view.ActualizarHora(campo, $"{h:D2}:{m:D2}");
                    _view.LimpiarError(campo);
                    return;
                }
            }
        }

        _view.MostrarError(campo, "Formato de hora inválido. Ej: 15, 1500 o 15:00");
    }

    public void ValidarCantidad(TextBox campo, string input)
    {
        input = input.Trim();

        if (string.IsNullOrWhiteSpace(input))
        {
            _view.MostrarError(campo, "El campo cantidad no puede estar vacío.");
            return;
        }

        if (!int.TryParse(input, out int cantidad))
        {
            _view.MostrarError(campo, "Ingrese un número entero válido.");
            return;
        }

        if (cantidad <= 0)
        {
            _view.MostrarError(campo, "La cantidad debe ser mayor a cero.");
            return;
        }

        _view.LimpiarError(campo);
    }
}
