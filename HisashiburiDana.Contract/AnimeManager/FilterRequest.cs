using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HisashiburiDana.Contract.Enumerations.StatusEnum;

namespace HisashiburiDana.Contract.AnimeManager
{
    public class FilterRequest
    {
        public AnimeStatus? Status { get; set; } 
        public int? EpisodesGreaterThan { get; set; }
        public string? Genre { get; set; }
        public bool? isAdult { get; set; }
    }
}
