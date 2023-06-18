using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HisashiburiDana.Contract.Enumerations
{
    public class StatusEnum
    {
        public enum AnimeStatus
        {
            FINISHED = 1,
            RELEASING = 2,
            NOT_YET_RELEASED = 3,
            CANCELLED = 4,
            HIATUS = 5
        }
    }
}
