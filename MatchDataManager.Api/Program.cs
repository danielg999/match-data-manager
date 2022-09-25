using FluentValidation;
using FluentValidation.AspNetCore;
using MatchDataManager.Api.Entities;
using MatchDataManager.Api.Middleware;
using MatchDataManager.Api.Models;
using MatchDataManager.Api.Models.Validations;
using MatchDataManager.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
AddServices(builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

void AddServices(IServiceCollection services)
{
    services.AddControllers();
    services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();

    services.AddTransient<ILocationService, LocationService>();
    services.AddTransient<ITeamService, TeamService>();

    services.AddScoped<IValidator<LocationCreateDto>, LocationCreateDtoValidator>();
    services.AddScoped<IValidator<LocationUpdateDto>, LocationUpdateDtoValidator>();
    services.AddScoped<IValidator<TeamCreateDto>, TeamCreateDtoValidator>();
    services.AddScoped<IValidator<TeamUpdateDto>, TeamUpdateDtoValidator>();
    services.AddScoped<ErrorHandlingMiddleware>();

    services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    services.AddDbContext<MatchDataManagerDbContext>();
}