using MagicVilla_API;
using MagicVilla_API.Datos;
using MagicVilla_API.Repositorio;
using MagicVilla_API.Repositorio.IRepositorio;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson(); // AddNewtonsoftJson
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options => {
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Ingresar Bearer [space] tuToken \r\n\r\n " +
                      "Ejemplo: Bearer 123456abcder",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id= "Bearer"
                },
                Scheme = "oauth2",
                Name="Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });

    //Documentación de  Swagger
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Magic Villa v1",
        Description = "API para Villas"
    });
    options.SwaggerDoc("v2", new OpenApiInfo
    {
        Version = "v2",
        Title = "Magic Villa v2",
        Description = "API para Villas"
    });
});




// Authentication
var key = builder.Configuration.GetValue<string>("ApiSettings:Secret");

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(x => {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });


//DbContext
builder.Services.AddDbContext<ApplicationDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnetion"));
});

//Automapper
builder.Services.AddAutoMapper(typeof(MappingConfig));

/// Interface Villa repositorio
builder.Services.AddScoped<IVillaRepositorio, VillaRepositorio>();
/// Interface Numero Villa repositorio
builder.Services.AddScoped<INumeroVillaRepositorio, NumeroVillaRepositorio>();
/// Interface usuario repositorio
builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();

// Add Service Versioning
builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0); // indicamos la versión por Default 
    options.ReportApiVersions = true;    // reportar el numero de versiones
});

// formato que se va aceptar en el numero de las versiones
builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{  
    app.UseSwagger();

    //Soportar versiones
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Magic_VillaV1");
        options.SwaggerEndpoint("/swagger/v2/swagger.json", "Magic_VillaV2");
    });

    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication(); // habilitar  Authentication
app.UseAuthorization();

app.MapControllers();

app.Run();
