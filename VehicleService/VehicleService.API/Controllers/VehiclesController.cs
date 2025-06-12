// VehicleService.API/Controllers/VehiclesController.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using VehicleService.Application.DTOs;
using VehicleService.Domain.Entities;
using VehicleService.Application.Services;

namespace VehicleService.API.Controllers
{
    [ApiController]
    [Route("api/vehicles")]
    public class VehiclesController : ControllerBase
    {
        private readonly IVehicleService _service;
        private readonly IMapper _mapper;

        public VehiclesController(IVehicleService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VehicleDto>>> GetAll()
        {
            var entities = await _service.GetAllAsync();
            var dtos = _mapper.Map<IEnumerable<VehicleDto>>(entities);
            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VehicleDto>> GetById(Guid id)
        {
            var entity = await _service.GetByIdAsync(id);
            if (entity == null) return NotFound();
            var dto = _mapper.Map<VehicleDto>(entity);
            return Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult<VehicleDto>> Create([FromBody] CreateVehicleDto createDto)
        {
            var entity = _mapper.Map<Vehicle>(createDto);
            var created = await _service.CreateAsync(entity);
            var dto = _mapper.Map<VehicleDto>(created);
            return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
        }

        [HttpGet("availability")]
        public async Task<ActionResult<IEnumerable<VehicleDto>>> GetAvailability(
            [FromQuery] string? type,
            [FromQuery] DateTime? date)
        {
            var list = await _service.GetAvailableAsync(type, date);
            var dtos = _mapper.Map<IEnumerable<VehicleDto>>(list);
            return Ok(dtos);
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateVehicleDto updateDto)
        {
            var existing = await _service.GetByIdAsync(id);
            if (existing == null) return NotFound();
            _mapper.Map(updateDto, existing);
            await _service.UpdateAsync(existing);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var existing = await _service.GetByIdAsync(id);
            if (existing == null) return NotFound();
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}