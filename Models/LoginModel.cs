using System;

public class LoginModel
{
    public bool ValidarDni(string dni)
    {
        // Simulación de validación simple
        return dni == "12345678";  // o podés consultar una DB real
    }
}