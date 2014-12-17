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

        public MainViewModel.Depot Depot { get; set; }
        private string _cle;

        public string Cle
        {
            get
            {
                return Word + "-" + Depot;
            }
            set
            {
                _cle = value;
                if (value == null) return;
                string[] words = value.Split('-');
                Word = words[0];
                if (words[1] == MainViewModel.Depot.Local.ToString())
                    Depot = MainViewModel.Depot.Local;
                else if (words[1] == MainViewModel.Depot.Roaming.ToString())
                    Depot = MainViewModel.Depot.Roaming;
                else if (words[1] == MainViewModel.Depot.Public.ToString())
                    Depot = MainViewModel.Depot.Public;
            }
        }

    }
}
