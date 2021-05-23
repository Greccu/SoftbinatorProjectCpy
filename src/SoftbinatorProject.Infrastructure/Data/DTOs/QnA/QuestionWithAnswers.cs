using SoftbinatorProject.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftbinatorProject.Infrastructure.Data.DTOs
{
    public class QuestionWithAnswers : QuestionInfo
    {
        public List<AnswerInfo> Answers { get; set; }

        public QuestionWithAnswers() { }
        public QuestionWithAnswers(Question question) : base(question)
        {
            Answers = question.Answers.Select(ans => new AnswerInfo(ans)).ToList();
        }
    }
}
