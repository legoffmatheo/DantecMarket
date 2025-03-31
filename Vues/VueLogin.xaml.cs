using AP4DantecMarket.Modeles;
using Newtonsoft.Json;
using System.Text;
using System.Net.Http;
using Microsoft.Maui.Storage; // Import pour Preferences

namespace AP4DantecMarket.Vues
{
    public partial class VueLogin : ContentPage
    {
        private bool isPasswordVisible = false;

        public VueLogin()
        {
            InitializeComponent();
            LoadSavedCredentials(); // Charger les identifiants si sauvegardés
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            string email = emailEntry.Text;
            string password = passwordEntry.Text;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                messageLabel.Text = "Veuillez remplir tous les champs.";
                messageLabel.IsVisible = true;
                return;
            }

            loadingIndicator.IsVisible = true;
            loadingIndicator.IsRunning = true;
            messageLabel.IsVisible = false;

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var user = new User(email, password);
                    var json = JsonConvert.SerializeObject(user);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync("http://213.130.144.159/api/mobile/GetFindUser", content);
                    string responseString = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        var userData = JsonConvert.DeserializeObject<User>(responseString);

                        if (userData != null && userData.Id > 0)
                        {
                            Preferences.Set("UserId", userData.Id);
                            Application.Current.MainPage = new AppShell();
                        }
                        else
                        {
                            Console.WriteLine("❌ Erreur : utilisateur introuvable ou ID invalide.");
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                messageLabel.Text = "Erreur de connexion : " + ex.Message;
                messageLabel.IsVisible = true;
            }
            finally
            {
                loadingIndicator.IsVisible = false;
                loadingIndicator.IsRunning = false;
            }
        }


        private void OnTogglePasswordVisibility(object sender, EventArgs e)
        {
            isPasswordVisible = !isPasswordVisible; // Change l'état actuel
            passwordEntry.IsPassword = !isPasswordVisible;

            // Met à jour l'icône de l'œil
            togglePasswordButton.Source = isPasswordVisible ? "eye.png" : "hidden.png";
        }

     
        private void SaveCredentials(string email, string password, bool remember)
        {
            if (remember)
            {
                Preferences.Set("SavedEmail", email);
                Preferences.Set("SavedPassword", password);
                Preferences.Set("RememberMe", true);
            }
            else
            {
                Preferences.Remove("SavedEmail");
                Preferences.Remove("SavedPassword");
                Preferences.Remove("RememberMe");
            }
        }


        private void LoadSavedCredentials()
        {
            if (Preferences.Get("RememberMe", false))
            {
                emailEntry.Text = Preferences.Get("SavedEmail", string.Empty);
                passwordEntry.Text = Preferences.Get("SavedPassword", string.Empty);
                rememberMeCheckBox.IsChecked = true; 
            }
        }

    }
}
