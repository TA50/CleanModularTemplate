using CleanModularTemplate.IAM.Api.Data;
using CleanModularTemplate.Shared;
using FastEndpoints;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace CleanModularTemplate.IAM.Api.Endpoints;

//ref: https://github.com/dotnet/aspnetcore > src/Identity/Core/src/IdentityApiEndpointRouteBuilderExtensions.cs:122
internal sealed class RefreshTokenEndpoint : Endpoint<RefreshTokenRequest>
{
  private readonly SignInManager<ApplicationUser> _signInManager;
  private readonly IOptionsMonitor<BearerTokenOptions> _bearerTokenOptions;
  private readonly ITimeProvider _timeProvider;
  public RefreshTokenEndpoint(IOptionsMonitor<BearerTokenOptions> bearerTokenOptions,
	  SignInManager<ApplicationUser> signInManager, ITimeProvider timeProvider)
  {
	_bearerTokenOptions = bearerTokenOptions;
	_signInManager = signInManager;
	_timeProvider = timeProvider;
  }
  public override void Configure()
  {
	Post("refresh");
	Group<IamGroup>();
	Summary(s =>
	{
	  s.Summary = "Refresh access token";
	  s.Description = "Refreshes the access token using a valid refresh token.";
	  s.ExampleRequest = new RefreshTokenRequest
	  {
		RefreshToken = "valid_refresh_token"
	  };
	  s.Responses[200] = "Access token refreshed successfully.";
	  s.Responses[401] = "Unauthorized.";
	});
	Description(b => b
		.Produces<AccessTokenResponse>()
		.Produces(StatusCodes.Status401Unauthorized)
	);
	AllowAnonymous();
  }
  public override async Task HandleAsync(RefreshTokenRequest req, CancellationToken ct)
  {
	var refreshTokenProtector = _bearerTokenOptions.Get(IdentityConstants.BearerScheme).RefreshTokenProtector;
	var refreshTicket = refreshTokenProtector.Unprotect(req.RefreshToken);

	if (refreshTicket?.Properties?.ExpiresUtc is not { } expiresUtc ||
		_timeProvider.GetUtcNow() >= expiresUtc ||
		await _signInManager.ValidateSecurityStampAsync(refreshTicket.Principal) is not { } user)

	{
	  await Send.UnauthorizedAsync(ct);
	  return;
	}
	_signInManager.AuthenticationScheme = IdentityConstants.BearerScheme;
	await _signInManager.SignInAsync(user, true);
  }
}

internal sealed record RefreshTokenRequest
{
  public string RefreshToken { get; set; } = string.Empty;
}
