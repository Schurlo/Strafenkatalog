using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CommunityToolkit.Mvvm.ComponentModel;

namespace Strafenkatalog.Model
{
    public partial class Player : ObservableObject
    {
        public int PlayerId { get; set; }

        [ObservableProperty]
        public string? firstName;

        [ObservableProperty]
        public string? lastName;

        [ObservableProperty]
        public string? nickName;

        [ObservableProperty]
        public string? fullName;

        [ObservableProperty]
        public decimal? betrag;

    }
}
