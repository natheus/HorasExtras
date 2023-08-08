using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ControleDeHorasExtras.Models;
using ControleDeHorasExtras.Application;
using ControleDeHorasExtras.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace ControleDeHorasExtras.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HorasExtrasController : ControllerBase
    {
        private readonly HorasExtrasDb _context;
        private readonly HorasExtrasApplication _appHorasExtras;

        public HorasExtrasController(HorasExtrasDb context)
        {
            _context = context;
            _appHorasExtras = new HorasExtrasApplication(context);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResult<HoraExtra>> PostHoraExtra(HoraExtra horasExtras)
        {
            _context.HorasExtras.Add(horasExtras);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetHoraExtra), new { id = horasExtras.Id }, horasExtras);
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HoraExtra>>> GetHorasExtras()
        {
            return await _context.HorasExtras.ToListAsync();
        }
        
        [HttpGet("byMonth/{month}")]
        public async Task<ActionResult<HorasExtrasMonthResponse>> GetHorasExtrasMonth(int month)
        {
            var horasExtrasMonth = await _context.HorasExtras.Where(h => h.HorarioInicial.Month == month).ToListAsync();
            return new HorasExtrasMonthResponse() { DiasHorasExtras = horasExtrasMonth.Count, HorasExtras = horasExtrasMonth };
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<HoraExtra>> GetHoraExtra(int id)
        {
            var horaExtra = await _context.HorasExtras.FindAsync(id);

            if (horaExtra == null)
            {
                return NotFound();
            }

            return horaExtra;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("calculated")]
        public async Task<ActionResult<HorasExtrasResponse>> GetCalculatedHorasExtras([FromQuery] decimal salario, [FromQuery] int month)
        {
            return await _appHorasExtras.Calculated(salario, month);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHoraExtra(int id, HoraExtra horaExtra)
        {
            if (id != horaExtra.Id)
            {
                return BadRequest();
            }

            _context.Entry(horaExtra).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HoraExtraExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHoraExtra(int id)
        {
            var horaExtra = await _context.HorasExtras.FindAsync(id);
            if (horaExtra == null)
            {
                return NotFound();
            }

            _context.HorasExtras.Remove(horaExtra);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool HoraExtraExists(int id)
        {
            return _context.HorasExtras.Any(e => e.Id == id);
        }
    }
}
