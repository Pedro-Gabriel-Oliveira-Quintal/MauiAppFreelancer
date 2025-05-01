using System.Threading.Tasks;

namespace MauiApp1.Views;

public partial class Login : ContentPage
{
	public Login()
	{
		InitializeComponent();
	}

    private async void Button_Clicked(object sender, EventArgs e)
    {
		try
		{
			await Navigation.PushAsync(new Views.Cadastro());
		}
		catch (Exception ex)
		{
			await DisplayAlert("Ops", ex.Message, "OK");
		}
    }

    private async void Button_Clicked_1(object sender, EventArgs e)
    {
		try
		{

            string email = txt_email.Text;
            string senha = txt_senha.Text;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(senha))
            {
				throw new Exception("Por favor, preencha todos os campos");
    
			}

			var usuario = await App.Db.GetUsuarioPorEmail(email);

			if (usuario == null)
			{
                throw new Exception("Usuário não encontrado.");
            }

			if (usuario.senha != senha)
			{
				throw new Exception("Senha incorreta");
			}

            await Navigation.PushAsync(new PaginaPerfil());
        }
        catch (Exception ex)
		{
			await DisplayAlert("Erro", ex.Message, "OK");
		}
    }
}