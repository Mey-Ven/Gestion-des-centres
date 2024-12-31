using System;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Maui.Controls;

namespace MauiApplication
{
    public partial class MainPage : ContentPage
    {
        private readonly DatabaseService _databaseService;

        public MainPage()
        {
            InitializeComponent();
            _databaseService = new DatabaseService(); // Initialiser le service de base de données
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
            saveButton.Clicked += async (s, args) =>
            {
                var etudiant = new Etudiant
                {
                    Nom = nomEntry.Text,
                    Prenom = prenomEntry.Text,
                    NiveauScolaire = niveauEntry.Text,
                    NumeroParent = parentEntry.Text
                };

                await _databaseService.AjouterEtudiant(etudiant); // Enregistrer dans la base de données
                await DisplayAlert("Succès", "Étudiant ajouté avec succès", "OK");
                DisplayEtudiantsMenu(null, null); // Retour au menu Étudiants
            };

            var retourButton = new Button { Text = "Retour" };
            retourButton.Clicked += (s, args) => DisplayEtudiantsMenu(null, null);

            Content = new StackLayout
            {
                Padding = 20,
                Children = { nomEntry, prenomEntry, niveauEntry, parentEntry, saveButton, retourButton }
            };
        }


        private async void ListeEtudiants()
        {
            var etudiants = await _databaseService.ObtenirEtudiants(); // Récupérer les étudiants depuis la base

            var listView = new ListView
            {
                ItemsSource = etudiants,
                ItemTemplate = new DataTemplate(() =>
                {
                    var nomLabel = new Label();
                    nomLabel.SetBinding(Label.TextProperty, "Nom");

                    var prenomLabel = new Label();
                    prenomLabel.SetBinding(Label.TextProperty, "Prenom");

                    return new ViewCell
                    {
                        View = new StackLayout
                        {
                            Orientation = StackOrientation.Horizontal,
                            Children = { nomLabel, prenomLabel }
                        }
                    };
                })
            };

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
            var telephoneEntry = new Entry { Placeholder = "Téléphone" };
            var emailEntry = new Entry { Placeholder = "Email" };
            var prixEntry = new Entry { Placeholder = "Prix par cours", Keyboard = Keyboard.Numeric };

            var saveButton = new Button { Text = "Enregistrer" };
            saveButton.Clicked += async (s, args) =>
            {
                var enseignant = new Professeur
                {
                    Nom = nomEntry.Text,
                    Prenom = prenomEntry.Text,
                    Matiere = matiereEntry.Text,
                    Telephone = telephoneEntry.Text,
                    Email = emailEntry.Text,
                    PrixCours = decimal.TryParse(prixEntry.Text, out var prix) ? prix : 0
                };

                await _databaseService.AjouterProfesseur(enseignant); // Utilise AjouterProfesseur pour ajouter un enseignant
                await DisplayAlert("Succès", "Enseignant ajouté avec succès", "OK");
                DisplayEnseignantsMenu(null, null); // Retour au menu Enseignants
            };

            var retourButton = new Button { Text = "Retour" };
            retourButton.Clicked += (s, args) => DisplayEnseignantsMenu(null, null);

            Content = new StackLayout
            {
                Padding = 20,
                Children = { nomEntry, prenomEntry, matiereEntry, telephoneEntry, emailEntry, prixEntry, saveButton, retourButton }
            };
        }


        private async void ListeEnseignants()
        {
            var enseignants = await _databaseService.ObtenirProfesseurs(); // Utilisez la méthode appropriée pour les enseignants

            var listView = new ListView
            {
                ItemsSource = enseignants,
                ItemTemplate = new DataTemplate(() =>
                {
                    var nomLabel = new Label();
                    nomLabel.SetBinding(Label.TextProperty, "Nom");

                    var prenomLabel = new Label();
                    prenomLabel.SetBinding(Label.TextProperty, "Prenom");

                    return new ViewCell
                    {
                        View = new StackLayout
                        {
                            Orientation = StackOrientation.Horizontal,
                            Children = { nomLabel, prenomLabel }
                        }
                    };
                })
            };

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
            var numeroSalleEntry = new Entry { Placeholder = "Numéro de la salle" };

            var saveButton = new Button { Text = "Enregistrer" };
            saveButton.Clicked += async (s, args) =>
            {
                var salle = new Salle
                {
                    NumeroSalle = numeroSalleEntry.Text
                };

                await _databaseService.AjouterSalle(salle); // Méthode correcte pour les salles
                await DisplayAlert("Succès", "Salle ajoutée avec succès", "OK");
                DisplaySallesMenu(null, null); // Retour au menu Salles
            };

            var retourButton = new Button { Text = "Retour" };
            retourButton.Clicked += (s, args) => DisplaySallesMenu(null, null);

            Content = new StackLayout
            {
                Padding = 20,
                Children = { numeroSalleEntry, saveButton, retourButton }
            };
        }

        private async void ListeSalles()
        {
            var salles = await _databaseService.ObtenirSalles(); // Méthode correcte pour récupérer les salles

            var listView = new ListView
            {
                ItemsSource = salles,
                ItemTemplate = new DataTemplate(() =>
                {
                    var numeroSalleLabel = new Label();
                    numeroSalleLabel.SetBinding(Label.TextProperty, "NumeroSalle");

                    return new ViewCell
                    {
                        View = new StackLayout
                        {
                            Orientation = StackOrientation.Horizontal,
                            Children = { numeroSalleLabel }
                        }
                    };
                })
            };

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
            saveButton.Clicked += async (s, args) =>
            {
                var cours = new Cours
                {
                    Nom = nomEntry.Text,
                    Horaires = horairesEntry.Text,
                    Salle = salleEntry.Text,
                    Enseignants = enseignantsEntry.Text,
                    Prix = decimal.TryParse(prixEntry.Text, out var prix) ? prix : 0
                };

                await _databaseService.AjouterCours(cours); // Méthode correcte pour les cours
                await DisplayAlert("Succès", "Cours ajouté avec succès", "OK");
                DisplayCoursMenu(null, null); // Retour au menu Cours
            };

            var retourButton = new Button { Text = "Retour" };
            retourButton.Clicked += (s, args) => DisplayCoursMenu(null, null);

            Content = new StackLayout
            {
                Padding = 20,
                Children = { nomEntry, horairesEntry, salleEntry, enseignantsEntry, prixEntry, saveButton, retourButton }
            };
        }
        private async void ListeCours()
        {
            var cours = await _databaseService.ObtenirCours(); // Méthode correcte pour récupérer les cours

            var listView = new ListView
            {
                ItemsSource = cours,
                ItemTemplate = new DataTemplate(() =>
                {
                    var nomLabel = new Label();
                    nomLabel.SetBinding(Label.TextProperty, "Nom");

                    var horairesLabel = new Label();
                    horairesLabel.SetBinding(Label.TextProperty, "Horaires");

                    return new ViewCell
                    {
                        View = new StackLayout
                        {
                            Orientation = StackOrientation.Horizontal,
                            Children = { nomLabel, horairesLabel }
                        }
                    };
                })
            };

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
