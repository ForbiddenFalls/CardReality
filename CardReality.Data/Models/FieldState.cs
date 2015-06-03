using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardReality.Data.Models
{
    public class FieldState
    {
        public int Id { get; set; }

        public virtual Battle Battle { get; set; }

        public int Row { get; set; }

        public int Col { get; set; }

        public virtual Player Owner { get; set; }

        public virtual Card Card { get; set; }
    }
}
