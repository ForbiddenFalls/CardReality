using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using CardReality.Data;
using CardReality.Data.Data;

namespace CardReality.Services
{
    public abstract class Service
    {
        public IApplicationData Data { get; private set; }

        public Service(IApplicationData data)
        {
            this.Data = data;
        }
    }
}