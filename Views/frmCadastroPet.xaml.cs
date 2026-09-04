using VanderPET_2._0.Dados;
using VanderPET_2._0.Models;

namespace VanderPET_2._0.Views;

public partial class frmCadastroPet : ContentPage
{
    private Pet? petEdicao;
    public frmCadastroPet()
    {
        InitializeComponent();

        CarregarEspecies();

        dtpDataNascimento.MaximumDate = DateTime.Today;
        dtpDataNascimento.Date = DateTime.Today;

        CalcularIdade(DateTime.Today);
    }
    public frmCadastroPet(Pet pet)
    {
        InitializeComponent();

        petEdicao = pet;

        CarregarEspecies();

        dtpDataNascimento.MaximumDate = DateTime.Today;

        CarregarDadosPet();
    }
    private void CarregarEspecies()
    {
        pckEspecie.ItemsSource = DadosApp.Especies;
    }
    private void CarregarDadosPet()
    {
        if (petEdicao == null)
        {
            return;
        }

        lblTitulo.Text = "Editar PET";

        txtNome.Text = petEdicao.Nome;
        txtPeso.Text = petEdicao.Peso.ToString();

        Especie? especie = DadosApp.Especies
            .FirstOrDefault(e => e.ID == petEdicao.EspecieID);

        pckEspecie.SelectedItem = especie;

        Raca? raca = DadosApp.Racas
            .FirstOrDefault(r => r.ID == petEdicao.RacaID);

        pckRaca.SelectedItem = raca;

        dtpDataNascimento.Date = petEdicao.DataNascimento;

        CalcularIdade(petEdicao.DataNascimento);
    }
    private void pckEspecie_SelectedIndexChanged(object sender, EventArgs e)
    {
        pckRaca.SelectedItem = null;

        if (pckEspecie.SelectedItem is not Especie especieSelecionada)
        {
            pckRaca.ItemsSource = null;
            pckRaca.IsEnabled = false;
            return;
        }

        List<Raca> racasFiltradas = DadosApp.Racas
            .Where(r => r.EspecieID == especieSelecionada.ID)
            .ToList();

        pckRaca.ItemsSource = racasFiltradas;
        pckRaca.IsEnabled = true;
    }
    private void dtpDataNascimento_DateSelected(object sender, DateChangedEventArgs e)
    {
        if(e.NewDate.HasValue)
            CalcularIdade(e.NewDate.Value);
    }
    private void CalcularIdade(DateTime dataNascimento)
    {
        DateTime hoje = DateTime.Today;

        int idade = hoje.Year - dataNascimento.Year;

        if (dataNascimento.Date > hoje.AddYears(-idade))
        {
            idade--;
        }

        txtIdade.Text = idade.ToString();
    }
    private async void btnSalvar_Clicked(object sender, EventArgs e)
    {
        lblErro.IsVisible = false;

        string nome = txtNome.Text?.Trim() ?? "";
        string pesoTexto = txtPeso.Text?.Trim() ?? "";

        if (string.IsNullOrWhiteSpace(nome))
        {
            MostrarErro("Informe o nome do PET.");
            txtNome.Focus();
            return;
        }

        if (pckEspecie.SelectedItem is not Especie especieSelecionada)
        {
            MostrarErro("Selecione a espécie do PET.");
            return;
        }

        if (pckRaca.SelectedItem is not Raca racaSelecionada)
        {
            MostrarErro("Selecione a raça do PET.");
            return;
        }

        if (!double.TryParse(pesoTexto, out double peso) || peso <= 0)
        {
            MostrarErro("Informe um peso válido.");
            txtPeso.Focus();
            return;
        }

        DateTime dataNascimento = dtpDataNascimento.Date ?? DateTime.Today;

        if (dataNascimento > DateTime.Today)
        {
            MostrarErro("A data de nascimento não pode ser futura.");
            return;
        }

        if (DadosApp.UsuarioLogado == null)
        {
            MostrarErro("Nenhum usuário está logado.");
            return;
        }

        if (petEdicao == null)
        {
            int proximoID = DadosApp.Pets.Count == 0
                ? 1
                : DadosApp.Pets.Max(p => p.ID) + 1;

            Pet novoPet = new Pet
            {
                ID = proximoID,
                UsuarioID = DadosApp.UsuarioLogado.ID,
                Nome = nome,
                EspecieID = especieSelecionada.ID,
                RacaID = racaSelecionada.ID,
                Peso = peso,
                DataNascimento = dataNascimento
            };

            DadosApp.Pets.Add(novoPet);

            await DisplayAlertAsync(
                "PET cadastrado",
                "PET cadastrado com sucesso.",
                "OK"
            );
        }
        else
        {
            petEdicao.Nome = nome;
            petEdicao.EspecieID = especieSelecionada.ID;
            petEdicao.RacaID = racaSelecionada.ID;
            petEdicao.Peso = peso;
            petEdicao.DataNascimento = dataNascimento;

            await DisplayAlertAsync(
                "PET atualizado",
                "PET atualizado com sucesso.",
                "OK"
            );
        }

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