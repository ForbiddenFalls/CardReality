using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CardReality.Models
{
    public class RankingViewModel
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public int Wins { get; set; }

        public int Loss { get; set; }

        public double Ratio { get; set; }

        public int Rank { get; set; }
    }
}