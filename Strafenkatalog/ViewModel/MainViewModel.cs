using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Strafenkatalog.Model;
using Strafenkatalog.View;
using Strafenkatalog.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Strafenkatalog.DataAccess;
using Microsoft.EntityFrameworkCore;
using CommunityToolkit.Maui.Storage;
using OfficeOpenXml;

namespace Strafenkatalog.ViewModel
{
    public partial class MainViewModel : ObservableObject
    {
        private HandyDbContext? _dbContext;

        public ObservableCollection<Player> PlayerList { get; set; } = new();

        IFileSaver fileSaver;

        public MainViewModel(IFileSaver fileSaver)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            this.fileSaver = fileSaver;

            Shell.Current.Navigated += (sender, e) =>
            {
                ListUpdate();
            };
        }

        public async Task ListUpdate()
        {
            _dbContext = new HandyDbContext();

            PlayerList.Clear();

            var test = await _dbContext.Players.ToListAsync();

            foreach (Player player in test)
            {
                PlayerList.Add(player);
            }
        }

        [RelayCommand]
        public async Task NewPlayer()
        {
            await Shell.Current.GoToAsync(nameof(EditPlayerPage));
        }

        [RelayCommand]
        public async Task Reset()
        {
            await Shell.Current.DisplayAlert("Achtung", "Alle Geldbeträge werden zurückgesetzt!", "Durchführen", "Abbrechen");

            foreach (FineGiven fine in _dbContext.FinesGiven)
            {
                _dbContext.FinesGiven.Remove(fine);
            }

            foreach (Player player in PlayerList)
            {
                player.Betrag = 0.00m;
            }

            _dbContext.SaveChanges();

        }

        [RelayCommand]
        public async Task DetailPlayer(Player player)
        {
            await Shell.Current.GoToAsync(nameof(DetailPlayerPage), new Dictionary<string, object>
            {
                ["Player"] = player
            });
        }

        [RelayCommand]
        public async Task ExcelSheet()
        {
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Strafen");
                worksheet.Cells["A1"].Value = "Hallo Welt!";
                var excelData = package.GetAsByteArray();

                using var stream = new MemoryStream(excelData);
                var path = await fileSaver.SaveAsync("Strafenkatalog.xlsx", stream, default);

            };
        }
    }
}
