using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Wiktionary.Model;
using Wiktionary.ViewModel;

namespace Wiktionary.Donnees
{
    public class BaseDeDonneesPublique : IBaseDeDonnees
    {

        private static BaseDeDonneesPublique _instance;

        private BaseDeDonneesPublique(){}

        public static BaseDeDonneesPublique Instance
        {
            get { return _instance ?? (_instance = new BaseDeDonneesPublique()); }
        }

        public ObservableCollection<Mot> RecupererDefinitions()
        {
            try
            {
            HttpResponseMessage response = new HttpClient().GetAsync(new Uri("http://wiktionary.azurewebsites.net/Wiktionary.svc/GetAllDefinitions")).Result;

            if (response.IsSuccessStatusCode)
            {
                string json = response.Content.ReadAsStringAsync().Result;
                var data = JArray.Parse(json);

                ObservableCollection<Mot> listeDefinitionsPubliques = new ObservableCollection<Mot>();
                foreach (var mot in data)
                {
                    Mot m = JsonConvert.DeserializeObject<Mot>(mot.ToString());
                    m.Depot = MainViewModel.Depot.Public;
                    listeDefinitionsPubliques.Add(m);
                }

                return listeDefinitionsPubliques;
            }
            }
            catch (Exception)
            {
                throw new Exception("La connexion au dépôt public n'a pas pu être établie.");
            }

            return new ObservableCollection<Mot>();
        }

        public bool AjouterMot(Mot motAjoute)
        {
            if (String.IsNullOrEmpty(motAjoute.Definition))
                throw new Exception("Une définition doit être renseignée pour ajouter un nouveau mot.");
            if (_instance.RecupererDefinitions().Any(m => m.Word == motAjoute.Word))
                throw new Exception("Le mot est déjà présent, veuillez modifier le mot existant");

            HttpResponseMessage response;
            try
            {
                response = new HttpClient().GetAsync("http://wiktionary.azurewebsites.net/Wiktionary.svc/AddDefinition/" + Uri.EscapeDataString(motAjoute.Word) + "/" + Uri.EscapeDataString(motAjoute.Definition) + "/anthopaul").Result;
            }
            catch (Exception)
            {
                throw new Exception("La connexion au dépôt public n'a pas pu être établie.");
            }

            if (response.StatusCode.Equals(HttpStatusCode.Unauthorized))
                throw new Exception("Vous n'êtes pas autorisé à éditer cette définition.");

            return response.IsSuccessStatusCode;
        }

        public bool SupprimerMot(Mot motSupprime)
        {
            HttpResponseMessage response;
            try
            {
                response = new HttpClient().GetAsync("http://wiktionary.azurewebsites.net/Wiktionary.svc/RemoveDefinition/" + Uri.EscapeDataString(motSupprime.Word) + "/anthopaul").Result;
            }
            catch (Exception)
            {
                throw new Exception("La connexion au dépôt public n'a pas pu être établie.");
            }

            if (response.StatusCode.Equals(HttpStatusCode.Unauthorized))
                throw new Exception("Vous n'êtes pas autorisé à éditer cette définition.");

            return response.IsSuccessStatusCode;
        }

        public bool ModifierMot(Mot motModifie)
        {
            if (String.IsNullOrEmpty(motModifie.Definition))
                throw new Exception("Une définition doit être renseignée pour ajouter un nouveau mot.");

            return SupprimerMot(motModifie) && AjouterMot(motModifie);
        }
    }
}
