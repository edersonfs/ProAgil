using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ProAgil.Repository;
using ProAgil.Domain;

namespace ProAgil.WebAPI.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class ValuesController : ControllerBase
  {
    public readonly ProAgilContext _context;

    public ValuesController(ProAgilContext context)
    {
      this._context = context;
    }

    // GET api/values
    [HttpGet]
    public async Task<IActionResult> Get()
    {
      try
      {
        var results = await _context.Eventos.ToListAsync();

        return Ok(results);

      }
      catch (System.Exception)
      {
        return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de Dados Falhou");
      }
    }

    // GET api/values/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Evento>> Get(int id)
    {
      return await _context.Eventos.FirstOrDefaultAsync(x => x.Id == id);
    }

    // POST api/values
    [HttpPost]
    public void Post([FromBody] string value)
    {
    }

    // PUT api/values/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }

    // DELETE api/values/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
  }
}
