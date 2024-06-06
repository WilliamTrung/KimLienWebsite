using Microsoft.Extensions.DependencyInjection;
using Models.Entities;
using Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KL_Core
{
    public static class BootstrapperExtension
    {
        public static void EnsureSqlDatabase(this IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var isNewDb = false;
                using (var dbContext = scope.ServiceProvider.GetRequiredService<KimLienContext>())
                {
                    isNewDb = dbContext.Database.EnsureCreated();
                    if(isNewDb)
                    {
                        var guid = Guid.Parse("FBEA9802-B624-42EA-82BF-5FE58A88474D");
                        var user = new User
                        {
                            Id = guid,
                            Password = "123",
                            Role = Role.Administrator,
                        };
                        dbContext.Add(user);
                        dbContext.SaveChanges();
                    } else
                    {
                        var guid = Guid.Parse("FBEA9802-B624-42EA-82BF-5FE58A88474D");
                        var user = dbContext.Find<User>(guid);
                        if(user == null)
                        {
                            user = new User
                            {
                                Id = guid,
                                Password = "123",
                                Role = Role.Administrator,
                            };
                            dbContext.Add(user);
                            dbContext.SaveChanges();
                        }
                    }
                }
            }
        }
    }
}
