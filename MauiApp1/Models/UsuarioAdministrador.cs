using SQLite;
using System.ComponentModel.DataAnnotations.Schema;

namespace MauiApp1.Models
{
    public class UsuarioAdministrador
    {
        [PrimaryKey, AutoIncrement]
        public int idAdmin {  get; set; }

        [Unique, NotNull]
        public string login { get; set; }

        [NotNull]
        public string senha { get; set; }
    }
}