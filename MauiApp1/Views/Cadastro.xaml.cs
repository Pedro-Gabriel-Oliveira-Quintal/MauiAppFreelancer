using MauiApp1.Models;
using System.Threading.Tasks;

namespace MauiApp1.Views;

public partial class Cadastro : ContentPage
{
	public Cadastro()
	{
		InitializeComponent();
	}
    private async void Button_Clicked(object sender, EventArgs e)
    {
		await Navigation.PopAsync();
    }

    private async void Button_Clicked_1(object sender, EventArgs e)
    {
        try
        {
            string cpfCnpjLimpo = new string(txt_cpf_cnpj.Text.Where(char.IsDigit).ToArray());

            TipoUsuario tipo;
            if (cpfCnpjLimpo.Length == 11)
            {
                tipo = TipoUsuario.autonomo;
            }
            else if (cpfCnpjLimpo.Length == 14)
            {
                tipo = TipoUsuario.empresa;
            }
            else
            {
                throw new Exception("CPF ou CNPJ inválido.");
            }

            if (!DateTime.TryParse(txt_data_nasc.Text, out DateTime dataNasc))
            {
                throw new Exception("Data de nascimento inválida.");
                
            }

            // Data mínima aceitável (ex: 01/01/1900)
            DateTime dataMinima = new DateTime(1900, 1, 1);

            if (dataNasc > DateTime.Now.AddYears(-18) || dataNasc < dataMinima)
            {
                throw new Exception("Você precisa ser maior de 18 anos, e a data deve ser válida para se cadastrar.");
            }

            string cepLimpo = new string(txt_cep.Text.Where(char.IsDigit).ToArray());
            if (cepLimpo.Length < 8)
            {
                throw new Exception("CEP inválido.");
            }

            if (string.IsNullOrWhiteSpace(txt_email.Text) || !txt_email.Text.Contains("@") || !txt_email.Text.Contains("."))
            {
               throw new Exception("E-mail inválido.");
                
            }

            if (txt_senha.Text.Length < 8)
            {
                throw new Exception ("A senha precisa conter no mínimo 8 caracteres");
                
            }


            Usuario u = new Usuario
            {
                nome = txt_nome_usuario.Text,
                dataNascimento = dataNasc,
                endereco = txt_endereco.Text,
                cep = txt_cep.Text,
                bairro = txt_bairro.Text,
                CPF_CNPJ = txt_cpf_cnpj.Text,
                telefone = txt_telefone.Text,
                email = txt_email.Text,
                senha = txt_senha.Text,
                tipo = tipo
            };

            await App.Db.InsertUsuario(u);
            await DisplayAlert("Sucesso!", "Conta criada", "OK");

            if (tipo == TipoUsuario.autonomo)
            {
                Autonomo autonomo = new Autonomo
                {
                    idAutonomo = u.idUsuario,
                    login = u.email,
                    senha = u.senha
                };
                await App.Db.InsertAutonomo(autonomo);
            }
            else if (tipo == TipoUsuario.empresa)
            {
                Empresa empresa = new Empresa
                {
                    idEmpresa = u.idUsuario,
                    login = u.email,
                    senha = u.senha
                };
                await App.Db.InsertEmpresa(empresa);
            }

            Perfil perfil = new Perfil
            {
                idUsuario = u.idUsuario,
                nomeExibicao = u.nome,
                fotoPerfil = "",
                documentos = "",
                biografia = ""
            };
            await App.Db.InsertPerfil(perfil);

        }catch(Exception ex)
        {
            if (ex.Message.Contains("UNIQUE constraint failed"))
            {
                if (ex.Message.Contains("email"))
                {
                    await DisplayAlert("Erro", "E-mail já cadastrado.", "OK");
                }
                else if (ex.Message.Contains("CPF_CNPJ"))
                {
                    await DisplayAlert("Erro", "CPF ou CNPJ já cadastrado.", "OK");
                }
            }
            else
            {
                await DisplayAlert("Erro", ex.Message, "OK");
            }
        }
    }

