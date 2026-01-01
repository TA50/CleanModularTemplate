using FastEndpoints;

namespace CleanModularTemplate.IAM.Api.Endpoints;

public sealed class IamGroup : Group
{
  public IamGroup()
  {
	Configure("iam/v1", ep =>
	{
	  ep.Summary(o => { o.Description = "IAM"; });
	});
  }
}
