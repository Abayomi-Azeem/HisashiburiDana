using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HisashiburiDana.Contract.Users
{
    public class UserAnime
    {
        public string Id { get; set; }

        public string UserId { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string? Status { get; set; }

        public string? SiteUrl { get; set; }

        public string? CoverUrl { get; set; }

        public int? Episodes { get; set; }

        public string? Genres { get; set; }

        public string? RankingId { get; set; }

        public DateTime? DateAdded { get; set; }

        public DateTime? DateFininshedWatching { get; set; }

        public DateTime? DateStartedWatching { get; set; }
    }
}
