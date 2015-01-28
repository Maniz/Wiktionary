using System;
using System.Collections.ObjectModel;
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

            return new ObservableCollection<Mot>();
        }

        public bool AjouterMot(Mot motAjoute)
        {
            try
            {
                HttpResponseMessage response = new HttpClient().GetAsync(new Uri(Uri.EscapeDataString("http://wiktionary.azurewebsites.net/Wiktionary.svc/AddDefinition/" + motAjoute.Word + "/" + motAjoute.Definition + "/anthopaul"))).Result;
                return response.IsSuccessStatusCode;
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
                HttpResponseMessage response = new HttpClient().GetAsync(new Uri(Uri.EscapeDataString("http://wiktionary.azurewebsites.net/Wiktionary.svc/RemoveDefinition/" + motSupprime.Word + "/anthopaul"))).Result;
                return response.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                return false;
            }
            
        }

        public bool ModifierMot(Mot motModifie)
        {
            return SupprimerMot(motModifie) && AjouterMot(motModifie);
        }
    }
}
