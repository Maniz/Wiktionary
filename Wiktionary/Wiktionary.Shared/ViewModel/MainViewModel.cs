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

        private Depot _depotRecherche;

        public Depot DepotRecherche
        {
            get { return _depotRecherche; }
            set
            {
                _depotRecherche = value;
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

        private Mot _motModifie;

        public Mot MotModifie
        {
            get { return _motModifie; }
            set
            {
                _motModifie = value;
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
            if (DepotRecherche == Depot.Tous)
                ListeDefinitionsFiltree = new ObservableCollection<Mot>(ListeDefinitions.Where(m => m.Word.ToLower().Contains(MotRecherche.ToLower())));
            else
                ListeDefinitionsFiltree = new ObservableCollection<Mot>(ListeDefinitions.Where(m => m.Word.ToLower().Contains(MotRecherche.ToLower()) && m.Depot == DepotRecherche));


        }

        #endregion

        #region Commandes

        public ICommand AjouterMotCommand { get; set; }
        public ICommand EditerMotCommand { get; set; }
        public ICommand SupprimerMotCommand { get; set; }
        public ICommand ModifierMotCommand { get; set; }
        public ICommand SelectionnerMotCommand { get; set; }


        #endregion

        /// <summary>
        /// Constructeur
        /// </summary>
        public MainViewModel()
        {
            Notification.AbonnementNotification();
            AjouterMotCommand = new RelayCommand(AjouterMot);
            SupprimerMotCommand = new RelayCommand<string>(param => SupprimerMot(new Mot() { Cle = param }));
            ModifierMotCommand = new RelayCommand(ModifierMot);
            SelectionnerMotCommand = new RelayCommand<string>(param => MotModifie = new Mot() { Cle = param });
            RecupererDefinitions();
            DepotAjout = Depot.Local;
            MotRecherche = "";
            DepotRecherche = Depot.Tous;
            
        }

        #region Méthodes

        private void RecupererDefinitions()
        {
            IEnumerable<Mot> liste = BaseDeDonneesPublique.Instance.RecupererDefinitions();
            IEnumerable<Mot> listeRoaming = BaseDeDonneesRoaming.Instance.RecupererDefinitions();
            ListeDefinitions = new ObservableCollection<Mot>(liste.Union(BaseDeDonneeLocale.Instance.RecupererDefinitions()).Union(listeRoaming));
            ListeDefinitionsFiltree = ListeDefinitions;
        }

        private void AjouterMot()
        {
            Mot mot = new Mot() { Word = MotRecherche, Definition = NouvelleDefinition, Depot = DepotAjout };
            bool result = false;
            switch (DepotAjout)
            {
                case Depot.Local:
                    result = BaseDeDonneeLocale.Instance.AjouterMot(mot);
                    break;
                case Depot.Roaming:
                    result = BaseDeDonneesRoaming.Instance.AjouterMot(mot);
                    break;
                case Depot.Public:
                    result = BaseDeDonneesPublique.Instance.AjouterMot(mot);
                    break;
                case Depot.Tous:

                    break;
            }

            if (result)
            {
                ListeDefinitions.Add(mot);
                RechargeList();
            }
        }

        private void SupprimerMot(Mot motSupprime)
        {
            bool result = false;

            switch (motSupprime.Depot)
            {
                case Depot.Local:
                    result = BaseDeDonneeLocale.Instance.SupprimerMot(motSupprime);
                    break;
                case Depot.Roaming:
                    result = BaseDeDonneesRoaming.Instance.SupprimerMot(motSupprime);
                    break;
                case Depot.Public:
                    result = BaseDeDonneesPublique.Instance.SupprimerMot(motSupprime);
                    break;
                case Depot.Tous:
                    break;
            }

            if (result)
            {
                ListeDefinitions.Remove(ListeDefinitions.Single(mot => mot.Word == motSupprime.Word && mot.Depot == motSupprime.Depot));
                RechargeList();
            }
        }

        private void ModifierMot()
        {
            bool result = false;

            switch (MotModifie.Depot)
            {
                case Depot.Local:
                    result = BaseDeDonneeLocale.Instance.ModifierMot(MotModifie);
                    break;
                case Depot.Roaming:
                    result = BaseDeDonneesRoaming.Instance.ModifierMot(MotModifie);
                    break;
                case Depot.Public:
                    result = BaseDeDonneesPublique.Instance.ModifierMot(MotModifie);
                    break;
            }

            if (result)
            {
                ListeDefinitions.Remove(ListeDefinitions.Single(mot => mot.Word == MotModifie.Word && mot.Depot == MotModifie.Depot));
                ListeDefinitions.Add(MotModifie);
                RechargeList();
            }

        }

        public static IEnumerable<Depot> DepotValeurs
        {
            get
            {
                return Enum.GetValues(typeof(Depot)).Cast<Depot>();
            }
        }
        #endregion

    }
}