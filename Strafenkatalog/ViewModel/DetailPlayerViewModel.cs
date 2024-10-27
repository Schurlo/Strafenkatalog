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
        Player player;

        public DetailPlayerViewModel()
        {
            _dbContext = new HandyDbContext();
        }

        public void Check()
        {
            FineList.Clear();

            foreach (FineGiven fine in _dbContext.FinesGiven.Where<FineGiven>(i => i.PlayerId == Player.PlayerId))
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
        public async Task DeleteFine(FineGiven fine)
        {
            if (await Shell.Current.DisplayAlert("Löschen", "Wollen Sie wirklich die Strafe löschen?", "Ja", "Nein"))
            {
                FineType? finetype = await _dbContext.FineTypes.FindAsync(fine.FineTypeId);

                Player.Betrag -= finetype.Sum * fine.Count;

                _dbContext.Players.Update(Player);

                _dbContext.FinesGiven.Remove(fine);
                _dbContext.SaveChanges();
                FineList.Remove(fine);
            }
        }
    }
}
