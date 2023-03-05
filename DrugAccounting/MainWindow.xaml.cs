using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace DrugAccounting
{
    public partial class MainWindow : Window
    {
        DispatcherTimer dispatcherTimerForClose = new DispatcherTimer();
        private bool isMaximized = false;
        private List<Patient> patientsList = new List<Patient>(); 
        private int id_selectedPatient;
        static string[] arrOfEl_DontMOve = new string[]
        {
            "Border_Menu", "Br_RollUp","Br_Expand","Br_Exit"
        };
        public MainWindow()
        {
            InitializeComponent();
            Win_MainWindow.Opacity = 0.8;
            AnimationOpacity(1, 0.8);
            MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight; //Установление значения параметра окна "MaxHeight" размером дисплея устройства
            if (!SystemClass.IsTableExists)
            {
                SqlDataAccess.CreateTableIfNotExist(); //Создание таблицы, если ее нет 
            }
            LoadPatientsList_ToDataGrid(); //Загрузка таблицы, пациентами, которых достали из БД
        }
        private void AnimationOpacity(double to, double sec)
        {
            DoubleAnimation gridAnimation = new DoubleAnimation();
            gridAnimation.From = Win_MainWindow.Opacity;
            gridAnimation.To = to;
            gridAnimation.Duration = TimeSpan.FromSeconds(sec);
            Win_MainWindow.BeginAnimation(OpacityProperty, gridAnimation);
        }
        //Очистка БД
        public void DeleteTable()
        {
            if (MessageBox.Show("Вы уверены, что хотете очистить базу данных?", "Подтверждение",
                MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                SqlDataAccess.DeleteTable();
                MessageBox.Show("База данных очищена");
            }
        }
        //Обновляем таблицу пациентов, пациентами, которых достали из БД
        public void LoadPatientsList_ToDataGrid()
        {
            patientsList = SqlDataAccess.LoadPatients(); // Инициализация листа, пациентами, кторых достали из БД
            DG_AllPatients.ItemsSource = patientsList; //Установление источника ресурсов для DataGrid (таблица пациентов)
        }
        //Нажатие на кнопку "Новый пацент"
        private void addPatient_Click(object sender, RoutedEventArgs e)
        {
            AddPatient();
        }
        //Добавление нового пациента
        private void AddPatient()
        {
            Win_addPatient win_AddPatient = new Win_addPatient(new Patient());
            win_AddPatient.Show();
            if (win_AddPatient.IsActive)
            {
                SetTimer_ForClose();
            }
        }
        //Выбор пациента из таблицы двойный нажатием левой кнопки мыши => Открытие карточки выбранного пациента 
        private void DataGrid_Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            Patient patient = (Patient)DG_AllPatients.SelectedItem;
            id_selectedPatient = patient.id;
            Win_addPatient win_AddPatient = new Win_addPatient(id_selectedPatient);
            win_AddPatient.Show();
            if (win_AddPatient.IsActive)
            {
                SetTimer_ForClose();
            }
        }
        //Обработчик действий нажатия и движениея курсора для изменния расположения и размеров окна
        private void WholeWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var el = e.Source as FrameworkElement;
            if (e.Source.ToString().Contains("Border") || e.Source.ToString().Contains("Image"))
            {
                try
                {
                    Grid grid = (Grid)sender;
                    Border border = (Border)VisualTreeHelper.GetChild(grid, 0);
                    border.BorderBrush = new SolidColorBrush(Color.FromRgb(171, 42, 23));
                }
                catch
                {
                    if (e.LeftButton == MouseButtonState.Pressed && Array.IndexOf(arrOfEl_DontMOve, el.Name) == -1)
                    {
                        DragMove();
                    }
                }
            }
            else
            {
                if (e.LeftButton == MouseButtonState.Pressed && Array.IndexOf(arrOfEl_DontMOve, el.Name) == -1)
                {
                    DragMove();
                }
            }
        }
        //Когда мы заходим курсором в системные кнопки (справа сверху)
        private void SistemBut_Enter(object sender, MouseEventArgs e)
        {
            Grid grid = (Grid)sender;
            Border border = (Border)VisualTreeHelper.GetChild(grid, 0);
            border.BorderBrush = new SolidColorBrush(Color.FromRgb(241, 72, 50));
        }
        //Когда мы выходим курсором из системных кнопок (справа сверху)
        private void SistemBut_Leave(object sender, MouseEventArgs e)
        {
            Grid grid = (Grid)sender;
            Border border = (Border)VisualTreeHelper.GetChild(grid, 0);
            border.BorderBrush = Brushes.Black;
        }
        //Нажатие на системные кнопки
        private void SistemBut_MouseDown(object sender, MouseEventArgs e)
        {
            Grid grid = (Grid)sender;
            Border border = (Border)VisualTreeHelper.GetChild(grid, 0);
            border.BorderBrush = new SolidColorBrush(Color.FromRgb(171, 42, 23));
        }
        //Отпускаеи курсор мыши над системными кнопками
        private void SistemBut_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Grid grid = (Grid)sender;
            switch (grid.Name)
            {
                case "Grid_RollUp":
                    {
                        WindowState = WindowState.Minimized;
                        break;
                    }
                case "Grid_Expand":
                    {
                        if (isMaximized)
                        {
                            ResizeMode = ResizeMode.CanResize;
                            WindowState = WindowState.Normal;
                            WindowStartupLocation = WindowStartupLocation.CenterScreen;
                            isMaximized = false;
                            ResizeMode = ResizeMode.NoResize;
                        }
                        else
                        {
                            WindowStyle = WindowStyle.None;
                            WindowState = WindowState.Maximized;
                            isMaximized = true;
                        }
                        break;
                    }
                case "Grid_Exit":
                    {
                        SetTimer_ForClose();
                        break;
                    }
                default: { break; }
            }
        }

        //Обработчик нажатий вкладок меня
        private void MenuItems_click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = (MenuItem)sender;
            switch (menuItem.Name)
            {
                case "Exit_Item":
                    {
                        SetTimer_ForClose();
                        break;
                    }
                case "NewPatient_Item":
                    {
                        AddPatient();
                        break;
                    }
                case "Refresh_Item":
                    {
                        LoadPatientsList_ToDataGrid();
                        break;
                    }
                case "Del_theTable_Item":
                    {
                        DeleteTable();
                        LoadPatientsList_ToDataGrid();
                        break;
                    }
            }
        }
        //Таймер для отложного закрытие окна
        private void SetTimer_ForClose()
        {
            dispatcherTimerForClose.Tick += new EventHandler(timerPick_CloseWin);
            dispatcherTimerForClose.Interval = TimeSpan.FromSeconds(0.3);
            AnimationOpacity(0.85, 0.4);
            if ((!IsWindowOpen<Window>("Win_AddPatient") && MessageBox.Show("Вы уверены, что хотете выйти?", "Подтверждение",
               MessageBoxButton.YesNo) == MessageBoxResult.Yes) || IsWindowOpen<Window>("Win_AddPatient"))
            {
                AnimationOpacity(0.4, 0.4);
                dispatcherTimerForClose.Start();
            }
            else
            {
                AnimationOpacity(1, 0.4);
            }
        }
        private void timerPick_CloseWin(object sender, EventArgs e)
        {
            dispatcherTimerForClose.Stop();
            Close();
        }
        //Проверка открыто ли окно
        public static bool IsWindowOpen<T>(string name = "") where T : Window
        {
            return string.IsNullOrEmpty(name)
               ? Application.Current.Windows.OfType<T>().Any()
               : Application.Current.Windows.OfType<T>().Any(w => w.Name.Equals(name));
        }
    }
}
