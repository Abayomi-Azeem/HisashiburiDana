using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HisashiburiDana.Contract.Common
{
    public class Enumerations
    {
        public enum AnimeStatus
        {
            EMPTY = 0,
            FINISHED = 1,
            RELEASING = 2,
            NOT_YET_RELEASED = 3,
            CANCELLED = 4,
            HIATUS = 5
        }
    }
}
