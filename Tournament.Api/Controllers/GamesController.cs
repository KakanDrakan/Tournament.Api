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
using System.Runtime.CompilerServices;

namespace Tournament.Api.Controllers
{
    [Route("api/tournament/{tournamentId}/games")]
    [ApiController]
    public class GamesController(IUnitOfWork UoW, IMapper mapper) : ControllerBase
    {
        // GET: api/Games
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GameDto>>> GetGames(int tournamentId)
        {
            var games = mapper.Map<IEnumerable<GameDto>>(await UoW.GameRepository.GetAllAsync(tournamentId));

            return Ok(games);
        }

        // GET: api/Games/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GameDto>> GetGame(int id)
        {
            var game = await UoW.GameRepository.GetByIdAsync(id);

            if (game == null)
            {
                return NotFound();
            }

            return mapper.Map<GameDto>(game);
        }

        // PUT: api/Games/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGame(int id, GameUpdateDto game)
        {
            if (id != game.Id)
            {
                return BadRequest();
            }
            var existingGame = await UoW.GameRepository.GetByIdAsync(id);
            if (existingGame == null)
            {
                return NotFound();
            }
            mapper.Map(game, existingGame);

            UoW.GameRepository.Update(existingGame);

            try
            {
                await UoW.SaveChangesAsync();
            }
            catch (Exception)
            {
                if (!await GameExists(id))
                {
                    return NotFound();
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the database.");
                }
            }

            return NoContent();
        }

        // POST: api/Games
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<GameDto>> PostGame(int tournamentId, GameCreateDto dto)
        {
            var game = mapper.Map<Game>(dto);
            game.TournamentId = tournamentId;
            UoW.GameRepository.Add(game);
            try
            {
                await UoW.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (await GameExists(game.Id))
                {
                    return Conflict("A game with the same ID already exists.");
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while saving to the database.");
                }
            }

            var createdGame = mapper.Map<GameDto>(game);
            return CreatedAtAction(nameof(GetGames), new { id = game.Id, createdGame }, createdGame);
        }

        // DELETE: api/Games/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGame(int id)
        {
            var game = await UoW.GameRepository.GetByIdAsync(id);
            if (game == null)
            {
                return NotFound();
            }

            UoW.GameRepository.Remove(game);
            try 
            { 
                await UoW.SaveChangesAsync();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while saving to the database.");
            }

            return NoContent();
        }

        private async Task<bool> GameExists(int id)
        {
            return await UoW.GameRepository.AnyAsync(id);
        }
    }
}
