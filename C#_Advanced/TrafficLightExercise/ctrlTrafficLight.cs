using System;
using System.Drawing;
using System.Windows.Forms;
using TrafficLightExercise.Properties;

namespace TrafficLightExercise
{
    public partial class ctrlTrafficLight : UserControl
    {

        // Durations
        private int _redLightDuration = 10;
        private int _greenLightDuration = 5;
        private int _orangeLightDuration = 5;

        public int RedTime
        {
            get => _redLightDuration;
            set => _redLightDuration = value;
        }

        public int GreenTime
        {
            get => _greenLightDuration;
            set => _greenLightDuration = value;
        }

        public int OrangeTime
        {
            get => _orangeLightDuration;
            set => _orangeLightDuration = value;
        }

        // Enum
        public enum LightEnum { Red, Orange, Green }

        private LightEnum _currentLight = LightEnum.Red;

        public LightEnum CurrentLight
        {
            get => _currentLight;
            set
            {
                _currentLight = value;

                switch (_currentLight)
                {
                    case LightEnum.Red:
                        pbLight.Image = Resources.Red;
                        lblCountDown.ForeColor = Color.Red;
                        RaiseRedLightOn();
                        break;

                    case LightEnum.Green:
                        pbLight.Image = Resources.Green;
                        lblCountDown.ForeColor = Color.Green;
                        RaiseGreenLightOn();
                        break;

                    case LightEnum.Orange:
                        pbLight.Image = Resources.Orange;
                        lblCountDown.ForeColor = Color.Orange;
                        RaiseOrangeLightOn();
                        break;
                }
            }
        }

        // Event Args
        public class TrafficLightEventArgs : EventArgs
        {
            public LightEnum CurrentLight { get; }
            public int LightDuration { get; }

            public TrafficLightEventArgs(LightEnum currentLight, int lightDuration)
            {
                CurrentLight = currentLight;
                LightDuration = lightDuration;
            }
        }

        // Events
        public event EventHandler<TrafficLightEventArgs> RedLightOn;
        public event EventHandler<TrafficLightEventArgs> GreenLightOn;
        public event EventHandler<TrafficLightEventArgs> OrangeLightOn;

        protected virtual void RaiseRedLightOn()
        {
            RedLightOn?.Invoke(this, new TrafficLightEventArgs(LightEnum.Red, _redLightDuration));
        }

        protected virtual void RaiseGreenLightOn()
        {
            GreenLightOn?.Invoke(this, new TrafficLightEventArgs(LightEnum.Green, _greenLightDuration));
        }

        protected virtual void RaiseOrangeLightOn()
        {
            OrangeLightOn?.Invoke(this, new TrafficLightEventArgs(LightEnum.Orange, _orangeLightDuration));
        }

        // Timer
        private Timer _timer = new Timer();
        private int _currentCountDownValue;

        public ctrlTrafficLight()
        {
            InitializeComponent();
            _timer.Interval = 1000;
            _timer.Tick += Timer_Tick;
        }

        // Start System
        public void Start()
        {
            CurrentLight = LightEnum.Red;
            _currentCountDownValue = GetCurrentTime();
            UpdateLabel();
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
        }

        // Timer Logic
        private void Timer_Tick(object sender, EventArgs e)
        {
            _currentCountDownValue--;
            UpdateLabel();

            if (_currentCountDownValue <= 0)
            {
                ChangeLight();
            }
        }

        // Helpers
        private void UpdateLabel()
        {
            lblCountDown.Text = _currentCountDownValue.ToString();
        }

        private int GetCurrentTime()
        {
            switch (CurrentLight)
            {
                case LightEnum.Red:
                    return RedTime;

                case LightEnum.Green:
                    return GreenTime;

                case LightEnum.Orange:
                    return OrangeTime;

                default:
                    return RedTime;
            }
        }

        private LightEnum _lightAfter;

        private void ChangeLight()
        {
            switch (CurrentLight)
            {
                case LightEnum.Red:
                    _lightAfter = LightEnum.Green;
                    CurrentLight = LightEnum.Orange;
                    _currentCountDownValue = OrangeTime;
                    break;

                case LightEnum.Orange:
                    if (_lightAfter == LightEnum.Green)
                    {
                        CurrentLight = LightEnum.Green;
                        _currentCountDownValue = GreenTime;
                    }
                    else
                    {
                        CurrentLight = LightEnum.Red;
                        _currentCountDownValue = RedTime;
                    }
                    break;

                case LightEnum.Green:
                    _lightAfter = LightEnum.Red;
                    CurrentLight = LightEnum.Orange;
                    _currentCountDownValue = OrangeTime;
                    break;
            }

            UpdateLabel();
        }
    }
}