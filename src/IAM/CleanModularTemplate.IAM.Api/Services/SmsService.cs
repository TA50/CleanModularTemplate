using Microsoft.Extensions.Logging;

namespace CleanModularTemplate.IAM.Api.Services;

public class SmsService
{
  private readonly ILogger<SmsService> _logger;
  public SmsService(ILogger<SmsService> logger)
  {
	_logger = logger;
  }
  public Task SendOtp(string phoneNumber, string code, CancellationToken cancellationToken = default)
  {
	cancellationToken.ThrowIfCancellationRequested();
	_logger.LogInformation("Sending OTP to {PhoneNumber} Code: {Code}", phoneNumber, code);
	return Task.CompletedTask;
  }
}
