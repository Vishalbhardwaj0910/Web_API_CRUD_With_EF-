using ASPNET6_WebAPI.Models;
using Microsoft.EntityFrameworkCore;
namespace ASPNET6_WebAPI.Data
{
  public class ContactsDbContext : DbContext
  {
    public ContactsDbContext(DbContextOptions options) : base(options)
    {
      
    }
    public DbSet<Contact> Contacts { get; set; }
  }
}