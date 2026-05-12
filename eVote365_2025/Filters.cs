using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

public class ValidarRolAttribute : ActionFilterAttribute
{
    private readonly string _rolRequerido;

    public ValidarRolAttribute(string rolRequerido)
    {
        _rolRequerido = rolRequerido;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var usuario = context.HttpContext.User;

        if (!usuario.Identity.IsAuthenticated)
        {
            context.Result = new RedirectToActionResult("Index", "Login", null);
            return;
        }

        var rol = usuario.FindFirst(ClaimTypes.Role)?.Value;

        if (rol != _rolRequerido)
        {
            context.Result = new RedirectToActionResult("AccessDenied", "Login", null);
        }
    }
}