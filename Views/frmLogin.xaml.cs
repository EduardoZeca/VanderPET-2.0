using VanderPET_2._0.Dados;
using VanderPET_2._0.Models;

namespace VanderPET_2._0.Views;

public partial class frmLogin : ContentPage
{
    public frmLogin()
    {
        InitializeComponent();
    }

    private async void btnLogin_Clicked(object sender, EventArgs e)
    {
        lblErro.IsVisible = false;

        string email = txtEmail.Text?.Trim() ?? "";
        string senha = txtSenha.Text ?? "";

        if (string.IsNullOrWhiteSpace(email))
        {
            MostrarErro("Informe o email.");
            txtEmail.Focus();
            return;
        }

        if (string.IsNullOrWhiteSpace(senha))
        {
            MostrarErro("Informe a senha.");
            txtSenha.Focus();
            return;
        }

        Usuario? usuario = DadosApp.Usuarios.FirstOrDefault(
            u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase)
                 && u.Senha == senha
        );

        if (usuario == null)
        {
            MostrarErro("Email ou senha inválidos.");
            return;
        }

        DadosApp.UsuarioLogado = usuario;

        await DisplayAlertAsync("Login", $"Bem-vindo, {usuario.Nome}!", "OK");
        await Navigation.PushAsync(new frmAgendamento());
    }
    private void MostrarErro(string mensagem)
    {
        lblErro.Text = mensagem;
        lblErro.IsVisible = true;
    }
    private async void lblCadastro_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new frmCadastroUsuario());
    }
    private async void lblRecuperarSenha_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new frmRecuperarSenha());
    }
}