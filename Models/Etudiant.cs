using SQLite;

namespace MauiApplication
{
    public class Etudiant
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string NiveauScolaire { get; set; }
        public string NumeroParent { get; set; }
        public string Sexe { get; set; }
        public List<string> CoursInscrits { get; set; } = new List<string>();
    }
}
