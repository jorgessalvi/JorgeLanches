namespace JorgeLanches.DTOs
{
    public class UsuarioTokenDTO
    {
        public bool Authenticated { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string? Token { get; set; }
        public string? Message { get; set; }
    }
}
