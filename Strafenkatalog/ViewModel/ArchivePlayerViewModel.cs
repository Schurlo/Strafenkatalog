using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using Strafenkatalog.DataAccess;
using Strafenkatalog.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Strafenkatalog.ViewModel
{
    [QueryProperty(nameof(Player), nameof(Player))]
    public partial class ArchivePlayerViewModel : ObservableObject
    {
        private HandyDbContext _dbContext;

        public ObservableCollection<FineGiven> FineList { get; set; } = new();

        [ObservableProperty]
        Player player;

        public ArchivePlayerViewModel()
        {
            _dbContext = new HandyDbContext();
        }

        public void Check()
        {
            FineList.Clear();

            foreach (FineGiven fine in _dbContext.FinesGiven.Where<FineGiven>(i => i.PlayerId == Player.PlayerId && i.Paid == true))
            {
                FineList.Add(fine);
            }
        }

        [RelayCommand]
        public async Task DeleteFine(FineGiven fine)
        {
            if (await Shell.Current.DisplayAlert("Wiederherstellen | Löschen", "Wollen Sie die Strafe wiederherstellen oder löschen?", "Löschen", "Wiederherstellen"))
            {
                _dbContext.FinesGiven.Remove(fine);
                _dbContext.SaveChanges();
                FineList.Remove(fine);
            }
            else
            {
                FineType? finetype = await _dbContext.FineTypes.FindAsync(fine.FineTypeId);
                fine.Paid = false;
                Player.Betrag += finetype.Sum * fine.Count;
                _dbContext.Players.Update(Player);
                _dbContext.FinesGiven.Update(fine);
                _dbContext.SaveChanges();
                FineList.Remove(fine);
            }
        }

        [RelayCommand]
        public async Task DeleteAllFine()
        {
            if(await Shell.Current.DisplayAlert("Löschen", "Wollen Sie wirklich alle Strafen löschen?", "Ja", "Nein"))
            {
                var cFineList = FineList.ToList();

                foreach(FineGiven fine in cFineList)
                {
                    _dbContext.FinesGiven.Remove(fine);
                    _dbContext.SaveChanges();
                    FineList.Remove(fine);
                }
            }
        }
    }
}
