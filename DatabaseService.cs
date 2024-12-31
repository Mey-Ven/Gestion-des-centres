using SQLite;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

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

            // Créer les tables pour chaque entité
            _database.CreateTableAsync<Etudiant>().Wait();
            _database.CreateTableAsync<Professeur>().Wait();
            _database.CreateTableAsync<Salle>().Wait();
            _database.CreateTableAsync<Cours>().Wait();
        }

        // Méthodes pour Étudiants
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

        // Méthodes pour Enseignants
        public Task<int> AjouterProfesseur(Professeur professeur)
        {
            return _database.InsertAsync(professeur);
        }

        public Task<List<Professeur>> ObtenirProfesseurs()
        {
            return _database.Table<Professeur>().ToListAsync();
        }

        public Task<int> SupprimerProfesseur(int id)
        {
            return _database.DeleteAsync<Professeur>(id);
        }

        public Task<int> ModifierProfesseur(Professeur professeur)
        {
            return _database.UpdateAsync(professeur);
        }

        // Méthodes pour Salles
        public Task<int> AjouterSalle(Salle salle)
        {
            return _database.InsertAsync(salle);
        }

        public Task<List<Salle>> ObtenirSalles()
        {
            return _database.Table<Salle>().ToListAsync();
        }

        public Task<int> SupprimerSalle(int id)
        {
            return _database.DeleteAsync<Salle>(id);
        }

        public Task<int> ModifierSalle(Salle salle)
        {
            return _database.UpdateAsync(salle);
        }

        // Méthodes pour Cours
        public Task<int> AjouterCours(Cours cours)
        {
            return _database.InsertAsync(cours);
        }

        public Task<List<Cours>> ObtenirCours()
        {
            return _database.Table<Cours>().ToListAsync();
        }

        public Task<int> SupprimerCours(int id)
        {
            return _database.DeleteAsync<Cours>(id);
        }

        public Task<int> ModifierCours(Cours cours)
        {
            return _database.UpdateAsync(cours);
        }
    }
}
