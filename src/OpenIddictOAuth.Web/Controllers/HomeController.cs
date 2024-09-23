using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using OpenIddictOAuth.Web.Models;
using OpenIddictOAuth.Web.Services;

namespace OpenIddictOAuth.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ICurrentUserProvider _currentUserProvider;
    
    public HomeController(
        ILogger<HomeController> logger, 
        ICurrentUserProvider currentUserProvider)
    {
        _logger = logger;
        _currentUserProvider = currentUserProvider;
    }

    public IActionResult Index()
    {
        if (!_currentUserProvider.GetCurrentUserId().HasValue)
        {
            return RedirectToAction("", "Login");
        }
        
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }
}