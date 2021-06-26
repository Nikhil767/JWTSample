using JWTSample.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace JWTSample.DataContext
{
    public class SampleDbContext : DbContext
    {
        public SampleDbContext(DbContextOptions<SampleDbContext> options)
        : base(options)
        {
            InitializeData();
        }

        private void InitializeData()
        {
            Users.AddRange(
                new User
                {
                    ID = Guid.NewGuid().ToString(),
                    UserName = "test",
                    Password = "test",
                    Role = "user",
                    UserDetail = new UserDetail
                    {
                        ID = Guid.NewGuid().ToString(),
                        DOB = DateTime.Now.AddYears(20),
                        Email = "test@gmail.com",
                        FirstName = "testing",
                        LastName = "Demo"
                    }
                },
                new User
                {
                    ID = Guid.NewGuid().ToString(),
                    UserName = "admin",
                    Password = "admin",
                    Role = "Admin",
                    UserDetail = new UserDetail
                    {
                        ID = Guid.NewGuid().ToString(),
                        DOB = DateTime.Now.AddYears(25),
                        Email = "admin@gmail.com",
                        FirstName = "Admin",
                        LastName = "Admin"
                    }
                },
                new User
                {
                    ID = Guid.NewGuid().ToString(),
                    UserName = "user",
                    Password = "user",
                    Role = "user",
                    UserDetail = new UserDetail
                    {
                        ID = Guid.NewGuid().ToString(),
                        DOB = DateTime.Now.AddYears(22),
                        Email = "user@gmail.com",
                        FirstName = "User",
                        LastName = "User"
                    }
                });
        }

        public DbSet<User> Users { get; set; }
    }
}
