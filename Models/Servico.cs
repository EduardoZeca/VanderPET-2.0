namespace VanderPET_2._0.Models
{
    public class Servico
    {
        public int ID { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Porte { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public int TempoMinutos { get; set; }
    }
}
