using SQLite;

namespace MauiApplication
{
    public class Cours
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Horaires { get; set; }
        public string Salle { get; set; }
        public string Enseignants { get; set; } // Si plusieurs enseignants, utilisez un format JSON ou une autre solution
        public decimal Prix { get; set; }
    }
}
