using System;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace DrugAccounting
{
    public partial class IntroWindow : Window
    {
        DispatcherTimer timerForAnimationOpacity = new DispatcherTimer(); //Таймер для начала анимации затухания
        DispatcherTimer timerForOpenWin = new DispatcherTimer(); //Таймер для открытия окна
        MainWindow mainWindow = new MainWindow();
        public IntroWindow()
        {
            InitializeComponent();
            Timer();
        }
        private void Timer()
        {
            timerForAnimationOpacity.Tick += new EventHandler(timer_Tick);
            timerForAnimationOpacity.Interval = new TimeSpan(0, 0, 4);
            timerForOpenWin.Interval = TimeSpan.FromSeconds(1.4);
            timerForOpenWin.Tick += new EventHandler(timer_Tick_Open);
            timerForAnimationOpacity.Start();
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            timerForAnimationOpacity.Stop();
            Animation();
            timerForOpenWin.Start();
        }
        private void timer_Tick_Open(object sender, EventArgs e)
        {
            timerForOpenWin.Stop();
            mainWindow.Show();
            if (mainWindow.IsActive)
            {
                Close();
            }
        }
        //Анимация затухания окна
        private void Animation()
        {
            DoubleAnimation gridAnimation = new DoubleAnimation();
            gridAnimation.From = MainGrid.Opacity;
            gridAnimation.To = 0;
            gridAnimation.Duration = TimeSpan.FromSeconds(1.4);
            MainGrid.BeginAnimation(OpacityProperty, gridAnimation);
        }
    }
}
