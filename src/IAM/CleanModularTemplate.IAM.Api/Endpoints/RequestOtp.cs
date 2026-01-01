using CleanModularTemplate.IAM.Api.Data;
using CleanModularTemplate.IAM.Api.Services;
using CleanModularTemplate.IAM.Contracts;
using CleanModularTemplate.Shared.Constants;
using FastEndpoints;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace CleanModularTemplate.IAM.Api.Endpoints;

internal sealed class RequestOtpEndpoint : Endpoint<RequestOtpRequest, RequestOtpResponse>
{
  private readonly UserManager<ApplicationUser> _userManager;
  private readonly SmsService _smsService;
  public RequestOtpEndpoint(UserManager<ApplicationUser> userManager, SmsService smsService)
  {
	_userManager = userManager;
	_smsService = smsService;
  }

  public override void Configure()
  {
	Post("otp/request");
	Group<IamGroup>();
	Description(b => b
		.Produces<RequestOtpResponse>()
		.Produces(StatusCodes.Status401Unauthorized)
		.Produces(StatusCodes.Status429TooManyRequests)
		.Produces(StatusCodes.Status401Unauthorized)
		.Produces(StatusCodes.Status500InternalServerError)
	);
	Summary(s =>
	{
	  s.Summary = "Request OTP for authentication";
	  s.Description = "Request OTP for authentication";
	  s.ExampleRequest = new RequestOtpRequest
	  {
		RequiredRole = "Customer",
		PhoneNumber = "+966512345678"
	  };
	});
	AllowAnonymous();
  }

  public override async Task HandleAsync(RequestOtpRequest req, CancellationToken ct)
  {
	var user = await _userManager.FindByNameAsync(req.PhoneNumber);

	if (user is null && req.RequiredRole == Role.Customer)
	{
	  user = new ApplicationUser
	  {
		UserName = req.PhoneNumber,
		PhoneNumber = req.PhoneNumber
	  };
	  var created = await _userManager.CreateAsync(user);

	  if (!created.Succeeded)
	  {
		ThrowError("Could not create user: " + string.Join(", ", created.Errors.Select(e => e.Description)));
		await Send.ErrorsAsync(StatusCodes.Status500InternalServerError, ct);
		return;
	  }
	  await _userManager.AddToRoleAsync(user, Role.Customer);
	}
	if (user is null)
	{
	  await Send.UnauthorizedAsync(ct);
	  return;
	}
	if (await _userManager.IsLockedOutAsync(user))
	{
	  await Send.ErrorsAsync(StatusCodes.Status429TooManyRequests, ct);
	  return;
	}


	var code = await _userManager.GenerateUserTokenAsync(user, TokenOptions.DefaultPhoneProvider, TokenPurpose.SmsLogin);
	await _smsService.SendOtp(req.PhoneNumber, code, ct);
	await Send.OkAsync(new RequestOtpResponse
	{
	  Message = "OTP sent successfully"
	}, ct);
  }

}

internal sealed record RequestOtpRequest
{
  public string PhoneNumber { get; set; } = string.Empty;
  public string RequiredRole { get; set; } = string.Empty;
}

internal sealed class RequestOtpValidator : Validator<RequestOtpRequest>
{

  public RequestOtpValidator()
  {
	RuleFor(x => x.PhoneNumber)
		.Matches(RegexPatterns.PhoneNumber).WithMessage("Phone number is not valid")
		.NotEmpty().WithMessage("Phone number is required");
	RuleFor(x => x.RequiredRole)
		.NotEmpty()
		.WithMessage("Roles are required")
		.Must(role => Role.All.Any(r => r == role))
		.WithMessage($"Role should be one of {string.Join(",", Role.All)}");
  }
}

internal sealed class RequestOtpResponse
{
  public string Message { get; set; } = string.Empty;
}
