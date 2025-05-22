namespace MauiApp1.Views;

public partial class PaginaPerfil : ContentPage
{
	public PaginaPerfil()
	{
		InitializeComponent();
	}

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