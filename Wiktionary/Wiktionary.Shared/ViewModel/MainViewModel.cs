using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Wiktionary.Donnees;
using Wiktionary.Model;

namespace Wiktionary.ViewModel
{

    public class MainViewModel : ViewModelBase
    {

        #region Propriétés
        
        public enum Depot
        {
            Local,
            Roaming,
            Public,
            Tous
        }

        private Depot _depotAjout;

        public Depot DepotAjout
        {
            get { return _depotAjout; }
            set
            {
                _depotAjout = value;
                RaisePropertyChanged();
            }
        }

        private string _motRecherche;

        public string MotRecherche
        {
            get { return _motRecherche; }
            set
            {
                _motRecherche = value;
                RechargeList();
                RaisePropertyChanged();
            }
        }

        private string _nouvelleDefinition;
                
        public string NouvelleDefinition
        {
            get { return _nouvelleDefinition; }
            set
            {
                _nouvelleDefinition = value;
                RaisePropertyChanged();
            }
        }


        private ObservableCollection<Mot> ListeDefinitions { get; set; }


        private ObservableCollection<Mot> _listeDefinitionFiltree = new ObservableCollection<Mot>();
        public ObservableCollection<Mot> ListeDefinitionsFiltree
        {
            get { return _listeDefinitionFiltree; }
            set
            {
                _listeDefinitionFiltree = value;
                RaisePropertyChanged();
            }
        }

        private void RechargeList()
        {
            ListeDefinitionsFiltree = new ObservableCollection<Mot>(ListeDefinitions.Where(m => m.Word.ToLower().Contains(MotRecherche.ToLower())));
        }

        #endregion

        #region Commandes

        public ICommand AjouterMotCommand { get; set; }
        public ICommand EditerMotCommand { get; set; }
        public ICommand SupprimerMotCommand { get; set; }


        #endregion

        /// <summary>
        /// Constructeur
        /// </summary>
        public MainViewModel()
        {
            Notification.AbonnementNotification();

            BaseDeDonneeLocale.Instance.InitialiserBddLocale();
            AjouterMotCommand = new RelayCommand(AjouterMotLocal);
            EditerMotCommand = new RelayCommand(EditerMotLocal);
            SupprimerMotCommand = new RelayCommand<string>(param => SupprimerMot(new Mot() { Cle = param}));
            RecupererDefinitions();
            DepotAjout = Depot.Local;
        }

        #region Méthodes

        private async void RecupererDefinitions()
        {
            IEnumerable<Mot> liste = await BaseDeDonneesPublique.Instance.RecupererDefinitions();
            ListeDefinitions = new ObservableCollection<Mot>(liste.Union(await BaseDeDonneeLocale.Instance.RecupererDefinitions()));
            ListeDefinitionsFiltree = ListeDefinitions;
        }

        private void AjouterMotLocal()
        {
            Mot mot = new Mot() { Word = MotRecherche, Definition = NouvelleDefinition, Depot = DepotAjout };
            string result = null;
            switch (DepotAjout)
            {
                case Depot.Local:
                    result = BaseDeDonneeLocale.Instance.AjouterMot(mot).Result;
                    break;
                case Depot.Roaming:
                    //Message = BaseDeDonneeLocale.Instance.AjouterMot(mot).Result;
                    break;
                case Depot.Public:
                    result = BaseDeDonneesPublique.Instance.AjouterMot(mot).Result;
                    break;
                case Depot.Tous:

                    break;
            }

            if (result == "Element ajouté")
                ListeDefinitions.Add(mot);
        }

        private void EditerMotLocal()
        {
            
            string result = null;
            switch (DepotAjout)
            {
                case Depot.Local:
                    
                    break;
                case Depot.Roaming:
                    
                    break;
                case Depot.Public:
                    
                    break;
                case Depot.Tous:

                    break;
            }

            if (result == "Element édité")
            {
                
            }
        }

        private async Task SupprimerMot(Mot motSupprime)
        {
            string result = null;

            switch (motSupprime.Depot)
            {
                case Depot.Local:
                    result = await BaseDeDonneeLocale.Instance.SupprimerMot(motSupprime);
                    break;
                case Depot.Roaming:
                    //Message = BaseDeDonneeLocale.Instance.AjouterMot(mot).Result;
                    break;
                case Depot.Public:
                    //result = BaseDeDonneesPublique.Instance.AjouterMot(mot).Result;
                    break;  
                case Depot.Tous:
                    break;
            }

            if(result == "Element supprimé")
                ListeDefinitions.Remove(ListeDefinitions.Single(mot => mot.Word == motSupprime.Word));
        }

        public IEnumerable<Depot> DepotValeurs
        {
            get
            {
                return Enum.GetValues(typeof(Depot)).Cast<Depot>();
            }
        }
        #endregion

    }
}