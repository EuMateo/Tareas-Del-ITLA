using Microsoft.AspNetCore.Mvc;
using TraductorBasico.Contract;
using TraductorBasico.Dtos;

namespace TraductorBasico.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FrasesController : ControllerBase
    {
        private readonly IFraseService _fraseService;

        public FrasesController(IFraseService fraseService)
        {
            _fraseService = fraseService;
        }

        [HttpGet]
        public async Task<ActionResult> GetFrases()
        {
            var result = await _fraseService.GetAllFrasesAsync();

            if (!result.Success)
            {
                return BadRequest(new
                {
                    message = result.Message,
                    errors = result.Errors
                });
            }

            return Ok(new
            {
                message = result.Message,
                data = result.DataList,
                count = result.DataList?.Count() ?? 0
            });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetFrase(int id)
        {
            var result = await _fraseService.GetFraseByIdAsync(id);

            if (!result.Success)
            {
                return NotFound(new
                {
                    message = result.Message,
                    errors = result.Errors
                });
            }

            return Ok(new
            {
                message = result.Message,
                data = result.Data
            });
        }

        [HttpGet("categoria/{categoria}")]
        public async Task<ActionResult> GetFrasesByCategoria(string categoria)
        {
            var result = await _fraseService.GetFrasesByCategoriaAsync(categoria);

            if (!result.Success)
            {
                return BadRequest(new
                {
                    message = result.Message,
                    errors = result.Errors
                });
            }

            return Ok(new
            {
                message = result.Message,
                data = result.DataList,
                categoria = categoria,
                count = result.DataList?.Count() ?? 0
            });
        }

       
        [HttpPost]
        public async Task<ActionResult> PostFrase(CreateFraseDto createFraseDto)
        {
            if (!ModelState.IsValid)
            {
                var modelErrors = ModelState
                    .Where(x => x.Value?.Errors.Count > 0)
                    .SelectMany(x => x.Value!.Errors)
                    .Select(x => x.ErrorMessage)
                    .ToList();

                return BadRequest(new
                {
                    message = "Errores de validación en el modelo",
                    errors = modelErrors
                });
            }
            var result = await _fraseService.CreateFraseAsync(createFraseDto);

            if (!result.Success)
            {
                return BadRequest(new
                {
                    message = result.Message,
                    errors = result.Errors
                });
            }

            return CreatedAtAction(nameof(GetFrase),
                new { id = result.Data!.Id },
                new
                {
                    message = result.Message,
                    data = result.Data
                });
        }

       
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFrase(int id, UpdateFraseDto updateFraseDto)
        {
            if (!ModelState.IsValid)
            {
                var modelErrors = ModelState
                    .Where(x => x.Value?.Errors.Count > 0)
                    .SelectMany(x => x.Value!.Errors)
                    .Select(x => x.ErrorMessage)
                    .ToList();

                return BadRequest(new
                {
                    message = "Errores de validación en el modelo",
                    errors = modelErrors
                });
            }

            var result = await _fraseService.UpdateFraseAsync(id, updateFraseDto);

            if (!result.Success)
            {
                if (result.Message.Contains("No se encontró"))
                {
                    return NotFound(new
                    {
                        message = result.Message,
                        errors = result.Errors
                    });
                }

                return BadRequest(new
                {
                    message = result.Message,
                    errors = result.Errors
                });
            }

            return Ok(new
            {
                message = result.Message,
                data = result.Data
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFrase(int id)
        {
            var result = await _fraseService.DeleteFraseAsync(id);

            if (!result.Success)
            {
                if (result.Message.Contains("No se encontró"))
                {
                    return NotFound(new
                    {
                        message = result.Message,
                        errors = result.Errors
                    });
                }

                return BadRequest(new
                {
                    message = result.Message,
                    errors = result.Errors
                });
            }

            return Ok(new
            {
                message = result.Message
            });
        }
    }
}