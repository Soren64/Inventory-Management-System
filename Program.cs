using StockManager.Services;
using StockManager.Models;
using StockManager.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Collections.Generic;

var options = new DbContextOptionsBuilder<StockContext>()
    .UseSqlite("Data Source=stockmanager.db")
    .Options;

var context = new StockContext(options);
var productService = new ProductService(context);

bool running = true;

while (running)
{
    Console.Clear();

    Console.WriteLine("=== Stock Manager Menu ===");
    Console.WriteLine("1. Add Product");
    Console.WriteLine("2. List Products");
    Console.WriteLine("3. Update Product");
    Console.WriteLine("4. Delete Product");
    Console.WriteLine("5. Exit");
    Console.Write("Select an option: ");

    string choice = Console.ReadLine();
    bool goBack = false;
    
    switch (choice)
    {
        case "1":
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("1. (ADD)");

            string name;
            string category;
            decimal price;
            int? quantity = null;  // Nullable


            // Poll Name 
            Console.Write("Enter Product Name (leave blank to go back): ");
            name = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(name)) break; // Go back

            // Poll Category 
            Console.Write("Enter Product Category: ");
            category = Console.ReadLine();

            // Poll Price 
            while (true)
            {
                Console.Write("Enter Product Price: ");
                string input = Console.ReadLine();

                if (decimal.TryParse(input, out price))
                    break;
                else
                    Console.WriteLine("Invalid input. Please enter a valid price.");
            }

            // Poll Quantity (optional)
            while (true)
            {
                Console.Write("Enter Quantity (or leave blank for services): ");
                string input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                {
                    quantity = null;
                    break;
                }

                if (int.TryParse(input, out int qty))
                {
                    quantity = qty;
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid number.");
                }
            }

            // Create Product and Add
            var product = new Product
            {
                Name = name,
                Category = category,
                Price = price,
                Quantity = quantity
            };

            productService.AddProduct(product);
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
            break;


        // ----------
        case "2":
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("2. (LIST)");
            
            var products = productService.GetAllProducts();

            Console.WriteLine($"Current Products ({products.Count} Available):");
            if (!products.Any())
            {
                Console.WriteLine("None available.");
            }
            else
            {
                // Print header
                Console.WriteLine("--------------------");
                Console.WriteLine($"{"ID",-5} {"Name",-20} {"Category",-15} {"Price",-10} {"Qty",-5}");

                // Print products
                foreach (var p in products)
                {
                    //Console.WriteLine($"{p.Id}: {p.Name} | {p.Category} | ${p.Price} | Quantity: {p.Quantity}");
                    Console.WriteLine("{0,-5} {1,-20} {2,-15:C} {3,-10} {4,-5}", 
                        p.Id, 
                        p.Name, 
                        p.Category, 
                        p.Price,
                        p.Quantity);
                }
            }
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
            break;


        // ----------
        case "3":
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("3. (UPDATE)");

            int idToUpdate = 0;
            goBack = false;

            // Poll Id
            while (true)
            {
                Console.Write("Enter Product ID to update (leave blank to go back): ");
                string idInput = Console.ReadLine();

                // Blank = go back
                if (string.IsNullOrWhiteSpace(idInput))
                {
                    goBack = true;
                    break;
                }

                if (int.TryParse(idInput, out idToUpdate))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid number.");
                }
            }

            // Exit 
            if (goBack) break;



            // Fetch product
            var existing = productService.GetAllProducts().FirstOrDefault(p => p.Id == idToUpdate);

            if (existing == null)
            {
                Console.WriteLine("Product not found.");
                break;
            }

            // Update Values (Leave blank to keep existing value)
            Console.WriteLine("Enter new value or leave blank to keep existing.");

            // Name
            Console.Write($"Enter new name (current: {existing.Name}): ");
            string newName = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newName))
                existing.Name = newName;

            // Category
            Console.Write($"Enter new category (current: {existing.Category}): ");
            string newCategory = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newCategory))
                existing.Category = newCategory;

            // Poll price
            while (true)
            {
                Console.Write($"Enter new price (current: {existing.Price}): ");
                string priceInput = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(priceInput)) break;

                if (decimal.TryParse(priceInput, out decimal newPrice))
                {
                    existing.Price = newPrice;
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid price.");
                }
            }

            // Poll Quantity
            while (true)
            {
                Console.Write($"Enter new quantity (current: {existing.Quantity?.ToString() ?? "N/A"}): ");
                string input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                    break;

                if (int.TryParse(input, out int newQty))
                {
                    existing.Quantity = newQty;
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid number.");
                }
            }

            productService.UpdateProduct(existing); //Update
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
            break;


        // ----------
        case "4":
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("4. (DELETE)");
            Console.WriteLine("Please give the Id number of the product you wish to delete.");

            int idToDelete = 0;
            goBack = false;

            // Poll
            while (true)
            {
                Console.Write("Enter Product ID (leave blank to go back): ");
                string idInput = Console.ReadLine();

                // Check if user wants to go back
                if (string.IsNullOrWhiteSpace(idInput))
                {
                    goBack = true;
                    break;
                }

                // Try to parse the input as an integer
                if (int.TryParse(idInput, out idToDelete))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid number.");
                }
            }
            // Exit
            if (goBack) break;

            productService.DeleteProduct(idToDelete); // Delete
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
            break;


        // ----------
        case "5":
            // Quit
            Console.WriteLine();
            Console.WriteLine("Exiting Program. Goodbye!");
            running = false;
            break;

        default:
            Console.WriteLine("Invalid option. Try again.");
            Console.ReadLine();
            break;
    }

    Console.WriteLine();
}