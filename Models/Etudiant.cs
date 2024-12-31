using SQLite;
using System.Text.Json; // Importez System.Text.Json pour la sérialisation

public class Etudiant
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Nom { get; set; }
    public string Prenom { get; set; }
    public string NiveauScolaire { get; set; }
    public string NumeroParent { get; set; }
    public string Sexe { get; set; }

    // Propriété sérialisée pour SQLite
    public string CoursInscritsSerialized { get; set; }

    // Propriété ignorée par SQLite pour manipuler la liste directement
    [Ignore]
    public List<string> CoursInscrits
    {
        get => string.IsNullOrEmpty(CoursInscritsSerialized)
            ? new List<string>()
            : JsonSerializer.Deserialize<List<string>>(CoursInscritsSerialized);
        set => CoursInscritsSerialized = JsonSerializer.Serialize(value);
    }
}
