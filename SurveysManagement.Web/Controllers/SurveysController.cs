using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SurveysManagement.DataAccess.Entities;
using SurveysManagement.Web.Commons.Questions;
using SurveysManagement.Web.Models;
using static SurveysManagement.Web.Enums.Enums;
using Microsoft.AspNet.Identity;
using SurveysManagement.Web.Helpers;

namespace SurveysManagement.Web.Controllers
{
    public class SurveysController : BaseController
    {
        private SurveyModel db = new SurveyModel();

        // GET: Surveys
        [Authorize(Roles = "Admin, User")]
        public ActionResult Index()
        {
            //var surveys = db.Surveys.Include(s => s.Category);
            return View();
        }

        [Authorize(Roles = "User")]
        public ActionResult UserSurveys()
        {
            return View();
        }

        [Authorize(Roles = "Admin, User")]
        public ActionResult getUserSurveys(string username)
        {
            JsonResult result = new JsonResult();

            try
            {
                if (username != null)
                {
                    var user = from usr in db.Users
                               join reg in db.AspNetUsers on usr.ASPUserId equals reg.Id
                               where (usr.isDeleted != true && reg.UserName == username)
                               select (usr);
                    if (user != null)
                    {
                        // Initialization.
                        string search = Request.Form.GetValues("search[value]")[0];
                        string draw = Request.Form.GetValues("draw")[0];
                        string order = Request.Form.GetValues("order[0][column]")[0];
                        string orderDir = Request.Form.GetValues("order[0][dir]")[0];
                        int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
                        int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);

                        // Loading.
                        IQueryable<SurveyViewModel> data = db.UserSurveys.Include("Survey.Category").Where(x => x.UserId == user.FirstOrDefault().Id && x.isDeleted != true).Select(x => new SurveyViewModel
                        {
                            Id = x.Survey.Id,
                            Name = x.Survey.Name,
                            CategoryId = x.Survey.CategoryId,
                            CreationDate = x.Survey.CreationDate,
                            StartDate = x.Survey.StartDate,
                            EndDate = x.Survey.EndDate,
                            Agent = x.User.Name,
                            Category = x.Survey.Category.Name

                        }).AsQueryable();

                        // Total record count.
                        int totalRecords = data.Count();

                        // Verification.
                        List<SurveyViewModel> resultList;

                        if (!string.IsNullOrEmpty(search) &&
                            !string.IsNullOrWhiteSpace(search))
                        {
                            // Apply search
                            data = data.Where(p => p.Name.ToString().ToLower().Contains(search.ToLower()));
                        }

                        // Sorting.
                        //data = this.SortByColumnWithOrder(order, orderDir, data);

                        // Filter record count.
                        int recFilter = data.Count();

                        // Apply pagination.
                        data = data.OrderBy(x => x.Id).Skip(startRec).Take(pageSize);

                        resultList = data.ToList();
                        // Loading drop down lists.
                        result = this.Json(new { draw = Convert.ToInt32(draw), recordsTotal = totalRecords, recordsFiltered = recFilter, data = resultList }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return HttpNotFound();
                    }
                }
                else
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
                }
            }
            catch (Exception ex)
            {
                // Info
                Console.Write(ex);
            }

            // Return info.
            return result;

        }

        [Authorize(Roles = "Admin")]
        public ActionResult UserEntries(int? id)
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult getUserEntries(int? id)
        {
            JsonResult result = new JsonResult();

            try
            {
                if (id != null)
                {
                    var user = from usr in db.Users
                               join entry in db.SurveyEntries on usr.Id equals entry.UserSurvey.UserId
                               where (usr.isDeleted != true && entry.UserSurvey.Survey.isDeleted != true && entry.UserSurvey.SurveyId == id)
                               select (usr);
                    if (user != null)
                    {
                        // Initialization.
                        string search = Request.Form.GetValues("search[value]")[0];
                        string draw = Request.Form.GetValues("draw")[0];
                        string order = Request.Form.GetValues("order[0][column]")[0];
                        string orderDir = Request.Form.GetValues("order[0][dir]")[0];
                        int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
                        int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);

                        // Loading.
                        IQueryable<SurveyViewModel> data = db.SurveyEntries.Include("UserSurvey.Survey.Category").Where(x => x.UserSurvey.UserId == user.FirstOrDefault().Id && x.isDeleted != true).Select(x => new SurveyViewModel
                        {
                            Id = x.Id,
                            Name = x.UserSurvey.Survey.Name,
                            CategoryId = x.UserSurvey.Survey.CategoryId,
                            CreationDate = x.UserSurvey.Survey.CreationDate,
                            StartDate = x.UserSurvey.Survey.StartDate,
                            EndDate = x.UserSurvey.Survey.EndDate,
                            Agent = x.UserSurvey.User.Name,
                            Category = x.UserSurvey.Survey.Category.Name

                        }).AsQueryable();

                        // Total record count.
                        int totalRecords = data.Count();

                        // Verification.
                        List<SurveyViewModel> resultList;

                        if (!string.IsNullOrEmpty(search) &&
                            !string.IsNullOrWhiteSpace(search))
                        {
                            // Apply search
                            data = data.Where(p => p.Name.ToString().ToLower().Contains(search.ToLower()));
                        }

                        // Sorting.
                        //data = this.SortByColumnWithOrder(order, orderDir, data);

                        // Filter record count.
                        int recFilter = data.Count();

                        // Apply pagination.
                        data = data.OrderBy(x => x.Id).Skip(startRec).Take(pageSize);

                        resultList = data.ToList();
                        // Loading drop down lists.
                        result = this.Json(new { draw = Convert.ToInt32(draw), recordsTotal = totalRecords, recordsFiltered = recFilter, data = resultList }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return HttpNotFound();
                    }
                }
                else
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
                }
            }
            catch (Exception ex)
            {
                // Info
                Console.Write(ex);
            }

