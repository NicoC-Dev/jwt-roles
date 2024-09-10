using System.Text;
using jwtRoles.Data;
using jwtRoles.Services;
using jwtRoles.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

//Agrego los controladores
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Registrar AuthService
builder.Services.AddScoped<AuthService>();


//Conexion a la base de datos
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

//AutoMapper.
builder.Services.AddAutoMapper(typeof(Program));

//JWT
var key = builder.Configuration.GetValue<string>("Jwt:key");
var keyBytes = Encoding.ASCII.GetBytes(key!);

//Configuramos el esquema de autenticación para usar JwtBearer, que es el tipo de autenticación para tokens JWT.
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    
})
    .AddJwtBearer(options =>
    {
        
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true, //Verifica que el emisor del token coincida con el valor esperado.
            ValidateAudience = true, //Asegura que el token esté destinado a la audiencia correcta.
            ValidateLifetime = true, // Se asegura de que el token no haya expirado.
            ValidateIssuerSigningKey = true, //Verifica que el token esté firmado con la clave correcta.
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            //Es la clave secreta utilizada para firmar y validar los tokens. Debes mantenerla segura 
            //y nunca incluirla en el código fuente sin protección.
            IssuerSigningKey = new SymmetricSecurityKey(keyBytes) 
        };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("admin"));
    options.AddPolicy("UserPolicy", policy => policy.RequireRole("cliente", "admin"));
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.UseHttpsRedirection();

app.UseAuthentication(); // 
app.UseAuthorization(); 

app.MapControllers();

app.Run();

