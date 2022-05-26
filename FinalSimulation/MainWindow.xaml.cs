using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Generic;


namespace FinalSimulation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private double mAcceleration = 0;
        private double mInitialVelocity = 0;
        private double mDropHeight = 0;
        private double mMass = 0;
        private double mKineticEnergy = 0;
        private double mTime = 0;
        private double mFinalVelocity = 0;
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            StartSimulation.Content = "START";
           
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public double ControlEnergy
        {
            get
            {
                return mKineticEnergy;
            }
            set
            {
                mKineticEnergy = value;
                OnPropertyChanged("ControlEnergy");
            }
        }
        public double ControlVelocity
        {
            get
            {
                return mFinalVelocity;
            }
            set
            {
                mFinalVelocity = value;
                OnPropertyChanged("ControlVelocity");
            }
        }
        public double ControlTime
        {
            get
            {
                return mTime;
            }
            set
            {
                mTime = value;
                OnPropertyChanged("ControlTime");
            }
        }

        private bool wasResetHit = true;
        private int cycles = 0;
        private List<int> arr = new List<int>();
        public void ButtonOnClick(Object sender, RoutedEventArgs e)
        {
            StartSimulation.Content = "RESET";
            mInitialVelocity = VelocityUpper.ControlValue;
            mAcceleration = GravityUpper.ControlValue;
            mDropHeight = DropUpper.ControlValue;
            mMass = MassUpper.ControlValue;
            simulate();
            StartSimulation.Click += new RoutedEventHandler(ResetOnClick);
            //cycles++;
            //arr.Add(cycles);
        }

        public void ResetOnClick(Object sender, RoutedEventArgs e)
        {
            StartSimulation.Content = "START";
            StartSimulation.FontSize = 25;
            ControlEnergy = 0;
            ControlTime = 0;
            ControlVelocity = 0;
            linegraph.Plot(new double [0], new double [0]); // x and y are IEnumerable<double>
            StartSimulation.Click += new RoutedEventHandler(ButtonOnClick);
        }

        public void simulate()
        {
            if (VelocityUpper.ControlValue == 0 && GravityUpper.ControlValue == 0
                && DropUpper.ControlValue == 0 && MassUpper.ControlValue == 0)
            {
                ControlVelocity = 0;
                ControlTime = 0;
                ControlEnergy = 0;
            }
            else if (GravityUpper.ControlValue == 0)
            {
                ControlVelocity = 0;
                ControlTime = 0;
                ControlEnergy = 0;
            }
            else
            {
                ControlVelocity = calculateFinalVelocity();
                ControlTime = calculateTime();
                ControlEnergy = calculateKineticEnergy();
                double[] time = new double[(int)Math.Round(mTime)];
                double[] velocity = new double[(int)Math.Round(mTime)];
                for (int i = 0; i < time.Length; i++)
                {
                    time[i] = i;
                    velocity[i] = mInitialVelocity - (mAcceleration * i);
                    linegraph.Plot(time, velocity); // x and y are IEnumerable<double>
                }
            }
        }
        public double calculateFinalVelocity()
        {            
            mFinalVelocity = -(Math.Sqrt(Math.Pow(mInitialVelocity, 2.0) + (2.0 * mAcceleration * mDropHeight)));
            double fV = mFinalVelocity;
            return fV;
        }
        public double calculateTime()
        {
            mTime = (calculateFinalVelocity() - mInitialVelocity) / mAcceleration;
            if (mTime < 0)
            {
                mTime = Math.Abs(mTime);
            }
            double time = mTime;
            return time;
        }
        public double calculateKineticEnergy()
        {
            mKineticEnergy = (mMass * Math.Pow(mFinalVelocity, 2)) / 2.0;
            double energy = mKineticEnergy;
            return energy;
        }
    }
}
