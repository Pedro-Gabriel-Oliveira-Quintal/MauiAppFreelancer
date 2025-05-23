using Microsoft.Maui.Controls;
using MauiApp1.Helpers;
using MauiApp1.Models;
using MauiApp1.Views;
using System.Reflection;
using MauiApp1.Helpers;
using projetoIII.Helpers;

namespace MauiApp1.Views
{
    public partial class ConfiguracaoPerfilPage : ContentPage
    {
        public ConfiguracaoPerfilPage()
        {
            InitializeComponent();
            Shell.SetNavBarIsVisible(this, false);
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Busca perfil do usuário logado
            var perfil = await App.Db.GetPerfilPorUsuarioId(Sessao.IdUsuarioLogado);

            if (perfil != null)
            {
                entryNome.Text = perfil.nomeExibicao;
                entryBiografia.Text = perfil.biografia;

                // Define o tipo de conta nos radio buttons
                if (perfil.documentos == "CNPJ" || perfil.documentos?.ToUpper() == "CNPJ")
                {
                    radioCnpj.IsChecked = true;
                }
                else
                {
                    radioCpf.IsChecked = true;
                }
            }
        }


        private async void OnSalvarClicked(object sender, EventArgs e)
        {
            try
            {
                string nome = entryNome.Text?.Trim();
                string biografia = entryBiografia.Text?.Trim();

                // Determina se foi selecionado CPF ou CNPJ
                string documentos = (radioCpf?.IsChecked ?? false) ? "CPF" :
                                    (radioCnpj?.IsChecked ?? false) ? "CNPJ" : "";

                // Valida que todos os campos foram preenchidos
                if (string.IsNullOrWhiteSpace(nome) ||
                    string.IsNullOrWhiteSpace(biografia) ||
                    string.IsNullOrWhiteSpace(documentos))
                {
                    await DisplayAlert("Erro", "Preencha todos os campos e selecione se é CPF ou CNPJ.", "OK");
                    return;
                }

                // Cria ou atualiza o perfil
                var perfil = new Models.Perfil
                {
                    idUsuario = Sessao.IdUsuarioLogado,
                    nomeExibicao = nome,
                    biografia = biografia,
                    documentos = documentos
                };

                var perfilExistente = await App.Db.GetPerfilPorUsuarioId(Sessao.IdUsuarioLogado);

                if (perfilExistente == null)
                {
                    await App.Db.InsertPerfil(perfil);
                }
                else
                {
                    perfil.idPerfil = perfilExistente.idPerfil;
                    await App.Db.UpdatePerfil(perfil);
                }

                await DisplayAlert("Sucesso", "Perfil salvo com sucesso!", "OK");
                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", $"Erro ao salvar perfil: {ex.Message}", "OK");
            }
        }

        private async void OnVoltarClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
        private async void OnLogoutClicked(object sender, EventArgs e)
        {



            Application.Current.MainPage = new NavigationPage(new Views.Login());


        }
    }
}