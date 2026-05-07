using System;
using System.IO;
using System.Collections.Generic;
using System.Text.Json;

public class User
{
    public string? name { get; set; }
    public string? email { get; set; }
    public int id { get; set; }
}

public class SimpleUser
{
    public string? Name { get; set; }
    public string? Email { get; set; }
}
class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Henter data fra API...");

        var url = "https://jsonplaceholder.typicode.com/users";

        using var Client = new HttpClient();

        try
        {
            var response = await Client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var users = JsonSerializer.Deserialize<List<User>>(json);

            Console.WriteLine("\nBrukere:\n");


            if (users != null)
            {
                var SimpleUsers = new List<SimpleUser>();

                foreach (var user in users)
                {
                    if (user.email.Contains(".biz"))
                    {
                        Console.WriteLine($"Emails that contains .biz: {user.email}");
                    }

                    SimpleUsers.Add(new SimpleUser
                    {
                        Name = user.name,
                        Email = user.email
                    });
                }

                var outputJson = JsonSerializer.Serialize(SimpleUsers, new JsonSerializerOptions
                {
                    WriteIndented = true
                });

                await File.WriteAllTextAsync("users.json", outputJson);
                
                Console.WriteLine("\nusers.json er lagret");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Feil ved henting av data: {ex.Message}");
        }
    }
}