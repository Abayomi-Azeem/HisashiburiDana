using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HisashiburiDana.Contract.Users
{
    public class GetUserAnimesResponse
    {
        public  List<UserAnime> WatchList { get; set; } = new();

        public List<UserAnime> Watching { get; set; } = new();

        public List<UserAnime> Watched { get; set; } = new();
    }
}
