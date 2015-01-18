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
        private readonly StorageFolder _dossierRoamingWikitionary;

        private BaseDeDonneesRoaming()
        {
            try
            {
                _dossierRoamingWikitionary = ApplicationData.Current.RoamingFolder.GetFolderAsync("Wikitionary").AsTask().Result;
            }
            catch (Exception)
            {
                try
                {
                    _dossierRoamingWikitionary = ApplicationData.Current.RoamingFolder.CreateFolderAsync("Wikitionary").AsTask().Result;
                }
                catch(Exception)
                {
                    return;
                }
            }
        }

        public static BaseDeDonneesRoaming Instance
        {
            get { return _instance ?? (_instance = new BaseDeDonneesRoaming()); }
        }

        public ObservableCollection<Mot> RecupererDefinitions()
        {
            ObservableCollection<Mot> listeDefinitionsPubliques = new ObservableCollection<Mot>();
            List<StorageFile> listeFichiersRoaming = new List<StorageFile>(_dossierRoamingWikitionary.GetFilesAsync().AsTask().Result);

            foreach (var fichierRoaming in listeFichiersRoaming)
            {
                string json = FileIO.ReadTextAsync(fichierRoaming).AsTask().Result;
                var mot = JsonConvert.DeserializeObject<Mot>(json);
                mot.Depot = MainViewModel.Depot.Roaming;
                listeDefinitionsPubliques.Add(mot);
            }

            return listeDefinitionsPubliques;
        }

        public bool AjouterMot(Mot motAjoute)
        {
            try
            {
                var fichierRoaming = _dossierRoamingWikitionary.CreateFileAsync(motAjoute.Word + ".json", CreationCollisionOption.FailIfExists).AsTask().Result;

                using (var fichier = fichierRoaming.OpenAsync(FileAccessMode.ReadWrite).AsTask().Result)
                using (StreamWriter writer = new StreamWriter(fichier.AsStream()))
                using (JsonWriter jsonTextWriter = new JsonTextWriter(writer))
                {
                    jsonTextWriter.Formatting = Formatting.Indented;
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(jsonTextWriter, motAjoute);
                }
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
                var fichierModifie = _dossierRoamingWikitionary.GetFileAsync(motModifie.Word + ".json").AsTask().Result;
                fichierModifie.DeleteAsync().AsTask().Wait();
                return AjouterMot(motModifie);

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
                var fichierSupprime = _dossierRoamingWikitionary.GetFileAsync(motSupprime.Word + ".json").AsTask().Result;
                fichierSupprime.DeleteAsync().AsTask().Wait();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
