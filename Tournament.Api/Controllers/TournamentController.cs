using System;
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

namespace Tournament.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TournamentController(IUnitOfWork UoW, IMapper mapper) : ControllerBase
    {
        // GET: api/TournamentDetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TournamentDto>>> GetTournamentDetails(bool includeGames)
        {
            var tournaments = mapper.Map<IEnumerable<TournamentDto>>(await UoW.TournamentRepository.GetAllAsync(includeGames));
            return Ok(tournaments);
        }

        // GET: api/TournamentDetails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TournamentDto>> GetTournamentDetails(int id)
        {
            var tournament = await UoW.TournamentRepository.GetByIdAsync(id);

            if (tournament == null)
            {
                return NotFound();
            }

            return mapper.Map<TournamentDto>(tournament);
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
            var existingTournament = await UoW.TournamentRepository.GetByIdAsync(id);

            if (existingTournament == null)
            {
                return NotFound();
            }
            mapper.Map(tournament, existingTournament);

            UoW.TournamentRepository.Update(existingTournament);

            try
            {
                await UoW.SaveChangesAsync();
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

        // POST: api/TournamentDetails
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TournamentDto>> PostTournamentDetails(TournamentCreateDto tournamentDto)
        {
            var tournament = mapper.Map<Core.Entities.Tournament>(tournamentDto);
            UoW.TournamentRepository.Add(tournament);
            
            try 
            { 
                await UoW.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (await TournamentExists(tournament.Id))
                {
                    return Conflict("A tournament with that Id already exists.");
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while saving the tournament");
                }
            }

            var createdTournament = mapper.Map<TournamentDto>(tournament);
            return CreatedAtAction(nameof(GetTournamentDetails), new { id = tournament.Id }, createdTournament);
        }

        // DELETE: api/TournamentDetails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTournamentDetails(int id)
        {
            var tournamentDetails = await UoW.TournamentRepository.GetByIdAsync(id);
            if (tournamentDetails == null)
            {
                return NotFound();
            }

            UoW.TournamentRepository.Remove(tournamentDetails);

            try 
            {
                await UoW.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while deleting the tournament: {ex.Message}");
            }

            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchTournamentDetails(int id, JsonPatchDocument<TournamentUpdateDto> patchDoc)
        {
            var tournament = await UoW.TournamentRepository.GetByIdAsync(id);
            if (tournament == null)
            {
                return NotFound();
            }

            var tournamentToPatch = mapper.Map<TournamentUpdateDto>(tournament);
            patchDoc.ApplyTo(tournamentToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            mapper.Map(tournamentToPatch, tournament);
            UoW.TournamentRepository.Update(tournament);

            try
            {
                await UoW.SaveChangesAsync();
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
            return await UoW.TournamentRepository.AnyAsync(id);
        }
    }
}
