using manager_properties_usa.Data;
using manager_properties_usa.Data.interfaces;
using manager_properties_usa.Models.Context;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<RealEstatePropertyContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("RealEstatePropertyDatabase")));

builder.Services.AddTransient<IPropertyData, PropertyData>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(o => AddSwaggerDocumentation(o));

var app = builder.Build();

// For addd comments - swagger.
static void AddSwaggerDocumentation(SwaggerGenOptions o)
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    o.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
}

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