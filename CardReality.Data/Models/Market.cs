using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardReality.Data.Models
{
    public class Market
    {
        private uint price;

        public int Id { get; set; }

        public string CardName { get; set; }

        public int Price {
            get
            {
                checked
                {
                    return int.Parse(Convert.ToUInt32(this.price).ToString());
                }
            }
            set
            {
                checked
                {
                    this.price = (uint)value;
                } 
            } 
        }

        public virtual Player Owner { get; set; }

        public DateTime SoldOn { get; set; }
    }
}
