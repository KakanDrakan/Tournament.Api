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
using Microsoft.AspNetCore.JsonPatch;

namespace Tournament.Api.Controllers
{
    [Route("api/tournament/{tournamentId}/games")]
    [ApiController]
    public class GamesController(IUnitOfWork UoW, IMapper mapper) : ControllerBase
    {
        // GET: api/Games
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GameDto>>> GetGames(int tournamentId, GetGameQueryDto dto)
        {
            var games = mapper.Map<IEnumerable<GameDto>>(await UoW.GameRepository.GetAllAsync(tournamentId, dto));

            return Ok(games);
        }

        // GET: api/Games/5
        [HttpGet("{input}")]
        public async Task<ActionResult<GameDto>> GetGameById(bool byTitle, string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return BadRequest("Input cannot be null or empty.");
            }
            if (byTitle)
            {
                var game = await UoW.GameRepository.GetByTitleAsync(input);
                if (game == null)
                {
                    return NotFound($"Game with title '{input}' not found.");
                }
                return mapper.Map<GameDto>(game);
            }
            else if (!int.TryParse(input, out int id))
            {
                return BadRequest("Invalid game ID format.");
            }
            else
            { 
                var game = await UoW.GameRepository.GetByIdAsync(id);
                if (game == null)
                {
                    return NotFound($"Game with ID {id} not found.");
                }
                return mapper.Map<GameDto>(game);
            }
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

        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchGame(int id, JsonPatchDocument<GameUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest("Patch document cannot be null.");
            }

            var game = await UoW.GameRepository.GetByIdAsync(id);
            if (game == null)
            {
                return NotFound();
            }

            var gameToPatch = mapper.Map<GameUpdateDto>(game);
            patchDoc.ApplyTo(gameToPatch, ModelState);

            TryValidateModel(gameToPatch);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            mapper.Map(gameToPatch, game);

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

        private async Task<bool> GameExists(int id)
        {
            return await UoW.GameRepository.AnyAsync(id);
        }
    }
}
