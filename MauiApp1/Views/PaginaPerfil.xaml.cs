using MauiApp1.Helpers;
using MauiApp1.Helpers;
using MauiApp1.ViewModels;


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
            perfil.documentos
        );

        if (!string.IsNullOrEmpty(perfil.fotoPerfil) && File.Exists(perfil.fotoPerfil))
        {
            // Usa FromStream para evitar cache
            minhaImagem.Source = ImageSource.FromStream(() => File.OpenRead(perfil.fotoPerfil));
        }
        else
        {
            minhaImagem.Source = "perfil_padrao.png";
        }
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
                var nomeArquivo = $"fotoPerfil_{Sessao.IdUsuarioLogado}.jpg";
                var caminhoDestino = Path.Combine(FileSystem.AppDataDirectory, nomeArquivo);

                using (var inputStream = await resultado.OpenReadAsync())
                using (var outputStream = File.OpenWrite(caminhoDestino))
                {
                    await inputStream.CopyToAsync(outputStream);
                }

                // Limpa a imagem antes de carregar nova para evitar cache
                minhaImagem.Source = null;

                // Usa FromStream para evitar cache e bloquear arquivo
                minhaImagem.Source = ImageSource.FromStream(() => File.OpenRead(caminhoDestino));

                // Atualiza o banco
                var perfil = await App.Db.GetPerfilPorUsuarioId(Sessao.IdUsuarioLogado);
                if (perfil != null)
                {
                    perfil.fotoPerfil = caminhoDestino;
                    await App.Db.UpdatePerfil(perfil);
                }
            }
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Erro", ex.Message, "OK");
        }
    }

    
}