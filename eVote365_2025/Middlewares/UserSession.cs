using Evote365.Core.Application.Interfaces.Administrador;

namespace eVote365_2025.Middlewares
{
    public class UserSession : IUserSession
    {
        private readonly IHttpContextAccessor _httpContext;

        public UserSession(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }

        public string GetRol()
        {
            var claim = _httpContext.HttpContext?.User?.FindFirst("Rol");
            return claim?.Value ?? "";
        }

        public bool IsDirigente()
        {
            return _httpContext.HttpContext?.User?.IsInRole("DirigentePolitico") ?? false;
        }

        public int GetPartidoId()
        {
            var claim = _httpContext.HttpContext?.User?.FindFirst("PartidoId");
            if (claim == null)
            {
                throw new Exception("Claim 'PartidoId' no encontrado en el contexto de usuario.");
            }

            return int.Parse(claim.Value);
        }

        public bool HasUser()
        {
            return _httpContext.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
        }

        public bool IsAdmin()
        {
            return _httpContext.HttpContext?.User?.IsInRole("Administrador") ?? false;
        }

        public int GetUserId()
        {
            var claim = _httpContext.HttpContext?.User?.FindFirst("Id");
            return claim != null ? int.Parse(claim.Value) : 0;
        }

        public string GetUserName()
        {
            return _httpContext.HttpContext?.User?.Identity?.Name ?? "";
        }

        public string GetUserEmail()
        {
            var claim = _httpContext.HttpContext?.User?.FindFirst("Email");
            return claim?.Value ?? "";
        }
    }
}
