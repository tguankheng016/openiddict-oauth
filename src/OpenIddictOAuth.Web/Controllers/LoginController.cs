using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using OpenIddictOAuth.Web.Features.Login;
using OpenIddictOAuth.Web.Models.Login;

namespace OpenIddictOAuth.Web.Controllers;

[Route("login")]
[ApiExplorerSettings(IgnoreApi = true)]
public class LoginController : OAuthControllerBase
{
	private readonly IMediator _mediator;
	private readonly IMapper _mapper;

	public LoginController(
		IOpenIddictApplicationManager applicationManager,
		IMediator mediator,
		IMapper mapper
	) : base(
		applicationManager
	)
	{
		_mediator = mediator;
		_mapper = mapper;
	}

	[HttpGet]
	public async Task<IActionResult> Index()
	{
		var model = new LoginViewModel()
		{
			UsernameOrEmailAddress = "admin",
			Password = "123qwe"
		};

		var returnUrl = Request.Query["returnUrl"].ToString();

		VerifyReturnUrl(returnUrl);

		var application = await GetApplicationFromUrlAsync(returnUrl);

		if (application != null)
		{
			ViewBag.ApplicationName = await ApplicationManager.GetDisplayNameAsync(application);
		}

		ViewBag.ReturnUrl = string.IsNullOrEmpty(returnUrl) ? returnUrl : Uri.EscapeDataString(returnUrl);

		return View(model);
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Login(LoginViewModel model)
	{
		var returnUrl = Request.Query["returnUrl"].ToString();

		var loginCommand = _mapper.Map<Login>(model);

		await _mediator.Send(loginCommand);

		return Json(new { redirectUrl = RedirectionUrlAfterSignIn(returnUrl) });
	}
}