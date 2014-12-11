using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight;

namespace Wiktionary.ViewModel
{
    public class EditionViewModel : ViewModelBase
    {
        public enum Depot
        {
            Local,
            Roaming,
            Public
        }

        public EditionViewModel()
        {
            //DepotSelectionne = Depot.Local;
        }

        public IEnumerable<Depot> DepotValeurs
        {
            get
            {
                return Enum.GetValues(typeof(Depot)).Cast<Depot>();
            }
        }
    }
}
