using Newtonsoft.Json;
using SQLite;
using Wiktionary.ViewModel;

namespace Wiktionary.Model
{
    /// <summary>
    /// Classe Modele pour la gestion d'un mot
    /// </summary>
    public class Mot
    {
        [PrimaryKey, Column("word"), NotNull]
        public string Word { get; set; }
        [Column("definition"), NotNull]
        public string Definition { get; set; }
        [JsonIgnore]
        public MainViewModel.Depot Depot { get; set; }
        [JsonIgnore]
        private string _cle;
        [JsonIgnore]
        public string Cle
        {
            get
            {
                return Word + "-" + Definition + "-" + Depot;
            }
            set
            {
                _cle = value;
                if (value == null) return;
                string[] words = value.Split('-');
                Word = words[0];
                Definition = words[1];
                if (words[2] == MainViewModel.Depot.Local.ToString())
                    Depot = MainViewModel.Depot.Local;
                else if (words[2] == MainViewModel.Depot.Roaming.ToString())
                    Depot = MainViewModel.Depot.Roaming;
                else if (words[2] == MainViewModel.Depot.Public.ToString())
                    Depot = MainViewModel.Depot.Public;
            }
        }

    }
}
