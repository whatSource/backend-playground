using System;
using System.IO;
using System.Collections.Generic;
using System.Text.Json;
using System.Reflection;
using System.Linq;

public class Product
{
    public string? title { get; set; }
    public decimal price { get; set; }
    public string? category { get; set; }
}

public class SimpleProduct
{
    public string? Title { get; set; }
    public decimal Price { get; set; }
}
class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Henter produkt data fra API...");

        var url = "https://fakestoreapi.com/products";

        using var client = new HttpClient();

        try
        {
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var products = JsonSerializer.Deserialize<List<Product>>(json);

            Console.WriteLine("\nProdukter:\n");


            if (products != null)
            {
                var simpleProducts = new List<SimpleProduct>();

                foreach (var product in products)
                {
                    if (product.price < 100 || product.category == "electronics")
                    {
                        Console.WriteLine($"Eksporterer produkt: {product.title} - {product.price} kr");

                        simpleProducts.Add(new SimpleProduct
                        {
                            Title = product.title,
                            Price = product.price
                        });
                    }
                }

                simpleProducts = simpleProducts
                    .OrderBy(p => p.Price)
                    .ToList();

                var outputJson = JsonSerializer.Serialize(simpleProducts, new JsonSerializerOptions
                {
                    WriteIndented = true
                });

                await File.WriteAllTextAsync("products.json", outputJson);
                Console.WriteLine("\nproducts.json er lagret");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Feil ved henting av data: {ex.Message}");
        }
    }
}