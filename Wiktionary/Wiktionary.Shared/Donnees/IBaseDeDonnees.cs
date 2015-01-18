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
        ObservableCollection<Mot> RecupererDefinitions();
        bool AjouterMot(Mot motAjoute);
        bool ModifierMot(Mot motModifie);
        bool SupprimerMot(Mot motSupprime);

    }
}
