using System.Globalization;
using VanderPET_2._0.Dados;
using VanderPET_2._0.Models;

namespace VanderPET_2._0.Views;

public partial class frmAgendamento : ContentPage
{
    public frmAgendamento()
    {
        InitializeComponent();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();

        AdicionarAgendamentosTeste();
        CarregarAgendamentos();
    }
    private void CarregarAgendamentos()
    {
        if (DadosApp.UsuarioLogado == null)
        {
            cvAgendamentos.ItemsSource = null;
            return;
        }

        List<AgendamentoListaItem> agendamentos = DadosApp.Agendamentos
            .Where(a => a.UsuarioID == DadosApp.UsuarioLogado.ID)
            .OrderByDescending(a => a.Data)
            .ThenByDescending(a => a.Hora)
            .Take(4)
            .Select(a => new AgendamentoListaItem
            {
                Agendamento = a,
                Pet = BuscarNomePet(a.PetID),
                Servico = BuscarNomeServico(a.ServicoID),
                Valor = a.Valor.ToString(
                    "C2",
                    CultureInfo.GetCultureInfo("pt-BR")
                ),
                DataHora = $"{a.Data:dd/MM/yyyy} às {a.Hora:hh\\:mm}",
                Status = a.Status,
                PodeCancelar = a.Status == "Agendado"
            })
            .ToList();

        cvAgendamentos.ItemsSource = agendamentos;
    }
    private string BuscarNomePet(int petID)
    {
        Pet? pet = DadosApp.Pets
            .FirstOrDefault(p => p.ID == petID);

        return pet?.Nome ?? "PET não encontrado";
    }
    private string BuscarNomeServico(int servicoID)
    {
        Servico? servico = DadosApp.Servicos
            .FirstOrDefault(s => s.ID == servicoID);

        return servico?.Nome ?? "Serviço não encontrado";
    }
    private void AdicionarAgendamentosTeste()
    {
        if (DadosApp.UsuarioLogado == null)
            return;

        bool usuarioJaPossuiAgendamentos = DadosApp.Agendamentos
            .Any(a => a.UsuarioID == DadosApp.UsuarioLogado.ID);

        if (usuarioJaPossuiAgendamentos)
            return;

        Pet? pet = DadosApp.Pets
            .FirstOrDefault(p => p.UsuarioID == DadosApp.UsuarioLogado.ID);

        if (pet == null)
            return;

        Servico? banho = DadosApp.Servicos
            .FirstOrDefault(s =>
                s.Nome == "Banho" &&
                s.Porte == "Pequeno");

        Servico? tosa = DadosApp.Servicos
            .FirstOrDefault(s =>
                s.Nome == "Tosa" &&
                s.Porte == "Pequeno");

        if (banho == null || tosa == null)
        {
            return;
        }

        int proximoID = DadosApp.Agendamentos.Count == 0
            ? 1
            : DadosApp.Agendamentos.Max(a => a.ID) + 1;

        Agendamento agendamento1 = new Agendamento
        {
            ID = proximoID,
            UsuarioID = DadosApp.UsuarioLogado.ID,
            PetID = pet.ID,
            ServicoID = banho.ID,
            Porte = banho.Porte,
            Valor = banho.Valor,
            Data = DateTime.Today.AddDays(-7),
            Hora = new TimeSpan(10, 0, 0),
            Observacoes = "",
            Status = "Concluído"
        };
        Agendamento agendamento2 = new Agendamento
        {
            ID = proximoID + 1,
            UsuarioID = DadosApp.UsuarioLogado.ID,
            PetID = pet.ID,
            ServicoID = tosa.ID,
            Porte = tosa.Porte,
            Valor = tosa.Valor,
            Data = DateTime.Today.AddDays(-20),
            Hora = new TimeSpan(14, 30, 0),
            Observacoes = "",
            Status = "Concluído"
        };
        DadosApp.Agendamentos.Add(agendamento1);
        DadosApp.Agendamentos.Add(agendamento2);
    }
    private async void btnNovoAgendamento_Clicked(object sender, EventArgs e)
    {
        if (DadosApp.UsuarioLogado == null)
        {
            return;
        }

        bool possuiPet = DadosApp.Pets
            .Any(p => p.UsuarioID == DadosApp.UsuarioLogado.ID);

        if (!possuiPet)
        {
            await DisplayAlertAsync("Nenhum PET cadastrado","Nenhum PET cadastrado. Cadastre um PET antes de realizar um agendamento.","OK");

            await Navigation.PushAsync(new frmCadastroPet());
            return;
        }

        await Navigation.PushAsync(new frmCadastroAgendamento());
    }
    private async void btnCancelar_Clicked(object sender, EventArgs e)
    {
        if (sender is not Button botao ||
            botao.CommandParameter is not Agendamento agendamento)
        {
            return;
        }

        bool confirmar = await DisplayAlertAsync("Cancelar Agendamento","Deseja realmente cancelar este agendamento?","Cancelar","Voltar");

        if (!confirmar)
            return;

        agendamento.Status = "Cancelado";

        CarregarAgendamentos();

        await DisplayAlertAsync("Agendamento","Agendamento cancelado com sucesso.","OK");
    }
    private async void btnSair_Clicked(object sender, EventArgs e)
    {
        DadosApp.UsuarioLogado = null;
        await Navigation.PopToRootAsync();
    }
}