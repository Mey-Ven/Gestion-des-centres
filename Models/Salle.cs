using SQLite;

namespace MauiApplication
{
    public class Salle
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string NumeroSalle { get; set; }
    }
}
