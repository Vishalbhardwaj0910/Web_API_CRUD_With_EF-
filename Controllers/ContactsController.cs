using Microsoft.EntityFrameworkCore;
using ASPNET6_WebAPI.Models;
using ASPNET6_WebAPI.Data;
using Microsoft.AspNetCore.Mvc;

namespace ASPNET6_WebAPI.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class ContactsController : Controller
  {
    private readonly ContactsDbContext dbContext;

    public ContactsController(ContactsDbContext dbContext)
    {
      this.dbContext = dbContext;
    }

    [HttpGet]
    public async Task<IActionResult> GetContacts()
    {
      return Ok(await dbContext.Contacts.ToListAsync());
    }
    
    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> GetContact([FromRoute] int id)
    {
      var contact = await dbContext.Contacts.FindAsync(id);
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
      
      await dbContext.Contacts.AddAsync(contact);
      await dbContext.SaveChangesAsync();

      return Ok(contact);
    }

    [HttpPut]
    [Route("{id:int}")]
    public async Task<IActionResult> UpdateContact([FromRoute] int id, UpdateContactRequest updateContactRequest)
    {
      var contact = await dbContext.Contacts.FindAsync(id);
        if (contact !=null)
        {
          contact.FullName = updateContactRequest.FullName;
          contact.Email = updateContactRequest.Email;
          contact.Address = updateContactRequest.Address;
          contact.Phone = updateContactRequest.Phone;
          await dbContext.SaveChangesAsync();
          return Ok(contact);
        }
        
        return NotFound();
    }

    [HttpDelete]
    [Route("{id:int}")]
    public async Task<IActionResult> DeleteContact([FromRoute] int id) 
      {
        var contact = await dbContext.Contacts.FindAsync(id);
        if (contact !=null)
        {
          dbContext.Remove(contact);
          await dbContext.SaveChangesAsync();
          return Ok(contact);
        }
        return NotFound();
      }
  }
}