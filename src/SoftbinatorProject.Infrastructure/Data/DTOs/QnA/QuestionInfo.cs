using SoftbinatorProject.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftbinatorProject.Infrastructure.Data.DTOs
{
    public class QuestionInfo : QnAPost
    {
        public int Id { get; set; }
        public DateTime CretedAt { get; set; }
        public string UserId { get; set; }
        public int ChapterId { get; set; }


        public QuestionInfo() { }
        public QuestionInfo(Question question) : base(question)
        {
            Id = question.Id;
            CretedAt = question.CretedAt;
            UserId = question.UserId;
            ChapterId = question.ChapterId;
        }
    }
}
