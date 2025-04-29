using AppControle.Services;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

public class PolicyHandler : AuthorizationHandler<IAuthorizationRequirement>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly PermissionService _permissionService;

    public PolicyHandler(IHttpContextAccessor httpContextAccessor, PermissionService permissionService)
    {
        _httpContextAccessor = httpContextAccessor;
        _permissionService = permissionService;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, IAuthorizationRequirement requirement)
    {
        var httpContext = _httpContextAccessor.HttpContext;

        if (httpContext == null)
        {
            context.Fail();
            return;
        }

        string permissionName = httpContext.Request.Headers["Permission"].ToString();

        var token = httpContext.Request.Headers["Authorization"];
        var tokens = token[0].Substring("Bearer ".Length).Trim();
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(tokens);
        var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "UserId");
        int userId = int.Parse(userIdClaim?.Value);

        bool hasPermission = await _permissionService.Verifypermission(userId, permissionName);

        if (hasPermission)
        {
            context.Succeed(requirement);
        }
        else
        {
            context.Fail();
        }
    }
}
