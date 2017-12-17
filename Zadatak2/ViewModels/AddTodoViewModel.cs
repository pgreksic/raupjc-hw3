using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Zadatak2.ViewModels
{
    public class AddTodoViewModel
    {

        [Required]
        public string Text { get; set; }
        public DateTime? DateDue { get; set; }
        public HashSet<string> Labels { get; set; }

        public AddTodoViewModel( string text, DateTime dateDue, string labels)
        {
            Text = text;
            DateDue = dateDue;
            foreach(string tmp in labels.Split(':').ToList())
            {
                Labels.Add(tmp);
            }
        }

        public AddTodoViewModel()
        {

        }

    }

}
