using Application.Common;
using Application.Enum;
using IBLL.ServiceModels;
using IDAL;
using IDAL.DbModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBLL.Impl
{
    public class HomeService : IHomeService
    {
        public IDbServerEntity _db { get; set; }

        public HomeService() { }

        public HomeService(IDbServerEntity db)
        {
            this._db = db;
        }



        public List<Module> GetMenuModule()
        {
            UserData user = CookieHelper.GetUserData();


            List<SysMenu> menus = _db.QueryBySql<SysMenu>("SELECT * FROM SysMenu WHERE MID in (SELECT MID FROM SysRoleMenuRelation WHERE ROID = @Role)", new SqlParameter("@Role", (int)user.UserRole)).ToList();

            int max_mid = menus.Count > 0 ? menus.Max(m => m.MID) : 0;



            if (user.UserRole == UserRole.SchoolAdmin)
            {
                SysMenu menu_new = new SysMenu
                {
                    MID = max_mid++,
                    MIMG = "../Content/Images/成绩分析.png",
                    MNAME = "模拟试卷成绩统计",
                    MPID = -1
                };
                menus.Add(menu_new);
                string schoolId = user.SchoolCode;
                var result = _db.Set<IDAL.ServerModel.UserTable>().Where(u => u.FK_SchoolID == schoolId && u.UserType == 1)
                    .GroupBy(u => new { u.FK_Specialty, u.FK_SpecialtyName })
                    .Select(u => new { SpecialtyId = u.Key.FK_Specialty, SpecialtyName = u.Key.FK_SpecialtyName })
                    .ToList();
                foreach (var item in result)
                {
                    menus.Add(new SysMenu
                    {
                        MID = max_mid++,
                        MIMG = "../Content/Images/成绩分析.png",
                        MNAME = $"{item.SpecialtyName}成绩明细",
                        MURL = $"/Grade/Index?specialtyId={item.SpecialtyId}",
                        MPID = menu_new.MID
                    });
                }
            }

            if (menus != null && menus.Count > 0)
            {
                List<Module> modules = new List<Module>();
                var parents = from SysMenu menu in menus where menu.MPID == -1 select menu;
                foreach (var p in parents)
                {
                    Module pm = new Module
                    {
                        key = p.MID,
                        name = p.MNAME,
                        mimg = p.MIMG,
                        childs = new List<ChildModule>()
                    };
                    var childs = from SysMenu childmenu in menus where childmenu.MPID == p.MID select childmenu;
                    foreach (var c in childs)
                    {
                        ChildModule cm = new ChildModule
                        {
                            key = c.MID,
                            name = c.MNAME,
                            url = c.MURL,
                            mimg = c.MIMG
                        };
                        pm.childs.Add(cm);
                    }
                    modules.Add(pm);
                }
                return modules;
            }
            return null;
        }
    }
}
