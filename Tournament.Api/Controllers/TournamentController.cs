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

namespace Tournament.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TournamentController(IUnitOfWork UoW, IMapper mapper) : ControllerBase
    {
        // GET: api/TournamentDetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TournamentDto>>> GetTournamentDetails()
        {
            return Ok(mapper.Map<IEnumerable<TournamentDto>>(await UoW.TournamentRepository.GetAllAsync()));
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
        public async Task<IActionResult> PutTournamentDetails(int id, Core.Entities.Tournament tournamentDetails)
        {
            if (id != tournamentDetails.Id)
            {
                return BadRequest();
            }

            UoW.TournamentRepository.Update(tournamentDetails);

            try
            {
                await UoW.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await TournamentDetailsExists(id))
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
        public async Task<ActionResult<TournamentDto>> PostTournamentDetails(TournamentDto tournamentDto)
        {
            var tournament = mapper.Map<Core.Entities.Tournament>(tournamentDto);
            UoW.TournamentRepository.Add(tournament);
            await UoW.SaveChangesAsync();

            var createdTournament = mapper.Map<TournamentDto>(tournament);
            return CreatedAtAction("GetTournamentDetails", new { id = tournament.Id }, createdTournament);
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
            await UoW.SaveChangesAsync();

            return NoContent();
        }

        private async Task<bool> TournamentDetailsExists(int id)
        {
            return await UoW.TournamentRepository.AnyAsync(id);
        }
    }
}
