using CleanModularTemplate.Accounts.Api;
using CleanModularTemplate.IAM.Api;
using CleanModularTemplate.Shared.Api;
using FastEndpoints;
using FastEndpoints.Swagger;

namespace CleanModularTemplate.Host;

internal static class ProblemDetailsConfigExtensions
{
  public static void AddRfcTypes(this ErrorOptions.ProblemDetailsConfig config)
  {
	config.TypeTransformer = pd => pd.Status switch
	{
	  // RFC 7231: Semantics and Content
	  400 => "https://tools.ietf.org/html/rfc7231#section-6.5.1",
	  403 => "https://tools.ietf.org/html/rfc7231#section-6.5.3",
	  404 => "https://tools.ietf.org/html/rfc7231#section-6.5.4",
	  405 => "https://tools.ietf.org/html/rfc7231#section-6.5.5",
	  406 => "https://tools.ietf.org/html/rfc7231#section-6.5.6",
	  415 => "https://tools.ietf.org/html/rfc7231#section-6.5.13",
	  500 => "https://tools.ietf.org/html/rfc7231#section-6.6.1",
	  503 => "https://tools.ietf.org/html/rfc7231#section-6.6.4",
	  401 => "https://tools.ietf.org/html/rfc7235#section-3.1",
	  422 => "https://tools.ietf.org/html/rfc4918#section-11.2",
	  429 => "https://tools.ietf.org/html/rfc6585#section-4",
	  _ => "about:blank",
	};
  }
  public static void AddRfcTitles(this ErrorOptions.ProblemDetailsConfig config)
  {
	config.TitleTransformer = pd => pd.Status switch
	{
	  400 => "Bad Request", // RFC 7231
	  401 => "Unauthorized", // RFC 7235
	  403 => "Forbidden", // RFC 7231
	  404 => "Not Found", // RFC 7231
	  405 => "Method Not Allowed", // RFC 7231
	  406 => "Not Acceptable", // RFC 7231
	  415 => "Unsupported Media Type", // RFC 7231
	  422 => "Unprocessable Entity", // RFC 4918
	  429 => "Too Many Requests", // RFC 6585
	  500 => "Internal Server Error", // RFC 7231
	  _ => "An error occurred" // Fallback
	};
  }


  public static IServiceCollection ConfigureSwaggerDocumentApi(this IServiceCollection services)
  {
	return services.SwaggerDocument(o =>
	{
	  o.ShortSchemaNames = true;
	  o.DocumentSettings = s =>
		  {
		  s.Title = "Gold Loop API";
		  s.Version = "v1";
		  s.Description = "HTTP endpoints for the Gold Loop Api";
		  s.PostProcess = doc =>
			  {
			  var groups = new List<object>();
			  groups.AddTagGroups(AccountsApiExtensions.AccountsTagGroups, IamExtensions.IamTagGroups);
			  doc.ExtensionData?.Add("x-tagGroups", groups);
			};
		};
	});
  }

  private static void AddTagGroups(this List<object> groups, params SwaggerTagGroups[] tagGroups)
  {
	foreach (var tagGroup in tagGroups)
	{
	  groups.Add(new
	  {
		name = tagGroup.Name,
		tags = tagGroup.Tags
	  });
	}
  }
}
