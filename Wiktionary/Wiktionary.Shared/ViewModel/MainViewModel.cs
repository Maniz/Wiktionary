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
        private string _message;
        public enum Depot
        {
            Local,
            Roaming,
            Public
        }

        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                RaisePropertyChanged();
            }
        }


        private Mot _motAjoute;

        public Mot MotAjoute
        {
            get { return _motAjoute; }
            set
            {
                _motAjoute = value;
                RaisePropertyChanged();
            }
        }


        private String _motRecherche;
        
        public String MotRecherche
        {
            get { return _motRecherche; }
            set
            {
                _motRecherche = value;
                RechargeList();
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
            //TODO: Charger la liste via les WS
            _listeDefinition.Add(new Mot { Definition = "Test", Valeur = "Valeur Valeur Valeur Valeur Valeur ValeurValeurValeur Valeur Valeur" });
            _listeDefinition.Add(new Mot { Definition = "Test2", Valeur = "Valeur2" });
        }

        #endregion

        #region Commandes

        public ICommand AjouterMotCommand { get; set; }


        #endregion

        /// <summary>
        /// Constructeur
        /// </summary>
        public MainViewModel()
        {
            BaseDeDonnee.Instance.InitialiserBddLocale();
            AjouterMotCommand = new RelayCommand(AjouterMotLocal);
        }

        #region Méthodes

        private void AjouterMotLocal()
        {
            Message = BaseDeDonnee.Instance.AjouterMotLocal(_motAjoute).Result;
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