using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Wiktionary.Model;

namespace Wiktionary.Donnees
{
    public class BaseDeDonneesRoaming : IBaseDeDonnees
    {
        private static BaseDeDonneesRoaming _instance;
        //private 

        private BaseDeDonneesRoaming()
        {
            //_connection = new SQLiteAsyncConnection("wiktionaryLocal.bdd");
        }

        public static BaseDeDonneeLocale Instance
        {
            get { return _instance ?? (_instance = new BaseDeDonneesRoaming()); }
        }

        public Task<ObservableCollection<Mot>> RecupererDefinitions()
        {
            throw new NotImplementedException();
        }

        public Task<string> AjouterMot(Mot motAjoute)
        {
            throw new NotImplementedException();
        }

        public Task<string> ModifierMot(Mot motModifie)
        {
            throw new NotImplementedException();
        }

        public Task<string> SupprimerMot(Mot motSupprime)
        {
            throw new NotImplementedException();
        }
    }
}
