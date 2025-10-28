using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace TRPO7
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Doctor CurrentDoctor { get; set; }
        private int _kolvoUser = 0;
        private int _kolvoPacient = 0;

        public int KolvoUser
        {
            get => _kolvoUser;
            set
            {
                _kolvoUser = value;
                OnPropertyChanged();
            }
        }

        public int KolvoPacient
        {
            get => _kolvoPacient;
            set
            {
                _kolvoPacient = value;
                OnPropertyChanged();
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            CurrentDoctor = new Doctor();
            DataContext = this;           
            UpdateCounts();
        }
        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            var firstNameBox = FindName("FirstNameTextBox") as TextBox;
            var lastNameBox = FindName("LastNameTextBox") as TextBox;
            var middleNameBox = FindName("MiddleNameTextBox") as TextBox;
            var specialisationBox = FindName("SpecialisationTextBox") as TextBox;
            var passwordBox = FindName("PasswordTextBox") as TextBox;
            var confirmPasswordBox = FindName("ConfirmPasswordTextBox") as TextBox;
            if (string.IsNullOrWhiteSpace(firstNameBox?.Text) ||
                string.IsNullOrWhiteSpace(lastNameBox?.Text) ||
                string.IsNullOrWhiteSpace(middleNameBox?.Text) ||
                string.IsNullOrWhiteSpace(specialisationBox?.Text) ||
                string.IsNullOrWhiteSpace(passwordBox?.Text) ||
                string.IsNullOrWhiteSpace(confirmPasswordBox?.Text))
            {
                MessageBox.Show("Все поля обязательны для заполнения!");
                return;
            }
            if (passwordBox.Text != confirmPasswordBox.Text)
            {
                MessageBox.Show("Пароли не совпадают!");
                return;
            }
            CurrentDoctor.ID = GenerateUniqueDoctorId();
            CurrentDoctor.Name = firstNameBox.Text;
            CurrentDoctor.LastName = lastNameBox.Text;
            CurrentDoctor.MiddleName = middleNameBox.Text;
            CurrentDoctor.Specialisation = specialisationBox.Text;
            CurrentDoctor.Password = passwordBox.Text;
            var jsonString = JsonSerializer.Serialize(CurrentDoctor);
            File.WriteAllText($"D_{CurrentDoctor.ID}.txt", jsonString);
            MessageBox.Show($"Регистрация успешна!\nВаш ID: {CurrentDoctor.ID}");
            UpdateCounts();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var idBox = FindName("LoginIdTextBox") as TextBox;
            var passwordBox = FindName("LoginPasswordTextBox") as TextBox;
            if (string.IsNullOrWhiteSpace(idBox?.Text) || string.IsNullOrWhiteSpace(passwordBox?.Text))
            {
                MessageBox.Show("Введите ID и пароль!");
                return;
            }
            if (!int.TryParse(idBox.Text, out int id))
            {
                MessageBox.Show("Неверный формат ID!");
                return;
            }
            string filePath = $"D_{id}.txt";
            if (!File.Exists(filePath))
            {
                MessageBox.Show("Пользователь с таким ID не найден!");
                return;
            }
            try
            {
                var json = File.ReadAllText(filePath);
                var doctor = JsonSerializer.Deserialize<Doctor>(json);
                if (doctor.Password != passwordBox.Text)
                {
                    MessageBox.Show("Неверный пароль!");
                    return;
                }
                CurrentDoctor.ID = doctor.ID;
                CurrentDoctor.Name = doctor.Name;
                CurrentDoctor.LastName = doctor.LastName;
                CurrentDoctor.MiddleName = doctor.MiddleName;
                CurrentDoctor.Specialisation = doctor.Specialisation;
                CurrentDoctor.Password = doctor.Password;
                MessageBox.Show("Успешный вход!");
                UpdateCounts();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при входе: {ex.Message}");
            }
        }

        private int GenerateUniqueDoctorId()
        {
            Random rnd = new Random();
            int id;
            do
            {
                id = rnd.Next(10000, 99999);
            } while (File.Exists($"D_{id}.txt"));
            return id;
        }

        private void UpdateCounts()
        {
            KolvoUser = Directory.GetFiles(".", "D_*.txt").Length;
            KolvoPacient = Directory.GetFiles(".", "P_*.txt").Length;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}