using System;

namespace ProdLogApp.Models
{
    // Entidad de dominio para usuarios del sistema.
    // Mantiene datos mínimos + hash de password (si aplica gerente).
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Dni { get; set; } = string.Empty;
        public bool EsGerente { get; set; } = false;
        public DateTime? FechaIngreso { get; set; }
        public bool Activo { get; set; } = true;
        public string? PasswordHash { get; set; } // Almacenar hash + salt (no texto plano)
    }
}
