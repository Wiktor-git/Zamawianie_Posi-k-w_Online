using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ZamawianiePosiłkowOnline.Models;
using ZamawianiePosiłkowOnline.ViewModels;

namespace ZamawianiePosiłkowOnline.Data
{
    public class ApplicationDbContext : IdentityDbContext<Users>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Meal> Meals { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderedItems { get; set; }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<ZamawianiePosiłkowOnline.ViewModels.OrderDetailViewModel> OrderDetailViewModel { get; set; } = default!;
    }
}
