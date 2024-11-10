using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Strafenkatalog.DataAccess;
using Strafenkatalog.Model;
using Strafenkatalog.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Strafenkatalog.ViewModel
{
    [QueryProperty(nameof(Player), nameof(Player))]
    public partial class DetailPlayerViewModel : ObservableObject
    {
        private HandyDbContext _dbContext;

        public ObservableCollection<FineGiven> FineList { get; set; } = new();

        [ObservableProperty]
        Player? player;

        public DetailPlayerViewModel()
        {
            _dbContext = new HandyDbContext();
        }

        public void Check()
        {
            FineList.Clear();

            foreach (FineGiven fine in _dbContext.FinesGiven.Where<FineGiven>(i => i.PlayerId == Player.PlayerId && i.Paid == false))
            {
                FineList.Add(fine);
            }
        }

        [RelayCommand]
        public async Task EditPlayer()
        {
            await Shell.Current.GoToAsync(nameof(EditPlayerPage), new Dictionary<string, object>
            {
                ["Player"] = Player
            });
        }

        [RelayCommand]
        public async Task ArchiveFine(FineGiven fine)
        {
            if (await Shell.Current.DisplayAlert("Archivieren", "Wollen Sie wirklich die Strafe archivieren?", "Ja", "Nein"))
            {
                FineType? finetype = await _dbContext.FineTypes.FindAsync(fine.FineTypeId);
                fine.Paid = true;
                Player.Betrag -= finetype.Sum * fine.Count;
                _dbContext.Players.Update(Player);
                _dbContext.FinesGiven.Update(fine);
                _dbContext.SaveChanges();
                FineList.Remove(fine);
            }
        }

        [RelayCommand]
        public async Task ArchiveFineAll()
        {
            if (await Shell.Current.DisplayAlert("Archivieren", "Wollen Sie wirklich alle Strafen archivieren?", "Ja", "Nein"))
            {
                foreach (var fine in FineList.Where(f => f.PlayerId == Player.PlayerId && f.Paid == false).ToList())
                {
                    FineType? fineType = await _dbContext.FineTypes.FindAsync(fine.FineTypeId);

                    if (fineType != null)
                    {
                        fine.Paid = true;
                        Player.Betrag -= fineType.Sum * fine.Count;
                        _dbContext.FinesGiven.Update(fine);
                    }

                    FineList.Remove(fine);
                }

                _dbContext.Players.Update(Player);
                _dbContext.SaveChanges();
            }
        }

        [RelayCommand]
        public async Task Archive()
        {
            await Shell.Current.GoToAsync(nameof(ArchivePlayerPage), new Dictionary<string, object>
            {
                ["Player"] = Player
            });
        }
    }
}
