using SQLite;

public class Cours
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Nom { get; set; }
    public string Horaires { get; set; }
    public string Salle { get; set; } // Ce champ stockera le numéro de salle choisi
    public decimal Prix { get; set; }
}
