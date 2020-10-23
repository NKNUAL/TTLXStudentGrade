using Aspose.Cells;
using IBLL.ServiceModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace IBLL.Helper
{
    public class GradeHelper
    {

        public bool UserConvertToExcel(List<GradeModel> userGrades, string filepath)
        {
            if (System.IO.File.Exists(filepath))
            {
                System.IO.File.Delete(filepath);
            }
            DataTable dt = null;


            if (userGrades != null && userGrades.Count > 0)
            {
                dt = new DataTable();
                dt.Columns.Add(new DataColumn("乐学号", typeof(string)));
                dt.Columns.Add(new DataColumn("考号", typeof(string)));
                dt.Columns.Add(new DataColumn("姓名", typeof(string)));
                dt.Columns.Add(new DataColumn("班级", typeof(string)));
                dt.Columns.Add(new DataColumn("最高分", typeof(string)));
                dt.Columns.Add(new DataColumn("最低分", typeof(string)));
                dt.Columns.Add(new DataColumn("平均分", typeof(string)));
                dt.Columns.Add(new DataColumn("练习次数", typeof(string)));
                dt.Columns.Add(new DataColumn("最高分排名", typeof(string)));
                int i = 1;
                foreach (var grade in userGrades)
                {
                    DataRow rowData = dt.NewRow();
                    rowData["乐学号"] = grade.Lexueid;
                    rowData["考号"] = grade.Kaohao;
                    rowData["姓名"] = grade.UserName;
                    rowData["班级"] = grade.ClassName;
                    rowData["最高分"] = grade.MaxScore;
                    rowData["最低分"] = grade.MinScore;
                    rowData["平均分"] = grade.AvgScore;
                    rowData["练习次数"] = grade.WorkCount;
                    rowData["最高分排名"] = i++;
                    dt.Rows.Add(rowData);
                }
            }
            return ExportExcelWithAspose(dt, filepath);

        }

        public static bool ExportExcelWithAspose(System.Data.DataTable data, string filepath)
        {
            try
            {
                if (data == null)
                {
                    return false;
                }

                Workbook book = new Workbook(); //创建工作簿
                Worksheet sheet = book.Worksheets[0]; //创建工作表
                Cells cells = sheet.Cells; //单元格
                                           //创建样式
                Aspose.Cells.Style style = book.Styles[book.Styles.Add()];
                style.Borders[Aspose.Cells.BorderType.LeftBorder].LineStyle = Aspose.Cells.CellBorderType.Thin; //应用边界线 左边界线  
                style.Borders[Aspose.Cells.BorderType.RightBorder].LineStyle = Aspose.Cells.CellBorderType.Thin; //应用边界线 右边界线  
                style.Borders[Aspose.Cells.BorderType.TopBorder].LineStyle = Aspose.Cells.CellBorderType.Thin; //应用边界线 上边界线  
                style.Borders[Aspose.Cells.BorderType.BottomBorder].LineStyle = Aspose.Cells.CellBorderType.Thin; //应用边界线 下边界线   
                style.HorizontalAlignment = TextAlignmentType.Center; //单元格内容的水平对齐方式文字居中
                style.Font.Name = "宋体"; //字体
                                        //style1.Font.IsBold = true; //设置粗体
                style.Font.Size = 11; //设置字体大小
                int Colnum = data.Columns.Count;//表格列数 
                int Rownum = data.Rows.Count;//表格行数 
                                             //生成行 列名行 
                for (int i = 0; i < Colnum; i++)
                {
                    cells[0, i].PutValue(data.Columns[i].ColumnName); //添加表头
                    cells[0, i].SetStyle(style); //添加样式
                }
                //生成数据行 
                for (int i = 0; i < Rownum; i++)
                {
                    for (int k = 0; k < Colnum; k++)
                    {
                        cells[1 + i, k].PutValue(data.Rows[i][k].ToString()); //添加数据
                        cells[1 + i, k].SetStyle(style); //添加样式
                    }
                }
                sheet.AutoFitColumns(); //自适应宽
                book.Save(filepath); //保存
                GC.Collect();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}