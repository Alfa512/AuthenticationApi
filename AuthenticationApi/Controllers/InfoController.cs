using Microsoft.AspNetCore.Mvc;

namespace AuthenticationApi.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class InfoController : ControllerBase
{
    [HttpGet]
    [Route("/ip-address")]
    public async Task<string> GetIpAddress()
    {
        var remoteIpAddress = this.HttpContext.Request.HttpContext.Connection.RemoteIpAddress;
        if (remoteIpAddress != null)
            return await Task.FromResult(remoteIpAddress.ToString());
        return string.Empty;
    }
}