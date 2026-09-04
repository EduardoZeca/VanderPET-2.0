using VanderPET_2._0.Dados;
using VanderPET_2._0.Models;

namespace VanderPET_2._0.Views;

public partial class frmPets : ContentPage
{
    public frmPets()
    {
        InitializeComponent();
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
            cvPets.ItemsSource = null;
            return;
        }

        List<PetListaItem> pets = DadosApp.Pets
            .Where(p => p.UsuarioID == DadosApp.UsuarioLogado.ID)
            .Select(p => new PetListaItem
            {
                Pet = p,
                Nome = p.Nome,
                Raca = BuscarNomeRaca(p.RacaID),
                Idade = FormatarIdade(p.DataNascimento)
            })
            .ToList();

        cvPets.ItemsSource = pets;
    }
    private string BuscarNomeRaca(int racaID)
    {
        Raca? raca = DadosApp.Racas
            .FirstOrDefault(r => r.ID == racaID);

        return raca?.Nome ?? "Não informada";
    }
    private string FormatarIdade(DateTime dataNascimento)
    {
        DateTime hoje = DateTime.Today;
        int idade = hoje.Year - dataNascimento.Year;

        if (dataNascimento.Date > hoje.AddYears(-idade))
        {
            idade--;
        }
        if (idade == 1)
        {
            return "1 ano";
        }

        return $"{idade} anos";
    }
    private async void btnNovoPet_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new frmCadastroPet());
    }
    private async void btnEditar_Clicked(object sender, EventArgs e)
    {
        if (sender is Button botao &&
            botao.CommandParameter is Pet pet)
        {
            await Navigation.PushAsync(new frmCadastroPet(pet));
        }
    }
    private async void btnExcluir_Clicked(object sender, EventArgs e)
    {
        if (sender is not Button botao ||
            botao.CommandParameter is not Pet pet)
        {
            return;
        }

        bool confirmar = await DisplayAlertAsync(
            "Excluir PET",
            $"Deseja realmente excluir {pet.Nome}?",
            "Excluir",
            "Cancelar"
        );

        if (!confirmar)
        {
            return;
        }

        DadosApp.Pets.Remove(pet);

        CarregarPets();
    }
}