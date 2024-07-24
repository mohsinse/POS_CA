// See https://aka.ms/new-console-template for more information
using POS_CA.Data;
using POS_CA.Entities;
using POS_CA.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace POS_CA
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var host = CreateHostBuilder().Build();
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                UserServices userServices = services.GetRequiredService<UserServices>();
                ProductManagementServices productManagementServices = services.GetRequiredService<ProductManagementServices>();
                SaleServices saleServices = services.GetRequiredService<SaleServices>();
                CategoryServices categoryServices = services.GetRequiredService<CategoryServices>();



                User admin = new User("Mohsin Ali", "ali", "1", UserRole.Admin);
                await userServices.RegisterUser(admin);

                User cashier = new User("Hassan Ali", "hassan@gmail.com", "54321", UserRole.Cashier);
                await userServices.RegisterUser(cashier);

                var context = services.GetRequiredService<DataContext>();

                Category healthCategory = new Category { Name = "Health" };
                await categoryServices.AddCategory(healthCategory, admin);

                //Console.WriteLine("Select Category:");
                //for (int i = 0; i < context.Categories.Count(); i++)
                //{
                //    Console.WriteLine($"{i + 1}. {context.Categories.ToList()[i].Name}");
                //}

                // Add products
                var product1 = new Product { Name = "Laptop", Price = 1200, Quantity = 10, Type = "Gadget", Category = healthCategory };
                var product2 = new Product { Name = "Smartphone", Price = 800, Quantity = 20, Type = "Gadget", Category = healthCategory };
                await productManagementServices.AddProduct(product1, admin);
                await productManagementServices.AddProduct(product2, admin);





                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("=================================");
                    Console.WriteLine("        POS System Main Menu     ");
                    Console.WriteLine("=================================");
                    Console.WriteLine("1. Register User");
                    Console.WriteLine("2. Login");
                    Console.WriteLine("3. Exit");
                    Console.Write("Select an option: ");
                    var option = Console.ReadLine();

                    if (option == "1")
                    {
                        Console.Write("Name: ");
                        var name = Console.ReadLine();
                        Console.Write("Email: ");
                        var email = Console.ReadLine();
                        Console.Write("Password: ");
                        var password = Console.ReadLine();

                        var newUser = new User(name, email, password, UserRole.Cashier); // Default role is cashier
                        userServices.RegisterUser(newUser);
                        context.SaveChanges();
                        Console.WriteLine("User registered successfully. Please contact admin to assign role.");
                        Console.ReadLine();
                    }
                    else if (option == "2")
                    {
                        Console.Write("Email: ");
                        var email = Console.ReadLine();
                        Console.Write("Password: ");
                        var password = Console.ReadLine();

                        var loggedInUser = userServices.AuthenticateUser(email, password);

                        if (loggedInUser != null)
                        {
                            if (loggedInUser.UserRole == UserRole.Admin)
                            {
                                //await AdminMenuAsync(loggedInUser, productService, userService, context);

                                while (true)
                                {
                                    Console.Clear();
                                    Console.WriteLine("=================================");
                                    Console.WriteLine("          Admin Menu             ");
                                    Console.WriteLine("=================================");
                                    Console.WriteLine("1. Add Product");
                                    Console.WriteLine("2. View Products");
                                    Console.WriteLine("3. Update Product");
                                    Console.WriteLine("4. Remove Product");
                                    Console.WriteLine("5. Assign User Role");
                                    Console.WriteLine("6. View Sales");
                                    Console.WriteLine("7. Logout");
                                    Console.Write("Select an option: ");
                                    var adminOption = Console.ReadLine();

                                    if (adminOption == "1")
                                    {
                                        Console.Write("Product Name: ");
                                        var name = Console.ReadLine();
                                        Console.Write("Price: ");
                                        var price = double.Parse(Console.ReadLine());
                                        Console.Write("Quantity: ");
                                        var quantity = int.Parse(Console.ReadLine());
                                        Console.Write("Type: ");
                                        var type = Console.ReadLine();

                                        Console.WriteLine("Select Category:");
                                        for (int i = 0; i < context.Categories.Count(); i++)
                                        {
                                            Console.WriteLine($"{i + 1}. {context.Categories.ToList()[i].Name}");
                                        }
                                        var categoryIndex = int.Parse(Console.ReadLine()) - 1;
                                        var category = context.Categories.ToList()[categoryIndex];

                                        Product product = new Product { Name = name, Price = price, Quantity = quantity, Type = type, Category = category };
                                        await productManagementServices.AddProduct(product, admin);
                                        Console.WriteLine("Product added successfully.");
                                        Console.ReadLine();
                                    }
                                    else if (adminOption == "2")
                                    {
                                        Console.Clear();
                                        Console.WriteLine("=================================");
                                        Console.WriteLine("         Product List            ");
                                        Console.WriteLine("=================================");
                                        foreach (var product in context.Products.Include(p => p.Category).ToList())
                                        {
                                            Console.WriteLine($"Name: {product.Name}, Price: {product.Price}, Quantity: {product.Quantity}, Type: {product.Type}, Category: {product.Category.Name}");
                                            Console.WriteLine("---------------------------------");
                                        }
                                        Console.ReadLine();
                                    }
                                    else if (adminOption == "3")
                                    {
                                        Console.Write("Enter product name to update: ");
                                        var name = Console.ReadLine();
                                        var product = context.Products.Include(p => p.Category).FirstOrDefault(p => p.Name == name);
                                        if (product != null)
                                        {
                                            Console.Write("New Price: ");
                                            var price = double.Parse(Console.ReadLine());
                                            Console.Write("New Quantity: ");
                                            var quantity = int.Parse(Console.ReadLine());
                                            Console.Write("New Type: ");
                                            var type = Console.ReadLine();

                                            Console.WriteLine("Select New Category:");
                                            for (int i = 0; i < context.Categories.Count(); i++)
                                            {
                                                Console.WriteLine($"{i + 1}. {context.Categories.ToList()[i].Name}");
                                            }
                                            var categoryIndex = int.Parse(Console.ReadLine()) - 1;
                                            var category = context.Categories.ToList()[categoryIndex];

                                            Product updatedProduct = new Product { Name = name, Price = price, Quantity = quantity, Type = type, Category = category };
                                            await productManagementServices.UpdateProduct(updatedProduct, admin);

                                            Console.WriteLine("Product updated successfully.");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Product not found.");
                                        }
                                        Console.ReadLine();
                                    }
                                    else if (adminOption == "4")
                                    {
                                        Console.Write("Enter product name to remove: ");
                                        var name = Console.ReadLine();
                                        var product = context.Products.FirstOrDefault(p => p.Name == name);
                                        if (product != null)
                                        {
                                            productManagementServices.RemoveProduct(name, admin);
                                            context.SaveChanges();
                                            Console.WriteLine("Product removed successfully.");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Product not found.");
                                        }
                                        Console.ReadLine();
                                    }
                                    else if (adminOption == "5")
                                    {
                                        Console.Write("Enter user email to assign role: ");
                                        var userEmail = Console.ReadLine();
                                        var user = context.Users.FirstOrDefault(u => u.Email == userEmail);
                                        if (user != null)
                                        {
                                            Console.WriteLine("Select Role:");
                                            Console.WriteLine("1. Admin");
                                            Console.WriteLine("2. Cashier");
                                            var role = int.Parse(Console.ReadLine());
                                            userServices.SetUserRole(user, (UserRole)(role - 1));
                                            context.SaveChanges();
                                            Console.WriteLine("Role assigned successfully.");
                                        }
                                        else
                                        {
                                            Console.WriteLine("User not found.");
                                        }
                                        Console.ReadLine();
                                    }
                                    else if (adminOption == "6")
                                    {
                                        Console.Clear();
                                        Console.WriteLine("=================================");
                                        Console.WriteLine("           Sales List            ");
                                        Console.WriteLine("=================================");
                                        foreach (var sale in context.Sales.Include(s => s.SaleProducts).ToList())
                                        {
                                            Console.WriteLine($"Sale ID: {sale.Id} : ");
                                            foreach (var saleProduct in sale.SaleProducts)
                                            {
                                                Console.WriteLine($"    Product: {saleProduct.Name}, Quantity: {saleProduct.Quantity}, Price: {saleProduct.Price}");
                                            }
                                            Console.WriteLine("---------------------------------");
                                        }
                                        Console.ReadLine();
                                    }
                                    else if (adminOption == "7")
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        Console.WriteLine("Invalid option. Please try again.");
                                        Console.ReadLine();
                                    }
                                }
                            }

                            else if (loggedInUser.UserRole == UserRole.Cashier)
                            {
                                //await CashierMenuAsync(loggedInUser, saleTransactionService, productService, context);


                                while (true)
                                {
                                    Console.Clear();
                                    Console.WriteLine("-----------------------------------------------");
                                    Console.WriteLine("         Cashier Menu            ");
                                    Console.WriteLine("-----------------------------------------------");
                                    Console.WriteLine("1. View Products");
                                    Console.WriteLine("2. Start a new Sale");
                                    Console.WriteLine("3. Logout");
                                    Console.Write("Select an option: ");
                                    var cashierOption = Console.ReadLine();

                                    if (cashierOption == "1")
                                    {

                                        Console.Clear();
                                        Console.WriteLine("--------------------------------------------");
                                        Console.WriteLine("         Product List            ");
                                        Console.WriteLine("--------------------------------------------");
                                        foreach (var product in context.Products.Include(p => p.Category).ToList())
                                        {
                                            Console.WriteLine($"Name: {product.Name}, Price: {product.Price}, Quantity: {product.Quantity}, Type: {product.Type}, Category: {product.Category.Name}");
                                            Console.WriteLine("----------------------------------------");
                                        }
                                     
                                        Console.ReadLine();
                                    }
                                    else if (cashierOption == "2")
                                    {
                                        saleServices.startNewSale();
                                        var currentSale = saleServices.GetCurrentSale();
                                        if (currentSale != null)
                                        {
                                            Console.WriteLine("New sale started.");
                                            bool isSaleContinue = true;
                                            while (isSaleContinue)
                                            {
                                                Console.Write("Enter product name to add: ");
                                                var productName = Console.ReadLine();
                                                var product = context.Products.FirstOrDefault(p => p.Name == productName);
                                                if (product != null)
                                                {
                                                    Console.Write("Enter quantity: ");
                                                    var quantity = int.Parse(Console.ReadLine());

                                                    var success = saleServices.AddProductToSale(product, quantity, cashier);
                                                    if (success)
                                                    {
                                                        Console.WriteLine("Product added to sale.");
                                                    }
                                                    Console.WriteLine("=================================");
                                                    Console.WriteLine("Want to add more products?");
                                                    Console.WriteLine("1. Yes");
                                                    Console.WriteLine("2. No");
                                                    Console.Write("Select an option: ");
                                                    var saleContinueOption = Console.ReadLine();
                                                    if (saleContinueOption == "2")
                                                    {
                                                        Console.WriteLine("=================================");
                                                        Console.Write("Total Amount: ");
                                                        Console.WriteLine(saleServices.GetSaleAmount());
                                                        isSaleContinue = false;
                                                        saleServices.EndCurrentSale();


                                                    }

                                                }
                                                else
                                                {
                                                    Console.WriteLine("Product not found.");
                                                }
                                            }
                                            
                                        }
                                        else
                                        {
                                            Console.WriteLine("No active sale. Please start a new sale first.");
                                        }
                                        Console.ReadLine();
                                    }
                                    else if (cashierOption == "3")
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        Console.WriteLine("Invalid option. Please try again.");
                                        Console.ReadLine();
                                    }
                                }



                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid credentials. Please try again.");
                            Console.ReadLine();
                        }
                    }
                    else if (option == "3")
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid option. Please try again.");
                        Console.ReadLine();
                    }
                }
















    }
        }

        static IHostBuilder CreateHostBuilder() =>
        Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                services.AddDbContext<DataContext>(options =>
                    options.UseInMemoryDatabase("POSSystemDB"));

                services.AddScoped<UserServices>();
                services.AddScoped<ProductManagementServices>();
                services.AddScoped<SaleServices>();
                services.AddScoped<CategoryServices>();
            });
    }
}