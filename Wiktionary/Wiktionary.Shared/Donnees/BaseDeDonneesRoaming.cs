using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Wiktionary.Model;
using Wiktionary.ViewModel;

namespace Wiktionary.Donnees
{
    public class BaseDeDonneesRoaming : IBaseDeDonnees
    {
        private static BaseDeDonneesRoaming _instance;
        private StorageFolder _dossierRoamingWikitionary;

        private BaseDeDonneesRoaming()
        {
            Initialiser();
        }

        private async void Initialiser()
        {
            try
            {
                _dossierRoamingWikitionary = await ApplicationData.Current.RoamingFolder.GetFolderAsync("Wikitionary");
                return;
            }
            catch (FileNotFoundException)
            {
                ApplicationData.Current.RoamingFolder.CreateFolderAsync("Wikitionary");
            }

            _dossierRoamingWikitionary = await ApplicationData.Current.RoamingFolder.GetFolderAsync("Wikitionary");
        }

        public static BaseDeDonneesRoaming Instance
        {
            get { return _instance ?? (_instance = new BaseDeDonneesRoaming()); }
        }

        public async Task<ObservableCollection<Mot>> RecupererDefinitions()
        {
            ObservableCollection<Mot> listeDefinitionsPubliques = new ObservableCollection<Mot>();
            List<StorageFile> listeFichiersRoaming = new List<StorageFile>(await _dossierRoamingWikitionary.GetFilesAsync());

            foreach (var fichierRoaming in listeFichiersRoaming)
            {
                string json = await FileIO.ReadTextAsync(fichierRoaming);
                var mot = JsonConvert.DeserializeObject<Mot>(json);
                mot.Depot = MainViewModel.Depot.Roaming;
                listeDefinitionsPubliques.Add(mot);
            }

            return listeDefinitionsPubliques;
        }

        public async Task<string> AjouterMot(Mot motAjoute)
        {
            try
            {
                var fichierRoaming = _dossierRoamingWikitionary.CreateFileAsync(motAjoute.Word + ".json", CreationCollisionOption.FailIfExists).AsTask().Result;

                using (var fichier = await fichierRoaming.OpenAsync(FileAccessMode.ReadWrite))
                using (StreamWriter writer = new StreamWriter(fichier.AsStream()))
                using (JsonWriter jsonTextWriter = new JsonTextWriter(writer))
                {
                    jsonTextWriter.Formatting = Formatting.Indented;
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(jsonTextWriter, motAjoute);
                }
                return "Element ajouté";
            }
            catch (Exception)
            {
                return "Echec";
            }
        }

        public async Task<string> ModifierMot(Mot motModifie)
        {
            try
            {
                var fichierModifie = await _dossierRoamingWikitionary.GetFileAsync(motModifie.Word + ".json");
                fichierModifie.DeleteAsync();
                return await AjouterMot(motModifie);

            }
            catch (Exception)
            {
                return "Echec";
            }
            

        }

        public async Task<string> SupprimerMot(Mot motSupprime)
        {
            try
            {
                var fichierSupprime = await _dossierRoamingWikitionary.GetFileAsync(motSupprime.Word + ".json");
                fichierSupprime.DeleteAsync();
                return "Element supprimé";
            }
            catch (Exception)
            {
                return "Echec";
            }
        }
    }
}
