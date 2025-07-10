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
using Services.Contracts;

namespace Tournament.Api.Controllers
{
    [Route("api/tournament/{tournamentId}/games")]
    [ApiController]
    public class GamesController(IServiceManager manager) : ControllerBase
    {
        // GET: api/Games
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GameDto>>> GetGames(int tournamentId, [FromQuery] GetGameQueryDto dto)
        {

            var games = await manager.GameService.GetAllAsync(tournamentId, dto);

            Response.Headers.Append("X-Total-Count", games.TotalCount.ToString());
            Response.Headers.Append("X-Page-Size", games.PageSize.ToString());
            Response.Headers.Append("X-Page", games.Page.ToString());
            Response.Headers.Append("X-Total-Pages", games.TotalPages.ToString());

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
            var game = await manager.GameService.GetByInputAsync(byTitle, input);
            if (game == null)
            {
                return NotFound($"Game with {(byTitle ? "title" : "ID")} '{input}' was not found.");
            }
            return Ok(game);
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

            await manager.GameService.UpdateAsync(game);

            return NoContent();
        }

        // POST: api/Games
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<GameDto>> PostGame(int tournamentId, GameCreateDto dto)
        {

            int? createdId = null;
            GameDto? createdDto = null;
            try
            {
                (createdId, createdDto) = await manager.GameService.AddAsync(tournamentId, dto);
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while saving the game. {ex.InnerException?.Message}");
            }
            if (createdId == null || createdDto == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the game.");
            }
            return CreatedAtAction(nameof(GetGameById), new { tournamentId = tournamentId, input = createdId }, createdDto);
        }

        // DELETE: api/Games/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGame(int id)
        {

            await manager.GameService.DeleteAsync(id);


            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchGame(int id, JsonPatchDocument<GameUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest("Patch document cannot be null.");
            }

            var gameToPatch = await manager.GameService.MapToUpdateDtoAsync(id);
            patchDoc.ApplyTo(gameToPatch, ModelState);

            TryValidateModel(gameToPatch);
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }
            await manager.GameService.PatchAsync(id, gameToPatch);

            return NoContent();
        }

        private async Task<bool> GameExists(int id)
        {
            return await manager.GameService.AnyAsync(id);
        }
    }
}