    private System.Timers.Timer _cpfCnpjTimer;

    private void txt_cpf_cnpj_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (_cpfCnpjTimer != null)
        {
            _cpfCnpjTimer.Stop();
            _cpfCnpjTimer.Dispose();
        }

        _cpfCnpjTimer = new System.Timers.Timer(300); // 300ms
        _cpfCnpjTimer.Elapsed += (s, args) =>
        {
            _cpfCnpjTimer.Stop();

            MainThread.BeginInvokeOnMainThread(() =>
            {
                var entry = (Entry)sender;
                string digitos = new string(entry.Text.Where(char.IsDigit).ToArray());

                if (digitos.Length == 11)
                {
                    entry.Text = $"{digitos[..3]}.{digitos[3..6]}.{digitos[6..9]}-{digitos[9..]}";
                }
                else if (digitos.Length == 14)
                {
                    entry.Text = $"{digitos[..2]}.{digitos[2..5]}.{digitos[5..8]}/{digitos[8..12]}-{digitos[12..]}";
                }
                else
                {
                    entry.Text = digitos;
                }

                entry.CursorPosition = entry.Text.Length;
            });
        };

        _cpfCnpjTimer.Start();
    }


    private System.Timers.Timer _cepTimer;
    private void txt_cep_TextChanged(object sender, TextChangedEventArgs e)
    {

        if (_cepTimer != null)
        {
            _cepTimer.Stop();
            _cepTimer.Dispose();
        }


        _cepTimer = new System.Timers.Timer(300);
        _cepTimer.Elapsed += (s, args) =>
        {
            _cepTimer.Stop();
            MainThread.BeginInvokeOnMainThread(() =>
            {
                var entry = (Entry)sender;
                string digitos = new string(e.NewTextValue.Where(char.IsDigit).ToArray());

                if (digitos.Length == 8)
                {
                    entry.Text = $"{digitos.Substring(0, 5)}-{digitos.Substring(5)}";
                }
                else
                {
                    entry.Text = digitos;
                }

                entry.CursorPosition = entry.Text.Length;
            });
        };

        _cepTimer.Start();
    }

    private System.Timers.Timer _telefoneTimer;
    private void txt_telefone_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (_telefoneTimer != null)
        {
            _telefoneTimer.Stop();
            _telefoneTimer.Dispose();
        }

        _telefoneTimer = new System.Timers.Timer(300);
        _telefoneTimer.Elapsed += (s, args) =>
        {
            _telefoneTimer.Stop();
            MainThread.BeginInvokeOnMainThread(() =>
            {
                var entry = (Entry)sender;
                string digitos = new string(e.NewTextValue.Where(char.IsDigit).ToArray());

                if (digitos.Length == 11)
                {
                    entry.Text = $"({digitos.Substring(0, 2)}) {digitos.Substring(2, 5)}-{digitos.Substring(7)}";
                }
                else
                {
                    entry.Text = digitos;
                }

                entry.CursorPosition = entry.Text.Length;
            });
        };

        _telefoneTimer.Start();
    }

    private System.Timers.Timer _dataNascTimer;
    private void txt_data_nasc_TextChanged(object sender, TextChangedEventArgs e)
    {
        if(_dataNascTimer != null)
        {
            _dataNascTimer.Stop();
            _dataNascTimer.Dispose();
        }

        _dataNascTimer = new System.Timers.Timer(300);
        _dataNascTimer.Elapsed += (s, args) =>
        {
            _dataNascTimer.Stop();
            MainThread.BeginInvokeOnMainThread(() =>
            {
                var entry = (Entry)sender;
                string digitos = new string(e.NewTextValue.Where(char.IsDigit).ToArray());

                if (digitos.Length == 8)
                {
                    entry.Text = $"{digitos.Substring(0, 2)}/{digitos.Substring(2, 2)}/{digitos.Substring(4)}";
                }
                else
                {
                    entry.Text = digitos;
                }

                entry.CursorPosition = entry.Text.Length;
            });
        };

        _dataNascTimer.Start();
    }
}