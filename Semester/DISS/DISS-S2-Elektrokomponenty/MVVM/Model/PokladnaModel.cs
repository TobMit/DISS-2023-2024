using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DISS_Model_Elektrokomponenty.Entity.Pokladna;
using DISS_S2_Elektroomponenty.Core;

namespace DISS_S2_Elektroomponenty.MVVM.Model
{

    public class PokladnaModel : ObservableObjects
    {
        private bool _obsadena;
        private string _obsah;
        private string _name;

        public bool Obsadena
        {
            get => _obsadena;
            set
            {
                if (value != _obsadena)
                {
                    _obsadena = value;
                    OnPropertyChanged();
                }
            }
        }
        public string Obsah
        {
            get => _obsah;
            set
            {
                if (String.Compare(value, _obsah, StringComparison.Ordinal) != 0)
                {
                    _obsah = value;
                    OnPropertyChanged();
                }
            }
        }
        public string Name
        {
            get => _name;
            set
            {
                if (String.Compare(value, _name, StringComparison.Ordinal) != 0)
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }
        }

        public PokladnaModel()
        {
            Obsadena = false;
            Obsah = "-/-";
            Name = "Pokladna -/-";
        }
        public void Update(Pokladna pPokladna)
        {
            Obsadena = pPokladna.Obsadena;
            Obsah = pPokladna.ToString();
            Name = pPokladna.Name;
        }
    }

}
