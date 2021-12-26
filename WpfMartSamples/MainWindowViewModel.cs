using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Threading;

namespace WpfMartSamples
{
    class MainWindowViewModel: INotifyPropertyChanged
    {
        private DispatcherTimer _timer;

        private MachineState _machineState;
        private MachineState _currentMachineState;
        private CustomerType _customerType = WpfMartSamples.CustomerType.Loyal;
        private ObservableCollection<int> _numbers;
        private bool _boolProperty;
        private bool _redeemValuableCustomerDiscount;
        private bool? _nullBoolProperty;
        private int? _nullIntProperty;
        private string _stringProperty;
        private string _couponCode;
        private double? _nullDoubleProperty;
        private DateTime _dateProperty = DateTime.Now;
        private TimeSpan _elapsedTime;
        private Version _softwareVersion = new Version(2,5,0,0);

        public MainWindowViewModel()
        {
            _machineState = MachineState.On;
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(10),
            };
            _timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            ElapsedTime += _timer.Interval;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public MachineState MachineState
        {
            get { return _machineState; }
            set
            {
                _machineState = value;
                RaisePropertyChanged();
            }
        }

        public MachineState CurrentMachineState
        {
            get { return _currentMachineState; }
            set
            {
                _currentMachineState = value;
                RaisePropertyChanged();
            }
        }

        public CustomerType CustomerType
        {
            get { return _customerType; }
            set
            {
                _customerType = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<int> Numbers
        {
            get { return _numbers; }
            set
            {
                _numbers = value;
                RaisePropertyChanged();
            }
        }

        public bool BoolProperty
        {
            get { return _boolProperty; }
            set
            {
                _boolProperty = value;
                RaisePropertyChanged();
            }
        }
        public bool? NullBoolProperty
        {
            get { return _nullBoolProperty; }
            set
            {
                _nullBoolProperty = value;
                RaisePropertyChanged();
            }
        }
        public string StringProperty
        {
            get { return _stringProperty; }
            set
            {
                _stringProperty = value;
                RaisePropertyChanged();
            }
        }
        public int? NullIntProperty
        {
            get { return _nullIntProperty; }
            set
            {
                _nullIntProperty = value;
                RaisePropertyChanged();
            }
        }
        public double? NullDoubleProperty
        {
            get { return _nullDoubleProperty; }
            set
            {
                _nullDoubleProperty = value;
                RaisePropertyChanged();
            }
        }
        public DateTime DateProperty
        {
            get { return _dateProperty; }
            set
            {
                _dateProperty = value;
                RaisePropertyChanged();
            }
        }
        public TimeSpan ElapsedTime
        {
            get { return _elapsedTime; }
            set
            {
                _elapsedTime = value;
                RaisePropertyChanged();
            }
        }
        public string CouponCode
        {
            get { return _couponCode; }
            set
            {
                _couponCode = value;
                RaisePropertyChanged();
            }
        }
        public string SoftwareVersion
        {
            get { return _softwareVersion.ToString(); }
            set
            {
                _softwareVersion = Version.Parse(value);
                RaisePropertyChanged();
            }
        }
        public bool RedeemValuableCustomerDiscount
        {
            get { return _redeemValuableCustomerDiscount; }
            set
            {
                _redeemValuableCustomerDiscount = value;
                RaisePropertyChanged();
                SoftwareVersion = new Version(2, value? 0: 5, 0, 0).ToString();
            }
        }

    }
}
