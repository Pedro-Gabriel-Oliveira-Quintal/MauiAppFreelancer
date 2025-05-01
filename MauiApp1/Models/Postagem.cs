using SQLite;

namespace MauiApp1.Models
{
    public class Postagem
    {
        [PrimaryKey, AutoIncrement]
        public int idPostagem { get; set; }

        [NotNull]
        public int idUsuario { get; set; }

        [NotNull]
        public string conteudo { get; set; }

        public DateTime data_publicacao { get; set; } = DateTime.Now;
    }
}
