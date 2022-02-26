using Account.DataAccess.IRepository;
using Account.DataAccess.Repository;
using Account.DomainModels.Models;
using Account.PresentationModels.Dtos.Cashier;
using Account.Services;
using Account.WEB.Mapping;
using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMvc()
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CashierForUpsertDto>());

builder.Services.AddControllersWithViews()
                .AddNewtonsoftJson(options =>
                             options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);
builder.Services.AddRazorPages()
    .AddRazorRuntimeCompilation();


builder.Services.AddDbContext<ArmyTechTaskContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

var mappingConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new MappingProfile());
});

builder.Services.AddScoped<ExceptionService>();

builder.Services.AddScoped<LoggerView>();

builder.Services.AddControllers(config =>
{
    config.Filters.Add<ExceptionService>();
    config.Filters.Add<LoggerView>();

});
builder.Services.AddAutoMapper(typeof(Program).Assembly);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Cashier}/{action=Index}/{id?}");

app.Run();
