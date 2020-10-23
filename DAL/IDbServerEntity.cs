using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    public interface IDbServerEntity
    {
        T QuerySingle<T>(Expression<Func<T, bool>> selectWhere) where T : class;

        List<T> QueryBySql<T>(string sql, params object[] parameters);

        DbSet<T> Set<T>() where T : class;

        bool SaveChanges();
    }
}
