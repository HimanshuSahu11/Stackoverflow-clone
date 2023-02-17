﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StackOverflowProject.ViewModels;
using StackOverflowProject.ServiceLayer;

namespace StackOverflowProject.Controllers
{
    public class QuestionsController : Controller
    {
        IQuestionsService qs;
        IAnswersService asr;
        ICategoriesService cs;

        public QuestionsController(IQuestionsService qs,IAnswersService asr,ICategoriesService cs)
        {
            this.qs = qs;
            this.asr = asr;
            this.cs = cs;
        }

        public ActionResult View(int id)
        {
            this.qs.UpdateQuestionViewsCount(id, 1);
            int uid = Convert.ToInt32(Session["CurrentUserID"]);
            QuestionViewModel qvm = this.qs.GetQuestionByQuestionID(id, uid);

            return View(qvm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddAnswer(NewAnswerViewModel navm)
        {
            
            navm.UserID = Convert.ToInt32(Session["CurrentUserID"]);
            navm.AnswerDateAndTime = DateTime.Now;
            navm.VotesCount = 0;
            if (ModelState.IsValid)
            {
                this.asr.InsertAnswer(navm);
                return RedirectToAction("View", "Questions", new { id = navm.QuestionID });

            }
            else
            {
                ModelState.AddModelError("x", "Invalid Data");
                QuestionViewModel qvm = this.qs.GetQuestionByQuestionID(navm.QuestionID, navm.UserID);
                return View("View", qvm);
            }
        }
    }
}