using Microsoft.EntityFrameworkCore;
using SoftbinatorProject.Core.Models;
using SoftbinatorProject.Infrastructure.Data;
using SoftbinatorProject.Infrastructure.Data.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftbinatorProject.Api.Services
{
    public interface IAnswerService
    {
        public AnswerInfo GetAnswer(int id);
        public List<AnswerInfo> GetAnswers(int questionId);
        public AnswerInfo CreateAnswer(int questionId, QnAPost answer, string creatorId);
        public AnswerInfo UpdateAnswer(int id, QnAPost answer, string userId);
        public AnswerInfo DeleteAnswer(int id, string userId);
    }

    public class AnswerService : IAnswerService
    {
        private readonly AppDbContext _context;

        public AnswerService(AppDbContext context)
        {
            _context = context;
        }

        public AnswerInfo GetAnswer(int id)
        {
            Answer answer = _context.Answers.Include(a => a.User).Where(a => a.Id == id).FirstOrDefault();
            if(answer == null)
            {
                return null;
            }
            return new AnswerInfo(answer);
        }
        public List<AnswerInfo> GetAnswers(int questionId)
        {
            if(_context.Questions.Find(questionId) == null)
            {
                return null;
            }
            return _context.Answers.Include(a => a.User).Where(ans => ans.QuestionId == questionId).Select(ans => new AnswerInfo(ans)).ToList();
        }
        public AnswerInfo CreateAnswer(int questionId, QnAPost answer, string creatorId)
        {
            if (_context.Questions.Find(questionId) == null)
            {
                return null;
            }
            Answer answerToAdd = new Answer
            {
                Title = answer.Title,
                Content = answer.Content,
                Anonymous = answer.Anonymous,
                QuestionId = questionId,
                CretedAt = DateTime.Now,
                UserId = creatorId
            };
            _context.Answers.Add(answerToAdd);
            _context.SaveChanges();
            return new AnswerInfo(answerToAdd);
        }
        public AnswerInfo UpdateAnswer(int id, QnAPost answer, string userId)
        {
            Answer answerToUpdate = _context.Answers.Include(a => a.User).Where(a => a.Id == id).FirstOrDefault();
            if(answerToUpdate == null || answerToUpdate.UserId != userId)
            {
                return null;
            }
            answerToUpdate.Title = answer.Title;
            answerToUpdate.Content = answer.Content;
            answerToUpdate.Anonymous = answer.Anonymous;
            _context.SaveChanges();
            return new AnswerInfo(answerToUpdate);
        }
        public AnswerInfo DeleteAnswer(int id, string userId)
        {
            Answer answerToDelete = _context.Answers.Find(id);
            if (answerToDelete == null || (answerToDelete.UserId != userId && userId != "Moderator"))
            {
                return null;
            }
            _context.Answers.Remove(answerToDelete);
            _context.SaveChanges();
            return new AnswerInfo(answerToDelete);
        }


    }
}
