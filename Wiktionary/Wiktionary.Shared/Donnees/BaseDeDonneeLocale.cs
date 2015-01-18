using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using SQLite;
using Wiktionary.Model;
using Wiktionary.ViewModel;

namespace Wiktionary.Donnees
{
    public class BaseDeDonneeLocale : IBaseDeDonnees
    {
        private static BaseDeDonneeLocale _instance;

        private BaseDeDonneeLocale()
        {
            _connection = new SQLiteAsyncConnection("wiktionaryLocal.bdd");
            InitialiserBddLocale();
        }

        public static BaseDeDonneeLocale Instance
        {
            get { return _instance ?? (_instance = new BaseDeDonneeLocale()); }
        }

        private readonly SQLiteAsyncConnection _connection;

        private async void InitialiserBddLocale()
        {
            //await _connection.DropTableAsync<Mot>();
            await _connection.CreateTableAsync<Mot>();
        }

        public ObservableCollection<Mot> RecupererDefinitions()
        {
            try
            {
                var listeDefinitionsLocales = new ObservableCollection<Mot>(_connection.Table<Mot>().ToListAsync().Result);
                
                foreach (var mot in listeDefinitionsLocales)
                {
                    mot.Depot = MainViewModel.Depot.Local;
                }

                return listeDefinitionsLocales;
            }
            catch (Exception)
            {
                return new ObservableCollection<Mot>();
            }
        }

        public bool AjouterMot(Mot motAjoute)
        {
            try
            {
                _connection.InsertAsync(motAjoute).Wait();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public bool ModifierMot(Mot motModifie)
        {
            try
            {
                _connection.UpdateAsync(motModifie).Wait();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool SupprimerMot(Mot motSupprime)
        {
            try
            {
                _connection.DeleteAsync(motSupprime).Wait();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
