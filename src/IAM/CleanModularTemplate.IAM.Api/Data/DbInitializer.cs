// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Globalization;
using System.Security.Claims;
using CleanModularTemplate.IAM.Contracts;
using Microsoft.AspNetCore.Identity;

namespace CleanModularTemplate.IAM.Api.Data;

internal static class IamDbInitializer
{
  public static async Task SeedAsync(RoleManager<ApplicationRole> roleManager)
  {
	// Define your Role -> Permissions mapping

	foreach (var role in Role.All)
	{
	  // 1. Ensure the Role exists
	  var applicationRole = await roleManager.FindByNameAsync(role.Name);
	  if (applicationRole == null)
	  {
		applicationRole = new ApplicationRole
		{
		  Name = role.Name,
		  NormalizedName = role.Name.ToUpper(CultureInfo.InvariantCulture)
		};
		await roleManager.CreateAsync(applicationRole);
	  }

	  // 2. Get existing claims for this role to avoid duplicates
	  var existingClaims = await roleManager.GetClaimsAsync(applicationRole);


	  foreach (var permission in role.Permissions)
	  {
		// Only add the claim if it doesn't already exist
		if (!existingClaims.Any(c => c.Type == "permission" && c.Value == permission))
		{
		  await roleManager.AddClaimAsync(applicationRole, new Claim("permission", permission));
		}
	  }
	}
  }
}
