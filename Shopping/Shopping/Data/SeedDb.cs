using Microsoft.EntityFrameworkCore;
using Shopping.Data.Entities;
using Shopping.Enums;
using Shopping.Helpers;

namespace Shopping.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        private readonly IBlobHelper _blobHelper;

        public SeedDb(DataContext context, IUserHelper userHelper, IBlobHelper blobHelper)
        {
            _context = context;
            _userHelper = userHelper;
            _blobHelper = blobHelper;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckCategoriesAsync();
            await CheckCountriesAsync();
            await CheckRolesAsync();
            await CheckUserAsync("1010", "Wilson", "Loayza", "will@yopmail.com", "322 311 4620", "Calle Luna Calle Sol", "Papa.jpg", UserType.Admin);
            await CheckUserAsync("2020", "Jéssica", "Íñiguez", "jessy@yopmail.com", "322 311 4620", "Calle Luna Calle Sol", "Mama.jpg", UserType.User);
            await CheckUserAsync("3030", "Xavi", "Loayza Íñiguez", "xavi@yopmail.com", "322 311 4620", "Calle Luna Calle Sol", "Xavi.jpg", UserType.User);
            await CheckUserAsync("4040", "Liam", "Loayza Íñiguez", "liam@yopmail.com", "322 311 4620", "Calle Luna Calle Sol", "Liam.jpg", UserType.User);
            await CheckProductsAsync();
        }

        private async Task CheckProductsAsync()
        {
            if (!_context.Products.Any())
            {
                await AddProductAsync("Adidas Barracuda", 80M, 12F, new List<string>() { "Calzado", "Deportes" }, new List<string>() { "adridas_dragon.jpg" });
                await AddProductAsync("Adidas Superstar", 100M, 12F, new List<string>() { "Calzado", "Deportes" }, new List<string>() { "adidas_superstar.webp" });
                await AddProductAsync("AirPods", 130M, 12F, new List<string>() { "Tecnología", "Apple" }, new List<string>() { "airpods.png", "airpods2.png" });
                await AddProductAsync("Auriculares Bose", 87M, 12F, new List<string>() { "Tecnología" }, new List<string>() { "auriculares_bose.png" });
                await AddProductAsync("Bicicleta Ribble", 1200M, 6F, new List<string>() { "Deportes" }, new List<string>() { "bicicleta_ribble.png" });
                await AddProductAsync("Camiseta Quiksilver", 56M, 24F, new List<string>() { "Ropa" }, new List<string>() { "camiseta_quiksilver.png" });
                await AddProductAsync("Casco Bicicleta", 20M, 12F, new List<string>() { "Deportes" }, new List<string>() { "casco_bici.jpg", "casco.png" });
                await AddProductAsync("iPad", 450M, 6F, new List<string>() { "Tecnología", "Apple" }, new List<string>() { "ipad.png" });
                await AddProductAsync("iPhone 13", 900M, 6F, new List<string>() { "Tecnología", "Apple" }, new List<string>() { "iphone13.webp", "iphone13b.webp", "iphone13c.webp", "iphone13d.png" });
                await AddProductAsync("Mac Book Pro", 1300M, 6F, new List<string>() { "Tecnología", "Apple" }, new List<string>() { "mac_book_pro.jpg" });
                await AddProductAsync("Mancuernas", 35M, 12F, new List<string>() { "Deportes" }, new List<string>() { "mancuernas.png" });
                await AddProductAsync("Mascarilla Cara", 8M, 100F, new List<string>() { "Belleza" }, new List<string>() { "mascarilla_cara.webp" });
                await AddProductAsync("New Balance 530", 120M, 12F, new List<string>() { "Calzado", "Deportes" }, new List<string>() { "new_balance_530.png" });
                await AddProductAsync("New Balance 565", 130M, 12F, new List<string>() { "Calzado", "Deportes" }, new List<string>() { "new-balance-565.webp" });
                await AddProductAsync("Nike Air", 150M, 12F, new List<string>() { "Calzado", "Deportes" }, new List<string>() { "nike_air_max.png" });
                await AddProductAsync("Nike Zoom", 125M, 12F, new List<string>() { "Calzado", "Deportes" }, new List<string>() { "nike_zoom.webp" });
                await AddProductAsync("Camiseta FC Barcelona 22/23", 80M, 12F, new List<string>() { "Ropa", "Deportes" }, new List<string>() { "camiseta_barcelona.png" });
                await AddProductAsync("Suplemento Boots Original", 50M, 12F, new List<string>() { "Nutrición" }, new List<string>() { "suplemento_boost.jpg" });
                await AddProductAsync("Whey Protein", 80M, 12F, new List<string>() { "Nutrición" }, new List<string>() { "whey_protein.png" });
                await AddProductAsync("Arnes Mascota", 25M, 12F, new List<string>() { "Mascotas" }, new List<string>() { "arnes_mascota.webp" });
                await AddProductAsync("Cama Mascota", 20M, 12F, new List<string>() { "Mascotas" }, new List<string>() { "cama-perro.png" });
                await AddProductAsync("Teclado Gamer", 55M, 12F, new List<string>() { "Gamer", "Tecnología" }, new List<string>() { "teclado_gamer.webp" });
                await AddProductAsync("Silla Gamer", 110M, 12F, new List<string>() { "Gamer", "Tecnología" }, new List<string>() { "silla_gamer.png" });
                await AddProductAsync("Mouse Gamer", 60M, 12F, new List<string>() { "Gamer", "Tecnología" }, new List<string>() { "mouse_gamer.webp" });
                await _context.SaveChangesAsync();
            }
        }

        private async Task AddProductAsync(string name, decimal price, float stock, List<string> categories, List<string> images)
        {
            Product prodcut = new()
            {
                Description = name,
                Name = name,
                Price = price,
                Stock = stock,
                ProductCategories = new List<ProductCategory>(),
                ProductImages = new List<ProductImage>()
            };

            foreach (string? category in categories)
            {
                prodcut.ProductCategories.Add(new ProductCategory { Category = await _context.Categories.FirstOrDefaultAsync(c => c.Name == category) });
            }


            foreach (string? image in images)
            {
                Guid imageId = await _blobHelper.UploadBlobAsync($"{Environment.CurrentDirectory}\\wwwroot\\images\\products\\{image}", "products");
                prodcut.ProductImages.Add(new ProductImage { ImageId = imageId });
            }

            _context.Products.Add(prodcut);
        }


        private async Task<User> CheckUserAsync(
        string document,
        string firstName,
        string lastName,
        string email,
        string phone,
        string address,
        string image,
        UserType userType)
        {
            User user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                Guid imageId = await _blobHelper.UploadBlobAsync($"{Environment.CurrentDirectory}\\wwwroot\\images\\users\\{image}", "users");
                user = new User
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    UserName = email,
                    PhoneNumber = phone,
                    Address = address,
                    Document = document,
                    City = _context.Cities.FirstOrDefault(),
                    UserType = userType,
                    ImageId = imageId
                };

                await _userHelper.AddUserAsync(user, "123456");
                await _userHelper.AddUserToRoleAsync(user, userType.ToString());

                string token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                await _userHelper.ConfirmEmailAsync(user, token);
            }

            return user;
        }


        private async Task CheckRolesAsync()
        {
            await _userHelper.CheckRoleAsync(UserType.Admin.ToString());
            await _userHelper.CheckRoleAsync(UserType.User.ToString());
        }

        private async Task CheckCountriesAsync()
        {
            if (!_context.Countries.Any())
            {
                _context.Countries.Add(new Country
                {
                    Name = "Colombia",
                    States = new List<State>()
            {
                new State()
                {
                    Name = "Antioquia",
                    Cities = new List<City>() {
                        new City() { Name = "Medellín" },
                        new City() { Name = "Itagüí" },
                        new City() { Name = "Envigado" },
                        new City() { Name = "Bello" },
                        new City() { Name = "Sabaneta" },
                        new City() { Name = "La Ceja" },
                        new City() { Name = "La Union" },
                        new City() { Name = "La Estrella" },
                        new City() { Name = "Copacabana" },
                    }
                },
                new State()
                {
                    Name = "Bogotá",
                    Cities = new List<City>() {
                        new City() { Name = "Usaquen" },
                        new City() { Name = "Champinero" },
                        new City() { Name = "Santa fe" },
                        new City() { Name = "Usme" },
                        new City() { Name = "Bosa" },
                    }
                },
                new State()
                {
                    Name = "Valle",
                    Cities = new List<City>() {
                        new City() { Name = "Calí" },
                        new City() { Name = "Jumbo" },
                        new City() { Name = "Jamundí" },
                        new City() { Name = "Chipichape" },
                        new City() { Name = "Buenaventura" },
                        new City() { Name = "Cartago" },
                        new City() { Name = "Buga" },
                        new City() { Name = "Palmira" },
                    }
                },
                new State()
                {
                    Name = "Santander",
                    Cities = new List<City>() {
                        new City() { Name = "Bucaramanga" },
                        new City() { Name = "Málaga" },
                        new City() { Name = "Barrancabermeja" },
                        new City() { Name = "Rionegro" },
                        new City() { Name = "Barichara" },
                        new City() { Name = "Zapatoca" },
                    }
                },
            }
                });
                _context.Countries.Add(new Country
                {
                    Name = "Estados Unidos",
                    States = new List<State>()
            {
                new State()
                {
                    Name = "Florida",
                    Cities = new List<City>() {
                        new City() { Name = "Orlando" },
                        new City() { Name = "Miami" },
                        new City() { Name = "Tampa" },
                        new City() { Name = "Fort Lauderdale" },
                        new City() { Name = "Key West" },
                    }
                },
                new State()
                {
                    Name = "Texas",
                    Cities = new List<City>() {
                        new City() { Name = "Houston" },
                        new City() { Name = "San Antonio" },
                        new City() { Name = "Dallas" },
                        new City() { Name = "Austin" },
                        new City() { Name = "El Paso" },
                    }
                },
                new State()
                {
                    Name = "California",
                    Cities = new List<City>() {
                        new City() { Name = "Los Angeles" },
                        new City() { Name = "San Francisco" },
                        new City() { Name = "San Diego" },
                        new City() { Name = "San Bruno" },
                        new City() { Name = "Sacramento" },
                        new City() { Name = "Fresno" },
                    }
                },
            }
                });
                _context.Countries.Add(new Country
                {
                    Name = "Ecuador",
                    States = new List<State>()
            {
                new State()
                {
                    Name = "El Oro",
                    Cities = new List<City>()
                    {
                        new City() { Name = "Machala" },
                        new City() { Name = "Piñas" },
                        new City() { Name = "Zaruma" },
                        new City() { Name = "Pasaje" },
                        new City() { Name = "Santa Rosa" },
                    }
                },
                new State() {
                    Name = "Guayas",
                    Cities = new List<City>()
                    {
                        new City() { Name = "Guayaquil" },
                        new City() { Name = "Milagro" },
                        new City() { Name = "Playas" },
                        new City() { Name = "Naranjal" },
                        new City() { Name = "Balao" },
                    }
                },
                new State()
                {
                    Name = "Pichincha",
                    Cities = new List<City>() {
                        new City() { Name = "Quito" },
                    }
                },
                new State()
                {
                    Name = "Esmeraldas",
                    Cities = new List<City>() {
                        new City() { Name = "Esmeraldas" },
                    }
                },
            }
                });
            }

            await _context.SaveChangesAsync();
        }

        private async Task CheckCategoriesAsync()
        {
            if (!_context.Categories.Any())
            {
                _context.Categories.Add(new Category { Name = "Tecnología" });
                _context.Categories.Add(new Category { Name = "Ropa" });
                _context.Categories.Add(new Category { Name = "Gamer" });
                _context.Categories.Add(new Category { Name = "Calzado" });
                _context.Categories.Add(new Category { Name = "Belleza" });
                _context.Categories.Add(new Category { Name = "Nutrición" });
                _context.Categories.Add(new Category { Name = "Deportes" });
                _context.Categories.Add(new Category { Name = "Apple" });
                _context.Categories.Add(new Category { Name = "Mascotas" });
                await _context.SaveChangesAsync();
            }
        }
    }
}
