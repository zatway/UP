using System.Collections.ObjectModel;
using EKZ.Models;
using EKZ.Services;
using Microsoft.EntityFrameworkCore;

namespace EKZ.ViewModels;

public class CarsReportViewModell
{
    private readonly MyDbContext _dbContext;
    public ObservableCollection<CarsReport> CarsReportData { get; set; }

    public CarsReportViewModell(MyDbContext dbContext)
    {
        _dbContext = dbContext;
        CarsReportData = new ObservableCollection<CarsReport>(_dbContext.CarsReport.ToList());
    }
}