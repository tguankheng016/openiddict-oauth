using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using OpenIddictOAuth.Web.Extensions;
using OpenIddictOAuth.Web.Models;
using OpenIddictOAuth.Web.Services;

namespace OpenIddictOAuth.Web.Controllers;

public class HomeController : Controller
{
	private readonly ILogger<HomeController> _logger;
	private readonly ICurrentUserProvider _currentUserProvider;
	private readonly IOpenIddictAuthorizationManager _authorizationManager;

	public HomeController(
		ILogger<HomeController> logger,
		ICurrentUserProvider currentUserProvider,
		IOpenIddictAuthorizationManager authorizationManager)
	{
		_logger = logger;
		_currentUserProvider = currentUserProvider;
		_authorizationManager = authorizationManager;
	}

	public async Task<IActionResult> Index()
	{
		if (!_currentUserProvider.GetCurrentUserId().HasValue)
		{
			return RedirectToAction("", "Login");
		}

		var authorizations = await _authorizationManager
			.FindBySubjectAsync(_currentUserProvider.GetCurrentUserId()!.Value.ToString())
			.ToListAsync();

		ViewBag.HasConnectedApps = authorizations.Count > 0;

		return View();
	}

	public IActionResult Privacy()
	{
		return View();
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> RemoveConnectedApps()
	{
		if (!_currentUserProvider.GetCurrentUserId().HasValue)
		{
			return RedirectToAction("", "Login");
		}

		var authorizations = await _authorizationManager
			.FindBySubjectAsync(_currentUserProvider.GetCurrentUserId()!.Value.ToString())
			.ToListAsync();

		foreach (var authorization in authorizations)
		{
			await _authorizationManager.DeleteAsync(authorization);
		}

		return RedirectToAction("Index");
	}
}