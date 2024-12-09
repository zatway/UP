using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using EKZ.Models;
using UP.Command;
using EKZ.Services;
using EKZ.Views;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using RepairSummary = EKZ.Views.RepairSummary;

namespace EKZ.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public ObservableCollection<string> FilterOptions { get; set; } = new ObservableCollection<string> { "Все", "По клиенту", "По статусу" };
        public string SelectedFilter { get; set; }
        public string SearchText { get; set; }
        public ObservableCollection<ReportRow> FilteredData { get; set; } = new ObservableCollection<ReportRow>();

        private readonly MyDbContext _dbContext;
        private readonly DataService _dataService;

        public readonly string Role;
        public ICommand ApplyFilterCommand { get; }
        public ICommand ResetFilterCommand { get; }
        public ICommand GeneratePdfCommand { get; }
        public ICommand CancelFilterCommand { get; }
        public ICommand GoCarsReportCommand { get; }
        public ICommand GoRepairSummaryCommand { get; }
        public ICommand AddCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand DeleteCommand { get; }

        public MainViewModel(MyDbContext myDbContext, DataService dataService, string role)
        {
            _dbContext = myDbContext;
            _dataService = dataService;
            Role = role;
            OnPropertyChanged("Role");

            ApplyFilterCommand = new RelayCommand(ApplyFilter);
            ResetFilterCommand = new RelayCommand(ResetFilter);
            GeneratePdfCommand = new RelayCommand(GeneratePdf);
            CancelFilterCommand = new RelayCommand(ResetFilter);
            GoCarsReportCommand = new RelayCommand(GoCarsReport);
            GoRepairSummaryCommand = new RelayCommand(GoRepairSummary);
            AddCommand = new RelayCommand(EditСlient);
            UpdateCommand = new RelayCommand(UpdateRecord);

            LoadData();
        }
        
        private void LoadData()
        {
            var data = from request in _dbContext.Requests
                join client in _dbContext.Clients on request.ClientID equals client.ID
                join repair in _dbContext.Repairs on request.ID equals repair.RequestID
                join service in _dbContext.Services on repair.ServiceID equals service.ID
                select new ReportRow
                {
                    RequestId = request.ID,
                    RequestDate = request.CreationDate,
                    ClientName = client.FullName,
                    ServiceName = service.Name,
                    ServiceCost = repair.Cost,
                    RequestStatus = request.Status
                };

            FilteredData = new ObservableCollection<ReportRow>(data.ToList());
        }

        private async void EditСlient(object parameter)
        {
                if (Role == "admin")
                { 
                    var currentWindow = parameter as Window;
                    currentWindow?.Close(); // Закрытие текущего окна

                    var clientAddedView = new ClientAddedView()
                    {
                        DataContext = new ClientAddedViewModel(_dbContext) // Привязка DataContext
                    };
                    clientAddedView.Show();
                }
                else
                {
                    MessageBox.Show("У вас нет прав для выполнения этого действия.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
        }

        
            private async void UpdateRecord(object parameter)
            {
                // Пример обновления записи
                var requestToUpdate = FilteredData.FirstOrDefault();
                if (requestToUpdate != null)
                {
                    requestToUpdate.RequestStatus = "Обновлено"; // Пример обновления
                    await _dataService.UpdateAsync(requestToUpdate);
                    LoadData(); // Перезагружаем данные
                }
            }
        
        private void GoCarsReport(object parameter)
        {
            var currentWindow = parameter as Window;
            currentWindow?.Close(); // Закрытие текущего окна

            var mainView = new CarsReportView()
            {
                DataContext = new CarsReportViewModell(_dbContext) // Привязка DataContext
            };
            mainView.Show();
        }

        private void GoRepairSummary(object parameter)
        {
            var currentWindow = parameter as Window;
            currentWindow?.Close(); // Закрытие текущего окна

            var repairSummaryView = new RepairSummary()
            {
                DataContext = new RepairSummaryViewModel(_dbContext) // Привязка DataContext
            };
            repairSummaryView.Show();
        }
        
        private void ApplyFilter(object parameter)
        {
            var data = from request in _dbContext.Requests
                join client in _dbContext.Clients on request.ClientID equals client.ID into clients
                from client in clients.DefaultIfEmpty()
                join repair in _dbContext.Repairs on request.ID equals repair.RequestID into repairs
                from repair in repairs.DefaultIfEmpty()
                join service in _dbContext.Services on repair.ServiceID equals service.ID into services
                from service in services.DefaultIfEmpty()
                select new ReportRow
                {
                    RequestId = request.ID,
                    RequestDate = request.CreationDate,
                    ClientName = client.FullName,
                    ServiceName = service.Name,
                    ServiceCost = repair.Cost,
                    RequestStatus = request.Status
                };

            if (!string.IsNullOrEmpty(SearchText))
            {
                string lowerSearchText = SearchText.ToLower();

                data = data.Where(d => (d.ClientName.ToLower().Contains(lowerSearchText)) ||
                                       (d.RequestStatus.ToLower().Contains(lowerSearchText)) ||
                                       (d.ServiceName.ToLower().Contains(lowerSearchText)));
            }

            FilteredData.Clear();
            foreach (var item in data)
            {
                FilteredData.Add(item);
            }
        }

        private void ResetFilter(object parameter)
        {
            SelectedFilter = "Все";
            SearchText = string.Empty;
            LoadData();
        }

        private void GeneratePdf(object parameter)
{

    // Создание документа PDF
    Document doc = new Document(PageSize.A4);
    MemoryStream ms = new MemoryStream();
    PdfWriter writer = PdfWriter.GetInstance(doc, ms);

    doc.Open();

    // Заголовок
    Font headerFont = new Font(Font.FontFamily.HELVETICA, 14, Font.BOLD);
    Paragraph header = new Paragraph("Отчет по заявкам", headerFont);
    header.Alignment = Element.ALIGN_CENTER;
    doc.Add(header);

    doc.Add(new Phrase("\n")); // Добавим пустую строку для отделения заголовка

    // Таблица с данными
    PdfPTable table = new PdfPTable(6);
    table.WidthPercentage = 100;
    table.SetWidths(new float[] { 10f, 20f, 20f, 20f, 20f, 20f });

    // Заголовки таблицы
    table.AddCell("Request number");
    table.AddCell("Request date");
    table.AddCell("Client");
    table.AddCell("Service name");
    table.AddCell("Cost");
    table.AddCell("Status");

    // Добавляем данные в таблицу
    foreach (var row in FilteredData)
    {
        table.AddCell(row.RequestId.ToString());
        table.AddCell(row.RequestDate.ToString("dd/MM/yyyy"));
        table.AddCell(row.ClientName);
        table.AddCell(row.ServiceName);
        table.AddCell(row.ServiceCost.ToString("C2"));
        table.AddCell(row.RequestStatus);
    }

    doc.Add(table);

    // Закрытие документа
    doc.Close();

    // Сохранение PDF файла на диск
    SaveFileDialog saveFileDialog = new SaveFileDialog
    {
        FileName = "Report.pdf",
        Filter = "PDF Files (*.pdf)|*.pdf"
    };

    if (saveFileDialog.ShowDialog() == true)
    {
        // Сохранение файла по выбранному пути
        File.WriteAllBytes(saveFileDialog.FileName, ms.ToArray());
    }}
    }

    public class ReportRow
    {
        public int RequestId { get; set; }
        public DateTime RequestDate { get; set; }
        public string ClientName { get; set; }
        public string ServiceName { get; set; }
        public decimal ServiceCost { get; set; }
        public string RequestStatus { get; set; }
    }
}
