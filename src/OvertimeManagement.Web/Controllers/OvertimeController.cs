using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OvertimeManagement.Application.Interfaces;
using OvertimeManagement.Domain.Models;
using OvertimeManagement.Domain.Models.ViewModels.Response;
using System.Net;

namespace OvertimeManagement.Web.Controllers;

[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
public class OvertimeController(IOvertimeApp overtimeApp) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<Overtime>> PostOvertime(Overtime overtime)
    {
        try
        {
            await overtimeApp.SaveAsync(overtime);

            return CreatedAtAction(
                nameof(GetOvertimeByIdAsync),
                new { id = overtime.Id },
                overtime
            );
        }
        catch (Exception ex)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError,
                $"Error creating overtime: {ex.Message}");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Overtime>> GetOvertimeByIdAsync(int id)
    {
        try
        {
            return Ok(await overtimeApp.GetOvertimeByIdAsync(id));
        }
        catch (Exception ex)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError,
                $"Error fetching overtime: {ex.Message}");
        }
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Overtime>>> GetAllAsync()
    {
        try
        {
            return Ok(await overtimeApp.GetAllAsync());
        }
        catch (Exception ex)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError,
                $"Error fetching overtimes: {ex.Message}");
        }
    }

    [HttpGet("by-month/{month}")]
    public async Task<ActionResult<OverTimeMonthResponse>> GetOvertimeByMonthAsync(int month)
    {
        try
        {
            return Ok(await overtimeApp.GetOvertimeByMonthAsync(month));
        }
        catch (Exception ex)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError,
                $"Error fetching overtimes for the month: {ex.Message}");
        }
    }

    [HttpGet("calculated")]
    public async Task<ActionResult<OvertimeResponse>> GetCalculateOvertimesAsync
    (
        [FromQuery] decimal salary,
        [FromQuery] int month,
        [FromQuery] int? initialDay,
        [FromQuery] int? finalDay
    )
    {
        try
        {
            return await overtimeApp.Calculate(salary, month, initialDay, finalDay);
        }
        catch (Exception ex)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError,
                $"Error calculating overtimes: {ex.Message}");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutOvertimeAsync(int id, Overtime overtime)
    {
        if (id != overtime.Id)
            return BadRequest();

        try
        {
            await overtimeApp.PutOvertimeAsync(overtime);

            return Ok("Overtime updated successfully.");
        }
        catch (Exception ex)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError,
                $"Error updating overtime: {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOvertimeAsync(int id)
    {
        try
        {
            await overtimeApp.DeleteOvertimeAsync(id);
            return Ok("Overtime deleted successfully");
        }
        catch (Exception ex)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError,
                $"Error deleting overtime: {ex.Message}");
        }
    }
}
