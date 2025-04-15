namespace LoginApp.View
{
    public interface ILoginView
    {
        void ShowMessage(string message);
        string Username { get; }
        string Password { get; }
    }
}
