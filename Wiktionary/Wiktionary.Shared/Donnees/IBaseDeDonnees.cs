using System.Collections.ObjectModel;
using Wiktionary.Model;

namespace Wiktionary.Donnees
{
    /// <summary>
    /// Interface pour les différentes bases de données
    /// </summary>
    public interface IBaseDeDonnees
    {
        ObservableCollection<Mot> RecupererDefinitions();
        bool AjouterMot(Mot motAjoute);
        bool ModifierMot(Mot motModifie);
        bool SupprimerMot(Mot motSupprime);

    }
}
