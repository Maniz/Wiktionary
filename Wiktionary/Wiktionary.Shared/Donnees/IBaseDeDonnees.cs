using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Wiktionary.Model;

namespace Wiktionary.Donnees
{
    public interface IBaseDeDonnees
    {
        Task<ObservableCollection<Mot>> RecupererDefinitions();
        Task<string> AjouterMot(Mot motAjoute);
        Task<string> ModifierMot(Mot motModifie);
        Task<string> SupprimerMot(Mot motSupprime);

    }
}
