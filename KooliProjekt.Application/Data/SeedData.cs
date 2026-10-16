using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace KooliProjekt.Application.Data
{
    public static class SeedData
    {

        public static void Generate(ApplicationDbContext context)
        {
            if (context.Users.Any())
            {
                return;
            }

            SeedUsers(context);
            SeedTodoLists(context);
            SeedTodoItems(context);
        }

        private static void SeedUsers(ApplicationDbContext context)
        {
            // Generate realistic-looking user names
            var firstNames = new[] { "Marta", "Karl", "Liis", "Juhan", "Mari", "Peeter", "Kristi", "Andres", "Maarja", "Tõnis", "Katrin", "Siim", "Anu", "Rasmus", "Evelin", "Sven", "Kaire", "Priit", "Helen", "Marko", "Triin", "Erik", "Airi", "Rain", "Pille", "Viktor", "Kertu", "Indrek", "Grete", "Taavi", "Liina", "Ott", "Maris", "Jaan", "Heli", "Rainer" };
            var lastNames = new[] { "Tamm", "Kask", "Saar", "Põder", "Mägi", "Vaher", "Rebane", "Põllu", "Lepp", "Koppel", "Karu", "Sipelgas", "Oja", "Kivirand", "Paas", "Ruut", "Siim", "Nurm", "Aun", "Karu", "Jõe", "Lai", "Künnap", "Laur", "Kangur", "Lill", "Eensaar", "Sild", "Peetson", "Kivi", "Uibo", "Kaljurand", "Erikson", "Rannap", "Vanem" };

            var users = new List<User>();
            var usedUsernames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var rand = new Random(42);

            // Create at least 35 users
            for (int i = 0; i < 35; i++)
            {
                var first = firstNames[rand.Next(firstNames.Length)];
                var last = lastNames[rand.Next(lastNames.Length)];
                var baseName = (first + "." + last).ToLower();
                var username = baseName;
                var suffix = 1;
                while (usedUsernames.Contains(username))
                {
                    username = baseName + suffix;
                    suffix++;
                }
                usedUsernames.Add(username);

                var password = "Password!" + (1000 + i);
                var passwordHash = ComputeSha256Hash(password);

                users.Add(new User { UserName = username, PasswordHash = passwordHash });
            }

            context.Users.AddRange(users);
            context.SaveChanges();
        }

        private static void SeedTodoLists(ApplicationDbContext context)
        {
            var titlesStart = new[] { "School", "Home", "Work", "Shopping", "Personal", "Project", "Reading", "Travel", "Fitness", "Chores" };
            var titlesEnd = new[] { "Tasks", "ToDo", "Checklist", "Plans", "Ideas", "Notes", "Goals", "Errands", "Backlog", "Inbox" };

            var users = context.Users.OrderBy(u => u.Id).ToList();
            var todoLists = new List<TodoList>();

            // Ensure at least 35 todo lists, assign them to users round-robin
            var rand = new Random(123);
            for (int i = 0; i < 35; i++)
            {
                var title = titlesStart[rand.Next(titlesStart.Length)] + " " + titlesEnd[rand.Next(titlesEnd.Length)];
                var user = users[i % users.Count];
                todoLists.Add(new TodoList { Title = title + " - " + user.UserName, UserId = user.Id });
            }

            context.TodoLists.AddRange(todoLists);
            context.SaveChanges();
        }

        private static void SeedTodoItems(ApplicationDbContext context)
        {
            var lists = context.TodoLists.OrderBy(l => l.Id).ToList();
            var items = new List<TodoItem>();
            var rand = new Random(999);

            var sampleTasks = new[]
            {
                "Finish math homework",
                "Prepare presentation slides",
                "Buy groceries: milk, bread, eggs",
                "Call the dentist to book appointment",
                "Read chapter 5 of history book",
                "Clean the kitchen",
                "Pay electricity bill",
                "Send project update email",
                "Schedule team meeting",
                "Practice guitar 30 minutes",
                "Write blog post about C#",
                "Review pull requests",
                "Plan weekend trip",
                "Organize documents",
                "Water the plants",
                "Backup laptop files",
                "Update CV",
                "Buy birthday gift",
                "Walk the dog",
                "Finish unit tests for feature"
            };

            // Create multiple items per list so every list looks realistic
            foreach (var list in lists)
            {
                // create 3-6 items per list
                int count = rand.Next(3, 7);
                for (int i = 0; i < count; i++)
                {
                    var title = sampleTasks[rand.Next(sampleTasks.Length)];
                    // make titles slightly unique
                    var titleUnique = title + (rand.NextDouble() < 0.25 ? " (important)" : "") + (rand.NextDouble() < 0.15 ? " - ASAP" : "");
                    var isCompleted = rand.NextDouble() < 0.35;
                    items.Add(new TodoItem { Title = titleUnique, IsCompleted = isCompleted, TodoListId = list.Id });
                }
            }

            // Ensure at least 35 todo items in total (in case lists were fewer)
            if (items.Count < 35)
            {
                var needed = 35 - items.Count;
                var firstListId = lists.First().Id;
                for (int i = 0; i < needed; i++)
                {
                    items.Add(new TodoItem { Title = "Extra task " + (i + 1), IsCompleted = false, TodoListId = firstListId });
                }
            }

            context.TodoItems.AddRange(items);
            context.SaveChanges();
        }

        private static string ComputeSha256Hash(string rawData)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                var sb = new StringBuilder();
                foreach (var b in bytes)
                {
                    sb.Append(b.ToString("x2"));
                }
                return sb.ToString();
            }
        }
    }
}
