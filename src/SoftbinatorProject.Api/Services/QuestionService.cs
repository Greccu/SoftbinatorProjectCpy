using Microsoft.EntityFrameworkCore;
using SoftbinatorProject.Core.Constants;
using SoftbinatorProject.Core.Models;
using SoftbinatorProject.Infrastructure.Data;
using SoftbinatorProject.Infrastructure.Data.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftbinatorProject.Api.Services
{
    public interface IQuestionService {
        public QuestionWithAnswers GetQuestion(int id);
        public List<QuestionWithAnswers> GetQuestions(int chapterId, string sort, int page);
        public QuestionInfo CreateQuestion(int chapterId, QnAPost question, string creatorId);
        public QuestionInfo UpdateQuestion(int id, QnAPost question, string userId);
        public QuestionInfo DeleteQuestion(int id, string userId);
    }

    public class QuestionService : IQuestionService
    {
        private readonly AppDbContext _context;

        public QuestionService(AppDbContext context)
        {
            _context = context;
        }

        public QuestionWithAnswers GetQuestion(int id)
        {

            Question question = _context.Questions
                .Include(q=>q.Answers)
                .ThenInclude(a=>a.User)
                .Where(que => que.Id == id)
                .FirstOrDefault();
            if(question == null)
            {
                return null;
            }
            return new QuestionWithAnswers(question);
        }


        public List<QuestionWithAnswers> GetQuestions(int chapterId, string sort, int page)
        {
            if(_context.Chapters.Find(chapterId) == null)
            {
                return null;
            }
            var l = _context
                .Questions
                .Include(q => q.Answers)
                .ThenInclude(a => a.User)
                .Where(que => que.ChapterId == chapterId);
                
            switch (sort)
            {
                case "titleA":
                    l = l.OrderBy(c => c.Title);
                    break;
                case "titleD":
                    l = l.OrderByDescending(c => c.Title);
                    break;
                case "dateA":
                    l = l.OrderBy(q => q.CretedAt);
                    break;
                case "dateB":
                    l = l.OrderByDescending(q => q.CretedAt);
                    break;
                default:
                    break;
            }

            var nr = RandomConstants.QuestionsPerPage;
            return l.Skip(page > 0 ? (page - 1) * nr : 0)
                .Take(nr)
                .Select(que => new QuestionWithAnswers(que))
                .ToList();
        }


        public QuestionInfo CreateQuestion(int chapterId, QnAPost question, string creatorId)
        {
            if (_context.Chapters.Find(chapterId) == null)
            {
                return null;
            }
            Question questionToAdd = new Question
            {
                Title = question.Title,
                Content = question.Content,
                Anonymous = question.Anonymous,
                ChapterId = chapterId,
                CretedAt = DateTime.Now,
                UserId = creatorId
            };
            _context.Questions.Add(questionToAdd);
            _context.SaveChanges();
            return new QuestionInfo(questionToAdd);
        }
        public QuestionInfo UpdateQuestion(int id, QnAPost question, string userId)
        {
            Question questionToUpdate = _context.Questions.Find(id);
            if(questionToUpdate == null || questionToUpdate.UserId != userId)
            {
                return null;
            }
            questionToUpdate.Title = question.Title;
            questionToUpdate.Content = question.Content;
            questionToUpdate.Anonymous = question.Anonymous;
            _context.SaveChanges();
            return new QuestionInfo(questionToUpdate);
        }
        public QuestionInfo DeleteQuestion(int id, string userId)
        {
            Question questionToDelete = _context.Questions.Find(id);
            if (questionToDelete == null || (questionToDelete.UserId != userId && userId != "Moderator"))
            {
                return null;
            }
            _context.Questions.Remove(questionToDelete);
            _context.SaveChanges();
            return new QuestionInfo(questionToDelete);
        }

    }
}
