﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ContosoUniversity.Controllers
{
    public interface IDataStore : DbContext
    {

    }
}