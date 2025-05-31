using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Company.CRUD.MVC.BLL;
using Company.CRUD.MVC.BLL.Interfaces;
using Company.CRUD.MVC.BLL.Repositories;
using Company.CRUD.MVC.DAL.Data.Contexts;
using Company.CRUD.MVC.DAL.Models;
using Company.CRUD.MVC.PL.Mapping;
using Company.CRUD.MVC.PL.Services;
using System.Reflection;

namespace Company.CRUD.MVC.PL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            //To Add Dependency Injection Service we use 
            //builder.Services.AddTransient
            //builder.Services.AddSingleton
            //builder.Services.AddScoped<AppDbContext>(); //Allow DI For AppDbContext

            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });



            //builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>(); //Allow DI For DepartmentRepository
            //builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>(); //Allow DI For EmployeeRepository

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            //builder.Services.AddAutoMapper(typeof(EmployeeProfile));
            builder.Services.AddAutoMapper(M => M.AddProfile(new EmployeeProfile()));
            //builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());


            // Life Time 
            //builder.Services.AddScoped();     // Life Time Per Request , Object UnReachable
            //builder.Services.AddTransient();  // Life Time Per Operations 
            //builder.Services.AddSingleton();  // Life Time Per Application




            builder.Services.AddScoped<IScopedService,ScopedService>();          // Per Request
            builder.Services.AddTransient<ITransientService,TransientService>(); // Per Operations 
            builder.Services.AddSingleton<ISingletonService,SingletonService>(); // Per Application


            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                            .AddEntityFrameworkStores<AppDbContext>()
                            .AddDefaultTokenProviders();

            builder.Services.ConfigureApplicationCookie(config => {

                config.LoginPath = "/Account/SignIn"; 
            }); 


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

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
