using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SAM.Entities.Enum;
using SAM.Entities.Interfaces;
using System.Security.Claims;

namespace SAM.Api.Token;

public class AuthorizationFilter : IAsyncAuthorizationFilter
{
    private readonly ILogger logger;
    private ICurrentUser currentUser;

    public AuthorizationFilter(ILogger<AuthorizationFilter> logger, ICurrentUser currentUser)
    {
        this.logger = logger;
        this.currentUser = currentUser;
    }

    public Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        string controllerName = ((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor).ControllerName;

        if (controllerName == "Login")
            return Task.CompletedTask;

        string actionName = ((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor).ActionName;
        var role = Enum.Parse<LevelEnum>(context.HttpContext.User.Claims.FirstOrDefault(p => p.Type == ClaimTypes.Role)?.Value!);
        currentUser.Id = int.Parse(context.HttpContext.User.Claims.FirstOrDefault(p => p.Type == "idUser")?.Value!);
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
        if (!ok)
        {
            logger.LogError($"Usuário {currentUser.Id} não autorizado a acessar {controllerName}/{actionName}.");
            context.Result = new ChallengeResult();
        }
        return Task.CompletedTask;
    }

    private static bool ValidateTechnician(string controller, string action)
    {
        return controller switch
        {
            "Machine" => action == "Search",
            "OrderService" => true,
            "Unit" => action == "Search",
            "User" => action == "Search",
            _ => false,
        };
    }

    private static bool ValidateEmployee(string controller, string action)
    {
        return controller switch
        {
            "Machine" => action == "Search",
            "OrderService" => (action == "Create") || (action == "Search"),
            "Unit" => action == "Search",
            "User" => action == "Search",
            _ => false,
        };
    }


}
