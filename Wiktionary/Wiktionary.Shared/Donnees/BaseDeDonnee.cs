using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using Wiktionary.Model;

namespace Wiktionary.Donnees
{
    public class BaseDeDonnee
    {
        private static BaseDeDonnee _instance;

        private BaseDeDonnee()
        {
            _connection = new SQLiteAsyncConnection("wiktionaryLocal.bdd");
        }

        public static BaseDeDonnee Instance
        {
            get { return _instance ?? (_instance = new BaseDeDonnee()); }
        }

        private SQLiteAsyncConnection _connection;

        public async void InitialiserBddLocale()
        {
            await _connection.CreateTableAsync<Mot>();
        }

        public async Task<string> AjouterMotLocal(Mot motAjoute)
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
    }
}
