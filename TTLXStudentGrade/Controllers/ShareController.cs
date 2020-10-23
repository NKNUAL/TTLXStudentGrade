using Application;
using Application.Common;
using IBLL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TTLXStudentGrade.Models;

namespace TTLXStudentGrade.Controllers
{
    [SysAuth(Roles = "0")]
    public class ShareController : Controller
    {
        static readonly string _baseUrl = ConfigurationManager.AppSettings["share_api_url"];
        WebHeaderCollection _header = new WebHeaderCollection
        {
            { "x-ttlx-token", "ttlx@pd" }
        };
        public IBaseService _baseService { get; set; }
        public ShareController(IBaseService baseService)
        {
            _baseService = baseService;
        }
        // GET: Share
        public ActionResult Index()
        {
            return View();
        }


        public JsonResult GetSpecialty()
        {
            var specialties = _baseService.GetSpecialty();
            specialties.Insert(0, new IBLL.ServiceModels.SpecialtyKVModel
            {
                SpecialtyId = "-1",
                SpecialtyName = "全部"
            });
            return Json(specialties, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetUsers(string specialtyId)
        {
            if (specialtyId == "-1")
                specialtyId = null;
            HttpItem item = new HttpItem
            {
                URL = _baseUrl + $"/api/share2/users?specialtyId=" + specialtyId,
                Method = "GET",
                ResultType = ResultType.String,
                PostDataType = PostDataType.String,
                Header = _header
            };
            var result = new HttpHelper().GetData(item);
            HttpResultModel httpResult = new HttpResultModel();
            if (result.StatusCode != HttpStatusCode.OK)
            {
                httpResult = new HttpResultModel { success = false, message = "获取失败" };
            }
            else
            {
                httpResult = JsonConvert.DeserializeObject<HttpResultModel>(result.Data);
            }

            if (httpResult.success)
            {
                List<BindUserModel> users = JsonConvert.DeserializeObject<List<BindUserModel>>(httpResult.data.ToString());

                List<dynamic> listUsers = new List<dynamic>();

                foreach (var user in users)
                {
                    listUsers.Add(new
                    {
                        TeacherUserToken = user.UserToken,
                        TeacherUserName = user.UserName + $"({user.SchoolName})"
                    });
                }
                return Json(new { success = true, data = listUsers }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false, message = httpResult.message }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPapers(int? page, int? rows, PaperQueryModel query)
        {
            if (query.CheckStatu == -1)
                query.CheckStatu = null;
            if (query.PaperStatu == -1)
                query.PaperStatu = null;
            if (query.SpecialtyId == "-1")
                query.SpecialtyId = null;
            query.OrderType = 3;

            HttpItem item = new HttpItem
            {
                URL = _baseUrl + $"/api/share2/paper/get/{page ?? 1}/{rows ?? 10}",
                Postdata = Newtonsoft.Json.JsonConvert.SerializeObject(query),
                ContentType = "application/json",
                Method = "POST",
                ResultType = ResultType.String,
                PostDataType = PostDataType.String,
                Header = _header
            };
            var result = new HttpHelper().GetData(item);
            HttpResultModel httpResult = new HttpResultModel();
            if (result.StatusCode != HttpStatusCode.OK)
            {
                httpResult = new HttpResultModel { success = false, message = "获取失败" };
            }
            else
            {
                httpResult = JsonConvert.DeserializeObject<HttpResultModel>(result.Data);
            }

            if (httpResult.success)
            {
                SharePaperTotalModel data = JsonConvert.DeserializeObject<SharePaperTotalModel>(httpResult.data.ToString());
                return Json(new { total = data.TotalCount, rows = data.PaperData }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false, httpResult.message }, JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult GetQuestions(string paperId)
        {
            HttpItem item = new HttpItem
            {
                URL = _baseUrl + $"/api/share2/paper/questions/{paperId}",
                Method = "GET",
                ResultType = ResultType.String,
                PostDataType = PostDataType.String,
                Header = _header
            };
            var result = new HttpHelper().GetData(item);
            HttpResultModel httpResult = new HttpResultModel();
            if (result.StatusCode != HttpStatusCode.OK)
            {
                httpResult = new HttpResultModel { success = false, message = "获取失败" };
            }
            else
            {
                httpResult = JsonConvert.DeserializeObject<HttpResultModel>(result.Data);
            }

            if (httpResult.success)
            {
                List<ReviewQuestionModel> ques =
                    JsonConvert.DeserializeObject<List<ReviewQuestionModel>>(httpResult.data.ToString());

                List<ReviewQuestionViewModel> viewQues = new List<ReviewQuestionViewModel>();

                foreach (var que in ques)
                {
                    viewQues.Add(new ReviewQuestionViewModel
                    {
                        Similarity = que.Similarity,
                        StandardAnwser = que.StandardAnwser,
                        ContentImg = QuestionsHelper.Instance.SaveQueImage(paperId, que.QueNo, que.ContentImg, ImageSource.QueContent),
                        DifficultLevel = que.DifficultLevel,
                        Option0 = que.Option0,
                        Option0Img = QuestionsHelper.Instance.SaveQueImage(paperId, que.QueNo, que.Option0Img, ImageSource.OptionA),
                        Option1 = que.Option1,
                        Option1Img = QuestionsHelper.Instance.SaveQueImage(paperId, que.QueNo, que.Option1Img, ImageSource.OptionB),
                        Option2 = que.Option2,
                        Option2Img = QuestionsHelper.Instance.SaveQueImage(paperId, que.QueNo, que.Option2Img, ImageSource.OptionC),
                        Option3 = que.Option3,
                        Option3Img = QuestionsHelper.Instance.SaveQueImage(paperId, que.QueNo, que.Option3Img, ImageSource.OptionD),
                        QueContent = que.QueContent,
                        QueNo = que.QueNo,
                        QueType = que.QueType,
                        ResolutionTips = que.ResolutionTips
                    });
                }

                return Json(new { success = true, data = viewQues }, JsonRequestBehavior.AllowGet);

            }

            return Json(new { success = false, message = httpResult.message }, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetSimilarQuestions(string queNo)
        {
            HttpItem item = new HttpItem
            {
                URL = _baseUrl + $"/api/share2/paper/similar_questions/{queNo}",
                Method = "GET",
                ResultType = ResultType.String,
                PostDataType = PostDataType.String,
                Header = _header
            };
            var result = new HttpHelper().GetData(item);
            HttpResultModel httpResult = new HttpResultModel();
            if (result.StatusCode != HttpStatusCode.OK)
            {
                httpResult = new HttpResultModel { success = false, message = "获取失败" };
            }
            else
            {
                httpResult = JsonConvert.DeserializeObject<HttpResultModel>(result.Data);
            }

            if (httpResult.success)
            {
                List<SimilarityQuestionsModel> ques =
                    JsonConvert.DeserializeObject<List<SimilarityQuestionsModel>>(httpResult.data.ToString());


                return Json(new { success = true, data = ques }, JsonRequestBehavior.AllowGet);

            }

            return Json(new { success = false, httpResult.message }, JsonRequestBehavior.AllowGet);
        }


        public JsonResult CheckPass(int commentLevel, string commentDesc, string paperId)
        {
            CheckPassModel check = new CheckPassModel
            {
                CheckUserId = CookieHelper.GetUserId(),
                CommentLevel = commentLevel,
                PaperID = paperId,
                Reason = commentDesc
            };

            HttpItem item = new HttpItem
            {
                URL = _baseUrl + $"/api/share2/paper/check/pass",
                Postdata = JsonConvert.SerializeObject(check),
                ContentType = "application/json",
                Method = "POST",
                ResultType = ResultType.String,
                PostDataType = PostDataType.String,
                Header = _header
            };
            var result = new HttpHelper().GetData(item);
            HttpResultModel httpResult = new HttpResultModel();
            if (result.StatusCode != HttpStatusCode.OK)
            {
                httpResult = new HttpResultModel { success = false, message = "获取失败" };
            }
            else
            {
                httpResult = JsonConvert.DeserializeObject<HttpResultModel>(result.Data);
            }

            if (httpResult.success)
            {
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false, httpResult.message }, JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult CheckRefuse(string reason, string paperId)
        {
            CheckRefouseModel check = new CheckRefouseModel
            {
                CheckUserId = CookieHelper.GetUserId(),
                PaperID = paperId,
                Reason = reason,
            };

            HttpItem item = new HttpItem
            {
                URL = _baseUrl + $"/api/share2/paper/check/refuse",
                Postdata = JsonConvert.SerializeObject(check),
                ContentType = "application/json",
                Method = "POST",
                ResultType = ResultType.String,
                PostDataType = PostDataType.String,
                Header = _header
            };
            var result = new HttpHelper().GetData(item);
            HttpResultModel httpResult = new HttpResultModel();
            if (result.StatusCode != HttpStatusCode.OK)
            {
                httpResult = new HttpResultModel { success = false, message = "获取失败" };
            }
            else
            {
                httpResult = JsonConvert.DeserializeObject<HttpResultModel>(result.Data);
            }

            if (httpResult.success)
            {
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false, httpResult.message }, JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult PutOff(string paperId)
        {

            HttpItem item = new HttpItem
            {
                URL = _baseUrl + $"/api/share2/paper/putoff/{paperId}",
                Method = "GET",
                ResultType = ResultType.String,
                PostDataType = PostDataType.String,
                Header = _header
            };
            var result = new HttpHelper().GetData(item);
            HttpResultModel httpResult = new HttpResultModel();
            if (result.StatusCode != HttpStatusCode.OK)
            {
                httpResult = new HttpResultModel { success = false, message = "下架失败" };
            }
            else
            {
                httpResult = JsonConvert.DeserializeObject<HttpResultModel>(result.Data);
            }

            if (httpResult.success)
            {
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false, httpResult.message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}