using SoftbinatorProject.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftbinatorProject.Infrastructure.Data.DTOs
{
    public class QnAPost
    {
        [Required, MinLength(2), MaxLength(100)]
        public string Title { get; set; }
        [Required, MinLength(2)]
        public string Content { get; set; }
        public bool Anonymous { get; set; } = false;

        public QnAPost() { }
        public QnAPost(Question question)
        {
            Title = question.Title;
            Content = question.Content;
            Anonymous = question.Anonymous;
        }
        public QnAPost(Answer answer)
        {
            Title = answer.Title;
            Content = answer.Content;
            Anonymous = answer.Anonymous;
        }
    }
}
