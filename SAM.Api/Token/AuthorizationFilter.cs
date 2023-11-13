using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SAM.Entities.Enum;
using System.Diagnostics.Eventing.Reader;
using System.Security.Claims;

namespace SAM.Api.Token;

public class AuthorizationFilter : IAsyncAuthorizationFilter
{
    public Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        string controllerName = ((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor).ControllerName;

        if (controllerName == "Login" )
            return Task.CompletedTask;

        string actionName = ((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor).ActionName;
        var role = Enum.Parse<LevelEnum>(context.HttpContext.User.Claims.FirstOrDefault(p => p.Type == ClaimTypes.Role)?.Value);
        bool ok = false;

        if (actionName == "Get" || actionName == "GetAll")
        {
            ok = true;
        }
        else
        {
            switch (role)
            {
                case LevelEnum.Manager: ok = true; break;
                case LevelEnum.Technician: ok = ValidateTechnician(controllerName, actionName); break;
                case LevelEnum.Employee: ok = ValidateEmployee(controllerName, actionName); break;
            }
        }
        if (!ok) context.Result = new ChallengeResult();
        return Task.CompletedTask;
    }

    private bool ValidateTechnician(string controller, string action)
    {
        bool ok = true;
        switch (controller)
        {
            case "Machine": ok = false; break;
            case "OrderService": ok = true; break;
            case "Unit": ok = false; break;
            case "User": ok = false; break;
        }
        return ok;
    }

    private bool ValidateEmployee(string controller, string action)
    {
        bool ok = true;

        switch (controller)
        {
            case "Machine": ok = false; break;
            case "OrderService": ok = (action == "Create"); break;
            case "Unit": ok = false; break;
            case "User": ok = false; break;
        }

        return ok;
    }


}
