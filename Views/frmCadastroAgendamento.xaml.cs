using System.Globalization;
using VanderPET_2._0.Dados;
using VanderPET_2._0.Models;

namespace VanderPET_2._0.Views;

public partial class frmCadastroAgendamento : ContentPage
{
    private readonly TimeSpan horarioAbertura = new TimeSpan(8, 0, 0);
    private readonly TimeSpan horarioFechamento = new TimeSpan(18, 0, 0);
    public frmCadastroAgendamento()
    {
        InitializeComponent();

        CarregarPortes();
        ConfigurarDataHorario();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();

        CarregarPets();
    }
    private void CarregarPets()
    {
        if (DadosApp.UsuarioLogado == null)
        {
            pckPet.ItemsSource = null;
            return;
        }

        List<Pet> petsUsuario = DadosApp.Pets
            .Where(p => p.UsuarioID == DadosApp.UsuarioLogado.ID)
            .ToList();

        pckPet.ItemsSource = petsUsuario;
    }
    private void CarregarPortes()
    {
        pckPorte.ItemsSource = new List<string>
        {
            "Pequeno",
            "Médio",
            "Grande"
        };
    }
    private void ConfigurarDataHorario()
    {
        DateTime amanha = DateTime.Today.AddDays(1);

        dtpData.MinimumDate = amanha;
        dtpData.Date = amanha;

        tmpHorario.Time = horarioAbertura;
    }
    private bool ExisteConflitoHorario(DateTime data, TimeSpan horaInicio, Servico novoServico)
    {
        TimeSpan horaFim = horaInicio.Add(
            TimeSpan.FromMinutes(novoServico.TempoMinutos)
        );

        foreach (Agendamento agendamento in DadosApp.Agendamentos)
        {
            if (agendamento.Data.Date != data.Date)
            {
                continue;
            }

            if (agendamento.Status.Equals(
                "Cancelado",
                StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            Servico? servicoExistente = DadosApp.Servicos
                .FirstOrDefault(s => s.ID == agendamento.ServicoID);

            if (servicoExistente == null)
            {
                continue;
            }

            TimeSpan inicioExistente = agendamento.Hora;

            TimeSpan fimExistente = inicioExistente.Add(
                TimeSpan.FromMinutes(servicoExistente.TempoMinutos)
            );

            bool existeSobreposicao =
                horaInicio < fimExistente &&
                horaFim > inicioExistente;

            if (existeSobreposicao)
            {
                return true;
            }
        }

        return false;
    }
    private void pckPorte_SelectedIndexChanged(object sender, EventArgs e)
    {
        pckServico.SelectedItem = null;
        txtValor.Text = "";

        if (pckPorte.SelectedItem is not string porteSelecionado)
        {
            pckServico.ItemsSource = null;
            pckServico.IsEnabled = false;
            return;
        }

        List<Servico> servicosFiltrados = DadosApp.Servicos
            .Where(s => s.Porte == porteSelecionado)
            .ToList();

        pckServico.ItemsSource = servicosFiltrados;
        pckServico.IsEnabled = true;
    }
    private void pckServico_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (pckServico.SelectedItem is not Servico servicoSelecionado)
        {
            txtValor.Text = "";
            return;
        }

        txtValor.Text = servicoSelecionado.Valor.ToString(
            "C2",
            CultureInfo.GetCultureInfo("pt-BR")
        );
    }
    private async void btnVerPets_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new frmPets());
    }
    private async void btnAgendar_Clicked(object sender, EventArgs e)
    {
        lblErro.IsVisible = false;

        if (DadosApp.UsuarioLogado == null)
        {
            MostrarErro("Nenhum usuário está logado.");
            return;
        }

        if (pckPet.SelectedItem is not Pet petSelecionado)
        {
            MostrarErro("Selecione um PET.");
            return;
        }

        if (pckPorte.SelectedItem is not string porteSelecionado)
        {
            MostrarErro("Selecione o porte.");
            return;
        }

        if (pckServico.SelectedItem is not Servico servicoSelecionado)
        {
            MostrarErro("Selecione o serviço.");
            return;
        }

        if (!dtpData.Date.HasValue)
        {
            MostrarErro("Selecione a data.");
            return;
        }

        DateTime dataSelecionada = dtpData.Date.Value;

        if (dataSelecionada < DateTime.Today.AddDays(1))
        {
            MostrarErro("O agendamento deve ser realizado a partir de amanhã.");
            return;
        }

        if (!tmpHorario.Time.HasValue)
        {
            MostrarErro("Selecione o horário.");
            return;
        }

        TimeSpan horarioSelecionado = tmpHorario.Time.Value;

        if (horarioSelecionado < horarioAbertura ||
            horarioSelecionado >= horarioFechamento)
        {
            MostrarErro(
                "Selecione um horário entre 08:00 e 18:00."
            );
            return;
        }
        
        TimeSpan horarioFim = horarioSelecionado.Add(TimeSpan.FromMinutes(servicoSelecionado.TempoMinutos));

        if (horarioFim > horarioFechamento)
        {
            MostrarErro(
                "O serviço deve terminar até às 18:00."
            );
            return;
        }

        if (ExisteConflitoHorario(dataSelecionada, horarioSelecionado, servicoSelecionado))
        {
            MostrarErro(
                "Já existe um agendamento ocupando este horário."
            );
            return;
        }

        int proximoID = DadosApp.Agendamentos.Count == 0
            ? 1
            : DadosApp.Agendamentos.Max(a => a.ID) + 1;

        Agendamento novoAgendamento = new Agendamento
        {
            ID = proximoID,
            UsuarioID = DadosApp.UsuarioLogado.ID,
            PetID = petSelecionado.ID,
            ServicoID = servicoSelecionado.ID,
            Porte = porteSelecionado,
            Valor = servicoSelecionado.Valor,
            Data = dataSelecionada,
            Hora = horarioSelecionado,
            Observacoes = txtObservacoes.Text?.Trim() ?? "",
            Status = "Agendado"
        };

        DadosApp.Agendamentos.Add(novoAgendamento);

        await DisplayAlertAsync("Agendamento","Agendamento realizado com sucesso.","OK");
        await Navigation.PopAsync();
    }
    private void MostrarErro(string mensagem)
    {
        lblErro.Text = mensagem;
        lblErro.IsVisible = true;
    }
    private async void btnVoltar_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}