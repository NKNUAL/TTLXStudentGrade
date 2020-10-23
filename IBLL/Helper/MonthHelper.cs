using Aspose.Cells;
using IBLL.ServiceModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace IBLL.Helper
{
    public class MonthHelper
    {
        #region 单例

        private static readonly object padlock = new object();
        private static MonthHelper _helper;

        public static MonthHelper Instance
        {
            get
            {
                if (_helper == null)
                {
                    lock (padlock)
                    {
                        if (_helper == null)
                        {
                            _helper = new MonthHelper();
                        }
                    }
                }
                return _helper;
            }
        }

        private MonthHelper()
        {
            string jsonFile = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/json/specialtyPaper.json");
            StreamReader file = new StreamReader(jsonFile);
            JsonTextReader reader = new JsonTextReader(file);
            var obj = JToken.ReadFrom(reader);
            _DicSpecialtyScoreLine = JsonConvert.DeserializeObject<Dictionary<string, ScoreLine>>(obj.ToString());
            file.Close();
            reader.Close();

            jsonFile = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/json/specialtyMerge.json");
            StreamReader file1 = new StreamReader(jsonFile, Encoding.Default);
            JsonTextReader reader1 = new JsonTextReader(file1);
            var obj1 = JToken.ReadFrom(reader1);
            DicSpecialtyMerge = JsonConvert.DeserializeObject<Dictionary<int, SpecialtyMerge>>(Regex.Unescape(obj1.ToString()));
            file1.Close();
            reader1.Close();

        }

        #endregion 单例

        public Dictionary<string, ScoreLine> _DicSpecialtyScoreLine;

        private Dictionary<int, List<QuestionDetailModel>> _DicPlanQuestionDetail;

        private Dictionary<int, List<StudentScoreModel>> _DicSpecialtyStudentScore;//专业课成绩

        private Dictionary<int, List<StudentScoreModel>> _DicCultureStudentScore;//文化课成绩

        public Dictionary<int, SpecialtyMerge> DicSpecialtyMerge = new Dictionary<int, SpecialtyMerge>();


        public List<StudentScoreModel> GetSpecialtyStudentScore(int planId)
        {
            if (_DicSpecialtyStudentScore == null)
                _DicSpecialtyStudentScore = new Dictionary<int, List<StudentScoreModel>>();
            if (_DicSpecialtyStudentScore.ContainsKey(planId))
                return _DicSpecialtyStudentScore[planId];
            return null;
        }

        public List<StudentScoreModel> GetCultureStudentScore(int planId)
        {
            if (_DicCultureStudentScore == null)
                _DicCultureStudentScore = new Dictionary<int, List<StudentScoreModel>>();
            if (_DicCultureStudentScore.ContainsKey(planId))
                return _DicCultureStudentScore[planId];
            return null;
        }
        public void SetSpecialtyStudentScore(int planId, List<StudentScoreModel> models)
        {
            if (_DicSpecialtyStudentScore == null)
                _DicSpecialtyStudentScore = new Dictionary<int, List<StudentScoreModel>>();
            if (!_DicSpecialtyStudentScore.ContainsKey(planId))
            {
                _DicSpecialtyStudentScore.Add(planId, models);
            }
        }
        public void SetCultureStudentScore(int planId, List<StudentScoreModel> models)
        {
            if (_DicCultureStudentScore == null)
                _DicCultureStudentScore = new Dictionary<int, List<StudentScoreModel>>();
            if (!_DicCultureStudentScore.ContainsKey(planId))
            {
                _DicCultureStudentScore.Add(planId, models);
            }
        }

        public List<QuestionDetailModel> GetPlanQuestionDetail(int planId)
        {
            if (_DicPlanQuestionDetail == null)
                _DicPlanQuestionDetail = new Dictionary<int, List<QuestionDetailModel>>();
            if (_DicPlanQuestionDetail.ContainsKey(planId))
                return _DicPlanQuestionDetail[planId];
            return null;
        }

        public void SetPlanQuestionDetail(int planId, List<QuestionDetailModel> models)
        {
            if (_DicPlanQuestionDetail == null)
                _DicPlanQuestionDetail = new Dictionary<int, List<QuestionDetailModel>>();
            if (!_DicPlanQuestionDetail.ContainsKey(planId))
            {
                _DicPlanQuestionDetail.Add(planId, models);
            }
        }


        public bool UserConvertToExcel(ImportExcelGradeModel excelModel, string filepath)
        {
            if (System.IO.File.Exists(filepath))
            {
                System.IO.File.Delete(filepath);
            }

            DataTable dt1 = new DataTable();
            dt1.Columns.Add(new DataColumn("考试名称", typeof(string)));
            dt1.Columns.Add(new DataColumn("学校名称", typeof(string)));
            dt1.Columns.Add(new DataColumn("专业名称", typeof(string)));
            DataRow rowData1 = dt1.NewRow();
            rowData1["考试名称"] = excelModel.PlanModel.PlanName;
            rowData1["学校名称"] = excelModel.PlanModel.SchoolName;
            rowData1["专业名称"] = excelModel.PlanModel.SpecialtyName;
            dt1.Rows.Add(rowData1);


            DataTable dt2 = new DataTable();
            if (excelModel.SpecialtySchoolScore != null && excelModel.SpecialtySchoolScore.Count > 0)
            {
                dt2.Columns.Add(new DataColumn("学校", typeof(string)));
                dt2.Columns.Add(new DataColumn("最高分", typeof(string)));
                dt2.Columns.Add(new DataColumn("最低分", typeof(string)));
                dt2.Columns.Add(new DataColumn("平均分", typeof(string)));
                dt2.Columns.Add(new DataColumn("实考人数", typeof(string)));
                dt2.Columns.Add(new DataColumn("考试有效分", typeof(string)));
                foreach (var grade in excelModel.SpecialtySchoolScore)
                {
                    DataRow rowData2 = dt2.NewRow();
                    rowData2["学校"] = grade.RegionName;
                    rowData2["最高分"] = grade.MaxScore;
                    rowData2["最低分"] = grade.MinScore;
                    rowData2["平均分"] = grade.AvgScore;
                    rowData2["实考人数"] = grade.ExamCount;
                    rowData2["考试有效分"] = grade.EffectiveScore;
                    dt2.Rows.Add(rowData2);
                }
            }

            DataTable dt3 = new DataTable();
            if (excelModel.CultureSchoolScore != null && excelModel.CultureSchoolScore.Count > 0)
            {
                dt3.Columns.Add(new DataColumn("学校", typeof(string)));
                dt3.Columns.Add(new DataColumn("最高分", typeof(string)));
                dt3.Columns.Add(new DataColumn("最低分", typeof(string)));
                dt3.Columns.Add(new DataColumn("平均分", typeof(string)));
                dt3.Columns.Add(new DataColumn("实考人数", typeof(string)));
                foreach (var grade in excelModel.CultureSchoolScore)
                {
                    DataRow rowData3 = dt3.NewRow();
                    rowData3["学校"] = grade.RegionName;
                    rowData3["最高分"] = grade.MaxScore;
                    rowData3["最低分"] = grade.MinScore;
                    rowData3["平均分"] = grade.AvgScore;
                    rowData3["实考人数"] = grade.ExamCount;
                    dt3.Rows.Add(rowData3);
                }
            }



            DataTable dt4 = null;
            if (excelModel.StudentModels != null && excelModel.StudentModels.Count > 0)
            {
                dt4 = new DataTable();
                //dt4.Columns.Add(new DataColumn("考号", typeof(string)));
                dt4.Columns.Add(new DataColumn("姓名", typeof(string)));
                dt4.Columns.Add(new DataColumn("专业", typeof(string)));
                dt4.Columns.Add(new DataColumn("作答用时（分钟）", typeof(string)));
                if (excelModel.PlanModel.SpecialtyName.Contains("计算机"))
                {
                    dt4.Columns.Add(new DataColumn("应知题得分", typeof(string)));
                    dt4.Columns.Add(new DataColumn("Windows题得分", typeof(string)));
                    dt4.Columns.Add(new DataColumn("网络题得分", typeof(string)));
                    dt4.Columns.Add(new DataColumn("Word题得分", typeof(string)));
                    dt4.Columns.Add(new DataColumn("Excel题得分", typeof(string)));
                    dt4.Columns.Add(new DataColumn("Ppt题得分", typeof(string)));
                    dt4.Columns.Add(new DataColumn("Access得分", typeof(string)));
                    dt4.Columns.Add(new DataColumn("C语言得分", typeof(string)));
                }
                dt4.Columns.Add(new DataColumn("专业课总得分", typeof(string)));
                dt4.Columns.Add(new DataColumn("语文", typeof(string)));
                dt4.Columns.Add(new DataColumn("数学", typeof(string)));
                dt4.Columns.Add(new DataColumn("英语", typeof(string)));
                dt4.Columns.Add(new DataColumn("文化课总得分", typeof(string)));
                dt4.Columns.Add(new DataColumn("专业+文化课总得分", typeof(string)));


                foreach (var grade in excelModel.StudentModels)
                {
                    DataRow rowData3 = dt4.NewRow();
                    //rowData3["考号"] = grade.Lexueid;
                    rowData3["姓名"] = grade.UserName;
                    rowData3["专业"] = grade.SpecialtyName;
                    rowData3["作答用时（分钟）"] = grade.AnswerTime;
                    if (excelModel.PlanModel.SpecialtyName.Contains("计算机"))
                    {
                        rowData3["应知题得分"] = grade.SelectScore;
                        rowData3["Windows题得分"] = grade.WinScore;
                        rowData3["网络题得分"] = grade.NetScore;
                        rowData3["Word题得分"] = grade.WordScore;
                        rowData3["Excel题得分"] = grade.ExcelScore;
                        rowData3["Ppt题得分"] = grade.PptScore;
                        rowData3["Access得分"] = grade.AccessScore;
                        rowData3["C语言得分"] = grade.ProgramScore;
                    }
                    rowData3["专业课总得分"] = grade.StudentScore;

                    rowData3["语文"] = grade.ChineseScore;
                    rowData3["数学"] = grade.MathScore;
                    rowData3["英语"] = grade.EnglishScore;
                    rowData3["文化课总得分"] = grade.CultureStudentScore;

                    rowData3["专业+文化课总得分"] = grade.StudentTotalScore;

                    dt4.Rows.Add(rowData3);
                }
            }
            return ExportExcelWithAspose(new List<DataTable> { dt1, dt2, dt3, dt4 }, filepath);

        }

        public bool ExportExcelWithAspose(List<DataTable> datas, string filepath)
        {
            try
            {
                if (datas == null)
                {
                    return false;
                }

                Workbook book = new Workbook(); //创建工作簿
                Worksheet sheet = book.Worksheets[0]; //创建工作表
                Cells cells = sheet.Cells; //单元格
                                           //创建样式
                Aspose.Cells.Style style1 = book.Styles[book.Styles.Add()];
                style1.Borders[Aspose.Cells.BorderType.LeftBorder].LineStyle = Aspose.Cells.CellBorderType.Thin; //应用边界线 左边界线  
                style1.Borders[Aspose.Cells.BorderType.RightBorder].LineStyle = Aspose.Cells.CellBorderType.Thin; //应用边界线 右边界线  
                style1.Borders[Aspose.Cells.BorderType.TopBorder].LineStyle = Aspose.Cells.CellBorderType.Thin; //应用边界线 上边界线  
                style1.Borders[Aspose.Cells.BorderType.BottomBorder].LineStyle = Aspose.Cells.CellBorderType.Thin; //应用边界线 下边界线   
                style1.Borders[Aspose.Cells.BorderType.LeftBorder].Color = Color.Red;
                style1.Borders[Aspose.Cells.BorderType.RightBorder].Color = Color.Red;
                style1.Borders[Aspose.Cells.BorderType.TopBorder].Color = Color.Red;
                style1.Borders[Aspose.Cells.BorderType.BottomBorder].Color = Color.Red;
                style1.HorizontalAlignment = TextAlignmentType.Center; //单元格内容的水平对齐方式文字居中
                style1.Font.Name = "宋体"; //字体
                style1.Font.Size = 13; //设置字体大小

                Aspose.Cells.Style style2 = book.Styles[book.Styles.Add()];
                style2.Borders[Aspose.Cells.BorderType.LeftBorder].LineStyle = Aspose.Cells.CellBorderType.Thin; //应用边界线 左边界线  
                style2.Borders[Aspose.Cells.BorderType.RightBorder].LineStyle = Aspose.Cells.CellBorderType.Thin; //应用边界线 右边界线  
                style2.Borders[Aspose.Cells.BorderType.TopBorder].LineStyle = Aspose.Cells.CellBorderType.Thin; //应用边界线 上边界线  
                style2.Borders[Aspose.Cells.BorderType.BottomBorder].LineStyle = Aspose.Cells.CellBorderType.Thin; //应用边界线 下边界线   
                style2.HorizontalAlignment = TextAlignmentType.Center; //单元格内容的水平对齐方式文字居中
                style2.Font.Name = "宋体"; //字体
                style2.Font.Size = 13; //设置字体大小

                Aspose.Cells.Style style4 = book.Styles[book.Styles.Add()];
                style4.Borders[Aspose.Cells.BorderType.LeftBorder].LineStyle = Aspose.Cells.CellBorderType.Thin; //应用边界线 左边界线  
                style4.Borders[Aspose.Cells.BorderType.RightBorder].LineStyle = Aspose.Cells.CellBorderType.Thin; //应用边界线 右边界线  
                style4.Borders[Aspose.Cells.BorderType.TopBorder].LineStyle = Aspose.Cells.CellBorderType.Thin; //应用边界线 上边界线  
                style4.Borders[Aspose.Cells.BorderType.BottomBorder].LineStyle = Aspose.Cells.CellBorderType.Thin; //应用边界线 下边界线   
                style4.Borders[Aspose.Cells.BorderType.LeftBorder].Color = Color.Blue;
                style4.Borders[Aspose.Cells.BorderType.RightBorder].Color = Color.Blue;
                style4.Borders[Aspose.Cells.BorderType.TopBorder].Color = Color.Blue;
                style4.Borders[Aspose.Cells.BorderType.BottomBorder].Color = Color.Blue;
                style4.HorizontalAlignment = TextAlignmentType.Center; //单元格内容的水平对齐方式文字居中
                style4.Font.Name = "宋体"; //字体
                style4.Font.Size = 13; //设置字体大小


                Dictionary<int, Style> dic = new Dictionary<int, Style>
                {
                    { 0, style1 },
                    { 1, style2 },
                     { 2, style2 },
                    { 3, style4 }
                };
                int StartRownum = 0;
                for (int j = 0; j < datas.Count; j++)
                {
                    int Colnum = datas[j].Columns.Count;//表格列数 
                    int Rownum = datas[j].Rows.Count;//表格行数 
                                                     //生成行 列名行 

                    if (j == 1)
                    {
                        //var cellsss = cells["A"];
                        cells.Merge(StartRownum, 0, 1, Colnum);
                        cells[StartRownum, 0].PutValue("技能高考专业课");
                        for (int k = 0; k < Colnum; k++)
                        {
                            cells[StartRownum, k].SetStyle(dic[j]);
                        }
                        //添加样式
                        StartRownum++;
                    }
                    if (j == 2)
                    {
                        cells.Merge(StartRownum, 0, 1, Colnum);
                        cells[StartRownum, 0].PutValue("技能高考文化课");
                        for (int k = 0; k < Colnum; k++)
                        {
                            cells[StartRownum, k].SetStyle(dic[j]);
                        }
                        StartRownum++;
                    }

                    for (int i = 0; i < Colnum; i++)
                    {
                        cells[StartRownum, i].PutValue(datas[j].Columns[i].ColumnName); //添加表头
                        cells[StartRownum, i].SetStyle(dic[j]); //添加样式
                    }
                    //生成数据行 
                    for (int i = 0; i < Rownum; i++)
                    {
                        for (int k = 0; k < Colnum; k++)
                        {
                            cells[StartRownum + 1 + i, k].PutValue(datas[j].Rows[i][k].ToString()); //添加数据
                            cells[StartRownum + 1 + i, k].SetStyle(dic[j]); //添加样式
                        }
                    }
                    StartRownum += Rownum + 5;
                    sheet.AutoFitColumns(); //自适应宽
                    sheet.AutoFitRows();
                }


                book.Save(filepath); //保存
                GC.Collect();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public bool UserConvertToExcel(ImportProvinceDataModel excelModel, string filepath)
        {
            if (System.IO.File.Exists(filepath))
            {
                System.IO.File.Delete(filepath);
            }

            DataTable dt1 = new DataTable();
            dt1.Columns.Add(new DataColumn("省份", typeof(string)));
            dt1.Columns.Add(new DataColumn("考试名称", typeof(string)));
            dt1.Columns.Add(new DataColumn("专业名称", typeof(string)));
            DataRow rowData1 = dt1.NewRow();
            rowData1["省份"] = excelModel.PlanModel.SchoolName;
            rowData1["考试名称"] = excelModel.PlanModel.PlanName;
            rowData1["专业名称"] = excelModel.PlanModel.SpecialtyName;
            dt1.Rows.Add(rowData1);

            DataTable dt2 = new DataTable();
            dt2.Columns.Add(new DataColumn("最高分", typeof(string)));
            dt2.Columns.Add(new DataColumn("最低分", typeof(string)));
            dt2.Columns.Add(new DataColumn("平均分", typeof(string)));
            dt2.Columns.Add(new DataColumn("考试人数", typeof(string)));
            dt2.Columns.Add(new DataColumn("有效分", typeof(string)));
            DataRow rowData2 = dt2.NewRow();
            rowData2["最高分"] = excelModel.SpecialtyProvinceScore.MaxScore;
            rowData2["最低分"] = excelModel.SpecialtyProvinceScore.MinScore;
            rowData2["平均分"] = excelModel.SpecialtyProvinceScore.AvgScore;
            rowData2["考试人数"] = excelModel.SpecialtyProvinceScore.ExamCount;
            rowData2["有效分"] = excelModel.SpecialtyProvinceScore.EffectiveScore;
            dt2.Rows.Add(rowData2);

            DataTable dt3 = new DataTable();
            dt3.Columns.Add(new DataColumn("最高分", typeof(string)));
            dt3.Columns.Add(new DataColumn("最低分", typeof(string)));
            dt3.Columns.Add(new DataColumn("平均分", typeof(string)));
            dt3.Columns.Add(new DataColumn("考试人数", typeof(string)));
            DataRow rowData3 = dt3.NewRow();
            rowData3["最高分"] = excelModel.CultureProvinceScore.MaxScore;
            rowData3["最低分"] = excelModel.CultureProvinceScore.MinScore;
            rowData3["平均分"] = excelModel.CultureProvinceScore.AvgScore;
            rowData3["考试人数"] = excelModel.CultureProvinceScore.ExamCount;
            dt3.Rows.Add(rowData3);


            DataTable dt4 = null;
            if (excelModel.StudentModels != null && excelModel.StudentModels.Count > 0)
            {
                dt4 = new DataTable();
                //dt3.Columns.Add(new DataColumn("考号", typeof(string)));
                dt4.Columns.Add(new DataColumn("姓名", typeof(string)));
                dt4.Columns.Add(new DataColumn("学校", typeof(string)));
                dt4.Columns.Add(new DataColumn("专业", typeof(string)));
                dt4.Columns.Add(new DataColumn("专业课总分", typeof(string)));
                dt4.Columns.Add(new DataColumn("文化课总分", typeof(string)));
                dt4.Columns.Add(new DataColumn("专业+文化课总分", typeof(string)));
                //dt3.Columns.Add(new DataColumn("学校排名", typeof(string)));
                //dt3.Columns.Add(new DataColumn("全省排名", typeof(string)));

                //if (excelModel.PlanModel.SpecialtyName.Contains("计算机"))
                //{
                //    dt3.Columns.Add(new DataColumn("应知题得分", typeof(string)));
                //    dt3.Columns.Add(new DataColumn("Windows题得分", typeof(string)));
                //    dt3.Columns.Add(new DataColumn("网络题得分", typeof(string)));
                //    dt3.Columns.Add(new DataColumn("Word题得分", typeof(string)));
                //    dt3.Columns.Add(new DataColumn("Excel题得分", typeof(string)));
                //    dt3.Columns.Add(new DataColumn("Ppt题得分", typeof(string)));
                //    dt3.Columns.Add(new DataColumn("Access得分", typeof(string)));
                //    dt3.Columns.Add(new DataColumn("C语言得分", typeof(string)));
                //}
                //dt4.Columns.Add(new DataColumn("总得分", typeof(string)));

                foreach (var grade in excelModel.StudentModels)
                {
                    DataRow rowData4 = dt4.NewRow();
                    //rowData3["考号"] = grade.Lexueid;
                    rowData4["姓名"] = grade.UserName;
                    rowData4["学校"] = grade.SchoolName;
                    rowData4["专业"] = grade.SpecialtyName;
                    rowData4["专业课总分"] = grade.StudentScore;
                    rowData4["文化课总分"] = grade.CultureStudentScore;
                    rowData4["专业+文化课总分"] = grade.StudentTotalScore;
                    //rowData3["学校排名"] = grade.SchoolRank;
                    //rowData3["全省排名"] = grade.ProvinceRank;

                    //if (excelModel.PlanModel.SpecialtyName.Contains("计算机"))
                    //{
                    //    rowData3["应知题得分"] = grade.SelectScore;
                    //    rowData3["Windows题得分"] = grade.WinScore;
                    //    rowData3["网络题得分"] = grade.NetScore;
                    //    rowData3["Word题得分"] = grade.WordScore;
                    //    rowData3["Excel题得分"] = grade.ExcelScore;
                    //    rowData3["Ppt题得分"] = grade.PptScore;
                    //    rowData3["Access得分"] = grade.AccessScore;
                    //    rowData3["C语言得分"] = grade.ProgramScore;
                    //}
                    //rowData3["总得分"] = grade.StudentScore;
                    dt4.Rows.Add(rowData4);
                }
            }

            return ExportExcelWithAspose(new List<DataTable> { dt1, dt2, dt3, dt4 }, filepath);
        }
    }

    public class ScoreLine
    {
        public double PassLine { get; set; }
        public double A { get; set; }//优
        public double B { get; set; }//良
        public double C { get; set; }//中
        public double D { get; set; }//差
        public Dictionary<string, int> EffectivePeopleNum { get; set; }
    }

    public class SpecialtyMerge
    {
        public List<int> planIds { get; set; }

        public string specialtyName { get; set; }

        public string planName { get; set; }
    }
}