using System.ComponentModel;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using System.Runtime.CompilerServices;

namespace SeuProjeto.ViewModels
{
    public class ConfigPerfil : INotifyPropertyChanged
    {
        private string nome;
        private string descricao;
        private string tipoConta;

        public string Nome
        {
            get => nome;
            set
            {
                nome = value;
                OnPropertyChanged();
            }
        }

        public string Descricao
        {
            get => descricao;
            set
            {
                descricao = value;
                OnPropertyChanged();
            }
        }

        public string TipoConta
        {
            get => tipoConta;
            set
            {
                tipoConta = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsCpfChecked));
                OnPropertyChanged(nameof(IsCnpjChecked));
            }
        }

        public bool IsCpfChecked
        {
            get => TipoConta == "CPF";
            set
            {
                if (value) TipoConta = "CPF";
            }
        }

        public bool IsCnpjChecked
        {
            get => TipoConta == "CNPJ";
            set
            {
                if (value) TipoConta = "CNPJ";
            }
        }

        public ICommand SalvarCommand { get; }

        public ConfigPerfil()
        {
            Nome = "";
            Descricao = "";
            TipoConta = "CPF";

            SalvarCommand = new Command(SalvarPerfil);
        }

        private void SalvarPerfil()
        {
            Application.Current.MainPage.DisplayAlert("Salvar", "Perfil salvo com sucesso!", "OK");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
