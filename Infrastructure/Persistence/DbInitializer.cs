using Microsoft.AspNetCore.Identity;
using Persistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Persistence
{
    public class DbInitializer : IDbInitializer
    {
        private readonly StoreContext _storeContext;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(StoreContext storeContext, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _storeContext = storeContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task InitializerAsync()
        {
            try
            {
                //Create Database if dosen't exist & appling any Pending Migrations
                if (_storeContext.Database.GetPendingMigrations().Any())
                    await _storeContext.Database.MigrateAsync();
                //App Data Seeding
                if (!_storeContext.ProductTypes.Any())
                {
                    //Read types from files as string
                    var typesData = await File.ReadAllTextAsync(@"..\Infrastructure\\Persistence\\Data\\Seeding\\types.json");

                    //Transform into C# objects
                    var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);

                    //Add To Db  & save Change
                    if (types != null && types.Any())
                    {
                        await _storeContext.ProductTypes.AddRangeAsync(types);
                        await _storeContext.SaveChangesAsync();

                    }
                }
                if (!_storeContext.ProductBrands.Any())
                {
                    //read types from files
                    var typesBrand = await File.ReadAllTextAsync(@"..\Infrastructure\\Persistence\\Data\\Seeding\\brands.json");

                    //Transform into C# objects
                    var brands = JsonSerializer.Deserialize<List<ProductBrand>>(typesBrand);

                    //Add To Db  & save Change
                    if (brands != null && brands.Any())
                    {
                        await _storeContext.ProductBrands.AddRangeAsync(brands);
                        await _storeContext.SaveChangesAsync();

                    }
                }
                if (!_storeContext.Products.Any())
                {
                    //read types from files
                    var productData = await File.ReadAllTextAsync(@"..\Infrastructure\Persistence\Data\Seeding\products.json");

                    //Transform into C# objects
                    var products = JsonSerializer.Deserialize<List<Product>>(productData);

                    //Add To Db  & save Change
                    if (products != null && products.Any())
                    {
                        await _storeContext.Products.AddRangeAsync(products);
                        await _storeContext.SaveChangesAsync();

                    }
                }
            }
            catch (Exception ex)
            {

            }

        }

        public async Task InitializerIdentityAsync()
        {

            //Seed Default Roles
            if (!_roleManager.Roles.Any())
            {
                await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
            }
            //seed Default User
            if (!_userManager.Users.Any())
            {
                var superAdminUser = new User
                {
                    DisplayName = "super Admin User",
                    Email = "superAdminUser@Gmail.com",
                    UserName = "superAdminUser",
                    PhoneNumber = "0112568666",

                };

                var adminUser = new User
                {
                    DisplayName = "Admin User",
                    Email = "AdminUser@Gmail.com",
                    UserName = "AdminUser",
                    PhoneNumber = "0111568766",

                };

                await _userManager.CreateAsync(superAdminUser, "Passw0rd");
                await _userManager.CreateAsync(adminUser, "Passw0rd");

                await _userManager.AddToRoleAsync(superAdminUser, "SuperAdmin");
                await _userManager.AddToRoleAsync(adminUser, "Admin");
            }

        }
    }
 }
