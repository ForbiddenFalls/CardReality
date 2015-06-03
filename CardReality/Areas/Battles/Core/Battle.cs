using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CardReality.Data.Data;
using CardReality.Data.Models;

namespace CardReality.Areas.Battles.Core
{
    public class Battle
    {
        private IApplicationData data;
        private Data.Models.Battle model;

        public Battle(IApplicationData data, Data.Models.Battle battleModel)
        {
            this.data = data;
            this.model = battleModel;
        }


    }
}