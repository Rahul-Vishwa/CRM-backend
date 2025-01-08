using System.Security.Claims;
using Artico.DbModels;
using Artico.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Win32;

namespace Artico.Controllers.CustomerController
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class CustomerController : ControllerBase
    {
        private readonly AppDbContext _context;
        public CustomerController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("add-customer")]
        public async Task<IActionResult> AddCustomer(AddCustomer customer)
        {
            int userid = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            
            Customers customerDetails = new Customers
            {
                userid = userid,
                name = customer.name,
                type = customer.type,
                status = customer.status,
                emailaddress = customer.emailaddress,
                phonenumber = customer.phonenumber,
                notes = customer.notes
            };

            _context.customers.Add(customerDetails);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(AddCustomer), new { id = customerDetails.id }, customerDetails);
        }

        [HttpGet("all-customers")]
        public async Task<IActionResult> GetAllCustomers(int page, int pagesize, string? searchterm)
        {
            if (page==0 || pagesize == 0)
            {
                return BadRequest("Params page and pagesize are required.");
            }
            int totalRecords = await _context.customers.CountAsync();
            List<AllCustomers> allCustomers = await (from customer in _context.customers
                                                     join user in _context.users on customer.userid equals user.id
                                                     where string.IsNullOrEmpty(searchterm) ||
                                                            customer.name.Contains(searchterm) ||
                                                            customer.emailaddress.Contains(searchterm) ||
                                                            customer.phonenumber.Contains(searchterm)
                                                     select new AllCustomers
                                                     {
                                                         id = customer.id,
                                                         name = customer.name,
                                                         type = customer.type == 1 ? "Business" : "Individual",
                                                         status = customer.status == 1 ? "Active" : "Inactive",
                                                         emailaddress = customer.emailaddress,
                                                         phonenumber = customer.phonenumber,
                                                         notes = customer.notes,
                                                         createdby = user.username,
                                                         createdat = customer.create_at
                                                     })
                                                     .OrderByDescending(c=>c.createdat)
                                                     .Skip((page-1)*pagesize)
                                                     .Take(pagesize)
                                                     .ToListAsync();
            return Ok(new { customers = allCustomers, totalRecords });
        }
    }
}
