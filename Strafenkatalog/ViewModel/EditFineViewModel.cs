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
        private HandyDbContext _dbContext;

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
                if(FineSum != Fine.Sum && await Shell.Current.DisplayAlert("Achtung","Durch das Ändern des Betrages, wird die Gesamtsumme der Spieler mitverändert.", "OK", "Abbrechen"))
                {
                    foreach(Player player in _dbContext.Players)
                    {
                        var count = _dbContext.FinesGiven.Where<FinesGiven>(f => f.PlayerId == player.PlayerId && f.FineTypeId == Fine.FineTypeId).Count();

                        if(FineSum > Fine.Sum)
                        {
                            decimal x = FineSum - Fine.Sum;
                            player.Betrag += count * x;
                        }
                        else
                        {
                            decimal x = Fine.Sum - FineSum;
                            player.Betrag -= count * x;
                        }

                        _dbContext.Players.Update(player);
                    }
                }

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

        [RelayCommand]
        async Task DeleteFine()
        {
            if(await Shell.Current.DisplayAlert("Löschen","Sind Sie sicher die Strafenart zu löschen? Es werden alle davon verteilten Strafen mitgelöscht", "Ja", "Nein"))
            {
                foreach(FineGiven fine in _dbContext.FinesGiven.Where<FineGiven>(i => i.FineTypeId == Fine.FineTypeId))
                {
                    _dbContext.FinesGiven.Remove(fine);
                }
                _dbContext.Fines.Remove(Fine);
                _dbContext.SaveChanges();

                await Shell.Current.GoToAsync("../..");
            }
        }

    }
}
