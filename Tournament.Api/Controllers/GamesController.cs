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
    public class GamesController(IUnitOfWork UoW, IMapper mapper) : ControllerBase
    {
        // GET: api/Games
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GameDto>>> GetGame()
        {
            return Ok(mapper.Map<IEnumerable<GameDto>>(await UoW.GameRepository.GetAllAsync()));
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
        public async Task<IActionResult> PutGame(int id, Game game)
        {
            if (id != game.Id)
            {
                return BadRequest();
            }

            UoW.GameRepository.Update(game);

            try
            {
                await UoW.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await GameExists(id))
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

        // POST: api/Games
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<GameDto>> PostGame(GameDto dto)
        {
            var game = mapper.Map<Game>(dto);
            UoW.GameRepository.Add(game);
            await UoW.SaveChangesAsync();

            var createdGame = mapper.Map<GameDto>(game);
            return CreatedAtAction("GetGame", new { id = game.Id, createdGame}, game);
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
            await UoW.SaveChangesAsync();

            return NoContent();
        }

        private async Task<bool> GameExists(int id)
        {
            return await UoW.GameRepository.AnyAsync(id);
        }
    }
}
