using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YesLocation.Domain.Entities;
using YesLocation.Domain.Interfaces;
using YesLocation.Infrastructure.Persistence;

namespace YesLocation.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public abstract class BaseController<TEntity, TInputDto, TBasicResponseDto> : ControllerBase
    where TEntity : BaseModel
    where TInputDto : class, IBaseInputModelDto
    where TBasicResponseDto : class, IBaseModelDto
{
  protected readonly YesLocationDbContext _context;
  protected readonly IMapper _mapper;

  protected BaseController(YesLocationDbContext context, IMapper mapper)
  {
    _context = context;
    _mapper = mapper;
  }

  [HttpGet]
  public virtual async Task<ActionResult<IEnumerable<TBasicResponseDto>>> GetAll()
  {
    var entities = await _context.Set<TEntity>().ToListAsync();
    return Ok(_mapper.Map<IEnumerable<TBasicResponseDto>>(entities));
  }

  [HttpGet("{id}")]
  public virtual async Task<ActionResult<TBasicResponseDto>> GetById(int id)
  {
    var entity = await _context.Set<TEntity>().FindAsync(id);

    if (entity == null)
    {
      return NotFound();
    }

    return Ok(_mapper.Map<TBasicResponseDto>(entity));
  }

  [HttpPost]
  public virtual async Task<ActionResult<TBasicResponseDto>> Create(TInputDto dto)
  {
    var entity = _mapper.Map<TEntity>(dto);
    _context.Set<TEntity>().Add(entity);
    await _context.SaveChangesAsync();

    var responseDto = _mapper.Map<TBasicResponseDto>(entity);
    return CreatedAtAction(nameof(GetById), new { id = entity.Id }, responseDto);
  }

  [HttpPut("{id}")]
  public virtual async Task<IActionResult> Update(int id, TInputDto dto)
  {
    if (id != dto.Id)
    {
      return BadRequest();
    }

    var entity = await _context.Set<TEntity>().FindAsync(id);
    if (entity == null)
    {
      return NotFound();
    }

    _mapper.Map(dto, entity);

    try
    {
      await _context.SaveChangesAsync();
    }
    catch (DbUpdateConcurrencyException)
    {
      if (!EntityExists(id))
      {
        return NotFound();
      }
      throw;
    }

    return NoContent();
  }

  [HttpDelete("{id}")]
  public virtual async Task<IActionResult> Delete(int id)
  {
    var entity = await _context.Set<TEntity>().FindAsync(id);
    if (entity == null)
    {
      return NotFound();
    }

    _context.Set<TEntity>().Remove(entity);
    await _context.SaveChangesAsync();

    return NoContent();
  }

  protected virtual bool EntityExists(int id)
  {
    return _context.Set<TEntity>().Any(e => e.Id == id);
  }
}