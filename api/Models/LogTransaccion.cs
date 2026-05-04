namespace EmpresaApi.Models
{
    public class LogTransaccion
    {
        public int Id { set; get; }
        public string VerboHttp { get; set; } = string.Empty;
        public string Endpoint { get; set; } = string.Empty;
        public int StatusCode { get; set; }
        public string Payload { get; set; } = string.Empty;
        public DateTime Fecha { get; set; } = DateTime.UtcNow;
    }
}
