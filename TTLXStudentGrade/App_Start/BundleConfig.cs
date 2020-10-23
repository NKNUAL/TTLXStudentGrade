using System.Web;
using System.Web.Optimization;

namespace TTLXStudentGrade
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/easyui/jquery.min.js"));

            // 添加符号库font-awesome.css easyui
            bundles.Add(new StyleBundle("~/Content1/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/style.css",
                      "~/Content/font-awesome.css"));
            //,
            //"~/Content/font-awesome.css"

            //内容视图所用样式
            bundles.Add(new StyleBundle("~/Content2/css").Include(
                      "~/Scripts/easyui/themes/metro/easyui.css",
                      "~/Scripts/easyui/themes/icon.css"));

            // easyui.js
            bundles.Add(new ScriptBundle("~/bundles/easyui").Include(
                      "~/Scripts/easyui/jquery.easyui.min.js"));

            // vue.js
            bundles.Add(new ScriptBundle("~/bundles/vue").Include(
                      "~/Scripts/vue/vue.min.js"));

            // 布局Bundle
            bundles.Add(new ScriptBundle("~/bundles/Layout").Include(
                      "~/Scripts/js/VueApp.js"));

            bundles.Add(new ScriptBundle("~/bundles/grade1").Include(
                      "~/Scripts/js/Grade.js"));

            bundles.Add(new ScriptBundle("~/bundles/month1").Include(
                      "~/Scripts/js/Month.js"));

            bundles.Add(new ScriptBundle("~/bundles/main1").Include(
                      "~/Scripts/js/Main.js"));

            bundles.Add(new ScriptBundle("~/bundles/user1").Include(
                      "~/Scripts/js/User.js"));
            bundles.Add(new ScriptBundle("~/bundles/student").Include(
                      "~/Scripts/js/Student.js"));
            bundles.Add(new ScriptBundle("~/bundles/phoneUser").Include(
                      "~/Scripts/js/PhoneUser.js"));
            bundles.Add(new ScriptBundle("~/bundles/share").Include(
                      "~/Scripts/js/Share.js"));

        }
    }
}
