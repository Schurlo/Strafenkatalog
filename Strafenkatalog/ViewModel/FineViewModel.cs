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
    public partial class FineViewModel : ObservableObject
    {
        private HandyDbContext _dbContext;

        public ObservableCollection<FineType> FineList { get; set; } = new();
        public ObservableCollection<Player> PlayerList { get; set; } = new();

        [ObservableProperty]
        FineType? selectedFine;

        [ObservableProperty]
        Player? selectedPlayer;

        [ObservableProperty]
        DateTime date = DateTime.Now;

        [ObservableProperty]
        int count = 1;

        public FineViewModel(HandyDbContext dbContext) 
        {
            _dbContext = dbContext;

            Shell.Current.Navigated += (sender, e) =>
            {
                ListUpdate();
            };
        }

        void ListUpdate()
        {
            _dbContext = new HandyDbContext();

            PlayerList.Clear();

            foreach (Player player in _dbContext.Players)
            {
                PlayerList.Add(player);
            }

            FineList.Clear();

            foreach (FineType fine in _dbContext.FineTypes)
            {
                FineList.Add(fine);
            }
        }

        [RelayCommand]
        public async Task AddFine()
        {
            if (SelectedFine?.FineTypeId != null && SelectedPlayer?.PlayerId != null)
            {
                FineGiven fineGiven = new FineGiven()
                {
                    FineTypeId = SelectedFine.FineTypeId,
                    PlayerId = SelectedPlayer.PlayerId,
                    FineName = SelectedFine.Name,
                    Date = DateOnly.FromDateTime(Date),
                    Count = Count
                };

                Player? player = await _dbContext.Players.FindAsync(SelectedPlayer.PlayerId);
                FineType? fine = await _dbContext.FineTypes.FindAsync(fineGiven.FineTypeId);

                player.Betrag += fine.Sum * Count;

                _dbContext.Players.Update(player);

                _dbContext.FinesGiven.Add(fineGiven);
                _dbContext.SaveChanges();


                SelectedFine = null;
                Date = DateTime.Now;
                Count = 1;
            }
            else
            {
                await Shell.Current.DisplayAlert("Ausfüllen", "Es sind nicht alle Felder ausgefüllt", "OK");
            }
        }

        [RelayCommand]
        async Task NewFine()
        {
            await Shell.Current.GoToAsync($"{nameof(EditFinePage)}?FineEdit={false}&FineAdd={true}", true);
        }

        [RelayCommand]
        async Task EditFine()
        {
            if(SelectedFine != null)
            {
                await Shell.Current.GoToAsync($"{nameof(EditFinePage)}?FineEdit={true}&FineAdd={false}", new Dictionary<string, object>
                {
                    ["Fine"] = SelectedFine
                });
            }
            else
            {
                await Shell.Current.DisplayAlert("Auswählen", "Wähle eine Strafe aus die du bearbeiten willst", "OK");
            }

        }

    }
}
