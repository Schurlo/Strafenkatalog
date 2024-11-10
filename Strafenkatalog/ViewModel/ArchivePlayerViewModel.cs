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
            if (await Shell.Current.DisplayAlert("Löschen", "Wollen Sie wirklich die Strafe löschen?", "Ja", "Nein"))
            {
                _dbContext.FinesGiven.Remove(fine);
                _dbContext.SaveChanges();
                FineList.Remove(fine);
            }
        }
    }
}
