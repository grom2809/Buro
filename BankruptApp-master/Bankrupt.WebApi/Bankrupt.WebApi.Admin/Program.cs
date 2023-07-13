using Bankrupt.Core.Entities;
using Bankrupt.Data;
using Bankrupt.Data.Reps;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
builder.Services.AddScoped<UserRep>();
builder.Services.Configure<BankruptDbSettings>(builder.Configuration.GetSection("BankruptDatabase"));
using IHost host = builder.Build();
await Run(host.Services);
await host.RunAsync();

static async Task Run(IServiceProvider hostProvider)
{
    using IServiceScope serviceScope = hostProvider.CreateScope();
    IServiceProvider provider = serviceScope.ServiceProvider;
    UserRep rep = provider.GetRequiredService<UserRep>();

    Console.Write("Email пользователя: ");
    var email = Console.ReadLine();
    var user = await rep.GetUser(email);
    Console.WriteLine($"Текущая роль: {Enum.GetName(user.Role)}");

    Console.Write("Новая роль пользователя (a - admin, u - user):");
    var role = Console.ReadLine();
    var newRole = role == "a" ? Role.Admin : Role.User;
    await rep.UpdateUserRole(email, newRole);
    var newUser = await rep.GetUser(email);
    Console.WriteLine($"Для пользователя {email} роль успешно сменена на: {Enum.GetName(newUser.Role)}");
}