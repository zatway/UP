using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using UP.Command;
using EKZ.Models;
using EKZ.Services;
using EKZ.Views;
using Microsoft.EntityFrameworkCore;

namespace EKZ.ViewModels
{
    public class AutorizationViewModel : BaseViewModel
    {
        // Модель пользователя, данные о котором будут использоваться в процессе авторизации
        private readonly User _model;
        private string _captchaInput; // Ввод капчи пользователем
        private DataService _dataService; // Сервис для работы с данными
        private MyDbContext _context; // Контекст базы данных

        public AutorizationViewModel()
        {
            _model = new User();
            _context = new MyDbContext();
            _dataService = new DataService(_context);
            GenerateCaptcha(); // Генерация капчи при инициализации

            // Инициализация команд
            LoginCommand = new RelayCommand(ExecuteLogin);
            RegisterCommand = new RelayCommand(ExecuteRegister);
            CloseCommand = new RelayCommand(CloseWindow);
        }

        // Свойства для получения и установки значений имени пользователя, пароля и введенной капчи
        public string Username
        {
            get => _model.Username;
            set
            {
                _model.Username = value;
                OnPropertyChanged(); // Уведомление об изменении свойства
            }
        }

        public string Password
        {
            get => _model.Password;
            set
            {
                _model.Password = value;
                OnPropertyChanged(); // Уведомление об изменении свойства
            }
        }
        
        public string CaptchaInput
        {
            get => _captchaInput;
            set
            {
                _captchaInput = value;
                OnPropertyChanged(); // Уведомление об изменении свойства
            }
        }

        // Свойства для отображения капчи
        private string _captchaText; // Сгенерированный текст капчи
        private BitmapImage _captchaImage; // Изображение капчи

        public BitmapImage CaptchaImage
        {
            get => _captchaImage;
            set
            {
                _captchaImage = value;
                OnPropertyChanged(); // Уведомление об изменении свойства
            }
        }

        // Команды для входа, регистрации и закрытия окна
        public RelayCommand LoginCommand { get; }
        public RelayCommand RegisterCommand { get; }
        public ICommand CloseCommand { get; private set; }

        // Метод для закрытия окна
        private void CloseWindow(object parameter)
        {
            var window = parameter as Window;
            window?.Close(); // Закрытие окна, если оно передано
        }  
        
