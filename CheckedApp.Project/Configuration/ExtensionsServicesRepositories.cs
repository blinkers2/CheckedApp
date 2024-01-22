﻿using CheckedAppProject.DATA.DbServices.Repository;
using CheckedAppProject.LOGIC.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace CheckedAppProject.API.Configuration
{
    public static class ExtensionsServicesRepositories
    {
        public static void AddAppServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserItemService, UserItemService>();
            services.AddScoped<IItemListService, ItemListService>();
            services.AddScoped<IItemService, ItemService>();
        }

        public static void AddAppRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserItemRepository, UserItemRepository>();
            services.AddScoped<IItemListRepository, ItemListRepository>();
            services.AddScoped<IItemRepository, ItemRepository>();

        }
    }
}
