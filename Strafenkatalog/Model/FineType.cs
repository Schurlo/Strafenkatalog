using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;

namespace Strafenkatalog.Model
{
    public partial class FineType : ObservableObject
    {
        public int FineTypeId { get; set; }

        [ObservableProperty]
        public string? name;

        [ObservableProperty]
        public decimal? sum;
    }
}
