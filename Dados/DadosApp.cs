using VanderPET_2._0.Models;

namespace VanderPET_2._0.Dados
{
    public static class DadosApp
    {
        // Usuários cadastrados
        public static List<Usuario> Usuarios { get; set; } = new List<Usuario>();
        // PETs cadastrados
        public static List<Pet> Pets { get; set; } = new List<Pet>();
        // Agendamentos realizados
        public static List<Agendamento> Agendamentos { get; set; } = new List<Agendamento>();
        // Usuário atualmente logado
        public static Usuario? UsuarioLogado { get; set; }
        public static List<Especie> Especies { get; set; } = new List<Especie>
        {
            new Especie
            {
                ID = 1, Nome = "Cachorro"
            },
            new Especie
            {
                ID = 2, Nome = "Gato"
            }
        };
        public static List<Raca> Racas { get; set; } = new List<Raca>
        {
            // Cachorros
            new Raca
            {
                ID = 1, Nome = "Chow-Chow", EspecieID = 1
            },
            new Raca
            {
                ID = 2, Nome = "Vira-Lata", EspecieID = 1
            },
            new Raca
            {
                ID = 3, Nome = "SRD", EspecieID = 1
            },
            // Gatos
            new Raca
            {
                ID = 4, Nome = "Siamês", EspecieID = 2
            },
            new Raca
            {
                ID = 5, Nome = "Persa", EspecieID = 2
            },
            new Raca
            {
                ID = 6, Nome = "SRD", EspecieID = 2
            }
        };
        public static List<Servico> Servicos { get; set; } = new List<Servico>
        {
            new Servico
            {
                ID = 1, Nome = "Banho", Porte = "Pequeno", Valor = 30.00m, TempoMinutos = 30
            },
            new Servico
            {
                ID = 2, Nome = "Banho", Porte = "Médio", Valor = 35.00m, TempoMinutos = 45
            },
            new Servico
            {
                ID = 3, Nome = "Banho", Porte = "Grande", Valor = 40.00m, TempoMinutos = 60
            },
            new Servico
            {
                ID = 4, Nome = "Tosa", Porte = "Pequeno", Valor = 35.00m, TempoMinutos = 45
            },
            new Servico
            {
                ID = 5, Nome = "Tosa", Porte = "Médio", Valor = 45.00m, TempoMinutos = 60
            },
            new Servico
            {
                ID = 6, Nome = "Tosa", Porte = "Grande", Valor = 55.00m, TempoMinutos = 90
            }
        };
    }
}