using System;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace DrugAccounting
{
    public partial class IntroWindow
    {
        readonly DispatcherTimer _timerForAnimationOpacity = new DispatcherTimer(); 
        readonly DispatcherTimer _timerForOpenWin = new DispatcherTimer(); 
        readonly MainWindow _mainWindow = new MainWindow();

        public IntroWindow()
        {
            InitializeComponent();
            Timer();
        }

        private void Timer()
        {
            _timerForAnimationOpacity.Tick += timer_Tick;
            _timerForAnimationOpacity.Interval = new TimeSpan(0, 0, 4);
            _timerForOpenWin.Interval = TimeSpan.FromSeconds(1.4);
            _timerForOpenWin.Tick += timer_Tick_Open;
            _timerForAnimationOpacity.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            _timerForAnimationOpacity.Stop();
            Animation();
            _timerForOpenWin.Start();
        }

        private void timer_Tick_Open(object sender, EventArgs e)
        {
            _timerForOpenWin.Stop();
            _mainWindow.Show();
            if (_mainWindow.IsActive)
            {
                Close();
            }
        }

        //Анимация затухания окна
        private void Animation()
        {
            DoubleAnimation gridAnimation = new DoubleAnimation
            {
                From = MainGrid.Opacity, To = 0, 
                Duration = TimeSpan.FromSeconds(1.4)
            };
            MainGrid.BeginAnimation(OpacityProperty, gridAnimation);
        }
    }
}
