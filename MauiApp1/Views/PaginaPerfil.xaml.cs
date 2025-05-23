using MauiApp1.Helpers;
using projetoIII.Helpers;
using projetoIII.ViewModels;


namespace MauiApp1.Views;

public partial class PaginaPerfil : ContentPage
{
	public PaginaPerfil()
	{
		InitializeComponent();
        LoadPerfil();
	}
    protected override void OnAppearing()
    {
        base.OnAppearing();
        LoadPerfil(); // Garante que os dados sejam recarregados sempre que a página aparecer
    }

    private async void LoadPerfil()
    {
        var perfil = await App.Db.GetPerfilPorUsuarioId(Sessao.IdUsuarioLogado);

        if (perfil != null)
        {
            BindingContext = new PerfilViewModel(
                perfil.nomeExibicao,
                perfil.biografia,
                perfil.documentos // Aqui deve vir "CPF" ou "CNPJ"
);
        }
        else
        {
            await DisplayAlert("Perfil", "Nenhum perfil configurado ainda.", "OK");
        }
    }

    public PaginaPerfil(string nome, string biografia, string tipoConta)
    {
        InitializeComponent();
        BindingContext = new PerfilViewModel(nome, biografia, tipoConta);
    }
    private async void OnEditarPerfilClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ConfiguracaoPerfilPage());
    }

    //Foto Perfil
    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        try
        {
            var resultado = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
            {
                Title = "Escolha uma imagem"
            });

            if (resultado != null)
            {
                var stream = await resultado.OpenReadAsync();
                minhaImagem.Source = ImageSource.FromStream(() => stream);
            }
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Erro", ex.Message, "OK");
        }
    }

    
}