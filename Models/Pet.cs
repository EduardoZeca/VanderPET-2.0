namespace VanderPET_2._0.Models
{
    public class Pet
    {
        public int ID { get; set; }
        public int UsuarioID { get; set; }
        public string Nome { get; set; } = string.Empty;
        public int EspecieID { get; set; }
        public int RacaID { get; set; }
        public double Peso { get; set; }
        public DateTime DataNascimento { get; set; }
    }
}
