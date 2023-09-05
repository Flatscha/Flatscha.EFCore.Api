using Flatscha.EFCore.Api.TestConsole.Context;
using Microsoft.EntityFrameworkCore;
using Flatscha.EFCore.Api.Generators;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<TestContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Test"));
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseRouting();

var api = app.MapGroup("api");

api.MapTestContext();

app.Run();