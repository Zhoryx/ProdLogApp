using System;

namespace ProdLogApp.Models
{
    public sealed class UserSession
    {
        private static readonly Lazy<UserSession> _inst = new(() => new UserSession());
        public static UserSession GetInstance() => _inst.Value;

        private UserSession() { }

        // Nullable para permitir sesión vacía
        public Usuario? ActiveUser { get; private set; }

        // Métodos existentes (se mantienen intactos)
        public void Set(Usuario user) => ActiveUser = user ?? throw new ArgumentNullException(nameof(user));
        public void Clear() => ActiveUser = null;
        public bool IsLoggedIn => ActiveUser != null;

        // ==== Nuevos alias compatibles con el resto del código ====

        /// <summary>
        /// Alias de Set(user), usado en distintos presenters.
        /// </summary>
        public void SignIn(Usuario user) => Set(user);

        /// <summary>
        /// Alias de Clear(), utilizado en ManagerMenu o logout.
        /// </summary>
        public void SignOut() => Clear();
    }
}
