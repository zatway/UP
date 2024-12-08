using System.Collections.ObjectModel;
using EKZ.Models;
using EKZ.Services;

namespace EKZ.ViewModels
{
    public class RepairSummaryViewModel
    {
        private readonly MyDbContext _dbContext;
        public ObservableCollection<RepairSummary> RepairSummaryData { get; set; }

        public RepairSummaryViewModel(MyDbContext dbContext)
        {
            _dbContext = dbContext;
            RepairSummaryData = new ObservableCollection<RepairSummary>(_dbContext.RepairSummary.ToList());
        }
    }
}