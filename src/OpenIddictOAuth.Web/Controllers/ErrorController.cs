using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using OpenIddictOAuth.Web.Models;

namespace OpenIddictOAuth.Web.Controllers;

public class ErrorController : Controller
{
    [Route("error")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error(int statusCode = 0)
    {
        if (statusCode == 404)
        {
            return E404();
        }
        
        var error = Request.Query["error"].ToString();
        var error_description = Request.Query["error_description"].ToString();

        return View(new ErrorViewModel()
        {
            Error = error,
            ErrorDescription = error_description
        });
    }
    
    public ActionResult E404()
    {
        return View("Error404");
    }
}