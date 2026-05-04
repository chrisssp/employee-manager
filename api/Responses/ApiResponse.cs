namespace EmpresaApi.Responses
{
    /// <summary>
    /// Response estándar para todas las operaciones
    /// Proporciona información consistente sobre el resultado de una operación
    /// </summary>
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public int? StatusCode { get; set; }

        /// <summary>
        /// Constructor para respuestas exitosas
        /// </summary>
        public static ApiResponse<T> SuccessResponse(T data, string message = "Operación realizada exitosamente")
        {
            return new ApiResponse<T>
            {
                Success = true,
                Message = message,
                Data = data,
                StatusCode = 200
            };
        }

        /// <summary>
        /// Constructor para respuestas de error
        /// </summary>
        public static ApiResponse<T> ErrorResponse(string message, int statusCode = 400)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = message,
                StatusCode = statusCode
            };
        }

        /// <summary>
        /// Constructor para respuestas de error con datos
        /// </summary>
        public static ApiResponse<T> ErrorResponse(string message, T? data, int statusCode = 400)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = message,
                Data = data,
                StatusCode = statusCode
            };
        }

        /// <summary>
        /// Constructor para respuestas de creación (201)
        /// </summary>
        public static ApiResponse<T> CreatedResponse(T data, string message = "Recurso creado exitosamente")
        {
            return new ApiResponse<T>
            {
                Success = true,
                Message = message,
                Data = data,
                StatusCode = 201
            };
        }
    }
}
