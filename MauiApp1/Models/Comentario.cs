using SQLite;

namespace MauiApp1.Models
{
    public class Comentario
    {
        [PrimaryKey, AutoIncrement]
        public int idComentario { get; set; }

        [NotNull]
        public int idUsuario { get; set; }

        [NotNull]
        public int idPostagem { get; set; }

        [NotNull]
        public string conteudo { get; set; }
        public DateTime data_comentario { get; set; } = DateTime.Now;
    }
}