        // Метод для выполнения входа
        public void ExecuteLogin(object parameter)
        {
            if (CaptchaInput != _captchaText)
            {
                MessageBox.Show("Капча введена некорректно", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                GenerateCaptcha();
                return;
            }

            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                MessageBox.Show("Все поля должны быть заполнены.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var hashedPassword = HashPassword(Password);
            var user = _context.Users.FirstOrDefault(u => u.Username == Username && u.Password == hashedPassword);

            if (user != null)
            {
                var role = _context.UserRoles
                    .Include(ur => ur.Role)
                    .FirstOrDefault(ur => ur.UserId == user.Id)?.Role.Name;
                MessageBox.Show(role, "Роль", MessageBoxButton.OK, MessageBoxImage.Information);

                    var currentWindow = parameter as Window;
                    currentWindow?.Close();
                    
                
                    var mainView = new MainView()
                    {
                        DataContext = new MainViewModel(_context, _dataService, role) // Привязка DataContext
                    };
                    mainView.Show();
                    CurrentRole.Role = role;
            }
            else
            {
                MessageBox.Show("Неверное имя пользователя или пароль.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Метод для выполнения регистрации
        private async void ExecuteRegister(object parameter)
{
    // Проверка правильности ввода капчи
    if (CaptchaInput != _captchaText)
    {
        MessageBox.Show("Капча введена некорректно", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        GenerateCaptcha(); // Генерация новой капчи
        return;
    }

    // Проверка на пустые поля
    if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
    {
        MessageBox.Show("Все поля должны быть заполнены.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        return;
    }

    // Хеширование пароля
    var hashedPassword = HashPassword(Password);

    // Проверка, существует ли уже пользователь с таким именем
    var userExists = await _context.Users.AnyAsync(u => u.Username == Username);

    // Если пользователь уже существует, выводим сообщение об ошибке
    if (userExists)
    {
        MessageBox.Show("Такой пользователь уже существует.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        return;
    }

    // Создание нового пользователя
    var newUser = new User { Username = Username, Password = hashedPassword };
    await _context.Users.AddAsync(newUser); // Добавление пользователя в базу данных
    await _context.SaveChangesAsync(); // Сохранение изменений в базе данных

    // Проверка и добавление ролей, если их нет
    var clientRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "client");
    if (clientRole == null)
    {
        clientRole = new Role { Name = "client" };
        await _context.Roles.AddAsync(clientRole);
        await _context.SaveChangesAsync();
    }

    var adminRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "admin");
    if (adminRole == null)
    {
        adminRole = new Role { Name = "admin" };
        await _context.Roles.AddAsync(adminRole);
        await _context.SaveChangesAsync();
    }

    // Добавление роли в таблицу UserRoles
    if (Username == "admin" && Password == "admin")
    {
        // Присваиваем роль admin
        var userRole = new UserRole { UserId = newUser.Id, RoleId = adminRole.Id };
        await _context.UserRoles.AddAsync(userRole);
    }
    else
    {
        // Присваиваем роль client
        var userRole = new UserRole { UserId = newUser.Id, RoleId = clientRole.Id };
        await _context.UserRoles.AddAsync(userRole);
    }

    await _context.SaveChangesAsync(); // Сохранение изменений в таблице UserRoles

    MessageBox.Show("Регистрация прошла успешно! Теперь вы можете войти в систему.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
}
        
        // Метод для генерации капчи
        private void GenerateCaptcha()
        {
            _captchaText = Guid.NewGuid().ToString("N").Substring(0, 6); // Генерация случайной строки длиной 6 символов
            CaptchaImage = GenerateCaptchaImage(_captchaText); // Генерация изображения капчи
        }

        // Метод для генерации изображения капчи
        private BitmapImage GenerateCaptchaImage(string text)
        {
            var random = new Random();
            var bitmap = new System.Drawing.Bitmap(200, 80); // Размер изображения

            using (var g = System.Drawing.Graphics.FromImage(bitmap))
            {
                // Заливка фона случайным цветом
                g.Clear(System.Drawing.Color.FromArgb(random.Next(200, 255), random.Next(200, 255), random.Next(200, 255)));

                // Рисование случайных линий
                for (int i = 0; i < 10; i++)
                {
                    var pen = new System.Drawing.Pen(System.Drawing.Color.FromArgb(random.Next(100, 255), random.Next(100, 255), random.Next(100, 255)));
                    g.DrawLine(pen, random.Next(bitmap.Width), random.Next(bitmap.Height), random.Next(bitmap.Width), random.Next(bitmap.Height));
                }

                // Рисование случайных точек
                for (int i = 0; i < 100; i++)
                {
                    bitmap.SetPixel(random.Next(bitmap.Width), random.Next(bitmap.Height), System.Drawing.Color.FromArgb(random.Next(150, 255), random.Next(150, 255), random.Next(150, 255)));
                }

                // Рисование текста капчи
                var font = new System.Drawing.Font("Arial", 28, System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic);
                var brush = new System.Drawing.SolidBrush(System.Drawing.Color.FromArgb(random.Next(0, 150), random.Next(0, 150), random.Next(0, 150)));

                // Создание кривой линии для текста
                var textPath = new System.Drawing.Drawing2D.GraphicsPath();
                textPath.AddString(text, font.FontFamily, (int)font.Style, font.Size, new System.Drawing.Point(random.Next(10, 30), random.Next(10, 30)), new System.Drawing.StringFormat());
                var matrix = new System.Drawing.Drawing2D.Matrix();
                matrix.RotateAt(random.Next(-20, 20), new System.Drawing.PointF(bitmap.Width / 2, bitmap.Height / 2));
                textPath.Transform(matrix);

                g.FillPath(brush, textPath); // Заполнение текста на изображении
            }

            // Конвертация изображения в формат BitmapImage для отображения в WPF
            var bitmapImage = new BitmapImage();
            using (var memory = new System.IO.MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Png); // Сохранение изображения в память
                memory.Position = 0;

                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
            }

            return bitmapImage; // Возвращение изображения
        }

        // Метод для хеширования пароля с использованием SHA256
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password); // Преобразование пароля в байты
                var hash = sha256.ComputeHash(bytes); // Хеширование
                return Convert.ToBase64String(hash); // Преобразование хеша в строку Base64
            }
        }
    }
}
