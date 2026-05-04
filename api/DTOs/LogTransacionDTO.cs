namespace EmpresaApi.DTOs
{
    public class LogTransacionDTO
    {
        public int Id { get; set; }
        public string VerboHttp { get; set; } = string.Empty;
        public string Endpoint { get; set; } = string.Empty;
        public int StatusCode { get; set; }
        public string Payload { get; set; } = string.Empty;
        public DateTimeOffset Fecha { get; set; }
    }
}
