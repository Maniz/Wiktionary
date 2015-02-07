using System;
using System.Collections.ObjectModel;
using System.Linq;
using SQLite;
using Wiktionary.Model;
using Wiktionary.ViewModel;

namespace Wiktionary.Donnees
{
    public class BaseDeDonneeLocale : IBaseDeDonnees
    {
        public async static void InitialiserBddLocale()
        {
            var connection = new SQLiteAsyncConnection("wiktionaryLocal.bdd");
            await connection.CreateTableAsync<Mot>();
        }

        public ObservableCollection<Mot> RecupererDefinitions()
        {
            try
            {
                var connection = new SQLiteAsyncConnection("wiktionaryLocal.bdd");

                var listeDefinitionsLocales = new ObservableCollection<Mot>(connection.Table<Mot>().ToListAsync().Result);

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
            var connection = new SQLiteAsyncConnection("wiktionaryLocal.bdd");

            if (String.IsNullOrEmpty(motAjoute.Definition))
                throw new Exception("Une définition doit être renseignée pour ajouter un nouveau mot.");
            if (connection.Table<Mot>().ToListAsync().Result.Any(mot => mot.Word == motAjoute.Word))
                throw new Exception("Le mot est déjà présent, veuillez modifier le mot existant");

            try
            {
                if (connection.Table<Mot>().ToListAsync().Result.All(mot => mot.Word != motAjoute.Word))
                {
                    connection.InsertAsync(motAjoute).Wait();
                    return true;
                }

                return false;
            }
            catch (SQLiteException)
            {
                return false;
            }
        }

        public bool ModifierMot(Mot motModifie)
        {
            if (String.IsNullOrEmpty(motModifie.Definition))
                throw new Exception("Une définition doit être renseignée pour ajouter un nouveau mot.");

            try
            {
                var connection = new SQLiteAsyncConnection("wiktionaryLocal.bdd");

                connection.UpdateAsync(motModifie).Wait();
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
                var connection = new SQLiteAsyncConnection("wiktionaryLocal.bdd");

                connection.DeleteAsync(motSupprime).Wait();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
