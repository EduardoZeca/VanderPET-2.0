using VanderPET_2._0.Dados;
using VanderPET_2._0.Models;

namespace VanderPET_2._0.Views;

public partial class frmRecuperarSenha : ContentPage
{
    public frmRecuperarSenha()
    {
        InitializeComponent();
    }
    private async void btnRecuperar_Clicked(object sender, EventArgs e)
    {
        lblErro.IsVisible = false;

        string email = txtEmail.Text?.Trim() ?? "";

        if (string.IsNullOrWhiteSpace(email))
        {
            MostrarErro("Informe seu email.");
            txtEmail.Focus();
            return;
        }

        Usuario? usuario = DadosApp.Usuarios.FirstOrDefault(
            u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase)
        );

        if (usuario == null)
        {
            MostrarErro("Email não encontrado.");
            txtEmail.Focus();
            return;
        }

        await DisplayAlertAsync("Recuperação de Senha", "As instruções para recuperação da senha foram enviadas para o seu email.", "OK");

        await Navigation.PopAsync();
    }
    private void MostrarErro(string mensagem)
    {
        lblErro.Text = mensagem;
        lblErro.IsVisible = true;
    }
    private async void lblVoltar_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PopAsync();
    }
}