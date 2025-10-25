using Data.Interface;
using Data.Repository;
using Logic;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add inyection dependencies

builder.Services.AddDependencies();

builder.Services.AddScoped<IAdmin, AdminRepository>();
builder.Services.AddScoped<AdminBL>();

builder.Services.AddScoped<IConsultories, ConsultoryRepository>();
builder.Services.AddScoped<ConsultoryBL>();

builder.Services.AddScoped<IService, ServiceRepository>();
builder.Services.AddScoped<ServiceBL>();

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
