using System;
using DISS_Model_AgentElektrokomponenty.Entity;
using DISS_S2_Elektroomponenty.Core;

namespace DISS_S2_Elektroomponenty.MVVM.Model
{
    public class ObsluzneMiestoModel : ObservableObjects
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

        public ObsluzneMiestoModel()
        {
            Obsadena = false;
            Obsah = "-/-";
            Name = "Obslužné miesto -/-";
        }

        public void Update(ObsluzneMiesto pObsluzneMiesto)
        {
            Obsadena = pObsluzneMiesto.Obsadena;
            Obsah = pObsluzneMiesto.ToString();
            Name = pObsluzneMiesto.Name;
        }

    }
}