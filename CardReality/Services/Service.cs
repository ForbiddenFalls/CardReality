using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using CardReality.Data;

namespace CardReality.Services
{
    public abstract class Service
    {
        public ApplicationDbContext Data { get; private set; }

        public Service(ApplicationDbContext context)
        {
            this.Data = context;
        }
    }
}