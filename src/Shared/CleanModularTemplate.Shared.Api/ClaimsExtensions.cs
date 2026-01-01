using System.Security.Claims;

namespace CleanModularTemplate.Shared.Api;

public static class ClaimsExtensions
{
  public static bool TryGetUserId(this ClaimsPrincipal principal, out Guid userId)
  {
	ArgumentNullException.ThrowIfNull(principal);
	var claim = principal.Claims.FirstOrDefault(x => string.Equals(x.Type, ClaimTypes.NameIdentifier, StringComparison.OrdinalIgnoreCase));
	return Guid.TryParse(claim?.Value, out userId);
  }
}
