using ControleDeHorasExtras.Domain.Models;
using ControleDeHorasExtras.Domain.Models.ViewModels.Response;
using ControleDeHorasExtras.Infra.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ControleDeHorasExtras.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HorasExtrasController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly HoraExtraApp _horaExtraApp;

        public HorasExtrasController(AppDbContext context, HoraExtraApp horaExtraApp)
        {
            (_context, _horaExtraApp) =
            (context, horaExtraApp);
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

        [HttpGet("by-month/{month}")]
        public async Task<ActionResult<HorasExtrasMonthResponse>> GetHorasExtrasMonth(int month)
        {
            var horasExtrasMonth = await _context.HorasExtras
                .Where(h => h.HorarioInicial.Month == month)
                .OrderBy(h => h.HorarioInicial)
                .ToListAsync();

            return new HorasExtrasMonthResponse() { DiasHorasExtras = horasExtrasMonth.Count, HorasExtras = horasExtrasMonth };
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<HoraExtra>> GetHoraExtra(int id)
        {
            var horaExtra = await _context.HorasExtras.FindAsync(id);

            if (horaExtra == null)
                return NotFound();

            return horaExtra;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("calculated")]
        public async Task<ActionResult<HorasExtrasResponse>> GetCalculateHorasExtras
            (
                [FromQuery] decimal salario,
                [FromQuery] int month,
                [FromQuery] int? initialDay,
                [FromQuery] int? finalDay
            )
        {
            return await _horaExtraApp.Calculate(salario, month, initialDay, finalDay);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHoraExtra(int id, HoraExtra horaExtra)
        {
            if (id != horaExtra.Id)
                return BadRequest();

            _context.Entry(horaExtra).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HoraExtraExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHoraExtra(int id)
        {
            var horaExtra = await _context.HorasExtras.FindAsync(id);

            if (horaExtra == null)
                return NotFound();

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
