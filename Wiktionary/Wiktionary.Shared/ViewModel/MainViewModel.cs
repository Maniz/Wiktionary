using System;
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

        #endregion

    }
}