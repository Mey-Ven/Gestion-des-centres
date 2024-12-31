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

        private async Task ModifierEtudiant(Etudiant etudiant)
        {
            var nomEntry = new Entry { Text = etudiant.Nom }; // Champ pré-rempli
            var prenomEntry = new Entry { Text = etudiant.Prenom };
            var niveauEntry = new Entry { Text = etudiant.NiveauScolaire };
            var parentEntry = new Entry { Text = etudiant.NumeroParent };

            var saveButton = new Button { Text = "Enregistrer" };
            saveButton.Clicked += async (s, args) =>
            {
                // Mettre à jour les informations de l'étudiant
                etudiant.Nom = nomEntry.Text;
                etudiant.Prenom = prenomEntry.Text;
                etudiant.NiveauScolaire = niveauEntry.Text;
                etudiant.NumeroParent = parentEntry.Text;

                await _databaseService.ModifierEtudiant(etudiant); // Mise à jour dans la base de données
                await DisplayAlert("Succès", "Étudiant modifié avec succès", "OK");
                ListeEtudiants(); // Retour à la liste des étudiants
            };

            var retourButton = new Button { Text = "Retour" };
            retourButton.Clicked += (s, args) => ListeEtudiants();

            Content = new StackLayout
            {
                Padding = 20,
                Children = { nomEntry, prenomEntry, niveauEntry, parentEntry, saveButton, retourButton }
            };
        }

        private async Task SupprimerEtudiant(Etudiant etudiant)
        {
            var confirmation = await DisplayAlert("Confirmation", "Voulez-vous vraiment supprimer cet étudiant ?", "Oui", "Non");
            if (confirmation)
            {
                await _databaseService.SupprimerEtudiant(etudiant.Id); // Supprimer de la base de données
                await DisplayAlert("Succès", "Étudiant supprimé avec succès", "OK");
                ListeEtudiants(); // Retour à la liste des étudiants
            }
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

                    var modifierButton = new Button { Text = "Modifier" };
                    modifierButton.SetBinding(Button.CommandParameterProperty, ".");
                    modifierButton.Clicked += async (s, args) =>
                    {
                        var etudiantSelectionne = (Etudiant)((Button)s).CommandParameter; // Récupérer l'étudiant
                        await ModifierEtudiant(etudiantSelectionne); // Appeler la méthode de modification
                    };

                    var supprimerButton = new Button
                    {
                        Text = "Supprimer",
                        BackgroundColor = Colors.Red, // Couleur de fond rouge
                        TextColor = Colors.White // Texte blanc
                    };
                    supprimerButton.SetBinding(Button.CommandParameterProperty, ".");
                    supprimerButton.Clicked += async (s, args) =>
                    {
                        var etudiantASupprimer = (Etudiant)((Button)s).CommandParameter; // Récupérer l'étudiant
                        await SupprimerEtudiant(etudiantASupprimer); // Appeler la méthode de suppression
                    };

                    return new ViewCell
                    {
                        View = new StackLayout
                        {
                            Orientation = StackOrientation.Horizontal,
                            Children = { nomLabel, prenomLabel, modifierButton, supprimerButton }
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

        private async Task ModifierEnseignant(Professeur enseignant)
        {
            var nomEntry = new Entry { Text = enseignant.Nom }; // Champ pré-rempli
            var prenomEntry = new Entry { Text = enseignant.Prenom };
            var matiereEntry = new Entry { Text = enseignant.Matiere };
            var telephoneEntry = new Entry { Text = enseignant.Telephone };
            var emailEntry = new Entry { Text = enseignant.Email };
            var prixEntry = new Entry { Text = enseignant.PrixCours.ToString(), Keyboard = Keyboard.Numeric };

            var saveButton = new Button { Text = "Enregistrer" };
            saveButton.Clicked += async (s, args) =>
            {
                // Mettre à jour les informations de l'enseignant
                enseignant.Nom = nomEntry.Text;
                enseignant.Prenom = prenomEntry.Text;
                enseignant.Matiere = matiereEntry.Text;
                enseignant.Telephone = telephoneEntry.Text;
                enseignant.Email = emailEntry.Text;
                enseignant.PrixCours = decimal.TryParse(prixEntry.Text, out var prix) ? prix : 0;

                await _databaseService.ModifierProfesseur(enseignant); // Mise à jour dans la base de données
                await DisplayAlert("Succès", "Enseignant modifié avec succès", "OK");
                ListeEnseignants(); // Retour à la liste des enseignants
            };

            var retourButton = new Button { Text = "Retour" };
            retourButton.Clicked += (s, args) => ListeEnseignants();

            Content = new StackLayout
            {
                Padding = 20,
                Children = { nomEntry, prenomEntry, matiereEntry, telephoneEntry, emailEntry, prixEntry, saveButton, retourButton }
            };
        }

        private async Task SupprimerEnseignant(Professeur enseignant)
        {
            var confirmation = await DisplayAlert("Confirmation", "Voulez-vous vraiment supprimer cet enseignant ?", "Oui", "Non");
            if (confirmation)
            {
                await _databaseService.SupprimerProfesseur(enseignant.Id); // Supprimer de la base de données
                await DisplayAlert("Succès", "Enseignant supprimé avec succès", "OK");
                ListeEnseignants(); // Retour à la liste des enseignants
            }
        }

        private async void ListeEnseignants()
        {
            var enseignants = await _databaseService.ObtenirProfesseurs(); // Récupérer les enseignants depuis la base

            var listView = new ListView
            {
                ItemsSource = enseignants,
                ItemTemplate = new DataTemplate(() =>
                {
                    var nomLabel = new Label();
                    nomLabel.SetBinding(Label.TextProperty, "Nom");

                    var prenomLabel = new Label();
                    prenomLabel.SetBinding(Label.TextProperty, "Prenom");

                    var modifierButton = new Button { Text = "Modifier" };
                    modifierButton.SetBinding(Button.CommandParameterProperty, ".");
                    modifierButton.Clicked += async (s, args) =>
                    {
                        var enseignantSelectionne = (Professeur)((Button)s).CommandParameter; // Récupérer l'enseignant
                        await ModifierEnseignant(enseignantSelectionne); // Appeler la méthode de modification
                    };

                    var supprimerButton = new Button
                    {
                        Text = "Supprimer",
                        BackgroundColor = Colors.Red, // Couleur de fond rouge
                        TextColor = Colors.White // Texte blanc
                    };
                    supprimerButton.SetBinding(Button.CommandParameterProperty, ".");
                    supprimerButton.Clicked += async (s, args) =>
                    {
                        var enseignantASupprimer = (Professeur)((Button)s).CommandParameter; // Récupérer l'enseignant
                        await SupprimerEnseignant(enseignantASupprimer); // Appeler la méthode de suppression
                    };

                    return new ViewCell
                    {
                        View = new StackLayout
                        {
                            Orientation = StackOrientation.Horizontal,
                            Children = { nomLabel, prenomLabel, modifierButton, supprimerButton }
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

        private async Task ModifierSalle(Salle salle)
        {
            var numeroSalleEntry = new Entry { Text = salle.NumeroSalle }; // Champ pré-rempli

            var saveButton = new Button { Text = "Enregistrer" };
            saveButton.Clicked += async (s, args) =>
            {
                // Mettre à jour les informations de la salle
                salle.NumeroSalle = numeroSalleEntry.Text;

                await _databaseService.ModifierSalle(salle); // Mise à jour dans la base de données
                await DisplayAlert("Succès", "Salle modifiée avec succès", "OK");
                ListeSalles(); // Retour à la liste des salles
            };

            var retourButton = new Button { Text = "Retour" };
            retourButton.Clicked += (s, args) => ListeSalles();

            Content = new StackLayout
            {
                Padding = 20,
                Children = { numeroSalleEntry, saveButton, retourButton }
            };
        }

        private async Task SupprimerSalle(Salle salle)
        {
            var confirmation = await DisplayAlert("Confirmation", "Voulez-vous vraiment supprimer cette salle ?", "Oui", "Non");
            if (confirmation)
            {
                await _databaseService.SupprimerSalle(salle.Id); // Supprimer de la base de données
                await DisplayAlert("Succès", "Salle supprimée avec succès", "OK");
                ListeSalles(); // Retour à la liste des salles
            }
        }
        private async void ListeSalles()
        {
            var salles = await _databaseService.ObtenirSalles(); // Récupérer les salles depuis la base

            var listView = new ListView
            {
                ItemsSource = salles,
                ItemTemplate = new DataTemplate(() =>
                {
                    var numeroLabel = new Label();
                    numeroLabel.SetBinding(Label.TextProperty, "NumeroSalle");

                    var modifierButton = new Button { Text = "Modifier" };
                    modifierButton.SetBinding(Button.CommandParameterProperty, ".");
                    modifierButton.Clicked += async (s, args) =>
                    {
                        var salleSelectionnee = (Salle)((Button)s).CommandParameter; // Récupérer la salle
                        await ModifierSalle(salleSelectionnee); // Appeler la méthode de modification
                    };

                    var supprimerButton = new Button
                    {
                        Text = "Supprimer",
                        BackgroundColor = Colors.Red, // Couleur de fond rouge
                        TextColor = Colors.White // Texte blanc
                    };
                    supprimerButton.SetBinding(Button.CommandParameterProperty, ".");
                    supprimerButton.Clicked += async (s, args) =>
                    {
                        var salleASupprimer = (Salle)((Button)s).CommandParameter; // Récupérer la salle
                        await SupprimerSalle(salleASupprimer); // Appeler la méthode de suppression
                    };

                    return new ViewCell
                    {
                        View = new StackLayout
                        {
                            Orientation = StackOrientation.Horizontal,
                            Children = { numeroLabel, modifierButton, supprimerButton }
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

        private async Task ModifierCours(Cours cours)
        {
            var nomEntry = new Entry { Text = cours.Nom }; // Champ pré-rempli
            var horairesEntry = new Entry { Text = cours.Horaires };
            var salleEntry = new Entry { Text = cours.Salle };
            var enseignantsEntry = new Entry { Text = cours.Enseignants };
            var prixEntry = new Entry { Text = cours.Prix.ToString(), Keyboard = Keyboard.Numeric };

            var saveButton = new Button { Text = "Enregistrer" };
            saveButton.Clicked += async (s, args) =>
            {
                // Mettre à jour les informations du cours
                cours.Nom = nomEntry.Text;
                cours.Horaires = horairesEntry.Text;
                cours.Salle = salleEntry.Text;
                cours.Enseignants = enseignantsEntry.Text;
                cours.Prix = decimal.TryParse(prixEntry.Text, out var prix) ? prix : 0;

                await _databaseService.ModifierCours(cours); // Mise à jour dans la base de données
                await DisplayAlert("Succès", "Cours modifié avec succès", "OK");
                ListeCours(); // Retour à la liste des cours
            };

            var retourButton = new Button { Text = "Retour" };
            retourButton.Clicked += (s, args) => ListeCours();

            Content = new StackLayout
            {
                Padding = 20,
                Children = { nomEntry, horairesEntry, salleEntry, enseignantsEntry, prixEntry, saveButton, retourButton }
            };
        }

        private async Task SupprimerCours(Cours cours)
        {
            var confirmation = await DisplayAlert("Confirmation", "Voulez-vous vraiment supprimer ce cours ?", "Oui", "Non");
            if (confirmation)
            {
                await _databaseService.SupprimerCours(cours.Id); // Supprimer de la base de données
                await DisplayAlert("Succès", "Cours supprimé avec succès", "OK");
                ListeCours(); // Retour à la liste des cours
            }
        }

        private async void ListeCours()
        {
            var cours = await _databaseService.ObtenirCours(); // Récupérer les cours depuis la base

            var listView = new ListView
            {
                ItemsSource = cours,
                ItemTemplate = new DataTemplate(() =>
                {
                    var nomLabel = new Label();
                    nomLabel.SetBinding(Label.TextProperty, "Nom");

                    var horairesLabel = new Label();
                    horairesLabel.SetBinding(Label.TextProperty, "Horaires");

                    var modifierButton = new Button { Text = "Modifier" };
                    modifierButton.SetBinding(Button.CommandParameterProperty, ".");
                    modifierButton.Clicked += async (s, args) =>
                    {
                        var coursSelectionne = (Cours)((Button)s).CommandParameter; // Récupérer le cours
                        await ModifierCours(coursSelectionne); // Appeler la méthode de modification
                    };

                    var supprimerButton = new Button
                    {
                        Text = "Supprimer",
                        BackgroundColor = Colors.Red, // Couleur de fond rouge
                        TextColor = Colors.White // Texte blanc
                    };
                    supprimerButton.SetBinding(Button.CommandParameterProperty, ".");
                    supprimerButton.Clicked += async (s, args) =>
                    {
                        var coursASupprimer = (Cours)((Button)s).CommandParameter; // Récupérer le cours
                        await SupprimerCours(coursASupprimer); // Appeler la méthode de suppression
                    };

                    return new ViewCell
                    {
                        View = new StackLayout
                        {
                            Orientation = StackOrientation.Horizontal,
                            Children = { nomLabel, horairesLabel, modifierButton, supprimerButton }
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
