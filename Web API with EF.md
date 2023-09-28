# Steps for creating web API using entity framework

## Step 1 : Create a folder inside your solution called "MODELS".

## Step 2 : Inside "MODELS" folder create a class called 'student' with following properties : 

### Contact.cs

using WebAPI.Models
{
  public class student
  {
    public int Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public long Phone { get; set; }
    public string Address { get; set; }
  } 
}

## Step 3 : Now we create a folder called "DATA".
## Step 4 : Inside "DATA" folder create a data context class for our solution which will talk to our database. 
##       -> The class name should be end with "DbContext" i.e. 'ContactDbContext' which will inherit the properties of "DbContext" class :

### ContactDbContext.cs

using Microsoft
using WebAPI.Models;
using Microsoft.EntityFrameworkCore;
namespace WebAPI.Data
{
  public class ContactDbContext : DbContext
  {
    public ContactDbContext(DbContextOptions options) : base(options)
    {
      
    }
    public DbSet<Contact> Contacts { get; set; }
  }
}

## Step 5 : Now we injecting the service in our 'Program.cs' file for our 'Dbcontext'. Adding the following lines in the 'Program.cs' file.

### Program.cs

**using WebAPI.Data;
using Microsoft.EntityFrameworkCore;**


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
**builder.Services.AddDbContext<ContactsDbContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));**


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

## Step 6 : Now we add a connection string in our 'appsetiings.json' file 

{
  **"ConnectionStrings": {
    "DefaultConnection": "Data Source=WebAPI.db"
  },**
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}

## Step 7 : Now add two Model class in 'Models' Folder. Example:-

### AddContactRequest.cs

namespace WebAPI.Models
{
  public class AddContactRequest
  {
    public string FullName { get; set; }
    public string Email { get; set; }
    public long Phone { get; set; }
    public string Address { get; set; }
  }
}

### UpdateContactRequest.cs

namespace WebAPI.Models
{
  public class UpdateContactRequest
  {
    public string FullName { get; set; }
    public string Email { get; set; }
    public long Phone { get; set; }
    public string Address { get; set; }
  }
}

## Step 8 : Now add a controller in a 'Controller' folder with "GET,PUT,POST,DELETE" method. Example:-

using Microsoft.EntityFrameworkCore;
using WebAPI.Models;
using WebAPI.Data;
using Microsoft.AspNetCore.Mvc;

namespace ASPNET6_WebAPI.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class ContactsController : Controller
  {
    private readonly ContactsDbContext dbcontext;

    public ContactsController(ContactsDbContext dbcontext)
    {
      this.dbcontext = dbcontext;
    }

    [HttpGet]
    public async Task<IActionResult> GetContacts()
    {
      return Ok(await dbcontext.Contacts.ToListAsync());
    }
    
    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> GetContact([FromRoute] int id)
    {
      var contact = await dbContext.Contact.FindAsync(id);
      if (contact ==null)
        {
          return NotFound();
        }
        return Ok(contact);
    }
    
    [HttpPost]
    public async Task<IActionResult> AddContact(AddContactRequest addContactRequest)
    {
      var contact = new Contact()
      {
        Id = 0,
        Address = addContactRequest.Address,
        Email = addContactRequest.Email,
        FullName = addContactRequest.FullName,
        Phone = addContactRequest.Phone
      };
      
      await dbcontext.Contacts.AddAsync(contact);
      await dbContext.SaveChangesAsync();

      return Ok(contact);
    }

    [HttpPut]
    [Route("{id:int}")]
    public async Task<IActionResult> UpdateContact([FromRoute] int id, UpdateContactRequest updateContactRequest)
    {
      var contact = await dbContext.Contact.FindAsync(id);
        if (contact !=null)
        {
          contact.FullName = updateContactRequest.FullName;
          contact.Email = updateContactRequest.Email;
          contact.Address = updateContactRequest.Address;
          contact.Phone = moupdateContactRequestdel.Phone;
          await dbContext.SaveChangesAsync();
          return Ok(contact);
        }
        
        return NotFound();
    }

    [HttpsDelete]
    [Route("{id:int}")]
    public async Task<IActionResult> DeleteContact([FromRoute] int id) 
      {
        var contact = await dbContext.Contact.FindAsync(id);
        if (contact ==null)
        {
          dbContext.Remove(contact);
          await dbContext.SaveChangesAsync();
          return Ok(contact);
        }
        return NotFound();
      }
  }
}

## Step 9 : It's time to Migrate our application to our database. For migrations we have install the following packages : 

### Packages :-

1. dotnet add package Microsoft.EntityFrameworkCore
2. dotnet add package Microsoft.EntityFrameworkCore.Sqlite
3. dotnet add package Microsoft.EntityFrameworkCore.Tools
4. dotnet add package Microsoft.EntityFrameworkCore.Design

### for other references : 

1. dotnet tool install --global dotnet-ef
2. export PATH="$PATH:~/.dotnet/tools"
3. wget https://dot.net/v1/dotnet-install.sh -O dotnet-install.sh
4. chmod +x ./dotnet-install.sh
5. ./dotnet-install.sh --version latest
6. ./dotnet-install.sh --version latest --runtime aspnetcore
7. export PATH=$PATH:$HOME/.dotnet/tools
8. export DOTNET_ROOT=$HOME/.dotnet
9. export PATH=$PATH:$DOTNET_ROOT
10. export DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=1

## Step 10 : Now we are ready to migrate our application to the database. Here is the command to initiate the migrations :

# Package Manager Console :

1. Add-Migration "Initial Migration"
2. Update-Database

# Command Line Interface :

1. dotnet ef migrations add Initial
2. dotnet ef database update