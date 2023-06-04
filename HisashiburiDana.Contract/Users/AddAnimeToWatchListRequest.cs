using HisashiburiDana.Contract.AnimeManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HisashiburiDana.Contract.Users
{
    public class AddAnimeToWatchListRequest
    {
        public string UserId { get; set; }

        public Medium Media { get; set; }
    }
}
