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

        public async Task<ObservableCollection<Mot>> RecupererDefinitions()
        {
            try
            {
                var listeDefinitionsLocales = new ObservableCollection<Mot>(await  _connection.Table<Mot>().ToListAsync());
                
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

        public async Task<string> AjouterMot(Mot motAjoute)
        {
            try
            {
                await _connection.InsertAsync(motAjoute);
                return "Element ajouté";
            }
            catch (Exception)
            {
                return "Erreur lors de l'insertion";
            }

        }

        public async Task<string> ModifierMot(Mot motModifie)
        {
            try
            {
                await _connection.UpdateAsync(motModifie);
                return "Element ajouté";
            }
            catch (Exception)
            {
                return "Erreur lors de la modification";
            }
        }

        public async Task<string> SupprimerMot(Mot motSupprime)
        {
            try
            {
                await _connection.DeleteAsync(motSupprime);
                return "Element supprimé";
            }
            catch (Exception)
            {
                return "Erreur lors de la suppression";
            }
        }
    }
}