            // Return info.
            return result;

        }

        [Authorize(Roles = "Admin,Client")]
        public ActionResult ClientSurveys()
        {
            return View();
        }

        [Authorize(Roles = "Admin,Client")]
        public ActionResult getClientSurveys(string username)
        {
            JsonResult result = new JsonResult();

            try
            {
                if (username != null)
                {
                    var user = from usr in db.Users
                               join reg in db.AspNetUsers on usr.ASPUserId equals reg.Id
                               where (usr.isDeleted != true && reg.UserName == username)
                               select (usr);
                    if (user != null)
                    {
                        // Initialization.
                        string search = Request.Form.GetValues("search[value]")[0];
                        string draw = Request.Form.GetValues("draw")[0];
                        string order = Request.Form.GetValues("order[0][column]")[0];
                        string orderDir = Request.Form.GetValues("order[0][dir]")[0];
                        int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
                        int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);

                        // Loading.
                        IQueryable<SurveyViewModel> data = db.Surveys.Include(usr => usr.User).Include(cat => cat.Category).Where(x => x.isDeleted != true && x.AgentId == user.FirstOrDefault().Id && x.StartDate > DateTime.Now && x.EndDate < DateTime.Now).Select(x => new SurveyViewModel
                        {
                            Id = x.Id,
                            Name = x.Name,
                            CategoryId = x.CategoryId,
                            CreationDate = x.CreationDate,
                            StartDate = x.StartDate,
                            EndDate = x.EndDate,
                            Agent = x.User.Name,
                            Category = x.Category.Name

                        }).AsQueryable();

                        // Total record count.
                        int totalRecords = data.Count();

                        // Verification.
                        List<SurveyViewModel> resultList;

                        if (!string.IsNullOrEmpty(search) &&
                            !string.IsNullOrWhiteSpace(search))
                        {
                            // Apply search
                            data = data.Where(p => p.Name.ToString().ToLower().Contains(search.ToLower()));
                        }

                        // Sorting.
                        //data = this.SortByColumnWithOrder(order, orderDir, data);

                        // Filter record count.
                        int recFilter = data.Count();

                        // Apply pagination.
                        data = data.OrderBy(x => x.Id).Skip(startRec).Take(pageSize);

                        resultList = data.ToList();
                        // Loading drop down lists.
                        result = this.Json(new { draw = Convert.ToInt32(draw), recordsTotal = totalRecords, recordsFiltered = recFilter, data = resultList }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return HttpNotFound();
                    }
                }
                else
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
                }
            }
            catch (Exception ex)
            {
                // Info
                Console.Write(ex);
            }

            // Return info.
            return result;

        }

        [Authorize(Roles = "Admin")]
        public ActionResult getsurveys()
        {
            JsonResult result = new JsonResult();

            try
            {
                // Initialization.
                string search = Request.Form.GetValues("search[value]")[0];
                string draw = Request.Form.GetValues("draw")[0];
                string order = Request.Form.GetValues("order[0][column]")[0];
                string orderDir = Request.Form.GetValues("order[0][dir]")[0];
                int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
                int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);

                // Loading.
                IQueryable<SurveyViewModel> data = db.Surveys.Include(usr => usr.User).Include(cat => cat.Category).Where(x => x.isDeleted != true).Select(x => new SurveyViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    CategoryId = x.CategoryId,
                    CreationDate = x.CreationDate,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    Agent = x.User.Name,
                    Category = x.Category.Name

                }).AsQueryable();

                // Total record count.
                int totalRecords = data.Count();

                // Verification.
                List<SurveyViewModel> resultList;

                if (!string.IsNullOrEmpty(search) &&
                    !string.IsNullOrWhiteSpace(search))
                {
                    // Apply search
                    data = data.Where(p => p.Name.ToString().ToLower().Contains(search.ToLower())
                    || p.EndDate.ToString().ToLower().Contains(search.ToLower())
                    || p.StartDate.ToString().ToLower().Contains(search.ToLower())
                    || p.Id.ToString().ToLower().Contains(search.ToLower())                    
                    || p.CreationDate.ToString().ToLower().Contains(search.ToLower())
                    );
                }

                // Sorting.
                //data = this.SortByColumnWithOrder(order, orderDir, data);

                // Filter record count.
                int recFilter = data.Count();

                // Apply pagination.
                data = data.OrderBy(x => x.Id).Skip(startRec).Take(pageSize);

                resultList = data.ToList();
                // Loading drop down lists.
                result = this.Json(new { draw = Convert.ToInt32(draw), recordsTotal = totalRecords, recordsFiltered = recFilter, data = resultList }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Info
                Console.Write(ex);
            }

            // Return info.
            return result;
        }

        // GET: Surveys/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                var Agents = from usr in db.Users
                             join reg in db.AspNetUsers on usr.ASPUserId equals reg.Id
                             where (usr.isDeleted != true && reg.AspNetRoles.FirstOrDefault().Name == Enums.Enums.RoleTypes.Client)
                             select (usr);
                SurveyViewModel surveyViewModel = new SurveyViewModel();
                surveyViewModel.Categories = db.Categories.Select(x => new HelperViewModel { Id = x.Id, Text = x.Name }).ToList();
                surveyViewModel.Agents = Agents.Select(x => new HelperViewModel { Id = x.Id, Text = x.Name }).ToList();
                
                return View(surveyViewModel);
            }
            else
            {
                int SurveyId;
                if (int.TryParse(id.ToString(), out SurveyId))
                {
                    Survey survey = db.Surveys.Include(x => x.Category).FirstOrDefault(x => x.Id == id);
                    if (survey == null)
                    {
                        return HttpNotFound();
                    }
                    else
                    {
                        SurveyViewModel surveyViewModel = new SurveyViewModel
                        {
                            Id = survey.Id,
                            CategoryId = survey.CategoryId,
                            Name = survey.Name,
                            EndDate = DateHelpers.ConvertStringToDate(survey.EndDate.ToString()),
                            CreationDate = DateHelpers.ConvertStringToDate(survey.CreationDate.ToString()),
                            StartDate = DateHelpers.ConvertStringToDate(survey.StartDate.ToString()),
                            AgentId = survey.AgentId
                            //hdnSurveyQuestions = string.Join("" survey.SurveyQuestions
                        };
                        //foreach (var ques in survey.SurveyQuestions)
                        //{
                        //    surveyViewModel.hdnSurveyQuestions += "," + ques.QuestionId + ",";
                        //}

                        //foreach (var usr in survey.UserSurveys)
                        //{
                        //    surveyViewModel.hdnUserSurveys += "," + usr.UserId + ",";
                        //}

                        //surveyViewModel.hdnUserSurveys = "";
                        surveyViewModel.SurveyQuestions = survey.SurveyQuestions.Where(x => x.Question.isDeleted != true).Select(x => new HelperViewModel { Id = x.QuestionId.Value }).ToList();
                        surveyViewModel.UserSurveys = survey.UserSurveys.Where(x => x.isDeleted != true).Select(x => new HelperViewModel { Id = x.UserId.Value }).ToList();
                        var Agents = from usr in db.Users
                                        join reg in db.AspNetUsers on usr.ASPUserId equals reg.Id
                                        where (usr.isDeleted != true && reg.AspNetRoles.FirstOrDefault().Name == Enums.Enums.RoleTypes.Client)
                                        select (usr);

                        surveyViewModel.Agents = Agents.Select(x => new HelperViewModel { Id = x.Id, Text = x.Name}).ToList();
                        surveyViewModel.Categories = db.Categories.Select(x => new HelperViewModel { Id = x.Id, Text = x.Name }).ToList();
                        return View(surveyViewModel);
                    }
                }
            }
            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Survey survey = db.Surveys.Find(id);
            if (survey == null)
            {
                return HttpNotFound();
            }
            survey.isDeleted = true;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // POST: Surveys/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(SurveyViewModel Survey)
        {
            var Agents = from usr in db.Users
                         join reg in db.AspNetUsers on usr.ASPUserId equals reg.Id
                         where (usr.isDeleted != true && reg.AspNetRoles.FirstOrDefault().Name == Enums.Enums.RoleTypes.Client)
                         select (usr);

            Survey.Categories = db.Categories.Select(x => new HelperViewModel { Id = x.Id, Text = x.Name }).ToList();
            Survey.Agents = Agents.Select(x => new HelperViewModel { Id = x.Id, Text = x.Name }).ToList();

            if (Survey.hfIsPreview.ToLower() == "true")
            {                
                Survey.hfPreviewId = EditTemp(Survey);
                return View(Survey);
            }
            int TempId = 0;
            if (Survey.hfIsPreview.ToLower() == "false" && Survey.hfPreviewId.HasValue)
            {
                TempId = Survey.hfPreviewId.Value;
                TempSurvey Temp = db.TempSurveys.FirstOrDefault(x => x.Id == TempId);
                db.TempSurveys.Remove(Temp);
            }
            if (Survey.Id == null)
            {
                Survey _Survey = new Survey
                {
                    //Id = Survey.Id.Value,
                    CategoryId = Survey.CategoryId,
                    Name = Survey.Name,
                    EndDate = Survey.EndDate,
                    CreationDate = DateTime.Now,
                    StartDate = Survey.StartDate,
                };
                //_Survey.SurveyQuestions = Survey.SurveyQuestions.Select(x => new SurveyQuestion {QuestionId = x.Id }).ToList();

                db.Surveys.Add(_Survey);
                db.SaveChanges();

                if (Survey.hdnSurveyQuestions != null && Survey.hdnSurveyQuestions.Length > 0)
                {
                    string[] lstSurveyQuestions = Survey.hdnSurveyQuestions.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (string item in lstSurveyQuestions)
                        db.SurveyQuestions.Add(new SurveyQuestion { SurveyId = _Survey.Id, QuestionId = Convert.ToInt32(item) });
                }

                if (Survey.hdnUserSurveys != null && Survey.hdnUserSurveys.Length > 0)
                {
                    string[] lstUserSurveys = Survey.hdnUserSurveys.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (string item in lstUserSurveys)
                        db.UserSurveys.Add(new UserSurvey { SurveyId = _Survey.Id, UserId = Convert.ToInt32(item) });
                }
                //survey.SurveyQuestions = Survey.SurveyQuestions.Select(x => new SurveyQuestion { QuestionId = x.Id, SurveyId = Survey.Id}).ToList();
                db.SaveChanges();
                return View(Survey);
            }
            else
            {
                Survey survey = db.Surveys.Find(Survey.Id);

                //survey.Id = Survey.Id;
                survey.CategoryId = Survey.CategoryId;
                survey.Name = Survey.Name;
                survey.EndDate = Survey.EndDate;
                //survey.CreationDate = Survey.CreationDate;
                survey.StartDate = Survey.StartDate;
                //db.Surveys.Add(survey);
                db.SaveChanges();

                foreach (SurveyQuestion item in db.SurveyQuestions.Where(x => x.SurveyId == Survey.Id))
                    db.SurveyQuestions.Remove(item);

                foreach (UserSurvey item in db.UserSurveys.Where(x => x.SurveyId == Survey.Id))
                    db.UserSurveys.Remove(item);

                if (Survey.hdnSurveyQuestions.Length > 0)
                {
                    string[] lstSurveyQuestions = Survey.hdnSurveyQuestions.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (string item in lstSurveyQuestions)
                        db.SurveyQuestions.Add(new SurveyQuestion { SurveyId = Survey.Id, QuestionId = Convert.ToInt32(item) });
                }

                if (Survey.hdnUserSurveys.Length > 0)
                {
                    string[] lstUserSurveys = Survey.hdnUserSurveys.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (string item in lstUserSurveys)
                        db.UserSurveys.Add(new UserSurvey { SurveyId = Survey.Id, UserId = Convert.ToInt32(item) });
                }
                //survey.SurveyQuestions = Survey.SurveyQuestions.Select(x => new SurveyQuestion { QuestionId = x.Id, SurveyId = Survey.Id}).ToList();
                db.SaveChanges();

                return View(Survey);
            }
        }

        //Preview
        public int EditTemp(SurveyViewModel Survey)
        {
            if (Survey.hfPreviewId == null)
            {
                TempSurvey _Survey = new TempSurvey
                {
                    //Id = Survey.Id.Value,
                    CategoryId = Survey.CategoryId,
                    Name = Survey.Name,
                    EndDate = Survey.EndDate,
                    CreationDate = DateTime.Now,
                    StartDate = Survey.StartDate,
                };
                //_Survey.SurveyQuestions = Survey.SurveyQuestions.Select(x => new SurveyQuestion {QuestionId = x.Id }).ToList();

                db.TempSurveys.Add(_Survey);
                db.SaveChanges();

                if (Survey.hdnSurveyQuestions != null && Survey.hdnSurveyQuestions.Length > 0)
                {
                    string[] lstSurveyQuestions = Survey.hdnSurveyQuestions.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (string item in lstSurveyQuestions)
                        db.TempSurveyQuestions.Add(new TempSurveyQuestion { TempSurveyId = _Survey.Id, TempQuestionId = Convert.ToInt32(item) });
                }

                //if (Survey.hdnUserSurveys != null && Survey.hdnUserSurveys.Length > 0)
                //{
                //    string[] lstUserSurveys = Survey.hdnUserSurveys.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

                //    foreach (string item in lstUserSurveys)
                //        db.UserSurveys.Add(new UserSurvey { SurveyId = _Survey.Id, UserId = Convert.ToInt32(item) });
                //}
                //survey.SurveyQuestions = Survey.SurveyQuestions.Select(x => new SurveyQuestion { QuestionId = x.Id, SurveyId = Survey.Id}).ToList();
                db.SaveChanges();
                return _Survey.Id;
            }
            else
            {
                TempSurvey survey = db.TempSurveys.Find(Survey.hfPreviewId);

                //survey.Id = Survey.Id;
                survey.CategoryId = Survey.CategoryId;
                survey.Name = Survey.Name;
                survey.EndDate = Survey.EndDate;
                //survey.CreationDate = Survey.CreationDate;
                survey.StartDate = Survey.StartDate;
                //db.Surveys.Add(survey);
                db.SaveChanges();

                foreach (TempSurveyQuestion item in db.TempSurveyQuestions.Where(x => x.TempSurveyId == Survey.Id))
                    db.TempSurveyQuestions.Remove(item);

                //foreach (UserSurvey item in db.UserSurveys.Where(x => x.SurveyId == Survey.Id))
                //    db.UserSurveys.Remove(item);

                if (Survey.hdnSurveyQuestions.Length > 0)
                {
                    string[] lstSurveyQuestions = Survey.hdnSurveyQuestions.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (string item in lstSurveyQuestions)
                        db.TempSurveyQuestions.Add(new TempSurveyQuestion { TempSurveyId = Survey.Id, TempQuestionId = Convert.ToInt32(item) });
                }

                //if (Survey.hdnUserSurveys.Length > 0)
                //{
                //    string[] lstUserSurveys = Survey.hdnUserSurveys.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

                //    foreach (string item in lstUserSurveys)
                //        db.UserSurveys.Add(new UserSurvey { SurveyId = Survey.Id, UserId = Convert.ToInt32(item) });
                //}
                //survey.SurveyQuestions = Survey.SurveyQuestions.Select(x => new SurveyQuestion { QuestionId = x.Id, SurveyId = Survey.Id}).ToList();
                db.SaveChanges();

                return survey.Id;
            }
        }
        // GET: Surveys/Delete/5
        //[Authorize(Roles = "Admin")]
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Survey survey = db.Surveys.Find(id);
        //    if (survey == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(survey);
        //}

        // POST: Surveys/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //[Authorize(Roles = "Admin")]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Survey survey = db.Surveys.Find(id);
        //    survey.isDeleted = true;
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}
        [Authorize(Roles = "Admin")]
        public ActionResult ViewTempSurvey(int id)
        {
            List<BaseQuestion> questions = new List<BaseQuestion>();
            if (id > 0)
            {
                TempSurvey survey = getTempSurveyById(id);
                questions = getTempSurveyQuestions(survey);
            }

            return View(questions);
        }

        [Authorize(Roles = "Admin,User")]
        public ActionResult AnswerSurvey(int id)
        {
            List<BaseQuestion> questions = new List<BaseQuestion>();
            if (id > 0)
            {
                Survey survey = getSurveyById(id);
                questions = getSurveyQuestions(survey);


            }

            return View(questions);
        }

        [Authorize(Roles = "Admin,User")]
        public ActionResult AnsweredSurvey(int id)
        {
            List<BaseQuestion> questions = new List<BaseQuestion>();
            if (id > 0)
            {

                //first get entry where userid and passed survey id is answered
                var userName = User.Identity.Name;
                if (!string.IsNullOrEmpty(userName))
                {
                    var userId = getUserIdFromName(userName);
                    if (userId > 0 && id > 0)
                    {

                        var entry = getSurveyEntryForUser(id, userId);

                        //then, if an entry is found, then load the survey, questions and user answers
                        if(entry != null)
                        {


                            questions = getSurveyQuestionsWithAnswers(entry);
                        }
                        //then bind

                        
                    }
                }
            }

            return View(questions);
        }


        private DataAccess.Entities.SurveyEntry getSurveyEntryForUser(int surveyEntryId, int userId)
        {
            DataAccess.Entities.SurveyEntry entry = default(DataAccess.Entities.SurveyEntry);
            try
            {
                entry = db.SurveyEntries.Include("UserSurvey").Include("UserSurvey.Survey")
                                        .Include("UserSurvey.Survey")
                                        .Include("UserSurvey.Survey.SurveyQuestions")
                                        .Include("UserSurvey.Survey.SurveyQuestions.Question")
                                        .Include("UserSurvey.Survey.SurveyQuestions.Question.Answers")
                                        .Include("UserSurvey.Survey.SurveyQuestions.Question.QuestionType")
                                        .Where(e => e.Id == surveyEntryId).FirstOrDefault();
                //.Where(e => e.UserSurvey.UserId == userId
                //&& e.UserSurvey.SurveyId == surveyId).FirstOrDefault();
            }
            catch (Exception ex)
            {

                //to hanlde later
            }

            return entry;
        }


        //Submit Answers
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AnswerSurvey(FormCollection form)
        {
            var userName = User.Identity.Name;
            if (!string.IsNullOrEmpty(userName))
            {
                var userId = getUserIdFromName(userName);
                var surveyId = getSurveyIdFromForm(form);
                if (userId > 0 && surveyId > 0)
                {
                    var answers = getAnswersFromForm(form);
                    if (answers != null && answers.Count > 0)
                    {

                        var userSurvey = db.UserSurveys.Where(x => x.SurveyId == surveyId && x.UserId == userId).FirstOrDefault();

                        if (userSurvey != null)
                        {
                            //add an entry

                            var entry = new DataAccess.Entities.SurveyEntry();
                            entry.Date = DateTime.Now;
                            entry.UserSurveyId = userSurvey.Id;

                            foreach (var answer in answers)
                            {
                                var surveyAnswer = new SurveyAnswer();
                                surveyAnswer.QuestionId = answer.QuestionID;
                                if (answer.AnswerID.HasValue)
                                {
                                    surveyAnswer.AnswerId = answer.AnswerID.Value;
                                    surveyAnswer.AnswerText = answer.AnswerText;
                                }
                                else
                                {
                                    surveyAnswer.AnswerText = answer.AnswerText;
                                }
                                entry.SurveyAnswers.Add(surveyAnswer);
                            }

                            db.SurveyEntries.Add(entry);

                            db.SaveChanges();
                        }
                    }
                }
            }
            return View();
        }

        private int getUserIdFromName(string userName)
        {
            int userId = 0;
            try
            {
                var user = (from usr in db.Users
                            join reg in db.AspNetUsers on usr.ASPUserId equals reg.Id
                            where (usr.isDeleted != true && reg.UserName == userName)
                            select (usr)).FirstOrDefault();

                if (user != null)
                {
                    userId = user.Id;
                }
            }
            catch (Exception ex)
            {

                throw;
            }


            return userId;
        }

        private int getSurveyIdFromForm(FormCollection form)
        {
            int surveyId = 0;

            foreach (var key in form.AllKeys.Where(x => x.ToString().ToLower().Contains("_") != true))
            {
                if (key.ToString().ToLower() == "surveyid")
                {
                    surveyId = Convert.ToInt32(form.GetValue(key).AttemptedValue);
                }
            }

            return surveyId;
        }

        private List<UserSurveyAnswer> getAnswersFromForm(FormCollection form)
        {
            List<UserSurveyAnswer> answers = new List<UserSurveyAnswer>();

            var others = form.AllKeys.Where(x => x.ToString().ToLower().Contains("_") != true &&
             x.ToString().ToLower().Contains("surveyid") != true && x.ToString().ToLower().Contains("other"));

            foreach (var question in form.AllKeys.Where(x => x.ToString().ToLower().Contains("_") != true &&
             x.ToString().ToLower().Contains("surveyid") != true && !x.ToString().ToLower().Contains("other")))
            {
                var questionValues = form.GetValues(question.ToString());

                if (questionValues != null && questionValues.Count() > 0)
                {
                    var isMultipleChoice = questionValues.Contains("__Multi__");
                    foreach (var answer in questionValues.Where(x => x.Contains("__Multi__") != true))
                    {
                        var userAnswer = new UserSurveyAnswer();

                        userAnswer.QuestionID = Convert.ToInt32(question);
                        int answerId = 0;
                        var isConvertableToInt = int.TryParse(answer, out answerId);

                        if (isConvertableToInt && isMultipleChoice)
                        {
                            userAnswer.AnswerID = answerId;
                            if (form.Get(answerId + "-" + question + "other") != null && form.Get(answerId + "-" + question + "other").Count() > 0)
                            {
                                userAnswer.AnswerText = form.Get(answerId + "-" + question + "other");
                            }
                            //userAnswer.AnswerText = answer.t
                        }
                        else
                        {
                            userAnswer.AnswerText = answer;
                        }
                        
                        answers.Add(userAnswer);

                    }
                }
            }

            return answers;
        }


        private Survey getSurveyById(int surveyId)
        {
            Survey survey = new Survey();
            try
            {
                var context = new SurveyModel();

                survey = context.Surveys.Include("SurveyQuestions")
                                           .Include("SurveyQuestions.Question")
                                           .Include("SurveyQuestions.Question.Answers")
                                           .Include("SurveyQuestions.Question.QuestionType")
                                           .Where(s => s.Id == surveyId).FirstOrDefault();




            }
            catch (Exception ex)
            {
                //to handle later
            }

            return survey;
        }

        private TempSurvey getTempSurveyById(int surveyId)
        {
            TempSurvey survey = new TempSurvey();
            try
            {
                var context = new SurveyModel();

                survey = context.TempSurveys.Include("TempSurveyQuestions")
                                           .Include("TempSurveyQuestions.Question")
                                           .Include("TempSurveyQuestions.Question.Answers")
                                           .Include("TempSurveyQuestions.Question.QuestionType")
                                           .Where(s => s.Id == surveyId).FirstOrDefault();




            }
            catch (Exception ex)
            {
                //to handle later
            }

            return survey;
        }

        private List<BaseQuestion> getSurveyQuestions(Survey survey)
        {
            List<BaseQuestion> questions = new List<BaseQuestion>();
            foreach (var question in survey.SurveyQuestions)
            {
                questions.Add(createInstance(question));
            }

            return questions;
        }

        private List<BaseQuestion> getTempSurveyQuestions(TempSurvey survey)
        {
            List<BaseQuestion> questions = new List<BaseQuestion>();
            foreach (var question in survey.TempSurveyQuestions)
            {
                questions.Add(createTempInstance(question));
            }

            return questions;
        }

        private List<BaseQuestion> getSurveyQuestionsWithAnswers(DataAccess.Entities.SurveyEntry surveyEntry)
        {
            List<BaseQuestion> questions = new List<BaseQuestion>();
            foreach (var question in surveyEntry.UserSurvey.Survey.SurveyQuestions)
            {
                questions.Add(createInstance(question));
            }


            foreach (var question in questions)
            {
                var answers = surveyEntry.SurveyAnswers.Where(x => x.QuestionId == question.QuestionUniqueID).ToList();

                foreach (var answer in answers)
                {
                    question.UserAnswers.Add(new Tuple<int, string, bool?>(answer.QuestionId.Value, answer.AnswerId.HasValue ? answer.AnswerId.Value.ToString() : answer.AnswerText, answer.Answer.isOther.HasValue ? answer.Answer.isOther.Value : false));
                    question.RenderQuestion();
                }
            }
            return questions;
        }

        private BaseQuestion createInstance(SurveyQuestion question)
        {
            BaseQuestion questionControl = null;
            switch ((QuestionTypes)question.Question.QuestionType.Id)
            {
                case QuestionTypes.Checkbox:
                    var checkbox = new CheckboxQuestion();
                    checkbox.QuestionType = QuestionTypes.Checkbox;
                    checkbox.QuestionText = question.Question.Name;
                    checkbox.QuestionUniqueID = question.Question.Id;
                    checkbox.Answers = question.Question.Answers.Select(a => new MultipleChoiseQuestionAnswers { QuestionID = question.Id, Value = a.Id, Text = a.Text, isOther = a.isOther}).ToList();
                    checkbox.RenderQuestion();
                    questionControl = checkbox;
                    break;
                case QuestionTypes.Multiplechoice:
                    var radioButton = new RadioButtonQuestion();
                    radioButton.QuestionType = QuestionTypes.Multiplechoice;
                    radioButton.QuestionText = question.Question.Name;
                    radioButton.QuestionUniqueID = question.Question.Id;
                    radioButton.Answers = question.Question.Answers.Select(a => new MultipleChoiseQuestionAnswers { QuestionID = question.Id, Value = a.Id, Text = a.Text, isOther = a.isOther }).ToList();
                    radioButton.RenderQuestion();
                    questionControl = radioButton;
                    break;
                case QuestionTypes.Text:
                    var textBox = new TextBoxQuestion();
                    textBox.QuestionType = QuestionTypes.Text;
                    textBox.QuestionText = question.Question.Name;
                    textBox.QuestionUniqueID = question.Question.Id;
                    textBox.PlaceHolder = "Your Answer...";
                    textBox.RenderQuestion();
                    questionControl = textBox;
                    break;
                case QuestionTypes.Percentage:
                    var percentage = new PercentageQuestion();
                    percentage.QuestionType = QuestionTypes.Percentage;
                    percentage.QuestionText = question.Question.Name;
                    percentage.QuestionUniqueID = question.Question.Id;
                    percentage.PlaceHolder = "Your Rate...";
                    percentage.RenderQuestion();
                    questionControl = percentage;
                    break;
                case QuestionTypes.Range:
                    var range = new RangeQuestion();
                    range.QuestionType = QuestionTypes.Range;
                    range.QuestionText = question.Question.Name;
                    range.QuestionUniqueID = question.Question.Id;
                    range.From = question.Question.From;
                    range.Step = question.Question.Step;
                    range.To = question.Question.To;
                    range.PlaceHolder = "Your Rate...";
                    range.RenderQuestion();
                    questionControl = range;
                    break;
                case QuestionTypes.Number:
                    var number = new NumberQuestion();
                    number.QuestionType = QuestionTypes.Number;
                    number.QuestionText = question.Question.Name;
                    number.QuestionUniqueID = question.Question.Id;
                    number.PlaceHolder = "Your Value...";
                    number.RenderQuestion();
                    questionControl = number;
                    break;
                case QuestionTypes.Dropdown:
                    var dropDown = new DropDownQuestion();
                    dropDown.QuestionType = QuestionTypes.Dropdown;
                    dropDown.QuestionText = question.Question.Name;
                    dropDown.QuestionUniqueID = question.Question.Id;
                    dropDown.Answers = question.Question.Answers.Select(a => new MultipleChoiseQuestionAnswers { QuestionID = question.Id, Value = a.Id, Text = a.Text, isOther = a.isOther }).ToList();
                    dropDown.RenderQuestion();
                    questionControl = dropDown;
                    break;
                case QuestionTypes.Slider:
                    var slider = new SliderQuestion();
                    slider.QuestionType = QuestionTypes.Slider;
                    slider.QuestionText = question.Question.Name;
                    slider.QuestionUniqueID = question.Question.Id;
                    slider.From = question.Question.From;
                    slider.Step = question.Question.Step;
                    slider.To = question.Question.To;
                    slider.PlaceHolder = "Your Rate...";
                    slider.RenderQuestion();
                    questionControl = slider;
                    break;
                case QuestionTypes.Date:
                    var date = new DateQuestion();
                    date.QuestionType = QuestionTypes.Date;
                    date.QuestionText = question.Question.Name;
                    date.QuestionUniqueID = question.Question.Id;
                    date.PlaceHolder = "Date ...";
                    date.RenderQuestion();
                    questionControl = date;
                    break;
                case QuestionTypes.Rate:
                    var rate = new RatingQuestion();
                    rate.QuestionType = QuestionTypes.Rate;
                    rate.QuestionText = question.Question.Name;
                    rate.QuestionUniqueID = question.Question.Id;
                    rate.From = question.Question.From;
                    rate.To = question.Question.To;
                    rate.PlaceHolder = "Your Rate...";
                    rate.RenderQuestion();
                    questionControl = rate;
                    break;
                default:
                    break;
            }


            return questionControl;
        }

        private BaseQuestion createTempInstance(TempSurveyQuestion question)
        {
            BaseQuestion questionControl = null;
            switch ((QuestionTypes)question.Question.QuestionType.Id)
            {
                case QuestionTypes.Checkbox:
                    var checkbox = new CheckboxQuestion();
                    checkbox.QuestionType = QuestionTypes.Checkbox;
                    checkbox.QuestionText = question.Question.Name;
                    checkbox.QuestionUniqueID = question.Question.Id;
                    checkbox.Answers = question.Question.Answers.Select(a => new MultipleChoiseQuestionAnswers { QuestionID = question.Id, Value = a.Id, Text = a.Text, isOther = a.isOther }).ToList();
                    checkbox.RenderQuestion();
                    questionControl = checkbox;
                    break;
                case QuestionTypes.Multiplechoice:
                    var radioButton = new RadioButtonQuestion();
                    radioButton.QuestionType = QuestionTypes.Multiplechoice;
                    radioButton.QuestionText = question.Question.Name;
                    radioButton.QuestionUniqueID = question.Question.Id;
                    radioButton.Answers = question.Question.Answers.Select(a => new MultipleChoiseQuestionAnswers { QuestionID = question.Id, Value = a.Id, Text = a.Text, isOther = a.isOther }).ToList();
                    radioButton.RenderQuestion();
                    questionControl = radioButton;
                    break;
                case QuestionTypes.Text:
                    var textBox = new TextBoxQuestion();
                    textBox.QuestionType = QuestionTypes.Text;
                    textBox.QuestionText = question.Question.Name;
                    textBox.QuestionUniqueID = question.Question.Id;
                    textBox.PlaceHolder = "Your Answer...";
                    textBox.RenderQuestion();
                    questionControl = textBox;
                    break;
                case QuestionTypes.Percentage:
                    var percentage = new PercentageQuestion();
                    percentage.QuestionType = QuestionTypes.Percentage;
                    percentage.QuestionText = question.Question.Name;
                    percentage.QuestionUniqueID = question.Question.Id;
                    percentage.PlaceHolder = "Your Rate...";
                    percentage.RenderQuestion();
                    questionControl = percentage;
                    break;
                case QuestionTypes.Range:
                    var range = new RangeQuestion();
                    range.QuestionType = QuestionTypes.Range;
                    range.QuestionText = question.Question.Name;
                    range.QuestionUniqueID = question.Question.Id;
                    range.From = question.Question.From;
                    range.To = question.Question.To;
                    range.Step = Convert.ToInt32(question.Question.Step);
                    range.PlaceHolder = "Your Rate...";
                    range.RenderQuestion();
                    questionControl = range;
                    break;
                case QuestionTypes.Number:
                    var number = new NumberQuestion();
                    number.QuestionType = QuestionTypes.Number;
                    number.QuestionText = question.Question.Name;
                    number.QuestionUniqueID = question.Question.Id;
                    number.PlaceHolder = "Your Value...";
                    number.RenderQuestion();
                    questionControl = number;
                    break;
                case QuestionTypes.Dropdown:
                    var dropDown = new DropDownQuestion();
                    dropDown.QuestionType = QuestionTypes.Dropdown;
                    dropDown.QuestionText = question.Question.Name;
                    dropDown.QuestionUniqueID = question.Question.Id;
                    dropDown.Answers = question.Question.Answers.Select(a => new MultipleChoiseQuestionAnswers { QuestionID = question.Id, Value = a.Id, Text = a.Text, isOther = a.isOther }).ToList();
                    dropDown.RenderQuestion();
                    questionControl = dropDown;
                    break;
                case QuestionTypes.Slider:
                    var slider = new SliderQuestion();
                    slider.QuestionType = QuestionTypes.Slider;
                    slider.QuestionText = question.Question.Name;
                    slider.QuestionUniqueID = question.Question.Id;
                    slider.From = question.Question.From;
                    slider.Step = Convert.ToInt32(question.Question.Step);
                    slider.To = question.Question.To;
                    slider.PlaceHolder = "Your Rate...";
                    slider.RenderQuestion();
                    questionControl = slider;
                    break;
                case QuestionTypes.Date:
                    var date = new DateQuestion();
                    date.QuestionType = QuestionTypes.Date;
                    date.QuestionText = question.Question.Name;
                    date.QuestionUniqueID = question.Question.Id;
                    date.PlaceHolder = "Date ...";
                    date.RenderQuestion();
                    questionControl = date;
                    break;
                case QuestionTypes.Rate:
                    var rate = new RatingQuestion();
                    rate.QuestionType = QuestionTypes.Rate;
                    rate.QuestionText = question.Question.Name;
                    rate.QuestionUniqueID = question.Question.Id;
                    rate.From = question.Question.From;
                    rate.To = question.Question.To;
                    rate.Step = Convert.ToInt32(question.Question.Step);
                    rate.PlaceHolder = "Your Rate...";
                    rate.RenderQuestion();
                    questionControl = rate;
                    break;
                default:
                    break;
            }


            return questionControl;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
