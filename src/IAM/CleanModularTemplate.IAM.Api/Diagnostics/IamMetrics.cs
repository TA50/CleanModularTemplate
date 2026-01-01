using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace CleanModularTemplate.IAM.Api.Diagnostics;

internal sealed class IamMetrics : IDisposable
{
  public const string MeterName = "CleanModularTemplate.Iam";

  public const string TokenRefreshCounterName = "iam.token.refreshes";
  private readonly Meter _meter;
  private readonly Counter<long> _tokenRefreshCounter;

  public IamMetrics(IMeterFactory meterFactory)
  {
	_meter = meterFactory.Create(MeterName);

	_tokenRefreshCounter = _meter.CreateCounter<long>(
	TokenRefreshCounterName,
	unit: "{refresh}",
	description: "Total number of Access Tokens refreshed.");
  }

  public void TokenRefreshed(bool success, Exception? exception = null)
  {
	if (!_tokenRefreshCounter.Enabled) return;

	var tags = new TagList
		{
			{
				"iam.status", success ? "success" : "failure"
			}
		};

	AddErrorTag(ref tags, exception);
	_tokenRefreshCounter.Add(1, tags);
  }

  public void Dispose()
  {
	_meter.Dispose();
  }

  // --- Helpers (Optimized for low allocation) ---

  private static void AddErrorTag(ref TagList tags, Exception? exception)
  {
	if (exception != null)
	{
	  tags.Add("error.type", exception.GetType().Name);
	}
  }
}
