using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore.Metadata;
using Strafenkatalog.DataAccess;
using Strafenkatalog.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Strafenkatalog.ViewModel
{
    [QueryProperty(nameof(Player),nameof(Player))]
    public partial class EditPlayerViewModel : ObservableObject
    {
        private readonly HandyDbContext _dbContext;

        [ObservableProperty]
        Player player;

        [ObservableProperty]
        bool playerAdd;

        [ObservableProperty]
        bool playerEdit;

        [ObservableProperty]
        public string? firstName;

        [ObservableProperty]
        public string? lastName;

        [ObservableProperty]
        public string? nickName;

        public EditPlayerViewModel()
        {
            _dbContext = new HandyDbContext();
        }

        public void Check()
        {
            if (Player != null)
            {
                PlayerAdd = false;
                PlayerEdit = true;

                FirstName = Player.FirstName;
                LastName = Player.LastName;
                NickName = Player.NickName;
            }
            else
            {
                PlayerAdd = true;
                PlayerEdit = false;
            }
        }

        [RelayCommand]
        public async Task AddPlayer()
        {
            if (!string.IsNullOrWhiteSpace(FirstName) || !string.IsNullOrWhiteSpace(LastName) || !string.IsNullOrWhiteSpace(NickName))
            {
                Player player = new Player()
                {
                    FirstName = this.FirstName,
                    LastName = this.LastName,
                    NickName = this.NickName,
                    FullName = $"{FirstName} {LastName}",
                    Betrag = 0.00m
                };

                _dbContext.Players.Add(player);
                _dbContext.SaveChanges();

                await Shell.Current.GoToAsync("..");
            }
            else await Shell.Current.DisplayAlert("Ausfüllen", "Es sind nicht alle Felder ausgefüllt", "OK");
        }

        [RelayCommand]
        public async Task EditPlayer()
        {
            if(!string.IsNullOrWhiteSpace(FirstName) || !string.IsNullOrWhiteSpace(LastName) || !string.IsNullOrWhiteSpace(NickName))
            {
                Player.FirstName = FirstName;
                Player.LastName = LastName;
                Player.NickName = NickName;
                Player.FullName = $"{FirstName} {LastName}";

                _dbContext.Players.Update(Player);
                _dbContext.SaveChanges();

                await Shell.Current.GoToAsync("..");
            }
            else await Shell.Current.DisplayAlert("Ausfüllen", "Es sind nicht alle Felder ausgefüllt", "OK");
        }

        [RelayCommand]
        public async Task DeletePlayer()
        {
            if(await Shell.Current.DisplayAlert("Löschen","Sind Sie sicher den Spieler zu löschen?", "Ja", "Nein"))
            {
                foreach(FineGiven fine in _dbContext.FinesGiven.Where<FineGiven>(i => i.PlayerId == Player.PlayerId))
                {
                    _dbContext.FinesGiven.Remove(fine);
                }
                _dbContext.Players.Remove(Player);
                _dbContext.SaveChanges();

                await Shell.Current.GoToAsync("../..");
            }
        }
    }
}
