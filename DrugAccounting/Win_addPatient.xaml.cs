using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace DrugAccounting
{
    public partial class WinAddPatient
    {
        readonly DispatcherTimer _dispatcherTimerForClose = new DispatcherTimer();
        private bool _isMaximized;
        private readonly int _idSelectedPatientAddWin;
        private Patient NewPatient { get; set; }
        private bool IsPatientNew { get; set; } //Является ли пациент новым 
        private bool _isSaved; //Сохранен ли пациент

        static readonly string[] ArrOfElDontMOve = { "Border_Menu", "Br_RollUp", "Br_Expand", "Br_Exit" };

        //Массивы серий в  зависимости от таблетки и от дозы 
        private string[] series_8_Kasark = { "K1238", "K3218" };
        private readonly string[] series_16_Kasark = { "K12316", "K32116" };
        private string[] series_32_Kasark = { "K12332", "K32132" };

        private string[] series_8_Atakand = { "A1238", "A3218" };
        private string[] series_16_Atakand = { "A12316", "A32116" };
        private string[] series_32_Atakand = { "A12332", "A3232" };

        public WinAddPatient(int idSelectedPatient)
        {
            InitializeComponent();
            Win_AddPatient.Opacity = 0.8;
            AnimationOpacity(1, 0.8);
            _idSelectedPatientAddWin = idSelectedPatient; //id выбранного пациента
            MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            NewPatient = SqlDataAccess.LoadSelectedPatient(idSelectedPatient); //Достаем выбранного пациента
            Title = NewPatient.MainInf;
            SetValues(); //Добавляем значения из БД в карточку пацента
            IsPatientNew = false;
            _isSaved = true;
        }

        public WinAddPatient(Patient patient)
        {
            InitializeComponent();
            MessageBox.Show(dateOf_visit1.ToString());
            Win_AddPatient.Opacity = 0.8;
            AnimationOpacity(1, 0.8);
            Title = "Новый пациент";
            MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            NewPatient = patient;
            IsPatientNew = true;
            _isSaved = false;
        }

        //Анимация изменения прозразности окна
        private void AnimationOpacity(double to, double sec)
        {
            DoubleAnimation gridAnimation = new DoubleAnimation();
            gridAnimation.From = Win_AddPatient.Opacity;
            gridAnimation.To = to;
            gridAnimation.Duration = TimeSpan.FromSeconds(sec);
            Win_AddPatient.BeginAnimation(OpacityProperty, gridAnimation);
        }

        //Сохранение данных пациента
        private void SavePatient()
        {
            var errors = new StringBuilder();
            //Проверка на NullOrWhiteSpace обязательных полей для заполнения для сохранения пациента в базу
            if (string.IsNullOrWhiteSpace(fullName_patient.Text))
            {
                errors.AppendLine("Укажите ФИО пациента");
            }

            if (string.IsNullOrWhiteSpace(numOf_center.Text))
            {
                errors.AppendLine("Укажите номер центра");
            }

            if (string.IsNullOrWhiteSpace(numOf_patient.Text))
            {
                errors.AppendLine("Укажите номер пациента");
            }

            if (Kasark.IsChecked != true && Atakand.IsChecked != true)
            {
                errors.AppendLine("Укажите препарат");
            }

            if (errors.Length > 0)
            {
                AnimationOpacity(0.95, 0.4);
                MessageBox.Show(errors.ToString(), "Ошибка при добавлении нового пациента", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                AnimationOpacity(1, 0.4);
                errors.Clear();
            }
            else
            {
                if (IsPatientNew)
                {
                    AddPatientToDB(); //Добавление пациента в БД
                    IsPatientNew = false;
                }
                else
                {
                    AddPatientToDB(_idSelectedPatientAddWin); //Добавление пациента в БД
                }

                _isSaved = true;
                AnimationOpacity(0.95, 0.4);
                MessageBox.Show("Успешное сохранение", "Подтверждение", MessageBoxButton.OK,
                    MessageBoxImage.Information);
                AnimationOpacity(1, 0.4);
            }
        }

        //Событие при нажатии кнопки "Сохранить"
        private void Bt_savePatient_Click(object sender, RoutedEventArgs e)
        {
            SavePatient();
        }

        //Событие при нажатии кнопки "Шлавное меню"
        private void Bt_MainMenuPatient_Click(object sender, RoutedEventArgs e)
        {
            if (IsWindowOpen<Window>("MainWindow"))
            {
                MessageBox.Show("Главное меню уже открыто");
            }
            else
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
            }
        }

        //Добавление пациента
        public void AddPatientToDB()
        {
            if (!SystemClass.IsTableExists)
            {
                SqlDataAccess.CreateTableIfNotExist(); //Создание таблицы если ее нет 
            }

            SqlDataAccess.SavePatients(NewPatient); //Сохранение данных пациента
        }

        //Обновление пациента
        public void AddPatientToDB(int idSelectedPatient)
        {
            SqlDataAccess.UpdatePatient(NewPatient, idSelectedPatient); //Обновление данных пациента
        }

        //Изменение поля ФИО
        private void Initials_patient_Changed(object sender, RoutedEventArgs e)
        {
            _isSaved = false;
            fullName_patient.Text = fullName_patient.Text.Trim(); //Невозможность ввода пробела в ФИО
            var fullNameLenth = fullName_patient.Text.Length;
            fullName_patient.CaretIndex = fullNameLenth; //Установка курсора на конец ФИО

            if (fullNameLenth <= 0)
            {
                return;
            }

            if (char.IsLetter(fullName_patient.Text[fullNameLenth - 1])) //Проверка введенного символа на букву
            {
                fullName_patient.Text = fullName_patient.Text.ToUpper();
                
                if (fullNameLenth != 3)
                {
                    return;
                }

                fullName_patient.CaretIndex = fullNameLenth;
                NewPatient.P_fullName_patient = fullName_patient.Text;
                if (NewPatient.P_numOf_patient.ToString().Length == 3)
                {
                    Title = NewPatient.MainInf;
                }
            }
            else
            {
                try
                {
                    fullName_patient.Text = fullName_patient.Text.Substring(0, fullNameLenth - 1);
                    fullName_patient.CaretIndex = fullName_patient.Text.Length;
                }
                catch { fullName_patient.Text = ""; }
            }
        }

        //Изменение числовых полей
        private void Num_field_Changed(object sender, RoutedEventArgs e)
        {
            _isSaved = false;
            TextBox textBox = (TextBox)sender;
            textBox.Text = textBox.Text.Trim();
            textBox.CaretIndex = textBox.Text.Length;
            
            try
            {
                switch (textBox.Name)
                {
                    case "numOf_patient":
                    {
                        NewPatient.P_numOf_patient = textBox.Text.Length > 0 ? int.Parse(textBox.Text) : 0;
                        if (numOf_patient.Text.Length == 3 && fullName_patient.Text.Length == 3)
                        {
                            Title = NewPatient.MainInf;
                        }

                        break;
                    }
                    case "numOf_center":
                    {
                        NewPatient.P_numOf_center = textBox.Text.Length > 0 ? int.Parse(textBox.Text) : 0;
                        break;
                    }
                    case "numOf_issuedPills_visit1":
                    {
                        NewPatient.P_numOf_issuedPills_visit1 = textBox.Text.Length > 0 ? int.Parse(textBox.Text) : 0;
                        Calculation_Pills();
                        break;
                    }
                    case "numOf_issuedPills_visit2":
                    {
                        NewPatient.P_numOf_issuedPills_visit2 = textBox.Text.Length > 0 ? int.Parse(textBox.Text) : 0;
                        Calculation_Pills();
                        break;
                    }
                    case "numOf_issuedPills_visit3":
                    {
                        NewPatient.P_numOf_issuedPills_visit3 = textBox.Text.Length > 0 ? int.Parse(textBox.Text) : 0;
                        Calculation_Pills();
                        break;
                    }
                    case "serial_number_visit1":
                    {
                        NewPatient.P_serial_number_visit1 = textBox.Text.Length > 0 ? textBox.Text : "";
                        break;
                    }
                    case "serial_number_visit2":
                    {
                        NewPatient.P_serial_number_visit2 = textBox.Text.Length > 0 ? textBox.Text : "";
                        break;
                    }
                    case "serial_number_visit3":
                    {
                        NewPatient.P_serial_number_visit3 = textBox.Text.Length > 0 ? textBox.Text : "";
                        break;
                    }
                }
            }
            catch
            {
                textBox.Text = textBox.Text[0..^1];
                textBox.CaretIndex = textBox.Text.Length;
            }
        }

        //Выбор препарата
        private void ChoiseOf_Drug_Changed(object sender, RoutedEventArgs e)
        {
            _isSaved = false;
            NewPatient.P_typeOf_drug = Kasark.IsChecked == true ? "Kasark" : "Atakand";
            if (NewPatient.P_doseOf_Drug_visit1 != 0)
            {
                SetSeriesInComboBox(serial_number_visit1, NewPatient.P_doseOf_Drug_visit1);
            }

            if (NewPatient.P_doseOf_Drug_visit2 != 0)
            {
                SetSeriesInComboBox(serial_number_visit2, NewPatient.P_doseOf_Drug_visit2);
            }

            if (NewPatient.P_doseOf_Drug_visit3 != 0)
            {
                SetSeriesInComboBox(serial_number_visit3, NewPatient.P_doseOf_Drug_visit3);
            }
        }

        //Выбор дозы препарта
        private void RadioButMassDrugs_Changed(object sender, RoutedEventArgs e)
        {
            _isSaved = false;
            RadioButton radioButton = (RadioButton)sender;
            switch (radioButton.GroupName)
            {
                case "mass_Drug_visit1":
                {
                    NewPatient.P_doseOf_Drug_visit1 = int.Parse(radioButton.Content.ToString() ?? string.Empty);
                    SetSeriesInComboBox(serial_number_visit1, NewPatient.P_doseOf_Drug_visit1);
                    break;
                }
                case "mass_Drug_visit2":
                {
                    NewPatient.P_doseOf_Drug_visit2 = int.Parse(radioButton.Content.ToString() ?? string.Empty);
                    SetSeriesInComboBox(serial_number_visit2, NewPatient.P_doseOf_Drug_visit2);
                    break;
                }
                case "mass_Drug_visit3":
                {
                    NewPatient.P_doseOf_Drug_visit3 = int.Parse(radioButton.Content.ToString() ?? string.Empty);
                    SetSeriesInComboBox(serial_number_visit3, NewPatient.P_doseOf_Drug_visit3);
                    break;
                }
            }
        }

        //Инициализация массива серийных номеров препарата в зависимости от массы дозы
        private void SetSeriesInComboBox(ComboBox comboBox, int mass)
        {
            string[] arrGetSeries = mass switch
            {
                8 => NewPatient.P_typeOf_drug == "Kasark" ? series_8_Kasark : series_8_Atakand,
                16 => NewPatient.P_typeOf_drug == "Kasark" ? series_16_Kasark : series_16_Atakand,
                32 => NewPatient.P_typeOf_drug == "Kasark" ? series_32_Kasark : series_32_Atakand,
                _ => new string[] { }
            };

            comboBox.Items.Clear();
            foreach (var str in arrGetSeries)
            {
                comboBox.Items.Add(str);
            }

            comboBox.SelectedIndex = 0;
        }

        //Изменение полей дат
        private void Date_field_Changed(object sender, SelectionChangedEventArgs e)
        {
            _isSaved = false;
            DatePicker datePicker = (DatePicker)sender;
            switch (datePicker.Name)
            {
                case "dateOf_visit1":
                {
                    NewPatient.P_dateOf_visit1 = datePicker.SelectedDate?.Date ?? new DateTime(0001, 01, 01);
                    Calculation_Pills();
                    break;
                }
                case "dateOf_visit2":
                {
                    NewPatient.P_dateOf_visit2 = datePicker.SelectedDate?.Date ?? new DateTime(0001, 01, 01);
                    Calculation_Pills();
                    break;
                }
                case "dateOf_visit3":
                {
                    NewPatient.P_dateOf_visit3 = datePicker.SelectedDate?.Date ?? new DateTime(0001, 01, 01);
                    Calculation_Pills();
                    break;
                }
                case "dateOf_visit4":
                {
                    NewPatient.P_dateOf_visit4 = datePicker.SelectedDate?.Date ?? new DateTime(0001, 01, 01);
                    Calculation_Pills();
                    break;
                }
                case "dateOf_shelfLife_visit1":
                {
                    NewPatient.P_dateOf_shelfLife_visit1 = datePicker.SelectedDate?.Date ?? new DateTime(0001, 01, 01);
                    break;
                }
                case "dateOf_shelfLife_visit2":
                {
                    NewPatient.P_dateOf_shelfLife_visit2 = datePicker.SelectedDate?.Date ?? new DateTime(0001, 01, 01);
                    break;
                }
                case "dateOf_shelfLife_visit3":
                {
                    NewPatient.P_dateOf_shelfLife_visit3 = datePicker.SelectedDate?.Date ?? new DateTime(0001, 01, 01);
                    break;
                }
                case "dateOf_startTakingPills_visit1":
                {
                    NewPatient.P_dateOf_startTakingPills_visit1 = datePicker.SelectedDate?.Date ?? new DateTime(0001, 01, 01);
                    Calculation_Pills();
                    break;
                }
                case "dateOf_startTakingPills_visit2":
                {
                    NewPatient.P_dateOf_startTakingPills_visit2 = datePicker.SelectedDate?.Date ?? new DateTime(0001, 01, 01);
                    Calculation_Pills();
                    break;
                }
                case "dateOf_startTakingPills_visit3":
                {
                    NewPatient.P_dateOf_startTakingPills_visit3 = datePicker.SelectedDate?.Date ?? new DateTime(0001, 01, 01);
                    Calculation_Pills();
                    break;
                }
            }
        }

        //Перерасчет таблеток
        private void Calculation_Pills()
        {
            if (dateOf_startTakingPills_visit1.SelectedDate != null && dateOf_visit2.SelectedDate != null)
            {
                var numAccV1 = int.Parse(dateOf_visit2.SelectedDate.Value.Date
                    .Subtract(dateOf_startTakingPills_visit1.SelectedDate.Value.Date).Days.ToString());
                NewPatient.P_numOf_acceptedPills_visit1 = numAccV1 > 0 ? numAccV1 : 0;
                numOf_acceptedPills_visit1.Text = NewPatient.P_numOf_acceptedPills_visit1.ToString();
            }

            if (dateOf_startTakingPills_visit2.SelectedDate != null && dateOf_visit3.SelectedDate != null)
            {
                var numAccV2 = int.Parse(dateOf_visit3.SelectedDate.Value.Date
                    .Subtract(dateOf_startTakingPills_visit2.SelectedDate.Value.Date).Days.ToString());
                NewPatient.P_numOf_acceptedPills_visit2 = numAccV2 > 0 ? numAccV2 : 0;
                numOf_acceptedPills_visit2.Text = NewPatient.P_numOf_acceptedPills_visit2.ToString();
            }

            if (dateOf_startTakingPills_visit3.SelectedDate != null && dateOf_visit4.SelectedDate != null)
            {
                var numAccV3 = int.Parse(dateOf_visit4.SelectedDate.Value.Date
                    .Subtract(dateOf_startTakingPills_visit3.SelectedDate.Value.Date).Days.ToString());
                NewPatient.P_numOf_acceptedPills_visit3 = numAccV3 > 0 ? numAccV3 : 0;
                numOf_acceptedPills_visit3.Text = NewPatient.P_numOf_acceptedPills_visit3.ToString();
            }

            try
            {
                Calc_BlankBlister_and_BalancePills(numOf_issuedPills_visit1,
                    int.Parse(numOf_acceptedPills_visit1.Text));
                Calc_BlankBlister_and_BalancePills(numOf_issuedPills_visit2,
                    int.Parse(numOf_acceptedPills_visit2.Text));
                Calc_BlankBlister_and_BalancePills(numOf_issuedPills_visit3,
                    int.Parse(numOf_acceptedPills_visit3.Text));
            }
            catch
            {
                // ignored
            }
        }

        //Расчет пустых блистеров и оставшихся таблеток
        private void Calc_BlankBlister_and_BalancePills(TextBox issuedPills, int acceptedPills)
        {
            int blankBlister = 0;
            for (int i = 10; i <= acceptedPills; i += 10) //Расчет пустых блистеров
            {
                blankBlister++;
            }

            switch (issuedPills.Name)
            {
                case "numOf_issuedPills_visit1":
                {
                    numOf_blankBlister_visit1.Text = blankBlister.ToString();
                    numOf_balancePills_visit1.Text = (int.Parse(issuedPills.Text) - acceptedPills) > 0
                        ? (int.Parse(issuedPills.Text) - acceptedPills).ToString()
                        : "0";
                    NewPatient.P_numOf_blankBlister_visit1 = int.Parse(numOf_blankBlister_visit1.Text);
                    NewPatient.P_numOf_balancePills_visit1 = int.Parse(numOf_balancePills_visit1.Text);
                    break;
                }
                case "numOf_issuedPills_visit2":
                {
                    numOf_blankBlister_visit2.Text = blankBlister.ToString();
                    numOf_balancePills_visit2.Text = (int.Parse(issuedPills.Text) - acceptedPills) > 0
                        ? (int.Parse(issuedPills.Text) - acceptedPills).ToString()
                        : "0";
                    NewPatient.P_numOf_blankBlister_visit2 = int.Parse(numOf_blankBlister_visit2.Text);
                    NewPatient.P_numOf_balancePills_visit2 = int.Parse(numOf_balancePills_visit2.Text);
                    break;
                }
                case "numOf_issuedPills_visit3":
                {
                    numOf_blankBlister_visit3.Text = blankBlister.ToString();
                    numOf_balancePills_visit3.Text = (int.Parse(issuedPills.Text) - acceptedPills) > 0
                        ? (int.Parse(issuedPills.Text) - acceptedPills).ToString()
                        : "0";
                    NewPatient.P_numOf_blankBlister_visit3 = int.Parse(numOf_blankBlister_visit3.Text);
                    NewPatient.P_numOf_balancePills_visit3 = int.Parse(numOf_balancePills_visit3.Text);
                    break;
                }
            }
        }

        //Изменение полей времени
        private void Time_field_Changed(object sender, RoutedPropertyChangedEventArgs<DateTime?> e)
        {
            _isSaved = false;
            MaterialDesignThemes.Wpf.TimePicker timePicker = (MaterialDesignThemes.Wpf.TimePicker)sender;
            switch (timePicker.Name)
            {
                case "timeOf_visit2":
                {
                    NewPatient.P_timeOf_visit2 = timePicker.SelectedTime ?? new DateTime(0001, 01, 01, 0, 0, 0);
                    break;
                }
                case "timeOf_visit3":
                {
                    NewPatient.P_timeOf_visit3 = timePicker.SelectedTime ?? new DateTime(0001, 01, 01, 0, 0, 0);
                    break;
                }
                case "timeOf_visit4":
                {
                    NewPatient.P_timeOf_visit4 = timePicker.SelectedTime ?? new DateTime(0001, 01, 01, 0, 0, 0);
                    break;
                }
                case "timeOf_startTakingPills_visit1":
                {
                    NewPatient.P_timeOf_startTakingPills_visit1 = timePicker.SelectedTime ?? new DateTime(0001, 01, 01, 0, 0, 0);
                    break;
                }
                case "timeOf_startTakingPills_visit2":
                {
                    NewPatient.P_timeOf_startTakingPills_visit2 = timePicker.SelectedTime ?? new DateTime(0001, 01, 01, 0, 0, 0);
                    break;
                }
                case "timeOf_startTakingPills_visit3":
                {
                    NewPatient.P_timeOf_startTakingPills_visit3 = timePicker.SelectedTime ?? new DateTime(0001, 01, 01, 0, 0, 0);
                    break;
                }
                case "timeOf_endTakingPills_visit1":
                {
                    NewPatient.P_timeOf_endTakingPills_visit1 = timePicker.SelectedTime ?? new DateTime(0001, 01, 01, 0, 0, 0);
                    break;
                }
                case "timeOf_endTakingPills_visit2":
                {
                    NewPatient.P_timeOf_endTakingPills_visit2 = timePicker.SelectedTime ?? new DateTime(0001, 01, 01, 0, 0, 0);
                    break;
                }
                case "timeOf_endTakingPills_visit3":
                {
                    NewPatient.P_timeOf_endTakingPills_visit3 = timePicker.SelectedTime ?? new DateTime(0001, 01, 01, 0, 0, 0);
                    break;
                }
            }
        }

        //Наведение курсором на системные кнопок
        private void SistemBut_Enter(object sender, MouseEventArgs e)
        {
            Grid grid = (Grid)sender;
            Border border = (Border)VisualTreeHelper.GetChild(grid, 0);
            border.BorderBrush = new SolidColorBrush(Color.FromRgb(241, 72, 50));
        }

        //Покидание курсора системных кнопок
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
                    if (_isMaximized)
                    {
                        ResizeMode = ResizeMode.CanResize;
                        WindowState = WindowState.Normal;
                        WindowStartupLocation = WindowStartupLocation.CenterScreen;
                        _isMaximized = false;
                        ResizeMode = ResizeMode.NoResize;
                    }
                    else
                    {
                        WindowStyle = WindowStyle.None;
                        WindowState = WindowState.Maximized;
                        _isMaximized = true;
                    }

                    break;
                }
                case "Grid_Exit":
                {
                    if (!_isSaved)
                    {
                        MessageBoxResult messageBoxResult = MessageBox.Show("Сохранить изменения?", "Подтверждение",
                            MessageBoxButton.YesNo);
                        if (messageBoxResult == MessageBoxResult.Yes)
                        {
                            SavePatient();
                        }
                        else
                        {
                            Close_Win();
                        }
                    }
                    else
                    {
                        Close_Win();
                    }

                    break;
                }
            }
        }

        //Закрытие из окна
        private void Close_Win()
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            
            if (!mainWindow.IsActive)
            {
                return;
            }

            AnimationOpacity(0.2, 3);
            SetTimer_ForClose();
        }

        //Изменение размеров и положения окна в зависимости от клика и движения курсра
        private void WholeWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var el = e.Source as FrameworkElement;
            if (e.Source.ToString()!.Contains("Border") || e.Source.ToString()!.Contains("Image"))
            {
                try
                {
                    Grid grid = (Grid)sender;
                    Border border = (Border)VisualTreeHelper.GetChild(grid, 0);
                    border.BorderBrush = new SolidColorBrush(Color.FromRgb(171, 42, 23));
                }
                catch
                {
                    if (el != null && 
                        e.LeftButton == MouseButtonState.Pressed && 
                        Array.IndexOf(ArrOfElDontMOve, el.Name) == -1)
                    {
                        DragMove();
                    }
                }
            }
            else
            {
                if (el != null && 
                    e.LeftButton == MouseButtonState.Pressed && 
                    Array.IndexOf(ArrOfElDontMOve, el.Name) == -1)
                {
                    DragMove();
                }
            }
        }

        //Заполнение полей карточки выбранного пациента
        private void SetValues()
        {
            fullName_patient.Text = NewPatient.P_fullName_patient;
            numOf_patient.Text = NewPatient.P_numOf_patient.ToString();
            numOf_center.Text = NewPatient.P_numOf_center.ToString();
            if (NewPatient.P_typeOf_drug == "Kasark")
            {
                Kasark.IsChecked = true;
            }
            else
            {
                Atakand.IsChecked = true;
            }

            SetDate(NewPatient.P_dateOf_visit1, dateOf_visit1);
            SetDate(NewPatient.P_dateOf_visit2, dateOf_visit2);
            SetDate(NewPatient.P_dateOf_visit3, dateOf_visit3);
            SetDate(NewPatient.P_dateOf_visit4, dateOf_visit4);

            SetTime(NewPatient.P_timeOf_visit2, timeOf_visit2);
            SetTime(NewPatient.P_timeOf_visit3, timeOf_visit3);
            SetTime(NewPatient.P_timeOf_visit4, timeOf_visit4);

            
            numOf_issuedPills_visit1.Text = (NewPatient.P_numOf_issuedPills_visit1 == 0
                ? null
                : NewPatient.P_numOf_issuedPills_visit1.ToString()) ?? string.Empty;

            numOf_issuedPills_visit2.Text = (NewPatient.P_numOf_issuedPills_visit2 == 0
                ? null
                : NewPatient.P_numOf_issuedPills_visit2.ToString()) ?? string.Empty;
            
            numOf_issuedPills_visit3.Text = (NewPatient.P_numOf_issuedPills_visit3 == 0
                ? null
                : NewPatient.P_numOf_issuedPills_visit3.ToString()) ?? string.Empty;

            switch (NewPatient.P_doseOf_Drug_visit1)
            {
                case 8:
                {
                    massDrug_8_visit1.IsChecked = true;
                    break;
                }
                case 16:
                {
                    massDrug_16_visit1.IsChecked = true;
                    break;
                }
                case 32:
                {
                    massDrug_32_visit1.IsChecked = true;
                    break;
                }
            }

            switch (NewPatient.P_doseOf_Drug_visit2)
            {
                case 8:
                {
                    massDrug_8_visit2.IsChecked = true;
                    break;
                }
                case 16:
                {
                    massDrug_16_visit2.IsChecked = true;
                    break;
                }
                case 32:
                {
                    massDrug_32_visit2.IsChecked = true;
                    break;
                }
            }

            switch (NewPatient.P_doseOf_Drug_visit3)
            {
                case 8:
                {
                    massDrug_8_visit3.IsChecked = true;
                    break;
                }
                case 16:
                {
                    massDrug_16_visit3.IsChecked = true;
                    break;
                }
                case 32:
                {
                    massDrug_32_visit3.IsChecked = true;
                    break;
                }
            }

            serial_number_visit1.SelectedItem = GetSeries(NewPatient.P_typeOf_drug, NewPatient.P_doseOf_Drug_visit1,
                NewPatient.P_serial_number_visit1);
            serial_number_visit2.SelectedItem = GetSeries(NewPatient.P_typeOf_drug, NewPatient.P_doseOf_Drug_visit2,
                NewPatient.P_serial_number_visit2);
            serial_number_visit3.SelectedItem = GetSeries(NewPatient.P_typeOf_drug, NewPatient.P_doseOf_Drug_visit3,
                NewPatient.P_serial_number_visit3);

            SetDate(NewPatient.P_dateOf_shelfLife_visit1, dateOf_shelfLife_visit1);
            SetDate(NewPatient.P_dateOf_shelfLife_visit2, dateOf_shelfLife_visit2);
            SetDate(NewPatient.P_dateOf_shelfLife_visit3, dateOf_shelfLife_visit3);

            SetDate(NewPatient.P_dateOf_startTakingPills_visit1, dateOf_startTakingPills_visit1);
            SetDate(NewPatient.P_dateOf_startTakingPills_visit2, dateOf_startTakingPills_visit2);
            SetDate(NewPatient.P_dateOf_startTakingPills_visit3, dateOf_startTakingPills_visit3);

            SetTime(NewPatient.P_timeOf_startTakingPills_visit1, timeOf_startTakingPills_visit1);
            SetTime(NewPatient.P_timeOf_startTakingPills_visit2, timeOf_startTakingPills_visit2);
            SetTime(NewPatient.P_timeOf_startTakingPills_visit3, timeOf_startTakingPills_visit3);

            SetTime(NewPatient.P_timeOf_endTakingPills_visit1, timeOf_endTakingPills_visit1);
            SetTime(NewPatient.P_timeOf_endTakingPills_visit2, timeOf_endTakingPills_visit2);
            SetTime(NewPatient.P_timeOf_endTakingPills_visit3, timeOf_endTakingPills_visit3);

            numOf_acceptedPills_visit1.Text = NewPatient.P_numOf_acceptedPills_visit1 == 0
                ? null
                : NewPatient.P_numOf_acceptedPills_visit1.ToString();
            numOf_acceptedPills_visit2.Text = NewPatient.P_numOf_acceptedPills_visit1 == 0
                ? null
                : NewPatient.P_numOf_acceptedPills_visit1.ToString();
            numOf_acceptedPills_visit3.Text = NewPatient.P_numOf_acceptedPills_visit1 == 0
                ? null
                : NewPatient.P_numOf_acceptedPills_visit1.ToString();

            numOf_blankBlister_visit1.Text = NewPatient.P_numOf_blankBlister_visit1 == 0
                ? null
                : NewPatient.P_numOf_blankBlister_visit1.ToString();
            numOf_blankBlister_visit2.Text = NewPatient.P_numOf_blankBlister_visit2 == 0
                ? null
                : NewPatient.P_numOf_blankBlister_visit2.ToString();
            numOf_blankBlister_visit3.Text = NewPatient.P_numOf_blankBlister_visit3 == 0
                ? null
                : NewPatient.P_numOf_blankBlister_visit3.ToString();

            numOf_balancePills_visit1.Text = NewPatient.P_numOf_balancePills_visit1 == 0
                ? null
                : NewPatient.P_numOf_balancePills_visit1.ToString();
            numOf_balancePills_visit2.Text = NewPatient.P_numOf_balancePills_visit2 == 0
                ? null
                : NewPatient.P_numOf_balancePills_visit2.ToString();
            numOf_balancePills_visit3.Text = NewPatient.P_numOf_balancePills_visit3 == 0
                ? null
                : NewPatient.P_numOf_balancePills_visit3.ToString();
        }

        //Установление времени
        private static void SetTime(DateTime patientTime, MaterialDesignThemes.Wpf.TimePicker timePicker)
        {
            if (patientTime == new DateTime(0001, 01, 01, 0, 0, 0))
            {
                timePicker.SelectedTime = null;
            }
            else
            {
                timePicker.SelectedTime = patientTime;
            }
        }

        //Установление даты
        private static void SetDate(DateTime patientDate, DatePicker datePicker)
        {
            if (patientDate == new DateTime(0001, 01, 01))
            {
                datePicker.DisplayDate = DateTime.Today;
            }
            else
            {
                datePicker.SelectedDate = patientDate;
            }
        }

        //Определение серии таблеток
        private int GetSeries(string drug, int dose, string serial)
        {
            return dose switch
            {
                8 => drug == "Kasark"
                    ? Array.IndexOf(series_8_Kasark, serial)
                    : Array.IndexOf(series_8_Atakand, serial),
                16 => drug == "Kasark"
                    ? Array.IndexOf(series_16_Kasark, serial)
                    : Array.IndexOf(series_16_Atakand, serial),
                _ => drug == "Kasark"
                    ? Array.IndexOf(series_32_Kasark, serial)
                    : Array.IndexOf(series_32_Atakand, serial)
            };
        }

        //Таймер для отложного закрытие окна
        private void SetTimer_ForClose()
        {
            _dispatcherTimerForClose.Tick += timerPick_CloseWin;
            _dispatcherTimerForClose.Interval = TimeSpan.FromSeconds(0.2);
            _dispatcherTimerForClose.Start();
        }

        //По истечению таймера
        private void timerPick_CloseWin(object sender, EventArgs e)
        {
            _dispatcherTimerForClose.Stop();
            Close();
        }

        //Проверка открыто ли окно
        private static bool IsWindowOpen<T>(string name = "") where T : Window
        {
            return string.IsNullOrEmpty(name)
                ? Application.Current.Windows.OfType<T>().Any()
                : Application.Current.Windows.OfType<T>().Any(w => w.Name.Equals(name));
        }
    }
}
