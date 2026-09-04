using System.Net.Mail;
using VanderPET_2._0.Dados;
using VanderPET_2._0.Models;
namespace VanderPET_2._0.Views;
public partial class frmCadastroUsuario : ContentPage
{
    public frmCadastroUsuario()
    {
        InitializeComponent();
    }
    private async void btnCadastrar_Clicked(object sender, EventArgs e)
    {
        lblErro.IsVisible = false;

        string nome = txtNome.Text?.Trim() ?? "";
        string email = txtEmail.Text?.Trim() ?? "";
        string cpf = txtCPF.Text?.Trim() ?? "";
        string telefone = txtTelefone.Text?.Trim() ?? "";
        string cep = txtCEP.Text?.Trim() ?? "";
        string endereco = txtEndereco.Text?.Trim() ?? "";
        string senha = txtSenha.Text ?? "";
        string confirmarSenha = txtConfirmarSenha.Text ?? "";

        if (string.IsNullOrWhiteSpace(nome))
        {
            MostrarErro("Informe seu nome.");
            txtNome.Focus();
            return;
        }
        if (string.IsNullOrWhiteSpace(email))
        {
            MostrarErro("Informe seu email.");
            txtEmail.Focus();
            return;
        }
        if (!EmailValido(email))
        {
            MostrarErro("Informe um email válido.");
            txtEmail.Focus();
            return;
        }
        if (DadosApp.Usuarios.Any(
            u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase)))
        {
            MostrarErro("Este email já está cadastrado.");
            txtEmail.Focus();
            return;
        }
        if (SomenteNumeros(cpf).Length != 11)
        {
            MostrarErro("Informe um CPF válido.");
            txtCPF.Focus();
            return;
        }
        if (DadosApp.Usuarios.Any(u => u.CPF == cpf))
        {
            MostrarErro("Este CPF já está cadastrado.");
            txtCPF.Focus();
            return;
        }
        if (SomenteNumeros(telefone).Length != 11)
        {
            MostrarErro("Informe um telefone válido.");
            txtTelefone.Focus();
            return;
        }
        if (SomenteNumeros(cep).Length != 8)
        {
            MostrarErro("Informe um CEP válido.");
            txtCEP.Focus();
            return;
        }
        if (string.IsNullOrWhiteSpace(endereco))
        {
            MostrarErro("Informe seu endereço.");
            txtEndereco.Focus();
            return;
        }
        if (string.IsNullOrWhiteSpace(senha))
        {
            MostrarErro("Informe uma senha.");
            txtSenha.Focus();
            return;
        }
        if (senha.Length < 6)
        {
            MostrarErro("A senha deve possuir pelo menos 6 caracteres.");
            txtSenha.Focus();
            return;
        }
        if (senha != confirmarSenha)
        {
            MostrarErro("As senhas não coincidem.");
            txtConfirmarSenha.Focus();
            return;
        }

        int novoID = DadosApp.Usuarios.Count == 0
            ? 1
            : DadosApp.Usuarios.Max(u => u.ID) + 1;

        Usuario usuario = new Usuario
        {
            ID = novoID,
            Nome = nome,
            Email = email,
            CPF = cpf,
            Telefone = telefone,
            CEP = cep,
            Endereco = endereco,
            Senha = senha
        };

        DadosApp.Usuarios.Add(usuario);

        await DisplayAlertAsync("Cadastro", "Usuário cadastrado com sucesso.", "OK");

        await Navigation.PopAsync();
    }
    private void txtCPF_TextChanged(object sender, TextChangedEventArgs e)
    {
        string numeros = SomenteNumeros(e.NewTextValue);

        if (numeros.Length > 11)
            numeros = numeros[..11];

        string cpfFormatado = numeros;

        if (numeros.Length > 3)
            cpfFormatado = numeros.Insert(3, ".");
        if (numeros.Length > 6)
            cpfFormatado = cpfFormatado.Insert(7, ".");
        if (numeros.Length > 9)
            cpfFormatado = cpfFormatado.Insert(11, "-");
        if (txtCPF.Text != cpfFormatado)
        {
            txtCPF.Text = cpfFormatado;
            txtCPF.CursorPosition = cpfFormatado.Length;
        }
    }
    private void txtTelefone_TextChanged(object sender, TextChangedEventArgs e)
    {
        string numeros = SomenteNumeros(e.NewTextValue);

        if (numeros.Length > 11)
            numeros = numeros[..11];

        string telefoneFormatado = numeros;

        if (numeros.Length > 0)
            telefoneFormatado = "(" + numeros;
        if (numeros.Length > 2)
            telefoneFormatado = telefoneFormatado.Insert(3, ") ");
        if (numeros.Length > 7)
            telefoneFormatado = telefoneFormatado.Insert(10, "-");
        if (txtTelefone.Text != telefoneFormatado)
        {
            txtTelefone.Text = telefoneFormatado;
            txtTelefone.CursorPosition = telefoneFormatado.Length;
        }
    }
    private void txtCEP_TextChanged(object sender, TextChangedEventArgs e)
    {
        string numeros = SomenteNumeros(e.NewTextValue);

        if (numeros.Length > 8)
            numeros = numeros[..8];

        string cepFormatado = numeros;

        if (numeros.Length > 5)
            cepFormatado = numeros.Insert(5, "-");
        if (txtCEP.Text != cepFormatado)
        {
            txtCEP.Text = cepFormatado;
            txtCEP.CursorPosition = cepFormatado.Length;
        }
    }
    private static string SomenteNumeros(string? texto)
    {
        if (string.IsNullOrEmpty(texto))
            return "";

        return new string(texto.Where(char.IsDigit).ToArray());
    }
    private static bool EmailValido(string email)
    {
        try
        {
            MailAddress enderecoEmail = new MailAddress(email);
            return enderecoEmail.Address == email;
        }
        catch
        {
            return false;
        }
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