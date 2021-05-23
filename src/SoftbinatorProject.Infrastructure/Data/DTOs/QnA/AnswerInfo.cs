using SoftbinatorProject.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftbinatorProject.Infrastructure.Data.DTOs
{
    public class AnswerInfo : QnAPost
    {
        public int Id { get; set; }
        public DateTime CretedAt { get; set; }
        public string UserId { get; set; }
        public int QuestionId { get; set; }

        public string UserName { get; set; }
        public AnswerInfo() { }
        public AnswerInfo(Answer answer) : base(answer)
        {
            Id = answer.Id;
            CretedAt = answer.CretedAt;
            UserId = answer.UserId;
            QuestionId = answer.QuestionId;
            if(answer.User != null)
            {
                UserName = answer.User.Username;
            }
        }
    }
}
