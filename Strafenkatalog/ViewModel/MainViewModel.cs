using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using Strafenkatalog.DataAccess;
using Strafenkatalog.Model;
using Strafenkatalog.View;
using System.Collections.ObjectModel;

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
            var fTypes = _dbContext.FineTypes.ToList();

            using (var package = new ExcelPackage())
            {
                int length = fTypes.Count() + 2;
                int height = PlayerList.Count() + 1;

                decimal? sum = 0.0m;
                var worksheet = package.Workbook.Worksheets.Add("Strafen");

                worksheet.Cells[1, 1].Value = "Namen";
                
                //Header der Tabelle
                for (int i = 0; i < fTypes.Count(); i++)
                {
                    worksheet.Cells[1, i + 2].Value = fTypes[i].Name;
                    worksheet.Cells[1, i + 2].AutoFitColumns();
                };

                worksheet.Cells[1, length].Value = "Betrag";

                //Spieler Auflistung und deren Strafen
                for(int i = 0; i < PlayerList.Count(); i++)
                {
                    var player = PlayerList[i];

                    worksheet.Cells[i + 2, 1].Value = player.NickName;
                    
                    for(int j = 0; j < fTypes.Count(); j++)
                    {
                        var test = _dbContext.FinesGiven.Where<FineGiven>(x => x.PlayerId == player.PlayerId && x.FineTypeId == fTypes[j].FineTypeId && x.Paid == false);
                        var result = test.ToList();

                        var count = result.Sum(x => x.Count);
                        sum += count * (fTypes[j].Sum ?? 0);
                        
                        worksheet.Cells[i + 2, j + 2].Value = count * fTypes[j].Sum;             
                        worksheet.Cells[i + 2, j + 2].Style.Numberformat.Format = "#,##0.00€";
                    }

                    worksheet.Cells[i + 2, length].Value = sum;
                    worksheet.Cells[i + 2, length].Style.Numberformat.Format = "#,##0.00€";
                    worksheet.Cells[i + 2, length].Style.Font.Bold = true;

                    sum = 0;
                }

                // Tabelle erstellen
                var range = worksheet.Cells[1, 1, height, length];
                var table = worksheet.Tables.Add(range, "Strafen");
                table.TableStyle = TableStyles.Medium4;

                var excelData = package.GetAsByteArray();

                using var stream = new MemoryStream(excelData);
                var path = await fileSaver.SaveAsync("Strafenkatalog.xlsx", stream, default);

            };
        }
    }
}
