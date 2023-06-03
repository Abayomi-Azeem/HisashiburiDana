using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HisashiburiDana.Contract.AnimeManager
{
    public class AnimeList
    {
        public Page Page { get; set; }
    }

    public class Page
    {
        public PageInfo PageInfo { get; set; }
        public List<Medium> Media { get; set; }
    }

    public class PageInfo
    {
        public int? Total { get; set; }
        public int? CurrentPage { get; set; }
        public int? LastPage { get; set; }
        public bool? HasNextPage { get; set; }
        public int? PerPage { get; set; }
    }


    public class Medium
    {
        public int? Id { get; set; }
        public Title? Title { get; set; }
        public string? Description { get; set; }
        public StartDate StartDate { get; set; }
        public EndDate EndDate { get; set; }
        public string? Status { get; set; }
        public string? SiteUrl { get; set; }
        public CoverImage CoverImage { get; set; }
        public int? Episodes { get; set; }
        public List<string> Genres { get; set; }
        //public List<StreamingEpisode> StreamingEpisodes { get; set; }
        public List<Ranking> Rankings { get; set; }
    }


    public class CoverImage
    {
        public string? Medium { get; set; }
    }
        
    public class EndDate
    {
        public int? Day { get; set; }
        public int? Month { get; set; }
        public int? Year { get; set; }
    }     
   

    public class StartDate
    {
        public int? Day { get; set; }
        public int? Month { get; set; }
        public int? Year { get; set; }
    }

    public class Title
    {
        public string? English { get; set; }
        public string? Romaji { get; set; }
    }

    public class StreamingEpisode
    {
        public string? Site { get; set; }

        public string? Url { get; set; }
    }

    public class Ranking
    {
        public int? Rank { get; set; }

        public bool? AllTime { get; set; }
    }

}
