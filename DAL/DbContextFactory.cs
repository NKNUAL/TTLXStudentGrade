using IDAL.DataContext;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    public class DbContextFactory
    {


        public static DbUseContext GetDbUserContext()
        {
            DbUseContext dbContext = (DbUseContext)CallContext.GetData("DbUserContext");
            if (dbContext == null)
            {
                dbContext = new DbUseContext();
                CallContext.SetData("DbUserContext", dbContext);
            }
            return dbContext;
        }

        public static DbServerContext GetDbServerContext()
        {
            DbServerContext dbContext = (DbServerContext)CallContext.GetData("DbServerContext");
            if (dbContext == null)
            {
                dbContext = new DbServerContext();
                CallContext.SetData("DbServerContext", dbContext);
            }
            return dbContext;
        }

        public static Db0905Context GetDb0905Context()
        {
            Db0905Context dbContext = (Db0905Context)CallContext.GetData("Db0905Context");
            if (dbContext == null)
            {
                dbContext = new Db0905Context();
                CallContext.SetData("Db0905Context", dbContext);
            }
            return dbContext;
        }

    }
}
