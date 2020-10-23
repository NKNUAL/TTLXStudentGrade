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
    public class DbUserEntity : IDbUserEntity
    {
        public DbUseContext _dbUser = DbContextFactory.GetDbUserContext();
        //public DbUseContext _dbUser = new DbUseContext();
        public bool Add<T>(T entity) where T : class
        {
            try
            {
                _dbUser.Entry(entity).State = EntityState.Added;
                return _dbUser.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                LogContent.Instance.WriteLog(new AppOpLog()
                {
                    LogMessage = ex.Message,
                    MemberID = CookieHelper.GetUserId(),
                    MethodName = $"[{MethodBase.GetCurrentMethod().DeclaringType.Name}:{MethodBase.GetCurrentMethod().Name}]"
                }, Log4NetLevel.Error);
                return false;
            }
        }

        public bool Add<T>(List<T> entities) where T : class
        {
            try
            {
                _dbUser.Set<T>().AddRange(entities);
                return _dbUser.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                LogContent.Instance.WriteLog(new AppOpLog()
                {
                    LogMessage = ex.Message,
                    MemberID = CookieHelper.GetUserId(),
                    MethodName = $"[{MethodBase.GetCurrentMethod().DeclaringType.Name}:{MethodBase.GetCurrentMethod().Name}]"
                }, Log4NetLevel.Error);
                return false;
            }
        }

        public bool Delete<T>(T entity) where T : class
        {
            try
            {
                _dbUser.Set<T>().Remove(entity);
                return _dbUser.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                LogContent.Instance.WriteLog(new AppOpLog()
                {
                    LogMessage = ex.Message,
                    MemberID = CookieHelper.GetUserId(),
                    MethodName = $"[{MethodBase.GetCurrentMethod().DeclaringType.Name}:{MethodBase.GetCurrentMethod().Name}]"
                }, Log4NetLevel.Error);
                return false;
            }
        }

        public bool Delete<T>(List<T> entities) where T : class
        {
            try
            {
                _dbUser.Set<T>().RemoveRange(entities);
                return _dbUser.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                LogContent.Instance.WriteLog(new AppOpLog()
                {
                    LogMessage = ex.Message,
                    MemberID = CookieHelper.GetUserId(),
                    MethodName = $"[{MethodBase.GetCurrentMethod().DeclaringType.Name}:{MethodBase.GetCurrentMethod().Name}]"
                }, Log4NetLevel.Error);
                return false;
            }
        }

        public bool Delete<T>(Expression<Func<T, bool>> deleteWhere) where T : class
        {
            try
            {
                List<T> entitys = _dbUser.Set<T>().Where(deleteWhere).ToList();
                return Delete<T>(entitys);
            }
            catch (Exception ex)
            {
                LogContent.Instance.WriteLog(new AppOpLog()
                {
                    LogMessage = ex.Message,
                    MemberID = CookieHelper.GetUserId(),
                    MethodName = $"[{MethodBase.GetCurrentMethod().DeclaringType.Name}:{MethodBase.GetCurrentMethod().Name}]"
                }, Log4NetLevel.Error);
                return false;
            }
        }

        public int GetCount<T>(Expression<Func<T, bool>> selectWhere) where T : class
        {
            try
            {
                return _dbUser.Set<T>().Count(selectWhere);
            }
            catch (Exception ex)
            {
                LogContent.Instance.WriteLog(new AppOpLog()
                {
                    LogMessage = ex.Message,
                    MemberID = CookieHelper.GetUserId(),
                    MethodName = $"[{MethodBase.GetCurrentMethod().DeclaringType.Name}:{MethodBase.GetCurrentMethod().Name}]"
                }, Log4NetLevel.Error);
                return 0;
            }
        }

        public List<T> Query<T>(Expression<Func<T, bool>> selectWhere) where T : class
        {
            try
            {
                return _dbUser.Set<T>().Where(selectWhere).ToList();
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

        public List<T> QueryBySql<T>(string sql, params object[] parameters)
        {
            try
            {
                return _dbUser.Database.SqlQuery<T>(sql, parameters).ToList();
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

                var ss = _dbUser.Set<T>().Where(selectWhere);
                var ss2 = _dbUser.LexueidRelationIDCard.Where(l => l.idcard == "421125200302040613");
                ss2.FirstOrDefault();
                return ss.FirstOrDefault();
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
            return _dbUser.Set<T>();
        }

        public bool SaveChanges()
        {
            return _dbUser.SaveChanges() > 0;
        }
    }
}
