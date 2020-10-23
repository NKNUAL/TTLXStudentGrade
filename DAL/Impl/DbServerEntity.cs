using Application.Common;
using Application.Logger;
using IDAL.DataContext;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IDAL.Impl
{
    public class DbServerEntity : IDbServerEntity
    {
        public DbServerContext _db = DbContextFactory.GetDbServerContext();

        public bool SaveChanges()
        {
            return _db.SaveChanges() > 0;
        }

        //public DbServerContext _db = new DbServerContext();

        public List<T> QueryBySql<T>(string sql, params object[] parameters)
        {
            try
            {
                return _db.Database.SqlQuery<T>(sql, parameters).ToList();
            }
            catch (Exception ex)
            {
                LogContent.Instance.WriteLog(new AppOpLog()
                {
                    LogMessage = ex.Message,
                    MemberID = CookieHelper.GetUserId(),
                    MethodName = $"[{MethodBase.GetCurrentMethod().DeclaringType.Name}:{MethodBase.GetCurrentMethod().Name}]"
                }, Log4NetLevel.Error);
                return null;
            }
        }


        public T QuerySingle<T>(Expression<Func<T, bool>> selectWhere) where T : class
        {
            try
            {
                return _db.Set<T>().Where(selectWhere).FirstOrDefault();
            }
            catch (Exception ex)
            {
                LogContent.Instance.WriteLog(new AppOpLog()
                {
                    LogMessage = ex.Message,
                    MemberID = CookieHelper.GetUserId(),
                    MethodName = $"[{MethodBase.GetCurrentMethod().DeclaringType.Name}:{MethodBase.GetCurrentMethod().Name}]"
                }, Log4NetLevel.Error);
                return default(T);
            }
        }

        public DbSet<T> Set<T>() where T : class
        {
            return _db.Set<T>();
        }
    }
}
