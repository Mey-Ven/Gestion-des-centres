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

        private async void AjouterEtudiant()
        {
            var nomEntry = new Entry { Placeholder = "Nom" };
            var prenomEntry = new Entry { Placeholder = "Prénom" };
            var niveauEntry = new Entry { Placeholder = "Niveau Scolaire" };
            var parentEntry = new Entry { Placeholder = "Numéro du Parent" };

            // Champ pour sélectionner le sexe
            var sexePicker = new Picker { Title = "Sexe" };
            sexePicker.Items.Add("Homme");
            sexePicker.Items.Add("Femme");

            // Récupérer les cours disponibles depuis la base de données
            var coursDisponibles = await _databaseService.ObtenirCours();
            var coursPicker = new Picker { Title = "Cours inscrits" };
            foreach (var cours in coursDisponibles)
            {
                coursPicker.Items.Add(cours.Nom); // Ajouter les noms des cours dans le picker
            }

            var saveButton = new Button { Text = "Enregistrer" };
            saveButton.Clicked += async (s, args) =>
            {
                if (sexePicker.SelectedIndex == -1 || coursPicker.SelectedIndex == -1)
                {
                    await DisplayAlert("Erreur", "Veuillez remplir tous les champs", "OK");
                    return;
                }

                var etudiant = new Etudiant
                {
                    Nom = nomEntry.Text,
                    Prenom = prenomEntry.Text,
                    NiveauScolaire = niveauEntry.Text,
                    NumeroParent = parentEntry.Text,
                    Sexe = sexePicker.SelectedItem.ToString(),
                    CoursInscrits = new List<string> { coursPicker.SelectedItem.ToString() } // Ajouter le cours sélectionné
                };

                await _databaseService.AjouterEtudiant(etudiant); // Enregistrer l'étudiant dans la base de données
                await DisplayAlert("Succès", "Étudiant ajouté avec succès", "OK");
                DisplayEtudiantsMenu(null, null); // Retour au menu des étudiants
            };

            var retourButton = new Button { Text = "Retour" };
            retourButton.Clicked += (s, args) => DisplayEtudiantsMenu(null, null);

            Content = new StackLayout
            {
                Padding = 20,
                Children = {
            nomEntry,
            prenomEntry,
            niveauEntry,
            parentEntry,
            sexePicker,
            coursPicker,
            saveButton,
            retourButton
        }
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
                    // Affichage des informations d'un étudiant
                    var nomLabel = new Label { FontAttributes = FontAttributes.Bold };
                    nomLabel.SetBinding(Label.TextProperty, "Nom");

                    var prenomLabel = new Label();
                    prenomLabel.SetBinding(Label.TextProperty, "Prenom");

                    var niveauLabel = new Label();
                    niveauLabel.SetBinding(Label.TextProperty, "NiveauScolaire");

                    var numeroParentLabel = new Label();
                    numeroParentLabel.SetBinding(Label.TextProperty, "NumeroParent");

                    var sexeLabel = new Label();
                    sexeLabel.SetBinding(Label.TextProperty, "Sexe");

                    var coursLabel = new Label { FontAttributes = FontAttributes.Italic };
                    coursLabel.SetBinding(Label.TextProperty, "CoursInscritsSerialized");

                    // Bouton Modifier
                    var modifierButton = new Button { Text = "Modifier" };
                    modifierButton.SetBinding(Button.CommandParameterProperty, ".");
                    modifierButton.Clicked += async (s, args) =>
                    {
                        var etudiantSelectionne = (Etudiant)((Button)s).CommandParameter; // Récupérer l'étudiant
                        await ModifierEtudiant(etudiantSelectionne); // Appeler la méthode de modification
                    };

                    // Bouton Supprimer
                    var supprimerButton = new Button
                    {
                        Text = "Supprimer",
                        BackgroundColor = Colors.Red,
                        TextColor = Colors.White
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
                            Padding = 10,
                            Children = {
                        new Label { Text = "Nom :", FontAttributes = FontAttributes.Bold },
                        nomLabel,
                        new Label { Text = "Prénom :", FontAttributes = FontAttributes.Bold },
                        prenomLabel,
                        new Label { Text = "Niveau scolaire :", FontAttributes = FontAttributes.Bold },
                        niveauLabel,
                        new Label { Text = "Numéro du parent :", FontAttributes = FontAttributes.Bold },
                        numeroParentLabel,
                        new Label { Text = "Sexe :", FontAttributes = FontAttributes.Bold },
                        sexeLabel,
                        new Label { Text = "Cours inscrits :", FontAttributes = FontAttributes.Bold },
                        coursLabel,
                        new StackLayout
                        {
                            Orientation = StackOrientation.Horizontal,
                            Children = { modifierButton, supprimerButton }
                        }
                    }
                        }
                    };
                })
            };

            // Bouton Retour
            var retourButton = new Button { Text = "Retour" };
            retourButton.Clicked += (s, args) => DisplayEtudiantsMenu(null, null);

            // Ajouter la ListView et le bouton Retour dans la page
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

        private async void AjouterEnseignant()
        {
            var nomEntry = new Entry { Placeholder = "Nom" };
            var prenomEntry = new Entry { Placeholder = "Prénom" };
            var telephoneEntry = new Entry { Placeholder = "Téléphone" };
            var emailEntry = new Entry { Placeholder = "Email" };

            // Récupérer les cours disponibles
            var coursDisponibles = await _databaseService.ObtenirCours(); // Méthode pour récupérer les cours
            var coursPicker = new Picker { Title = "Cours enseignés" };
            foreach (var cours in coursDisponibles)
            {
                coursPicker.Items.Add(cours.Nom); // Ajouter les noms des cours dans la liste
            }

            var saveButton = new Button { Text = "Enregistrer" };
            saveButton.Clicked += async (s, args) =>
            {
                if (coursPicker.SelectedIndex == -1)
                {
                    await DisplayAlert("Erreur", "Veuillez sélectionner au moins un cours", "OK");
                    return;
                }

                var enseignant = new Professeur
                {
                    Nom = nomEntry.Text,
                    Prenom = prenomEntry.Text,
                    Telephone = telephoneEntry.Text,
                    Email = emailEntry.Text,
                    CoursEnseignes = new List<string> { coursPicker.SelectedItem.ToString() } // Ajouter le cours sélectionné
                };

                await _databaseService.AjouterProfesseur(enseignant); // Enregistrer dans la base de données
                await DisplayAlert("Succès", "Enseignant ajouté avec succès", "OK");
                DisplayEnseignantsMenu(null, null); // Retour au menu des enseignants
            };

            var retourButton = new Button { Text = "Retour" };
            retourButton.Clicked += (s, args) => DisplayEnseignantsMenu(null, null);

            Content = new StackLayout
            {
                Padding = 20,
                Children = {
            nomEntry,
            prenomEntry,
            telephoneEntry,
            emailEntry,
            coursPicker,
            saveButton,
            retourButton
        }
            };
        }
        private async Task ModifierEnseignant(Professeur enseignant)
        {
            var nomEntry = new Entry { Text = enseignant.Nom, Placeholder = "Nom" };
            var prenomEntry = new Entry { Text = enseignant.Prenom, Placeholder = "Prénom" };
            var telephoneEntry = new Entry { Text = enseignant.Telephone, Placeholder = "Téléphone" };
            var emailEntry = new Entry { Text = enseignant.Email, Placeholder = "Email" };

            // Récupérer les cours disponibles
            var coursDisponibles = await _databaseService.ObtenirCours();
            var coursPicker = new Picker { Title = "Cours enseignés" };
            foreach (var cours in coursDisponibles)
            {
                coursPicker.Items.Add(cours.Nom);
            }

            // Pré-sélectionner un cours s'il existe
            if (enseignant.CoursEnseignes.Count > 0)
            {
                var index = coursDisponibles.FindIndex(c => c.Nom == enseignant.CoursEnseignes[0]);
                if (index >= 0)
                {
                    coursPicker.SelectedIndex = index;
                }
            }

            var saveButton = new Button { Text = "Enregistrer" };
            saveButton.Clicked += async (s, args) =>
            {
                if (coursPicker.SelectedIndex == -1)
                {
                    await DisplayAlert("Erreur", "Veuillez sélectionner au moins un cours", "OK");
                    return;
                }

                enseignant.Nom = nomEntry.Text;
                enseignant.Prenom = prenomEntry.Text;
                enseignant.Telephone = telephoneEntry.Text;
                enseignant.Email = emailEntry.Text;
                enseignant.CoursEnseignes = new List<string> { coursPicker.SelectedItem.ToString() };

                await _databaseService.ModifierProfesseur(enseignant);
                await DisplayAlert("Succès", "Enseignant modifié avec succès", "OK");
                ListeEnseignants(); // Retour à la liste des enseignants
            };

            var retourButton = new Button { Text = "Retour" };
            retourButton.Clicked += (s, args) => ListeEnseignants();

            Content = new StackLayout
            {
                Padding = 20,
                Children = {
            nomEntry,
            prenomEntry,
            telephoneEntry,
            emailEntry,
            coursPicker,
            saveButton,
            retourButton
        }
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

                    var telephoneLabel = new Label();
                    telephoneLabel.SetBinding(Label.TextProperty, "Telephone");

                    var emailLabel = new Label();
                    emailLabel.SetBinding(Label.TextProperty, "Email");

                    var coursLabel = new Label { FontAttributes = FontAttributes.Italic };
                    coursLabel.SetBinding(Label.TextProperty, "CoursEnseignesSerialized"); // Affiche les cours enseignés

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
                        BackgroundColor = Colors.Red,
                        TextColor = Colors.White
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
                            Padding = 10,
                            Children = {
                        new Label { Text = "Nom :", FontAttributes = FontAttributes.Bold }, nomLabel,
                        new Label { Text = "Prénom :", FontAttributes = FontAttributes.Bold }, prenomLabel,
                        new Label { Text = "Téléphone :", FontAttributes = FontAttributes.Bold }, telephoneLabel,
                        new Label { Text = "Email :", FontAttributes = FontAttributes.Bold }, emailLabel,
                        new Label { Text = "Cours enseignés :", FontAttributes = FontAttributes.Bold }, coursLabel,
                        new StackLayout
                        {
                            Orientation = StackOrientation.Horizontal,
                            Children = { modifierButton, supprimerButton }
                        }
                    }
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
        private async void AjouterCours()
        {
            var titreLabel = new Label
            {
                Text = "Ajouter un cours",
                FontAttributes = FontAttributes.Bold,
                FontSize = 28,
                TextColor = Colors.DarkSlateGray,
                HorizontalTextAlignment = TextAlignment.Center,
                Margin = new Thickness(0, 10, 0, 20)
            };

            // Champs modernes avec styles globaux
            var nomEntry = new Entry { Placeholder = "Nom du cours" };
            var horairesEntry = new Entry { Placeholder = "Horaires du cours" };
            var prixEntry = new Entry { Placeholder = "Prix du cours", Keyboard = Keyboard.Numeric };

            // Picker avec style global
            var sallesDisponibles = await _databaseService.ObtenirSalles();
            var sallePicker = new Picker { Title = "Sélectionnez une salle" };
            foreach (var salle in sallesDisponibles)
            {
                sallePicker.Items.Add(salle.NumeroSalle);
            }

            var saveButton = new Button
            {
                Text = "Enregistrer",
                BackgroundColor = Colors.MediumSeaGreen
            };
            saveButton.Clicked += async (s, args) =>
            {
                if (sallePicker.SelectedIndex == -1)
                {
                    await DisplayAlert("Erreur", "Veuillez sélectionner une salle", "OK");
                    return;
                }

                var cours = new Cours
                {
                    Nom = nomEntry.Text,
                    Horaires = horairesEntry.Text,
                    Salle = sallePicker.SelectedItem.ToString(),
                    Prix = decimal.TryParse(prixEntry.Text, out var prix) ? prix : 0
                };

                await _databaseService.AjouterCours(cours);
                await DisplayAlert("Succès", "Cours ajouté avec succès", "OK");
                DisplayCoursMenu(null, null);
            };

            var retourButton = new Button
            {
                Text = "Retour",
                BackgroundColor = Colors.IndianRed
            };
            retourButton.Clicked += (s, args) => DisplayCoursMenu(null, null);

            // Organisation de la page avec un style épuré
            Content = new ScrollView
            {
                Content = new StackLayout
                {
                    Padding = 30,
                    BackgroundColor = Colors.WhiteSmoke,
                    Children = {
                titreLabel,
                new Frame
                {
                    Content = new StackLayout
                    {
                        Spacing = 10,
                        Children = { nomEntry, horairesEntry, sallePicker, prixEntry, saveButton }
                    }
                },
                retourButton
            }
                }
            };
        }


        private async Task ModifierCours(Cours cours)
        {
            var nomEntry = new Entry { Text = cours.Nom, Placeholder = "Nom du cours" };
            var horairesEntry = new Entry { Text = cours.Horaires, Placeholder = "Horaires du cours" };
            var prixEntry = new Entry { Text = cours.Prix.ToString(), Placeholder = "Prix", Keyboard = Keyboard.Numeric };

            // Récupérer les salles disponibles
            var sallesDisponibles = await _databaseService.ObtenirSalles();
            var sallePicker = new Picker { Title = "Sélectionnez une salle" };
            foreach (var salle in sallesDisponibles)
            {
                sallePicker.Items.Add(salle.NumeroSalle);
            }

            // Pré-sélectionner la salle actuelle du cours
            var index = sallesDisponibles.FindIndex(s => s.NumeroSalle == cours.Salle);
            if (index >= 0)
            {
                sallePicker.SelectedIndex = index;
            }

            var saveButton = new Button { Text = "Enregistrer" };
            saveButton.Clicked += async (s, args) =>
            {
                if (sallePicker.SelectedIndex == -1)
                {
                    await DisplayAlert("Erreur", "Veuillez sélectionner une salle", "OK");
                    return;
                }

                cours.Nom = nomEntry.Text;
                cours.Horaires = horairesEntry.Text;
                cours.Salle = sallePicker.SelectedItem.ToString();
                cours.Prix = decimal.TryParse(prixEntry.Text, out var prix) ? prix : 0;

                await _databaseService.ModifierCours(cours);
                await DisplayAlert("Succès", "Cours modifié avec succès", "OK");
                ListeCours(); // Retour à la liste des cours
            };

            var retourButton = new Button { Text = "Retour" };
            retourButton.Clicked += (s, args) => ListeCours();

            Content = new StackLayout
            {
                Padding = 20,
                Children = {
            nomEntry,
            horairesEntry,
            sallePicker,
            prixEntry,
            saveButton,
            retourButton
        }
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
            var coursList = await _databaseService.ObtenirCours();

            var listView = new ListView
            {
                ItemsSource = coursList,
                ItemTemplate = new DataTemplate(() =>
                {
                    var nomLabel = new Label();
                    nomLabel.SetBinding(Label.TextProperty, "Nom");

                    var horairesLabel = new Label();
                    horairesLabel.SetBinding(Label.TextProperty, "Horaires");

                    var salleLabel = new Label();
                    salleLabel.SetBinding(Label.TextProperty, "Salle");

                    var prixLabel = new Label();
                    prixLabel.SetBinding(Label.TextProperty, "Prix");

                    var modifierButton = new Button { Text = "Modifier" };
                    modifierButton.SetBinding(Button.CommandParameterProperty, ".");
                    modifierButton.Clicked += async (s, args) =>
                    {
                        var coursSelectionne = (Cours)((Button)s).CommandParameter;
                        await ModifierCours(coursSelectionne); // Appeler la méthode ModifierCours
                    };

                    var supprimerButton = new Button
                    {
                        Text = "Supprimer",
                        BackgroundColor = Colors.Red,
                        TextColor = Colors.White
                    };
                    supprimerButton.SetBinding(Button.CommandParameterProperty, ".");
                    supprimerButton.Clicked += async (s, args) =>
                    {
                        var coursASupprimer = (Cours)((Button)s).CommandParameter;
                        await _databaseService.SupprimerCours(coursASupprimer.Id);
                        ListeCours(); // Rafraîchir la liste
                    };

                    return new ViewCell
                    {
                        View = new StackLayout
                        {
                            Padding = 10,
                            Children = {
                        new StackLayout
                        {
                            Orientation = StackOrientation.Horizontal,
                            Children = {
                                new Label { Text = "Nom :", FontAttributes = FontAttributes.Bold }, nomLabel,
                                new Label { Text = "Salle :", FontAttributes = FontAttributes.Bold }, salleLabel
                            }
                        },
                        new Label { Text = "Horaires :", FontAttributes = FontAttributes.Bold }, horairesLabel,
                        new Label { Text = "Prix :", FontAttributes = FontAttributes.Bold }, prixLabel,
                        new StackLayout
                        {
                            Orientation = StackOrientation.Horizontal,
                            Children = { modifierButton, supprimerButton }
                        }
                    }
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
