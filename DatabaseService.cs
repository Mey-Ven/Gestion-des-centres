using SQLite;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using MauiApplication;


namespace MauiApplication
{
    public class DatabaseService
    {
        private readonly SQLiteAsyncConnection _database;

        public DatabaseService()
        {
            // Définir le chemin de la base de données
            var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CentreCours.db");
            _database = new SQLiteAsyncConnection(dbPath);

            // Créer les tables
            _database.CreateTableAsync<Etudiant>().Wait();
            _database.CreateTableAsync<Professeur>().Wait();
            _database.CreateTableAsync<Salle>().Wait();
            _database.CreateTableAsync<Cours>().Wait();
        }

        // Méthodes pour gérer les Étudiants
        public Task<int> AjouterEtudiant(Etudiant etudiant)
        {
            return _database.InsertAsync(etudiant);
        }

        public Task<List<Etudiant>> ObtenirEtudiants()
        {
            return _database.Table<Etudiant>().ToListAsync();
        }

        public Task<int> SupprimerEtudiant(int id)
        {
            return _database.DeleteAsync<Etudiant>(id);
        }

        public Task<int> ModifierEtudiant(Etudiant etudiant)
        {
            return _database.UpdateAsync(etudiant);
        }
        
    }
}
