using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using OpenIddictOAuth.Infrastructure.CQRS;
using OpenIddictOAuth.Infrastructure.Exceptions;
using OpenIddictOAuth.Web.Data;

namespace OpenIddictOAuth.Web.Features.Login;

// Command
public record Login(string UsernameOrEmailAddress, string Password) : ICommand<LoginResult>;

// Validators
public class LoginValidator : AbstractValidator<Login>
{
    public LoginValidator()
    {
        RuleFor(x => x.UsernameOrEmailAddress).NotEmpty().WithMessage("Please enter the username or email address");
        RuleFor(x => x.Password).NotEmpty().WithMessage("Please enter the password");
    }
}

// Handlers
internal class LoginHandler(
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager
) : ICommandHandler<Login, LoginResult>
{
    public async Task<LoginResult> Handle(Login request, CancellationToken cancellationToken = default)
    {
        var applicationUser = await userManager.FindByNameAsync(request.UsernameOrEmailAddress)
            ?? await userManager.FindByEmailAsync(request.UsernameOrEmailAddress);

        if (applicationUser == null)
        {
            throw new InvalidUserException();
        }

        var signInResult = await signInManager.CheckPasswordSignInAsync(applicationUser, request.Password, false);

        if (signInResult.IsLockedOut)
        {
            throw new BadRequestException($"Your account has been temporarily locked due to multiple unsuccessful login attempts.");
        }

        if (!signInResult.Succeeded)
        {
            throw new InvalidUserException();
        }

        var authenticationProperties = new AuthenticationProperties
        {
            IsPersistent = true,
            ExpiresUtc = DateTimeOffset.Now.Add(TimeSpan.FromDays(30))
        };

        await signInManager.SignInAsync(applicationUser, authenticationProperties);

        return new LoginResult();
    }
}

// Results
public record LoginResult;