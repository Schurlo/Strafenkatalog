using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;

namespace Strafenkatalog.Model
{
    public partial class FineGiven : ObservableObject
    {
        public int FineGivenId { get; set; }

        [ObservableProperty]
        public string? fineName;

        [ObservableProperty]
        public DateOnly? date;

        [ObservableProperty]
        public int? count;

        [ObservableProperty]
        public int? playerId;

        [ObservableProperty]
        public bool? paid = false;

        [ObservableProperty]
        public int? fineTypeId;
    }
}
