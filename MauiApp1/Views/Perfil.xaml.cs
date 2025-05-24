namespace MauiApp1.Views;

public partial class Perfil : ContentPage
{
    internal int idUsuario;
    internal string nomeExibicao;
    internal string fotoPerfil;
    internal string documentos;
    internal string biografia;

    public Perfil()
    {
        InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);

    }
}