using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PremierBankTesting.Application.Services;
using PremierBankTesting.Bank;
using PremierZalTesting.Data;
using PremierZalTesting.Data.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

builder.Services.AddDbContext<PremierZalTestingBDContext>
    (options =>
    {
        options.UseNpgsql(builder.Configuration.GetConnectionString(nameof(PremierZalTestingBDContext)));
    }
  );
builder.Services.AddScoped<IBankApiClient, BankApiClient>();
builder.Services.AddScoped<IPremierBankTestingRepository, PremierBankTestingRepository>();
builder.Services.AddScoped<IPremierBankTestingServices, PremierBankTestingServices>();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();