using AddressBookHG_BL.EmailSenderProcess;
using AddressBookHG_BL.ImplementationOfManagers;
using AddressBookHG_BL.InterfacesOfManagers;
using AddressBookHG_DL.ContextInfo;
using AddressBookHG_DL.ImplementationsOfRepos;
using AddressBookHG_DL.InterfaceOfRepos;
using AddressBookHG_EL.IdentityModels;
using AddressBookHG_EL.Mappings;
using AddressBookHG_PL.CreateDefaultData;
using AutoMapper.Extensions.ExpressionMapping;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// serilog logger ayarlari
var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

//contexti ayarliyoruz.
builder.Services.AddDbContext<AddressbookContext>(options =>
{
    //klasik mvcde connection string web configte yer alir.
    //core mvcde connection string appsetting.json dosyasindan alinir.
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyLocal"));
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

});

//appuser ve approle identity ayari
builder.Services.AddIdentity<AppUser, AppRole>(opt =>
{
    opt.Password.RequiredLength = 8;
    opt.Password.RequireNonAlphanumeric = true;
    opt.Password.RequireLowercase = true;
    opt.Password.RequireUppercase = true;
    opt.Password.RequireDigit = true;
    opt.User.RequireUniqueEmail = true;
    //opt.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+&%";

}).AddDefaultTokenProviders().AddEntityFrameworkStores<AddressbookContext>();


//automapper ayari 
builder.Services.AddAutoMapper(a =>
{
    a.AddExpressionMapping();
    a.AddProfile(typeof(Maps));
});

//interfacelerin DI yasam dongusu
builder.Services.AddScoped<IEmailManager, EmailManager>();


builder.Services.AddScoped<ICityRepo, CityRepo>();
builder.Services.AddScoped<ICityManager, CityManager>();

builder.Services.AddScoped<IDistrictRepo, DistrictRepo>();
builder.Services.AddScoped<IDistrictManager, DistrictManager>();


builder.Services.AddScoped<INeighborhoodRepo, NeighborhoodRepo>();
builder.Services.AddScoped<INeighborhoodManager, NeighborhoodManager>();


builder.Services.AddScoped<IUserAddressRepo, UserAddressRepo>();
builder.Services.AddScoped<IUserAddressManager, UserAddressManager>();

builder.Services.AddScoped<IUserForgotPasswordsHistoricalRepo, UserForgotPasswordsHistoricalRepo>();
builder.Services.AddScoped<IUserForgotPasswordsHistoricalManager, UserForgotPasswordsHistoricalManager>();

builder.Services.AddScoped<IUserForgotPasswordTokensRepo, UserForgotPasswordTokensRepo>();
builder.Services.AddScoped<IUserForgotPasswordTokensManager, UserForgotPasswordTokensManager>();


// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); //login logout
app.UseAuthorization(); //yetki

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

//Sistem ilk ayaga kalktiginnda rolleri ekleyelim
//ADMIN, MEMBER, WAITINGFORACTIVATION, PASSIVE

using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;

    var roleManager = serviceProvider.
    GetRequiredService<RoleManager<AppRole>>();

    CreateData c = new CreateData(logger);
    
    //c.CreateRoles(serviceProvider);
    c.CreateAllCity(serviceProvider);

    var districtManager = serviceProvider.GetService<IDistrictManager>();
    var cityManager = serviceProvider.GetService<ICityManager>();
    c.CreateAllDistrict(districtManager,cityManager);

    c.CreateAllNeighborhood(serviceProvider);


}


app.Run();