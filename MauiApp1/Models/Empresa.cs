using SQLite;

namespace MauiApp1.Models
{
    public class Empresa
    {
        [PrimaryKey]
        public int idEmpresa { get; set; }

        [Unique, NotNull]
        public string login { get; set; }

        [NotNull]
        public string senha { get; set; }
    }
}
