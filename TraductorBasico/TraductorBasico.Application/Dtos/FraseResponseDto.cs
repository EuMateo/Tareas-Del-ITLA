using System.ComponentModel.DataAnnotations;

namespace TraductorBasico.Dtos
{
    public class FraseResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public FraseDto? Data { get; set; }
        public IEnumerable<FraseDto>? DataList { get; set; }
        public List<string> Errors { get; set; } = new List<string>();

        public static FraseResponseDto SuccessResponse(FraseDto data, string message = "Operación exitosa")
        {
            return new FraseResponseDto
            {
                Success = true,
                Message = message,
                Data = data
            };
        }

        public static FraseResponseDto SuccessResponse(IEnumerable<FraseDto> dataList, string message = "Operación exitosa")
        {
            return new FraseResponseDto
            {
                Success = true,
                Message = message,
                DataList = dataList
            };
        }

        public static FraseResponseDto SuccessResponse(string message = "Operación exitosa")
        {
            return new FraseResponseDto
            {
                Success = true,
                Message = message
            };
        }

        public static FraseResponseDto ErrorResponse(string errorMessage)
        {
            return new FraseResponseDto
            {
                Success = false,
                Message = errorMessage,
                Errors = new List<string> { errorMessage }
            };
        }

        public static FraseResponseDto ErrorResponse(List<string> errors)
        {
            return new FraseResponseDto
            {
                Success = false,
                Message = "Se encontraron errores de validación",
                Errors = errors
            };
        }
    }
}