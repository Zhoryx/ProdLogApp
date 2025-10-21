using System;

namespace ProdLogApp.Models
{
    // Sesión de usuario (Singleton simple).
    // Se usa para saber si hay un usuario logueado y quién es.
    public sealed class UserSession
    {
        private static readonly Lazy<UserSession> _inst = new(() => new UserSession());
        public static UserSession GetInstance() => _inst.Value;

        private UserSession() { }

        // Nullable para permitir sesión vacía
        public Usuario? ActiveUser { get; private set; }

        // Setea el usuario activo (login ok).
        public void Set(Usuario user) => ActiveUser = user ?? throw new ArgumentNullException(nameof(user));

        // Limpia la sesión (logout).
        public void Clear() => ActiveUser = null;

        // Indica si hay alguien autenticado.
        public bool IsLoggedIn => ActiveUser != null;

        // ==== Alias compatibles con el resto del código ====
        // Alias de Set(user), usado en distintos presenters.
        public void SignIn(Usuario user) => Set(user);

        // Alias de Clear(), utilizado en ManagerMenu o logout.
        public void SignOut() => Clear();
    }
}
