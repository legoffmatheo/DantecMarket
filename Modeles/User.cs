using Newtonsoft.Json;

namespace AP4DantecMarket.Modeles
{
    public class User
    {
        #region attributs

        private string email;
        private string password;
        private int id;

        #endregion

        #region constructeurs

        public User(string email, string password)
        {
            this.email = email;
            this.password = password;
        }
        [JsonConstructor]
        public User(int id,string email, string password)
        {
            this.id = id;
            this.email = email;
            this.password = password;
            
        }

        #endregion

        #region getters/setters
        [JsonProperty("Id")] // Vérifie si l'API retourne bien "id"
        public int Id { get => id; set => id= value; }
        [JsonProperty("Email")]
        public string Email { get => email; set => email = value; }
        [JsonProperty("Password")]
        public string Password { get => password; set => password = value; }


        #endregion

        #region methodes

        #endregion
    }
}
