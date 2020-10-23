using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    public interface IDbUserEntity
    {
        bool Add<T>(T entity) where T : class;
        bool Add<T>(List<T> entities) where T : class;

        bool Delete<T>(T entity) where T : class;
        bool Delete<T>(List<T> entities) where T : class;
        bool Delete<T>(Expression<Func<T, bool>> deleteWhere) where T : class;

        T QuerySingle<T>(Expression<Func<T, bool>> selectWhere) where T : class;
        List<T> Query<T>(Expression<Func<T, bool>> selectWhere) where T : class;

        List<T> QueryBySql<T>(string sql, params object[] parameters);

        int GetCount<T>(Expression<Func<T, bool>> selectWhere) where T : class;

        DbSet<T> Set<T>() where T : class;

        bool SaveChanges();
    }
}
