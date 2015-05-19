using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardReality.Data.Models
{
    public class Letter
    {
        public int Id { get; set; }

        public string Char { get; set; }

        public double Weight { get; set; }
    }
}
