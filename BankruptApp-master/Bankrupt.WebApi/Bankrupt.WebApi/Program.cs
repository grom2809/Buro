using Bankrupt.Auth.Common;
using Bankrupt.Core.Reps;
using Bankrupt.Core.Services;
using Bankrupt.Data;
using Bankrupt.Data.Reps;
using Bankrupt.Data.Services;
using Bankrupt.WebApi.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.Configure<BankruptDbSettings>(builder.Configuration.GetSection("BankruptDatabase"));

var authOptions = builder.Configuration.GetSection("Auth").Get<AuthOptions>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
   .AddJwtBearer(options => 
   { 
       options.RequireHttpsMetadata = true;
       options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
       {
           ValidateIssuer = true,
           ValidIssuer = authOptions.Issuer,

           ValidateAudience = true,
           ValidAudience = authOptions.Audience,

           ValidateLifetime = true,

           IssuerSigningKey = authOptions.GetSymmetricSecurityKey(), //HS256
           ValidateIssuerSigningKey = true
       };
   });

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {
            builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        }
        );
});

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IDocumentService, DocumentService>();
builder.Services.AddScoped<IUserRep, UserRep>();
builder.Services.AddScoped<IDocumentRep, DocumentRep>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<FormOptions>(o =>
{
    o.ValueLengthLimit = int.MaxValue;
    o.MultipartBodyLengthLimit = int.MaxValue;
    o.MemoryBufferThreshold = int.MaxValue;
});



var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
