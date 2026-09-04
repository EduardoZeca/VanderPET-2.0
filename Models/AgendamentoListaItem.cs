namespace VanderPET_2._0.Models;
public class AgendamentoListaItem
{
    public string Pet { get; set; } = "";
    public string Servico { get; set; } = "";
    public string Valor { get; set; } = "";
    public string DataHora { get; set; } = "";
    public string Status { get; set; } = "";
    public Agendamento Agendamento { get; set; } = null!;
    public bool PodeCancelar { get; set; }
}