using SQLite;

namespace MauiApplication
{
    public class Professeur
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Matiere { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }
        public decimal PrixCours { get; set; }
    }
}
