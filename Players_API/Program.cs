using Players_API.Models.Repositories;
using Players_API.Models.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
//  Not sure if it should be scoped or singleton as the repository is reading static file and the service is stateless
builder.Services.AddScoped<IPlayerRepository, PlayerJsonRepository>();
builder.Services.AddScoped<IPlayerService, PlayerService>();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();

app.Run();
