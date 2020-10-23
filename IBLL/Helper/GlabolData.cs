using Application;
using Application.Logger;
using IBLL.ServiceModels;
using IDAL.DataContext;
using IDAL.DbModel;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IBLL.Helper
{
    public class GlabolData
    {
        public Dictionary<string, string> _dicSpecialty = new Dictionary<string, string>();
        public Dictionary<string, string> _dicSchool = new Dictionary<string, string>();
        private static readonly object objlock = new object();

        #region single
        private static GlabolData _instance = null;
        private GlabolData()
        {
            using (DbUseContext db = new DbUseContext())
            {
                _dicSpecialty = db.Base_specialtyType.ToDictionary(k => k.No, v => v.Name);
                _dicSchool = db.Base_School.ToDictionary(k => k.SchoolNo, v => v.SchoolName);
            }
        }
        public static GlabolData Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (objlock)
                    {
                        if (_instance == null)
                        {
                            try
                            {
                                _instance = new GlabolData();
                            }
                            catch { }
                        }
                    }
                }
                return _instance;
            }
        }
        #endregion


        public Dictionary<QueueDataType, IQueueData> Datas = new Dictionary<QueueDataType, IQueueData>();

        public void Process()
        {
            foreach (var data in Datas)
            {
                data.Value.ToDb();
            }
        }


    }
}
