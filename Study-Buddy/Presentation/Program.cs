using Application.Interfaces.IRepositories;
using Application.Interfaces.IServices;
using Application.Interfaces.UOW;
using Application.Services;
using Domain.Models;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace Presentation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Register Application Context with lazy loading
            builder.Services.AddDbContext<ApplicationContext>(options =>
            {
                options.UseLazyLoadingProxies()
                       .UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            // Register Identity Service
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireUppercase = false;
            }).AddEntityFrameworkStores<ApplicationContext>();

            // Register Repositories (Data Stores)
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped<IRoomRepository, RoomRepository>();
            builder.Services.AddScoped<ITopicRepository, TopicRepository>();
            builder.Services.AddScoped<IMessageRepository, MessageRepository>();

            // Register Services (Application Specific Logic)
            builder.Services.AddScoped(typeof(IGenericService<>), typeof(GenericService<>));
            builder.Services.AddScoped<IRoomService, RoomService>();
            builder.Services.AddScoped<ITopicService, TopicService>();
            builder.Services.AddScoped<IMessageService, MessageService>();


            // Register UnitOfWork
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Register AutoMapper
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


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
                pattern: "{controller=Room}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
