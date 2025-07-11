﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tournament.Data.Data;
using Tournament.Core.Entities;
using Tournament.Core.Repositories;
using AutoMapper;
using Tournament.Core.Dto;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.IdentityModel.Tokens;
using Services.Contracts;

namespace Tournament.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TournamentController(IServiceManager manager) : ControllerBase
    {
        // GET: api/TournamentDetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TournamentDto>>> GetTournamentDetails([FromQuery] GetTournamentQueryDto dto)
        {
            var result = await manager.TournamentService.GetAllAsync(dto);
            
            if (!result.Items.Any())
            {
                return (!dto.Title.IsNullOrEmpty() || dto.FromYear.HasValue || dto.ToYear.HasValue) ? NotFound("Could not find any tournaments with those restrictions.") : NotFound("There are no tournaments in the database");
            }

            Response.Headers.Append("X-Total-Count", result.TotalCount.ToString());
            Response.Headers.Append("X-Page-Size", result.PageSize.ToString());
            Response.Headers.Append("X-Page", result.Page.ToString());
            Response.Headers.Append("X-Total-Pages", result.TotalPages.ToString());

            return Ok(result.Items);
        }

        // GET: api/TournamentDetails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TournamentDto>> GetTournamentDetails(int id)
        {
            var tournament = await manager.TournamentService.GetByIdAsync(id);

            return tournament;
        }

        // PUT: api/TournamentDetails/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTournamentDetails(int id, TournamentUpdateDto tournament)
        {
            if (id != tournament.Id)
            {
                return BadRequest();
            }

            await manager.TournamentService.UpdateAsync(tournament);

            return NoContent();
        }

        // POST: api/TournamentDetails
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TournamentDto>> PostTournamentDetails(TournamentCreateDto dto)
        {
            int? createdId = null;
            TournamentDto? createdDto = null;
            try
            {
                (createdId, createdDto) = await manager.TournamentService.AddAsync(dto);
            }
            catch (DbUpdateException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while saving the tournament");
            }
            if (createdId == null || createdDto == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the tournament.");
            }
            return CreatedAtAction(nameof(GetTournamentDetails), new { id = createdId }, createdDto);
        }

        // DELETE: api/TournamentDetails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTournamentDetails(int id)
        {
            
            await manager.TournamentService.DeleteAsync(id);

            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchTournamentDetails(int id, JsonPatchDocument<TournamentUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest("Patch document cannot be null.");
            }

            var tournament = await manager.TournamentService.GetByIdAsync(id);
            if (tournament == null)
            {
                return NotFound();
            }

            var tournamentToPatch = await manager.TournamentService.MapToUpdateDtoAsync(id);
            patchDoc.ApplyTo(tournamentToPatch, ModelState);

            TryValidateModel(tournamentToPatch);
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            try
            {
                await manager.TournamentService.PatchAsync(id, tournamentToPatch);

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await TournamentExists(id))
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

        private async Task<bool> TournamentExists(int id)
        {
            return await manager.TournamentService.AnyAsync(id);
        }
    }
}
