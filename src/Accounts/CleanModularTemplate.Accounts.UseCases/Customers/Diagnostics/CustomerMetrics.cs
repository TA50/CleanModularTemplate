using System.Diagnostics.Metrics;

namespace CleanModularTemplate.Accounts.UseCases.Customers.Diagnostics;

internal sealed class CustomerMetrics : IDisposable
{
  public const string MeterName = AccountDiagnostics.CustomersMeterName;

  public const string CustomerCreatedCounterName = "customers.created";
  private readonly Meter _meter;
  private readonly Counter<long> _customerCreatedCounter;

  public CustomerMetrics(IMeterFactory meterFactory)
  {
	_meter = meterFactory.Create(MeterName);

	_customerCreatedCounter = _meter.CreateCounter<long>(
		CustomerCreatedCounterName,
		unit: "{customer}",
		description: "Total number of customers created.");
  }

  public void CustomerCreated()
  {
	if (!_customerCreatedCounter.Enabled) return;
	_customerCreatedCounter.Add(1);
  }

  public void Dispose()
  {
	_meter.Dispose();
  }
}
