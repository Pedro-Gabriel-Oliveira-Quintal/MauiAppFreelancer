using SQLite;

namespace MauiApp1.Models
{
    public enum TipoUsuario
    {
        administrador,
        empresa, 
        autonomo
    }

    public class Usuario
    {
        string _nome;
        string _endereco;
        string _bairro;
        string _cep;
        string _telefone;
        string _email;
        string _senha;
        string _CPF_CNPJ;


        [PrimaryKey, AutoIncrement]
        public int idUsuario { get; set; }

        [NotNull]
        public string nome
        {
            get => _nome;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new Exception("Por favor, preencha o nome.");
                }
                _nome = value;
            }
        }

        [NotNull]
        public DateTime dataNascimento { get; set; }
        public string endereco
        {
            get => _endereco;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new Exception("Por favor, preencha o endereço.");
                }
                _endereco = value;
            }
        }
        public string bairro
        {
            get => _bairro;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new Exception("Por favor, preencha o bairro.");
                }
                _bairro = value;
            }
        }
        public string cep
        {
            get => _cep;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new Exception("Por favor, preencha o cep.");
                }
                _cep = value;
            }
        }
        public string telefone
        {
            get => _telefone;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new Exception("Por favor, preencha o telefone.");
                }
                _telefone = value;
            }
        }

        [Unique, NotNull]
        public string email
        {
            get => _email;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new Exception("Por favor, preencha o email.");
                }
                _email = value;
            }
        }

        [NotNull]
        public string senha
        {
            get => _senha;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new Exception("Por favor, preencha a senha.");
                }
                _senha = value;
            }
        }

        [Unique, NotNull]
        public string CPF_CNPJ
        {
            get => _CPF_CNPJ;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new Exception("Por favor, preencha o CPF ou CNPJ.");
                }
                _CPF_CNPJ = value;
            }
        }

        [NotNull]
        public TipoUsuario tipo { get; set; }


    }
}
