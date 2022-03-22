namespace Remato
{
    public interface IUserParams
    {
        IUserParams Id(string userId);
        IUserParams Username(string username);
        IUserParams Password(string password);
        IUserParams Mail(string mail);
        IUserParams Name(string name);
        IUserParams IsEnabled(bool isEnabled);
        IUserParams IsAdmin(bool isAdmin);
    }
}