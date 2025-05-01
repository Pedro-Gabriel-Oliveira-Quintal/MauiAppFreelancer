using SQLite;

namespace MauiApp1.Models
{
    public class Perfil
    {
        [PrimaryKey, AutoIncrement]
        public int idPerfil {  get; set; }

        [Unique, NotNull]
        public int idUsuario { get; set; }

        [MaxLength(100)]
        public string nomeExibicao { get; set; }
        public string documentos { get; set; }
        public string fotoPerfil { get; set; }
        public string biografia { get; set; }
    }
}
