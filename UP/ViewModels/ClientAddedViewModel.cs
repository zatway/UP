using System.Linq;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using EKZ.Models;
using EKZ.Services;
using UP.Command;

namespace EKZ.ViewModels
{
    public class ClientAddedViewModel : INotifyPropertyChanged
    {
        private readonly MyDbContext _dbContext;

        private string _fullName;
        private string _phone;
        private string _email;
        private string _address;

        private Client _selectedClient;

        public ObservableCollection<Client> Clients { get; set; }

        public string FullName
        {
            get => _fullName;
            set
            {
                _fullName = value;
                OnPropertyChanged();
            }
        }

        public string Phone
        {
            get => _phone;
            set
            {
                _phone = value;
                OnPropertyChanged();
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }

        public string Address
        {
            get => _address;
            set
            {
                _address = value;
                OnPropertyChanged();
            }
        }

        public Client SelectedClient
        {
            get => _selectedClient;
            set
            {
                _selectedClient = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddClientCommand { get; set; }
        public ICommand DeleteClientCommand { get; set; }

        public ClientAddedViewModel(MyDbContext myDbContext)
        {
            _dbContext = myDbContext;
            Clients = new ObservableCollection<Client>(_dbContext.Clients.ToList());
            AddClientCommand = new RelayCommand(AddClient);
            DeleteClientCommand = new RelayCommand(DeleteClient);
        }

        private void AddClient(object parameter)
        {
            var client = new Client
            {
                FullName = FullName,
                Phone = Phone,
                Email = Email,
                Address = Address
            };

            var searchDoubleClient = Clients.FirstOrDefault(c => c == client);
            if (searchDoubleClient == null)
            {
                Clients.Add(client);
                _dbContext.Clients.AddRange(client);
                
                // Очистка формы после добавления
                FullName = string.Empty;
                Phone = string.Empty;
                Email = string.Empty;
                Address = string.Empty;
            }
            else
            {
                MessageBox.Show("Такой клиент уже существует", FullName, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteClient(object parameter)
        {
            // Предположим, что параметр передает ID клиента или сам объект
            var clientToDelete = Clients.FirstOrDefault(c => c.ID == _selectedClient.ID); // или используйте выбранный клиент из UI

            if (clientToDelete != null)
            {
                try
                {
                    _dbContext.Clients.Remove(clientToDelete); // удаление клиента из базы данных
                    _dbContext.SaveChanges(); // сохранение изменений в базе данных
                    Clients.Remove(clientToDelete); // удаление из коллекции, отображаемой в UI
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при удалении клиента: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Клиент не найден для удаления.");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
