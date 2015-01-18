using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Wiktionary.Model;
using Wiktionary.ViewModel;

namespace Wiktionary.Donnees
{
    public class BaseDeDonneesPublique : IBaseDeDonnees
    {

        private static BaseDeDonneesPublique _instance;

        private BaseDeDonneesPublique()
        {
            
        }

        public static BaseDeDonneesPublique Instance
        {
            get { return _instance ?? (_instance = new BaseDeDonneesPublique()); }
        }

        public async Task<ObservableCollection<Mot>> RecupererDefinitions()
        {
            HttpResponseMessage response = await new HttpClient().GetAsync(new Uri("http://wiktionary.azurewebsites.net/Wiktionary.svc/GetAllDefinitions"));

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
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

            return new ObservableCollection<Mot>();
        }

        public async Task<string> AjouterMot(Mot motAjoute)
        {
            HttpResponseMessage response = await new HttpClient().GetAsync(new Uri(
                "http://wiktionary.azurewebsites.net/Wiktionary.svc/AddDefinition/" + motAjoute.Word +"/" + motAjoute.Definition + "/anthopaul"));

            if(response.IsSuccessStatusCode)
                return "ok";

            return "fail";
        }

        public async Task<string> SupprimerMot(Mot motSupprime)
        {
            HttpResponseMessage response = await new HttpClient().GetAsync(new Uri(
               "http://wiktionary.azurewebsites.net/Wiktionary.svc/RemoveDefinition/" + motSupprime.Word + "/anthopaul"));

            if(response.IsSuccessStatusCode)
                return "ok";

            return "fail";
        }

        public async Task<string> ModifierMot(Mot motModifie)
        {
            string reponse = await SupprimerMot(motModifie);
            if (reponse == "ok")
                return await AjouterMot(motModifie);

            return "Echec";
        }
    }
}
