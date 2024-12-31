using System;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Maui.Controls;

namespace MauiApplication
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BuildUI();
        }

        private void BuildUI()
        {
            var etudiantsButton = new Button
            {
                Text = "Étudiants",
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            etudiantsButton.Clicked += DisplayEtudiantsMenu;

            var enseignantsButton = new Button
            {
                Text = "Enseignants",
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            enseignantsButton.Clicked += DisplayEnseignantsMenu;

            var sallesButton = new Button
            {
                Text = "Salles",
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            sallesButton.Clicked += DisplaySallesMenu;

            var coursButton = new Button
            {
                Text = "Cours",
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            coursButton.Clicked += DisplayCoursMenu;

            Content = new StackLayout
            {
                Padding = 20,
                Children = { etudiantsButton, enseignantsButton, sallesButton, coursButton }
            };
        }

        private void RetourAuMenuPrincipal()
        {
            BuildUI();
        }


        private void DisplayEtudiantsMenu(object sender, EventArgs e)
        {
            var ajouterButton = new Button { Text = "Ajouter un étudiant" };
            ajouterButton.Clicked += (s, args) => AjouterEtudiant();

            var listeButton = new Button { Text = "Liste des étudiants" };
            listeButton.Clicked += (s, args) => ListeEtudiants();

            var retourButton = new Button { Text = "Retour" };
            retourButton.Clicked += (s, args) => RetourAuMenuPrincipal();

            Content = new StackLayout
            {
                Padding = 20,
                Children = { ajouterButton, listeButton, retourButton }
            };
        }


        private void AjouterEtudiant()
        {
            var nomEntry = new Entry { Placeholder = "Nom" };
            var prenomEntry = new Entry { Placeholder = "Prénom" };
            var niveauEntry = new Entry { Placeholder = "Niveau Scolaire" };
            var parentEntry = new Entry { Placeholder = "Numéro du Parent" };

            var saveButton = new Button { Text = "Enregistrer" };
            saveButton.Clicked += (s, args) =>
            {
                DisplayAlert("Succès", $"Étudiant {nomEntry.Text} {prenomEntry.Text} ajouté avec succès", "OK");
                DisplayEtudiantsMenu(null, null);
            };

            var retourButton = new Button { Text = "Retour" };
            retourButton.Clicked += (s, args) => DisplayEtudiantsMenu(null, null);

            Content = new StackLayout
            {
                Padding = 20,
                Children = { nomEntry, prenomEntry, niveauEntry, parentEntry, saveButton, retourButton }
            };
        }



        private void ListeEtudiants()
        {
            var etudiants = new List<string> { "Alice Durand", "Paul Morel" }; // Exemple de liste statique

            var listView = new ListView { ItemsSource = etudiants };

            var retourButton = new Button { Text = "Retour" };
            retourButton.Clicked += (s, args) => DisplayEtudiantsMenu(null, null);

            Content = new StackLayout
            {
                Padding = 20,
                Children = { listView, retourButton }
            };
        }




        private void DisplayEnseignantsMenu(object sender, EventArgs e)
        {
            var ajouterButton = new Button { Text = "Ajouter un enseignant" };
            ajouterButton.Clicked += (s, args) => AjouterEnseignant();

            var listeButton = new Button { Text = "Liste des enseignants" };
            listeButton.Clicked += (s, args) => ListeEnseignants();

            var retourButton = new Button { Text = "Retour" };
            retourButton.Clicked += (s, args) => RetourAuMenuPrincipal();

            Content = new StackLayout
            {
                Padding = 20,
                Children = { ajouterButton, listeButton, retourButton }
            };
        }


        private void AjouterEnseignant()
        {
            var nomEntry = new Entry { Placeholder = "Nom" };
            var prenomEntry = new Entry { Placeholder = "Prénom" };
            var matiereEntry = new Entry { Placeholder = "Matière enseignée" };
            var classesEntry = new Entry { Placeholder = "Classes où il enseigne" };
            var telephoneEntry = new Entry { Placeholder = "Numéro de téléphone" };
            var emailEntry = new Entry { Placeholder = "Email" };
            var prixEntry = new Entry { Placeholder = "Prix par cours", Keyboard = Keyboard.Numeric };

            var saveButton = new Button { Text = "Enregistrer" };
            saveButton.Clicked += (s, args) =>
            {
                DisplayAlert("Succès", $"Enseignant {nomEntry.Text} {prenomEntry.Text} ajouté avec succès", "OK");
                DisplayEnseignantsMenu(null, null);
            };

            var retourButton = new Button { Text = "Retour" };
            retourButton.Clicked += (s, args) => DisplayEnseignantsMenu(null, null);

            Content = new StackLayout
            {
                Padding = 20,
                Children = { nomEntry, prenomEntry, matiereEntry, classesEntry, telephoneEntry, emailEntry, prixEntry, saveButton, retourButton }
            };
        }


        private void ListeEnseignants()
        {
            var enseignants = new List<string> { "Jean Dupont (Maths)", "Luc Martin (Physique)" }; // Exemple de liste statique

            var listView = new ListView { ItemsSource = enseignants };

            var retourButton = new Button { Text = "Retour" };
            retourButton.Clicked += (s, args) => DisplayEnseignantsMenu(null, null);

            Content = new StackLayout
            {
                Padding = 20,
                Children = { listView, retourButton }
            };
        }




        private void DisplaySallesMenu(object sender, EventArgs e)
        {
            var ajouterButton = new Button { Text = "Ajouter une salle" };
            ajouterButton.Clicked += (s, args) => AjouterSalle();

            var listeButton = new Button { Text = "Liste des salles" };
            listeButton.Clicked += (s, args) => ListeSalles();

            var retourButton = new Button { Text = "Retour" };
            retourButton.Clicked += (s, args) => RetourAuMenuPrincipal();

            Content = new StackLayout
            {
                Padding = 20,
                Children = { ajouterButton, listeButton, retourButton }
            };
        }


        private void AjouterSalle()
        {
            var numeroEntry = new Entry { Placeholder = "Numéro de la salle" };

            var saveButton = new Button { Text = "Enregistrer" };
            saveButton.Clicked += (s, args) =>
            {
                DisplayAlert("Succès", $"Salle {numeroEntry.Text} ajoutée avec succès", "OK");
                DisplaySallesMenu(null, null);
            };

            var retourButton = new Button { Text = "Retour" };
            retourButton.Clicked += (s, args) => DisplaySallesMenu(null, null);

            Content = new StackLayout
            {
                Padding = 20,
                Children = { numeroEntry, saveButton, retourButton }
            };
        }


        private void ListeSalles()
        {
            var salles = new List<string> { "Salle 101", "Salle 102", "Salle 103" }; // Exemple de liste statique

            var listView = new ListView { ItemsSource = salles };

            var retourButton = new Button { Text = "Retour" };
            retourButton.Clicked += (s, args) => DisplaySallesMenu(null, null);

            Content = new StackLayout
            {
                Padding = 20,
                Children = { listView, retourButton }
            };
        }



        private void DisplayCoursMenu(object sender, EventArgs e)
        {
            var ajouterButton = new Button { Text = "Ajouter un cours" };
            ajouterButton.Clicked += (s, args) => AjouterCours();

            var listeButton = new Button { Text = "Liste des cours" };
            listeButton.Clicked += (s, args) => ListeCours();

            var retourButton = new Button { Text = "Retour" };
            retourButton.Clicked += (s, args) => RetourAuMenuPrincipal();

            Content = new StackLayout
            {
                Padding = 20,
                Children = { ajouterButton, listeButton, retourButton }
            };
        }


        private void AjouterCours()
        {
            var nomEntry = new Entry { Placeholder = "Nom du cours" };
            var horairesEntry = new Entry { Placeholder = "Horaires du cours" };
            var salleEntry = new Entry { Placeholder = "Salle" };
            var enseignantsEntry = new Entry { Placeholder = "Enseignants (séparés par des virgules)" };
            var prixEntry = new Entry { Placeholder = "Prix", Keyboard = Keyboard.Numeric };

            var saveButton = new Button { Text = "Enregistrer" };
            saveButton.Clicked += (s, args) =>
            {
                DisplayAlert("Succès", $"Cours {nomEntry.Text} ajouté avec succès", "OK");
                DisplayCoursMenu(null, null);
            };

            var retourButton = new Button { Text = "Retour" };
            retourButton.Clicked += (s, args) => DisplayCoursMenu(null, null);

            Content = new StackLayout
            {
                Padding = 20,
                Children = { nomEntry, horairesEntry, salleEntry, enseignantsEntry, prixEntry, saveButton, retourButton }
            };
        }


        private void ListeCours()
        {
            var cours = new List<string> { "Mathématiques - 8h-10h - Salle 101", "Physique - 10h-12h - Salle 102" }; // Exemple de liste statique

            var listView = new ListView { ItemsSource = cours };

            var retourButton = new Button { Text = "Retour" };
            retourButton.Clicked += (s, args) => DisplayCoursMenu(null, null);

            Content = new StackLayout
            {
                Padding = 20,
                Children = { listView, retourButton }
            };
        }


    }


}
