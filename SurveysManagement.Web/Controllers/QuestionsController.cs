using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SurveysManagement.DataAccess.Entities;
using SurveysManagement.Web.Models;
using static SurveysManagement.Web.Enums.Enums;

namespace SurveysManagement.Web.Controllers
{
    public class QuestionController : BaseController
    {
        private SurveyModel db = new SurveyModel();


        // GET: Questions
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            //var questions = null;

            //return View(questions.ToList());
            return View();
        }



        [Authorize(Roles = "Admin")]
        public ActionResult getquestions()
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
                IQueryable<QuestionViewModel> data = db.Questions.Where(x => x.isDeleted != true).Select(x => new QuestionViewModel {
                    Id = x.Id,
                    From = x.From,
                    To = x.To,
                    Step = x.Step,
                    Question = x.Name,
                    hasOtherOption = x.hasOtherOption,
                    Text = x.Text,
                    Type = x.QuestionType.TypeTextEn,
                    TypeAr = x.QuestionType.TypeTextAr
                }).AsQueryable();

                // Total record count.
                int totalRecords = data.Count();

                // Verification.
                List<QuestionViewModel> resultList;

                if (!string.IsNullOrEmpty(search) &&
                    !string.IsNullOrWhiteSpace(search))
                {
                    // Apply search
                    data = data.Where(p => p.Text.ToString().ToLower().Contains(search.ToLower()) ||
                                           p.Text.ToString().ToLower().Contains(search.ToLower()));
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

        // GET: Questions/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                QuestionViewModel quesViewModel = new QuestionViewModel();
                quesViewModel.QuestionTypes = db.QuestionTypes.Select(x => new HelperViewModel {Id = x.Id, Text = x.TypeTextEn }).ToList();
                return View(quesViewModel);
            }
            else
            {
                int QuestionId;
                if (int.TryParse(id.ToString(), out QuestionId))
                {
                    Question ques = db.Questions.Include(x => x.Answers).FirstOrDefault(x => x.Id == id);
                    if (ques == null)
                    {
                        return HttpNotFound();
                    }
                    else
                    {
                        QuestionViewModel quesViewModel = new QuestionViewModel
                        {
                            Id = ques.Id,
                            From = ques.From,
                            To = ques.To,
                            Step = ques.Step,
                            hasOtherOption = ques.hasOtherOption,
                            Question = ques.Name,
                            Text = string.Join("#^#", ques.Answers.Where(x => x.isOther != true && x.isDeleted != true).Select(x => x.Text)),
                            Type = ques.TypeId.ToString(),
                        };

                        quesViewModel.Answers = ques.Answers.Where(x => x.isOther != true && x.isDeleted != true).Select(x => new HelperViewModel {Id = x.Id, Text = x.Text}).ToList();
                        quesViewModel.QuestionTypes = db.QuestionTypes.Select(x => new HelperViewModel { Id = x.Id, Text = x.TypeTextEn }).ToList();
                        return View(quesViewModel);
                    }
                }
            }
            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
        }

        private void AddOtherOption(int questionId, QuestionViewModel question)
        {
            var quesAnswers = db.Answers.Where(x => x.QuestionId == questionId);
            if (question.hasOtherOption.HasValue && question.hasOtherOption.Value && quesAnswers.Where(x => x.isOther == true).Count() == 0)
            {
                Answer answer = new Answer()
                {
                    QuestionId = questionId,
                    Text = "Other",
                    isOther = true

                };
                db.Answers.Add(answer);
                db.SaveChanges();
            }
            else if (question.hasOtherOption.HasValue && question.hasOtherOption.Value && quesAnswers.Where(x => x.isOther == true).Count() > 0)
            {
                db.Answers.FirstOrDefault(x => x.QuestionId == questionId && x.isOther == true).isDeleted = false;
                db.SaveChanges();
            }
            else if (question.hasOtherOption.HasValue && !question.hasOtherOption.Value && quesAnswers.Where(x => x.isOther == true).Count() > 0)
            {
                db.Answers.FirstOrDefault(x => x.QuestionId == questionId && x.isOther == true).isDeleted = true;
                db.SaveChanges();
            }
        }

        // POST: Questions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(QuestionViewModel model)
        {
            if (model.Id == null)
            {
                Question ques = new Question {
                    //Id = question.Id.Value,
                    From = model.From,
                    hasOtherOption = model.hasOtherOptionval == "true",
                    To = model.To,
                    Step = model.Step,
                    Name = model.Question,
                    //Text = question.Text,
                    TypeId = Convert.ToInt32(model.Type)
                    
                };

                if (Convert.ToInt32(model.Type) == (int)QuestionTypes.Rate)
                {
                    ques.From = 1;
                    ques.To = 5;
                    ques.Step = 1;
                }

                model.hasOtherOption = model.hasOtherOptionval == "true";
                db.Questions.Add(ques);
                db.SaveChanges();

                if (Convert.ToInt32(model.Type) == (int)QuestionTypes.Checkbox){
                    string[] strAllAnswers = model.Text.Split(new string[] { "#^#" }, StringSplitOptions.None);
                    foreach (string str in strAllAnswers)
                    {
                        Answer answer = new Answer()
                        {
                            QuestionId = ques.Id,
                            Text = str
                        };
                        db.Answers.Add(answer);
                    }
                }
                else if(Convert.ToInt32(model.Type) == (int)QuestionTypes.Multiplechoice)
                {
                    string[] strAllAnswers = model.Text.Split(new string[] { "#^#" }, StringSplitOptions.None);
                    foreach (string str in strAllAnswers)
                    {
                        Answer answer = new Answer()
                        {
                            QuestionId = ques.Id,
                            Text = str
                        };
                        db.Answers.Add(answer);
                    }
                }
                //else if (question.TypeId == (int)QuestionTypes.Text)
                //{

                //}
                //else if (question.TypeId == (int)QuestionTypes.Percentage)
                //{

                //}
                //else if (question.TypeId == (int)QuestionTypes.Range)
                //{

                //}
                //else if (question.TypeId == (int)QuestionTypes.Number)
                //{

                //}
                else if (Convert.ToInt32(model.Type) == (int)QuestionTypes.Dropdown)
                {
                    string[] strAllAnswers = model.Text.Split(new string[] { "#^#" }, StringSplitOptions.None);
                    foreach (string str in strAllAnswers)
                    {
                        Answer answer = new Answer()
                        {
                            QuestionId = ques.Id,
                            Text = str
                        };
                        db.Answers.Add(answer);
                    }
                }
                //else if (question.TypeId == (int)QuestionTypes.Slider)
                //{
                //    Answer answer = new Answer()
                //    {
                //        QuestionId = ques.Id,
                //        from = question.From
                //    };
                //    db.Answers.Add(answer);
                //    answer = new Answer()
                //    {
                //        QuestionId = ques.Id,
                //        Text = question.Text
                //    };
                //    db.Answers.Add(answer);
                //}
                //else if (question.TypeId == (int)QuestionTypes.Date)
                //{

                //}
                //else if (Convert.ToInt32(model.Type) == (int)QuestionTypes.Rate)
                //{
                //    From = 1,                    
                //    To = 5,
                //    Step =1,
                //}
                db.SaveChanges();
                AddOtherOption(ques.Id, model);
                return RedirectToAction("Index", "Question");
            }
            else
            {
                model.hasOtherOption = model.hasOtherOptionval == "true";
                Question ques = db.Questions.Find(model.Id);

                //ques.Id = question.Id;
                ques.From = model.From;
                ques.To = model.To;
                ques.Step = model.Step;
                ques.Name = model.Question;
                ques.hasOtherOption = model.hasOtherOption;
                //ques.Text = question.Text;
                ques.TypeId = Convert.ToInt32(model.Type);                
                
                //db.Questions.ed(ques);
                db.SaveChanges();

                IEnumerable<Answer> lstAnswers = db.Answers.Where(x => x.QuestionId == model.Id);
                foreach (var answer in lstAnswers)
                    db.Answers.Remove(answer);
                db.SaveChanges();

                if (Convert.ToInt32(model.Type) == (int)QuestionTypes.Checkbox)
                {
                    if (!string.IsNullOrEmpty(model.Text))
                    {
                    string[] strAllAnswers = model.Text.Split(new string[] { "#^#" }, StringSplitOptions.None);
                    foreach (string str in strAllAnswers)
                    {
                        Answer answer = new Answer()
                        {
                            QuestionId = ques.Id,
                            Text = str
                        };
                            db.Answers.Add(answer);
                        }
                    }
                    
                }
                else if (Convert.ToInt32(model.Type) == (int)QuestionTypes.Multiplechoice)
                {
                    if(!string.IsNullOrEmpty(model.Text))
                    {
                    string[] strAllAnswers = model.Text.Split(new string[] { "#^#" }, StringSplitOptions.None);
                    foreach (string str in strAllAnswers)
                    {
                        Answer answer = new Answer()
                        {
                            QuestionId = ques.Id,
                            Text = str
                        };
                            db.Answers.Add(answer);
                        }
                    }
                    
                }
                //else if (question.TypeId == (int)QuestionTypes.Text)
                //{

                //}
                //else if (question.TypeId == (int)QuestionTypes.Percentage)
                //{

                //}
                //else if (question.TypeId == (int)QuestionTypes.Range)
                //{

                //}
                //else if (question.TypeId == (int)QuestionTypes.Number)
                //{

                //}
                else if (Convert.ToInt32(model.Type) == (int)QuestionTypes.Dropdown)
                {
                    if (!string.IsNullOrEmpty(model.Text))
                    {
                    string[] strAllAnswers = model.Text.Split(new string[] { "#^#" }, StringSplitOptions.None);
                    foreach (string str in strAllAnswers)
                    {
                        Answer answer = new Answer()
                        {
                            QuestionId = ques.Id,
                            Text = str
                        };
                            db.Answers.Add(answer);
                        }
                    }
                    
                }
                //else if (question.TypeId == (int)QuestionTypes.Slider)
                //{
                //    Answer answer = new Answer()
                //    {
                //        QuestionId = ques.Id,
                //        from = question.From
                //    };
                //    db.Answers.Add(answer);
                //    answer = new Answer()
                //    {
                //        QuestionId = ques.Id,
                //        Text = question.Text
                //    };
                //    db.Answers.Add(answer);
                //}
                //else if (question.TypeId == (int)QuestionTypes.Date)
                //{

                //}
                //else if (question.TypeId == (int)QuestionTypes.Rate)
                //{

                //}
                db.SaveChanges();
                AddOtherOption(ques.Id, model);
                return RedirectToAction("Index", "Question");
            }
        }

        // GET: Questions/Delete/5
        //[Authorize(Roles = "Admin")]
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Question question = db.Questions.Find(id);
        //    if (question == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(question);
        //}

        // POST: Questions/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Question question = db.Questions.Find(id);
        //    question.isDeleted = true;
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question Question = db.Questions.Find(id);
            if (Question == null)
            {
                return HttpNotFound();
            }
            Question.isDeleted = true;
            db.SaveChanges();
            return RedirectToAction("Index");
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
