namespace VanderPET_2._0.Models
{
    public class Agendamento
    {
        public int ID { get; set; }
        public int UsuarioID { get; set; }
        public int PetID { get; set; }
        public int ServicoID { get; set; }
        public string Porte { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public DateTime Data { get; set; }
        public TimeSpan Hora { get; set; }
        public string Observacoes { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}
