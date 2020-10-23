using Application.Logger;
using Aspose.Cells;
using IBLL.ServiceModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IBLL.Helper
{
    public class ExcelHelper
    {
        #region 单例
        private static readonly object padlock = new object();
        private static ExcelHelper _helper;

        public static ExcelHelper Instance
        {
            get
            {
                if (_helper == null)
                {
                    lock (padlock)
                    {
                        if (_helper == null)
                        {
                            _helper = new ExcelHelper();
                        }
                    }
                }
                return _helper;
            }
        }
        #endregion 


        public bool StudentConvertToFile(Dictionary<string, List<StudentMsgModel>> data, string filepath)
        {
            if (data == null || data.Count == 0)
                return false;

            DataSet ds = new DataSet();

            string idcard = "身份证";
            string lexueid = "乐学号";
            string kaohao = "考号";
            string name = "姓名";
            string pwd = "密码";
            string specialty = "专业";

            foreach (var vk in data)
            {
                DataTable dt = new DataTable
                {
                    TableName = vk.Key
                };
                dt.Columns.Add(new DataColumn(idcard, typeof(string)));
                dt.Columns.Add(new DataColumn(lexueid, typeof(string)));
                dt.Columns.Add(new DataColumn(kaohao, typeof(string)));
                dt.Columns.Add(new DataColumn(name, typeof(string)));
                dt.Columns.Add(new DataColumn(pwd, typeof(string)));
                dt.Columns.Add(new DataColumn(specialty, typeof(string)));
                foreach (var student in vk.Value)
                {
                    DataRow row = dt.NewRow();
                    row[idcard] = student.IDCard;
                    row[lexueid] = student.Lexueid;
                    row[kaohao] = student.Kaohao;
                    row[name] = student.UserName;
                    row[pwd] = student.Pwd;
                    row[specialty] = student.SpecialtyName;
                    dt.Rows.Add(row);
                }
                ds.Tables.Add(dt);
            }
            return ExportExcelWithAspose(ds, filepath);
        }

        public bool ExportExcelWithAspose(DataSet ds, string filepath)
        {
            try
            {
                if (ds == null || ds.Tables.Count == 0)
                {
                    return false;
                }

                Workbook book = new Workbook(); //创建工作簿


                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    book.Worksheets.Add(SheetType.Worksheet);
                    DataTable dt = ds.Tables[i];
                    Worksheet sheet = book.Worksheets[i]; //创建工作表
                    sheet.Name = dt.TableName;
                    Cells cells = sheet.Cells; //单元格
                    //创建样式
                    Style style = book.Styles[book.Styles.Add()];
                    style.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin; //应用边界线 左边界线  
                    style.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin; //应用边界线 右边界线  
                    style.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin; //应用边界线 上边界线  
                    style.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin; //应用边界线 下边界线   
                    style.HorizontalAlignment = TextAlignmentType.Center; //单元格内容的水平对齐方式文字居中
                    style.Font.Name = "宋体"; //字体
                    style.Font.Size = 13; //设置字体大小



                    int Colnum = dt.Columns.Count;//表格列数 
                    int Rownum = dt.Rows.Count;//表格行数 
                                               //生成行 列名行 
                    for (int k = 0; k < Colnum; k++)
                    {
                        cells[0, k].PutValue(dt.Columns[k].ColumnName); //添加表头
                        cells[0, k].SetStyle(style); //添加样式
                    }
                    //生成数据行 
                    for (int m = 0; m < Rownum; m++)
                    {
                        for (int n = 0; n < Colnum; n++)
                        {
                            cells[m + 1, n].PutValue(dt.Rows[m][n].ToString()); //添加数据
                            cells[m + 1, n].SetStyle(style); //添加样式
                        }
                    }
                    sheet.AutoFitColumns(); //自适应宽
                    sheet.AutoFitRows();

                }
                book.Save(filepath); //保存
                GC.Collect();
            }
            catch (Exception ex)
            {
                LogWriter.Instance.AddLog("导出excel:" + ex.Message);
                return false;
            }

            return true;
        }

        public bool DataCompareConvertToFile(List<SchoolCompareModel> data, string filepath)
        {
            if (data == null || data.Count == 0)
            {
                LogWriter.Instance.AddLog("导出excel:数据为空");
                return false;
            }


            DataSet ds = new DataSet();

            string schoolName = "学校";
            string number = "参考人数";
            string passCount = "及格数";
            string pass = "及格率";
            string maxScore = "最高分";
            string minSCore = "最低分";
            string avgScore = "平均分";
            string A = "优";
            string B = "良";
            string C = "中";
            string D = "差";
            string stand = "标准差";
            DataTable dt = new DataTable
            {
                TableName = "学校成绩比较"
            };
            dt.Columns.Add(new DataColumn(schoolName, typeof(string)));
            dt.Columns.Add(new DataColumn(number, typeof(string)));
            dt.Columns.Add(new DataColumn(passCount, typeof(string)));
            dt.Columns.Add(new DataColumn(pass, typeof(string)));
            dt.Columns.Add(new DataColumn(maxScore, typeof(string)));
            dt.Columns.Add(new DataColumn(minSCore, typeof(string)));
            dt.Columns.Add(new DataColumn(avgScore, typeof(string)));
            dt.Columns.Add(new DataColumn(A, typeof(string)));
            dt.Columns.Add(new DataColumn(B, typeof(string)));
            dt.Columns.Add(new DataColumn(C, typeof(string)));
            dt.Columns.Add(new DataColumn(D, typeof(string)));
            dt.Columns.Add(new DataColumn(stand, typeof(string)));
            foreach (var vk in data)
            {
                DataRow row = dt.NewRow();
                row[schoolName] = vk.SchoolName;
                row[number] = vk.JoinExamCount;
                row[passCount] = vk.PassCount;
                row[pass] = vk.PassingRate;
                row[maxScore] = vk.MaxScore;
                row[minSCore] = vk.MinScore;
                row[avgScore] = vk.AvgScore;
                row[A] = vk.A;
                row[B] = vk.B;
                row[C] = vk.C;
                row[D] = vk.D;
                row[stand] = vk.StandardDeviation;
                dt.Rows.Add(row);
            }
            ds.Tables.Add(dt);
            return ExportExcelWithAspose(ds, filepath);
        }
    }
}
