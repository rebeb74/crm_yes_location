using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YesLocation.Application.DTOs.Customer;
using YesLocation.Domain.Entities;
using YesLocation.Infrastructure.Persistence;

namespace YesLocation.Api.Controllers;

public class CustomersController : BaseController<Customer, CustomerInputDto, CustomerDto>
{
  public CustomersController(YesLocationDbContext context, IMapper mapper)
      : base(context, mapper)
  {
  }

  // Search Customers by name or email
  [HttpGet("Search/{query}")]
  public async Task<ActionResult<IEnumerable<CustomerDto>>> Search(string query)
  {
    var customers = await _context.Customers.Where(c =>
        (c.FirstName != null && c.FirstName.Contains(query)) ||
        (c.LastName != null && c.LastName.Contains(query)) ||
        (c.Email != null && c.Email.Contains(query))
    ).ToListAsync();
    return Ok(_mapper.Map<IEnumerable<CustomerDto>>(customers));
  }
}
