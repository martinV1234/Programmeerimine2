using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Data
{
    public static class SeedData
    {
        public static async Task InitializeAsync(
            ApplicationDbContext context)
        {
            // Kui algandmed on juba lisatud, ära lisa neid uuesti.
            if (await context.Users.AnyAsync())
            {
                return;
            }

            // Ensure LINQ is available for Select overloads

            // 1. AUTOTOOTJAD – 35
            string[] manufacturers =
            {
                "Toyota", "Volkswagen", "BMW", "Mercedes-Benz",
                "Audi", "Volvo", "Škoda", "Ford", "Honda", "Nissan",
                "Hyundai", "Kia", "Mazda", "Subaru", "Lexus",
                "Peugeot", "Renault", "Citroën", "Opel", "Fiat",
                "Tesla", "Porsche", "Land Rover", "Jaguar", "Jeep",
                "Mitsubishi", "Suzuki", "Dacia", "Seat", "Cupra",
                "Mini", "Alfa Romeo", "Chevrolet", "Saab", "BYD"
            };

            var manufacturerEntities = manufacturers
                .Select(name => new CarManufacturer { Name = name })
                .ToList(); 

            context.CarManufacturers.AddRange(manufacturerEntities);
            await context.SaveChangesAsync();

            // 2. AUTOMUDELID – 35
            string[] models =
            {
                "Corolla", "Golf", "3 Series", "C-Class", "A4",
                "XC60", "Octavia", "Focus", "Civic", "Qashqai",
                "Tucson", "Sportage", "CX-5", "Forester", "NX",
                "308", "Clio", "C4", "Astra", "500",
                "Model 3", "Macan", "Discovery Sport", "F-Pace",
                "Compass", "Outlander", "Vitara", "Duster",
                "Leon", "Formentor", "Cooper", "Giulia",
                "Trax", "9-3", "Atto 3"
            };

            var modelEntities = models
                .Select((name, i) => new CarModel
                {
                    Name = name,
                    CarManufacturerId = manufacturerEntities[i].Id,
                    KmRate = 0.20m + (i % 5) * 0.05m,
                    HourlyRate = 5m + (i % 6) * 2m
                })
                .ToList();

            context.CarModels.AddRange(modelEntities);
            await context.SaveChangesAsync();

            // 3. AUTOD – 35
            var carEntities = Enumerable.Range(0, 35)
                .Select(i => new Car
                {
                    RegistrationNo = $"ABC-{100 + i}",
                    CarModelId = modelEntities[i].Id
                })
                .ToList();

            context.Cars.AddRange(carEntities);
            await context.SaveChangesAsync();

            // 4. KASUTAJAD – 35
            var userEntities = Enumerable.Range(0, 35)
                .Select(i => new User
                {
                    Name = $"Klient {i + 1:00}",
                    Phone = $"+372555{i:00000}",
                    Email = $"klient{i + 1:00}@example.com",
                    IsAdmin = i == 0,
                    Password = "DemoPassword123!"
                })
                .ToList();

            context.Users.AddRange(userEntities);
            await context.SaveChangesAsync();

            // 5. BRONEERINGUD – 35
            var startDate = new DateTime(2026, 1, 1);

            var bookingEntities = Enumerable.Range(0, 35)
                .Select(i =>
                {
                    decimal startKm = 12000m + i * 850m;
                    decimal kmRate = 0.20m + (i % 5) * 0.05m;
                    decimal hourlyRate = 5m + (i % 6) * 2m;

                    return new Booking
                    {
                        Started = startDate.AddDays(i),
                        Finished = startDate.AddDays(i).AddHours(4),
                        StartKm = startKm,
                        FinishKm = startKm + 120m + i,
                        KmRate = kmRate,
                        HourlyRate = hourlyRate,
                        CarId = carEntities[i].Id,
                        UserId = userEntities[i].Id
                    };
                })
                .ToList();

            context.Bookings.AddRange(bookingEntities);
            await context.SaveChangesAsync();

            // 6. ARVED – 35
            var invoiceEntities = Enumerable.Range(0, 35)
                .Select(i =>
                {
                    var invoiceDate = startDate.AddDays(i + 1);
                    return new Invoice
                    {
                        InvoiceDate = invoiceDate,
                        DueDate = invoiceDate.AddDays(14),
                        UserId = userEntities[i].Id,
                        IsPaid = i % 2 == 0
                    };
                })
                .ToList();

            context.Invoices.AddRange(invoiceEntities);
            await context.SaveChangesAsync();

            // 7. ARVEREAD – 35
            var invoiceLineEntities = Enumerable.Range(0, 35)
                .Select(i =>
                {
                    decimal distance = 120m + i;
                    decimal kmRate = 0.20m + (i % 5) * 0.05m;
                    decimal price = Math.Round(distance * kmRate, 2);

                    return new InvoiceLine
                    {
                        LineItem = "Autorendi läbisõit",
                        Price = price,
                        Unit = "km",
                        Quantity = 1m,
                        Total = price,
                        BookingId = bookingEntities[i].Id
                    };
                })
                .ToList();

            context.InvoiceLines.AddRange(invoiceLineEntities);
            await context.SaveChangesAsync();
        }
    }
}