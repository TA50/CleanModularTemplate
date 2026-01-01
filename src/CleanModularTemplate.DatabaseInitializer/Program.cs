using CleanModularTemplate.Accounts.Api;
using CleanModularTemplate.Accounts.Infrastructure;
using CleanModularTemplate.IAM.Api;
using CleanModularTemplate.ModuleTemplate.Api;
using CleanModularTemplate.ModuleTemplate.Infrastructure;
using CleanModularTemplate.ServiceDefaults;
using CleanModularTemplate.Shared.Infrastructure;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);
builder.AddServiceDefaults();
builder.AddSharedServices();

builder.AddAccounts([]);
builder.AddIam([]);
builder.AddModuleTemplate([]);

var app = builder.Build();
await app.InitAccountsDb();
await app.InitIamDatabase();
await app.InitModuleTemplatesDb();


