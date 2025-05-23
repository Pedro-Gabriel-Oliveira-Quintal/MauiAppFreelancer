using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace projetoIII.ViewModels
{
    public class PerfilViewModel : INotifyPropertyChanged
    {
        private string nome;
        private string biografia;
        private string tipoConta;

        public string Nome
        {
            get => nome;
            set { nome = value; OnPropertyChanged(); }
        }

        public string Biografia
        {
            get => biografia;
            set { biografia = value; OnPropertyChanged(); }
        }

        public string TipoConta
        {
            get => tipoConta;
            set { tipoConta = value; OnPropertyChanged(); }
        }

        // Construtor Padrão (obrigatório para uso em XAML)
        public PerfilViewModel() { }

        // Construtor com dados
        public PerfilViewModel(string nome, string biografia, string tipoConta)
        {
            Nome = nome;
            Biografia = biografia;
            TipoConta = tipoConta;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

