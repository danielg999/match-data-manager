using FluentValidation;
using FluentValidation.AspNetCore;
using MatchDataManager.Api.Entities;
using MatchDataManager.Api.Middleware;
using MatchDataManager.Api.Models;
using MatchDataManager.Api.Models.Validations;
using MatchDataManager.Api.Repository;
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

app.UseMiddleware<ErrorHandlingMiddleware>();

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

    services.AddTransient<LocationCreateDtoValidator>();
    services.AddTransient<IValidator<LocationCreateDto>, LocationCreateDtoValidator>();
    services.AddTransient<IValidator<LocationUpdateDto>, LocationUpdateDtoValidator>();
    services.AddTransient<IValidator<TeamCreateDto>, TeamCreateDtoValidator>();
    services.AddTransient<IValidator<TeamUpdateDto>, TeamUpdateDtoValidator>();
    services.AddScoped<ErrorHandlingMiddleware>();
    services.AddTransient<ILocationRepository, LocationRepository>();
    services.AddTransient<ITeamRepository, TeamRepository>();

    services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    services.AddDbContext<MatchDataManagerDbContext>();
}