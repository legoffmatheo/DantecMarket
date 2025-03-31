using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DantecMarcket.Modeles
{
    public class Planning
    {
        #region Attributs
        private int id;
        private DateTime jour;
        private DateTime heureDebut;
        private DateTime heureFin;
        #endregion

        #region Constructeurs
        public Planning(int id, DateTime jour, DateTime heureDebut, DateTime heureFin)
        {
            this.id = id;
            this.jour = jour;
            this.heureDebut = heureDebut;
            this.heureFin = heureFin;
        }
        #endregion

        #region Getters/Setters
        [JsonProperty("id")]
        public int Id { get => id; set => id = value; }

        [JsonProperty("jour")]
        public DateTime Jour { get => jour; set => jour = value; }

        [JsonProperty("heureDebut")]
        public DateTime HeureDebut { get => heureDebut; set => heureDebut = value; }

        [JsonProperty("heureFin")]
        public DateTime HeureFin { get => heureFin; set => heureFin = value; }

        public string JourFormatte => Jour.ToString("dddd dd MMMM yyyy");
        public string PlageHoraire => $"🕒 {HeureDebut:HH:mm} - {HeureFin:HH:mm}";
        #endregion
    }
}
