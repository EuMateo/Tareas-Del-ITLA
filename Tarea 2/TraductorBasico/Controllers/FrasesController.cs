using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TraductorBasico.Models;
using TraductorBasico.Data;


namespace TraductorTurismoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FrasesController : ControllerBase
    {
        private readonly TraductorBasicoDataContext _context;

        public FrasesController(TraductorBasicoDataContext context)
        {
            _context = context;
        }


        // GET: api/Frases
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Frase>>> GetFrases()
        {
            return await _context.Frases.ToListAsync();
        }



        // GET: api/Frases/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Frase>> GetFrase(int id)
        {
            var frase = await _context.Frases.FindAsync(id);

            if (frase == null)
            {
                return NotFound();
            }

            return frase;
        }

        // POST: api/Frases
        [HttpPost]
        public async Task<ActionResult<Frase>> PostFrase(Frase frase)
        {
            _context.Frases.Add(frase);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetFrase), new { id = frase.Id }, frase);
        }

        // PUT: api/Frases/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFrase(int id, Frase frase)
        {
            if (id != frase.Id)
            {
                return BadRequest();
            }

            _context.Entry(frase).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Frases.Any(f => f.Id == id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // DELETE: api/Frases/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFrase(int id)
        {
            var frase = await _context.Frases.FindAsync(id);
            if (frase == null)
            {
                return NotFound();
            }

            _context.Frases.Remove(frase);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
