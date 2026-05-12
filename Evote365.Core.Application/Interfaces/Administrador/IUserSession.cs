namespace Evote365.Core.Application.Interfaces.Administrador
{
    public interface IUserSession
    {
        bool HasUser();
        bool IsAdmin();
        int GetUserId();
        string GetUserName();
        string GetUserEmail();

        int GetPartidoId();

        string GetRol();

        bool IsDirigente();

    }
}
