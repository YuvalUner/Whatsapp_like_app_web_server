using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using AdvancedProjectWebApi.Hubs;
using Services.DataManipulation.DatabaseContextBasedImplementations;
using Services.DataManipulation.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<Data.AdvancedProgrammingProjectsServerContext>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo {
        Title = "My API",
        Version = "v1"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
        In = ParameterLocation.Header,
        Description = "Please insert JWT with Bearer into field",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
   {
     new OpenApiSecurityScheme
     {
       Reference = new OpenApiReference
       {
         Type = ReferenceType.SecurityScheme,
         Id = "Bearer"
       }
      },
      new string[] { }
    }
  });
});

builder.Services.AddDistributedMemoryCache();


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {

    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters() {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWTBearerParams:Audience"],
        ValidIssuer = builder.Configuration["JWTBearerParams:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWTBearerParams:Key"]))
    };
});

builder.Services.AddCors(options => {
    options.AddPolicy("AllowAll", optionsBuilder => {
        optionsBuilder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().AllowCredentials()
        .WithOrigins(builder.Configuration["AllowedOrigins:React"])
        .WithOrigins(builder.Configuration["AllowedOrigins:WebAPI"])
        .WithOrigins(builder.Configuration["AllowedOrigins:WebServer"]);
    });
});

builder.Services.AddSignalR();
builder.Services.AddScoped<IRegisteredUsersService, DatabaseRegisteredUsersService>();
builder.Services.AddScoped<IPendingUsersService, DatabasePendingUsersService>();
builder.Services.AddScoped<IRefreshTokenService, DatabaseRefreshTokenService>();
builder.Services.AddScoped<IContactsService, DatabaseContactsService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();


app.UseCors("AllowAll");
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.UseEndpoints(endpoints => {
    endpoints.MapHub<ChatAppHub>("/ChatAppHub");
});

app.Run();
