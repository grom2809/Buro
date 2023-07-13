using Bankrupt.Auth.Common;
using Bankrupt.Auth.WebApi.Middlewares;
using Bankrupt.Auth.WebApi.Services;
using Bankrupt.Core.Reps;
using Bankrupt.Core.Services;
using Bankrupt.Data;
using Bankrupt.Data.Reps;
using Bankrupt.Data.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<BankruptDbSettings>(builder.Configuration.GetSection("BankruptDatabase"));

// Add services to the container.
builder.Services.AddControllers();
builder.Services.Configure<AuthOptions>(builder.Configuration.GetSection("Auth"));

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
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRep, UserRep>();

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

app.UseAuthorization();

app.MapControllers();

app.Run();
