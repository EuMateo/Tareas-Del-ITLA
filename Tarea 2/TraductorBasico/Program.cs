using Microsoft.EntityFrameworkCore;
using TraductorBasico.Infrastructure.Context;
using TraductorBasico.Application.Services;
using TraductorBasico.Contract;
using TraductorBasico.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services.AddDbContext<TraductorBasicoDataContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null)
    ));


builder.Services.AddScoped<IFraseService, FraseService>();
builder.Services.AddScoped<IFraseRepository, FraseRepository>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();