using IntegrationExcelImporter.Common;
using System;

namespace IntegrationExcelImporter.Model
{
    public class Plan : ObservableObjectBase<Plan>
    {
        private string _kindOfEducation;
        public string KindOfEducation
        {
            get { return _kindOfEducation; }
            set
            {
                _kindOfEducation = value;
                OnPropertyChanged(p => p.KindOfEducation);
            }
        }

        private string _educationName;
        public string EducationName
        {
            get { return _educationName; }
            set
            {
                _educationName = value;
                OnPropertyChanged(p => p.EducationName);
            }
        }

        private string _educationDay;
        public string EducationDay
        {
            get { return _educationDay; }
            set
            {
                _educationDay = value;
                OnPropertyChanged(p => p.EducationDay);
            }
        }

        private string _educationTime;
        public string EducationTime
        {
            get { return _educationTime; }
            set
            {
                _educationTime = value;
                OnPropertyChanged(p => p.EducationTime);
            }
        }

        private string _educationAgency;
        public string EducationAgency
        {
            get { return _educationAgency; }
            set
            {
                _educationAgency = value;
                OnPropertyChanged(p => p.EducationAgency);
            }
        }

        private string _team;
        public string Team
        {
            get { return _team; }
            set
            {
                _team = value;
                OnPropertyChanged(p => p.Team);
            }
        }

        private string _position;
        public string Position
        {
            get { return _position; }
            set
            {
                _position = value;
                OnPropertyChanged(p => p.Position);
            }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged(p => p.Name);
            }
        }

        private string _tuitionFee;
        public string TuitionFee
        {
            get { return _tuitionFee; }
            set
            {
                _tuitionFee = value;
                OnPropertyChanged(p => p.TuitionFee);
            }
        }

        private string _travelFee;
        public string TravelFee
        {
            get { return _travelFee; }
            set
            {
                _travelFee = value;
                OnPropertyChanged(p => p.TravelFee);
            }
        }

        private string _sumOfFee;
        public string SumOfFee
        {
            get { return _sumOfFee; }
            set
            {
                _sumOfFee = value;
                OnPropertyChanged(p => p.SumOfFee);
            }
        }

        private string _janaury;
        public string Janaury
        {
            get { return _janaury; }
            set
            {
                _janaury = value;
                OnPropertyChanged(p => p.Janaury);
            }
        }

        private string _faburary;
        public string Faburary
        {
            get { return _faburary; }
            set
            {
                _faburary = value;
                OnPropertyChanged(p => p.Faburary);
            }
        }

        private string _march;
        public string March
        {
            get { return _march; }
            set
            {
                _march = value;
                OnPropertyChanged(p => p.March);
            }
        }

        private string _april;
        public string April
        {
            get { return _april; }
            set
            {
                _april = value;
                OnPropertyChanged(p => p.April);
            }
        }

        private string _may;
        public string May
        {
            get { return _may; }
            set
            {
                _may = value;
                OnPropertyChanged(p => p.May);
            }
        }

        private string _june;
        public string June
        {
            get { return _june; }
            set
            {
                _june = value;
                OnPropertyChanged(p => p.June);
            }
        }

        private string _july;
        public string July
        {
            get { return _july; }
            set
            {
                _july = value;
                OnPropertyChanged(p => p.July);
            }
        }

        private string _august;
        public string August
        {
            get { return _august; }
            set
            {
                _august = value;
                OnPropertyChanged(p => p.August);
            }
        }

        private string _september;
        public string September
        {
            get { return _september; }
            set
            {
                _september = value;
                OnPropertyChanged(p => p.September);
            }
        }

        private string _october;
        public string October
        {
            get { return _october; }
            set
            {
                _october = value;
                OnPropertyChanged(p => p.October);
            }
        }

        private string _november;
        public string November
        {
            get { return _november; }
            set
            {
                _november = value;
                OnPropertyChanged(p => p.November);
            }
        }

        private string _december;
        public string December
        {
            get { return _december; }
            set
            {
                _december = value;
                OnPropertyChanged(p => p.December);
            }
        }

        private string _spot;
        public string Spot
        {
            get { return _spot; }
            set
            {
                _spot = value;
                OnPropertyChanged(p => p.Spot);
            }
        }

        private string _reason;
        public string Reason
        {
            get { return _reason; }
            set
            {
                _reason = value;
                OnPropertyChanged(p => p.Reason);
            }
        }

        private string _planning;
        public string Planning
        {
            get { return _planning; }
            set
            {
                _planning = value;
                OnPropertyChanged(p => p.Planning);
            }
        }
    }
}
