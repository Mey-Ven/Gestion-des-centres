using SQLite;
using System.Text.Json; // Pour gérer la sérialisation des listes

public class Professeur
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Nom { get; set; }
    public string Prenom { get; set; }
    public string Telephone { get; set; }
    public string Email { get; set; }

    // Propriété sérialisée pour gérer les cours enseignés
    public string CoursEnseignesSerialized { get; set; }

    // Propriété ignorée par SQLite pour manipuler directement une liste
    [Ignore]
    public List<string> CoursEnseignes
    {
        get => string.IsNullOrEmpty(CoursEnseignesSerialized)
            ? new List<string>()
            : JsonSerializer.Deserialize<List<string>>(CoursEnseignesSerialized);
        set => CoursEnseignesSerialized = JsonSerializer.Serialize(value);
    }
}
