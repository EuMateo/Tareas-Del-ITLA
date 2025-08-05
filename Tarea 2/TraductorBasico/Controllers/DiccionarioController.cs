using Microsoft.AspNetCore.Mvc;
using TraductorBasico.Contract;
using TraductorBasico.Dtos;

namespace TraductorBasico.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiccionarioController : ControllerBase
    {
        private readonly IDiccionarioService _diccionarioService;

        public DiccionarioController(IDiccionarioService diccionarioService)
        {
            _diccionarioService = diccionarioService;
        }

        [HttpGet("imagenes")]
        public async Task<ActionResult> GetImagenes()
        {
            var result = await _diccionarioService.GetAllImagenesAsync();

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

        [HttpGet("imagenes/{id}")]
        public async Task<ActionResult> GetImagen(int id)
        {
            var result = await _diccionarioService.GetImagenByIdAsync(id);

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

        [HttpGet("imagenes/categoria/{categoria}")]
        public async Task<ActionResult> GetImagenesByCategoria(string categoria)
        {
            var result = await _diccionarioService.GetImagenesByCategoriaAsync(categoria);

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

        [HttpGet("imagenes/{id}/frases")]
        public async Task<ActionResult> GetImagenConFrases(int id)
        {
            var result = await _diccionarioService.GetImagenWithFrasesAsync(id);

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

        [HttpGet("categorias")]
        public async Task<ActionResult> GetCategorias()
        {
            var result = await _diccionarioService.GetCategoriasDisponiblesAsync();

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

        [HttpPost("imagenes")]
        public async Task<ActionResult> PostImagen(CreateImagenDiccionarioDto createImagenDto)
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

            var result = await _diccionarioService.CreateImagenAsync(createImagenDto);

            if (!result.Success)
            {
                return BadRequest(new
                {
                    message = result.Message,
                    errors = result.Errors
                });
            }

            return CreatedAtAction(nameof(GetImagen),
                new { id = result.Data!.Id },
                new
                {
                    message = result.Message,
                    data = result.Data
                });
        }

        [HttpDelete("imagenes/{id}")]
        public async Task<IActionResult> DeleteImagen(int id)
        {
            var result = await _diccionarioService.DeleteImagenAsync(id);

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

        [HttpPost("imagenes/{id}/frases")]
        public async Task<IActionResult> AsignarFrases(int id, [FromBody] List<int> frasesIds)
        {
            var result = await _diccionarioService.AsignarFrasesAImagenAsync(id, frasesIds);

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
                data = result.Data
            });
        }
    }
}