using Application.Common;
using IDAL.Entity0905;
using IDAL.ServerModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace IDAL.DataContext
{
    public class Db0905Context : DbContext
    {

        public Db0905Context()
            : base()
        {
            Database.Connection.ConnectionString = ConfigTools.GetDBConnString("dbConn0905");
            Database.CommandTimeout = 100;
        }
        public virtual DbSet<ServerMachineRegInfo> ServerMachineRegInfo { get; set; }
        public virtual DbSet<Base_School> Base_School { get; set; }

    }
}