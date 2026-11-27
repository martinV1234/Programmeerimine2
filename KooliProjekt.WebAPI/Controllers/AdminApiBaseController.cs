using Microsoft.AspNetCore.Authorization;
namespace KooliProjekt.WebAPI.Controllers;
[Authorize(Roles = "Admin")]
public abstract class AdminApiBaseController : ApiControllerBase { }
