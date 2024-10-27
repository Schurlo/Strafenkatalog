using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Strafenkatalog.DataAccess;
using Strafenkatalog.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Strafenkatalog.ViewModel
{
    [QueryProperty(nameof(Fine), nameof(Fine))]
    [QueryProperty(nameof(FineEdit), nameof(FineEdit))]
    [QueryProperty(nameof(FineAdd), nameof(FineAdd))]
    public partial class EditFineViewModel : ObservableObject
    {
        private readonly HandyDbContext _dbContext;

        [ObservableProperty]
        bool fineEdit;

        [ObservableProperty]
        bool fineAdd;

        [ObservableProperty]
        FineType fine;

        [ObservableProperty]
        string? fineName;

        [ObservableProperty]
        decimal? fineSum;

        public EditFineViewModel() 
        {
            _dbContext = new HandyDbContext();
        }

        public void Check()
        {
            if (FineEdit == true)
            {
                FineName = Fine.Name;
                FineSum = Fine.Sum;
            }
        }

        [RelayCommand]
        async Task EditFine()
        {
            if (!string.IsNullOrWhiteSpace(FineName) || FineSum == null || FineSum <= 0)
            {
                Fine.Name = FineName;
                Fine.Sum = FineSum;

                _dbContext.FineTypes.Update(Fine);
                _dbContext.SaveChanges();

                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await Shell.Current.DisplayAlert("Ausfüllen", "Es sind nicht alle Felder ausgefüllt", "OK");
            }
        }

        [RelayCommand]
        async Task AddFine()
        {
            if (!string.IsNullOrWhiteSpace(FineName) || FineSum == null || FineSum <= 0)
            {
                FineType fine = new()
                {
                    Name = FineName,
                    Sum = FineSum
                };

                _dbContext.FineTypes.Add(fine);
                _dbContext.SaveChanges();

                await Shell.Current.GoToAsync("..");
            }
            else
            {
               await Shell.Current.DisplayAlert("Ausfüllen", "Es sind nicht alle Felder ausgefüllt", "OK");
            }
        }

    }
}
