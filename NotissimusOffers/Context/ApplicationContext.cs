using NotissimusOffers.Models;
using Microsoft.EntityFrameworkCore;

namespace NotissimusOffers.Context
{
	public class ApplicationContext : DbContext
	{
		public DbSet<Offer> Offers { get; set; }
		public ApplicationContext(DbContextOptions<ApplicationContext> options)
			: base(options)
		{
			Database.EnsureCreated();
		}
	}
}
