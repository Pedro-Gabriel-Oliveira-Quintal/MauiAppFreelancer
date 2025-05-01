using SQLite;

namespace MauiApp1.Models
{

    public enum TipoNotificacao
    {
        banimento,
        atualizacao,
        comentario,
        outro
    }
    public class Notificacao
    {
        [PrimaryKey, AutoIncrement]
        public int idNotificacao {  get; set; }

        [NotNull]
        public int idUsuarioDestinatario { get; set; }

        public int idUsuarioRemetente { get; set; }

        [NotNull]
        public string mensagem {  get; set; }

        [NotNull]
        public TipoNotificacao tipo { get; set; }

        public DateTime data_notificacao { get; set; }

        public bool Lido { get; set; }
    }
}
