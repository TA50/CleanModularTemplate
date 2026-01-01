using System.Net;
using CleanModularTemplate.IAM.Api.Data;
using CleanModularTemplate.Shared.Constants;
using FastEndpoints;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace CleanModularTemplate.IAM.Api.Endpoints;

internal sealed class VerifyOtpEndpoint : Endpoint<VerifyOtpRequest>
{
  private readonly SignInManager<ApplicationUser> _signInManager;
  private readonly UserManager<ApplicationUser> _userManager;
  public VerifyOtpEndpoint(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
  {
	_signInManager = signInManager;
	_userManager = userManager;
  }

  public override void Configure()
  {
	Post("otp/verify");
	Group<IamGroup>();

	Summary(s =>
	{
	  s.Summary = "Verify SMS OTP and retrieve access token";
	  s.Description = "Validates the one-time password (OTP) sent via SMS. " +
						  "If the code is valid and the user is not locked out, " +
						  "this endpoint signs the user in and returns an authentication token.\n\n" +
						  "**Security Note:** Repeated failed attempts will result in a temporary account lockout (HTTP 429).";

	  // Provides a pre-filled JSON example in the Swagger UI
	  s.ExampleRequest = new VerifyOtpRequest
	  {
		PhoneNumber = "+966512345678",
		Code = "123456"
	  };
	  s.Responses[200] = "Successfully authenticated. Returns the Access Token.";
	  s.Responses[401] = "Authentication failed. Invalid Phone Number or OTP Code.";
	  s.Responses[429] = "Account is locked out due to multiple failed attempts.";
	  s.ResponseExamples.Add(StatusCodes.Status400BadRequest, new ErrorResponse
	  {
		Message = "One or more errors occurred!",
		StatusCode = StatusCodes.Status400BadRequest,
		Errors =
			{
					["phoneNumber"] = ["Phone number is not valid", "Phone number is required"], ["code"] = ["Code is required"]
			}
	  });
	});

	Description(b => b
		.Produces<AccessTokenResponse>(200, "application/json")
		.Produces(StatusCodes.Status401Unauthorized)
		.Produces(StatusCodes.Status429TooManyRequests)
	);
	AllowAnonymous();
  }

  public override async Task HandleAsync(VerifyOtpRequest req, CancellationToken ct)
  {
	var user = await _userManager.FindByNameAsync(req.PhoneNumber);
	if (user == null)
	{
	  await Send.UnauthorizedAsync(ct);
	  return;
	}

	if (await _userManager.IsLockedOutAsync(user))
	{
	  AddError("Locked out.");
	  await Send.ErrorsAsync((int)HttpStatusCode.TooManyRequests, ct);
	  return;
	}

	var isValid = await _userManager.VerifyUserTokenAsync(user, TokenOptions.DefaultPhoneProvider, TokenPurpose.SmsLogin, req.Code);
	if (!isValid)
	{
	  await _userManager.AccessFailedAsync(user);
	  await Send.UnauthorizedAsync(ct);
	  return;
	}

	await _userManager.ResetAccessFailedCountAsync(user);

	_signInManager.AuthenticationScheme = IdentityConstants.BearerScheme;
	await _signInManager.SignInAsync(user, true);
  }
}

internal sealed record VerifyOtpRequest
{
  public string PhoneNumber { get; set; } = string.Empty;
  public string Code { get; set; } = string.Empty;

}

internal sealed class VerifyOtpValidator : Validator<VerifyOtpRequest>
{
  public VerifyOtpValidator()
  {
	RuleFor(x => x.PhoneNumber)
		.Matches(RegexPatterns.PhoneNumber)
		.WithMessage("Phone number is not valid")
		.NotEmpty().WithMessage("Phone number is required");

	RuleFor(x => x.Code).NotEmpty()
		.WithMessage("Code is required");
  }
}
