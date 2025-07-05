using System.ComponentModel;

namespace RTIC.Models
{
    public class Runner : INotifyPropertyChanged
    {
        private string ?_name;
        private string ?_level;
        private int _age;
        private double _height;
        private double _weight;
        private double _imc;
        private DateTime _date;     

        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        public string Level
        {
            get => _level;
            set
            {
                if (_level != value)
                {
                    _level = value;
                    OnPropertyChanged(nameof(Level));
                }
            }
        }

        public int Age
        {
            get => _age;
            set
            {
                if (_age != value)
                {
                    _age = value;
                    OnPropertyChanged(nameof(Age));
                }
            }
        }

        public double Height
        {
            get => _height;
            set
            {
                if (_height != value)
                {
                    _height = value;
                    OnPropertyChanged(nameof(Height));
                }
            }
        }

        public double Weight
        {
            get => _weight;
            set
            {
                if (_weight != value)
                {
                    _weight = value;
                    OnPropertyChanged(nameof(Weight));
                }
            }
        }
        public DateTime Date
        {
            get => _date;
            set
            {
                if (_date != value)
                {
                    _date = value;
                    OnPropertyChanged(nameof(Date));
                    OnPropertyChanged(nameof(FormattedDate));
                }
            }
        }

        public double Imc
        {
            get => _imc;
            set
            {
                if (_imc != value)
                {
                    _imc = value;
                    OnPropertyChanged(nameof(Imc));
                }
            }
        }

       public string FormattedDate
       {
            get => Date.ToString("dd-MM-yyyy");
            set
            {
                if (DateTime.TryParseExact(value, "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None, out var parsed))
                {
                    Date = parsed;
                }
            }
        }

        public string FormattedDate2
        {
            get => Date.ToString("dd-MM");
            set
            {
                if (DateTime.TryParseExact(value, "dd-MM", null, System.Globalization.DateTimeStyles.None, out var parsed))
                {
                    Date = parsed;
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}