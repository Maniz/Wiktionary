using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
            Public
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
        

        private ObservableCollection<Mot> _listeDefinition = new ObservableCollection<Mot>();
        public ObservableCollection<Mot> ListeDefinitions
        {
            get { return _listeDefinition; }
            set
            {
                _listeDefinition = value;
                RaisePropertyChanged();
            }
        }

        private void RechargeList()
        {
            //TODO
        }

        #endregion

        #region Commandes

        public ICommand AjouterMotCommand { get; set; }
        public ICommand SupprimerMotCommand { get; set; }


        #endregion

        /// <summary>
        /// Constructeur
        /// </summary>
        public MainViewModel()
        {
            BaseDeDonneeLocale.Instance.InitialiserBddLocale();
            AjouterMotCommand = new RelayCommand(AjouterMotLocal);
            SupprimerMotCommand = new RelayCommand<string>(param => SupprimerMot(new Mot() { Cle = param}));
            RecupererDefinitions();
            DepotAjout = Depot.Local;
        }

        #region Méthodes

        private async void RecupererDefinitions()
        {
            IEnumerable<Mot> liste = await BaseDeDonneesPublique.Instance.RecupererDefinitions();
            ListeDefinitions = new ObservableCollection<Mot>(liste.Union(await BaseDeDonneeLocale.Instance.RecupererDefinitions()));
        }

        private async void AjouterMotLocal()
        {
            Mot mot = new Mot() { Word = MotRecherche, Definition = NouvelleDefinition, Depot = DepotAjout};
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
            }

            if(result == "Element ajouté")
                ListeDefinitions.Add(mot);

        }

        private async void SupprimerMot(Mot motSupprime)
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