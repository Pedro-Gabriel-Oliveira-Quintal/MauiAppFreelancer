using SQLite;

namespace MauiApp1.Models
{
    public class Autonomo
    {
        [PrimaryKey]
        public int idAutonomo {  get; set; }

        [Unique, NotNull]
        public string login { get; set; }

        [NotNull]
        public string senha { get; set; }
    }
}
