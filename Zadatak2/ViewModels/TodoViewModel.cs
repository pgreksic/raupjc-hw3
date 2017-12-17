using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zadatak1.Models;

namespace Zadatak2.ViewModels
{
    public class TodoViewModel
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public String Text { set; get; }

        public DateTime DateCreated { get; set; }
        public DateTime? DateCompleted { get; set; }
        public DateTime? DateDue { get; set; }

        public List<TodoItemLabel> Labels { get; set; }

        public bool IsCompleted => DateCompleted.HasValue;

        public string getDaysUntilDue()
        {

                if (DateDue.HasValue && !IsCompleted)
                {
                    int days = ((DateTime)DateDue - DateTime.Now).Days;
                    if (days >= 0)
                    {
                        return string.Format("(za {0} dana!)", days);
                    }
                    else
                    {
                        return "(Rok je prošao!)";
                    }
                }
                else
                {
                    return "";
                }
        }

        public string getDateString()
        {
            if (!IsCompleted)
            {
                return getDateDueString();
            }

            return getDateCompletedString();
        }

        private string getDateDueString()
        {
            if (DateDue.HasValue)
            {
                return DateDue.ToString();
            }
            return "N/A";
        }

        private string getDateCompletedString()
        {
            if (DateCompleted.HasValue)
            {
                return DateCompleted.ToString();
            }
            return "N/A";
        }

        public TodoViewModel(string text, Guid userId)
        {
            Id = Guid.NewGuid();
            Text = text;
            DateCreated = DateTime.UtcNow;
            UserId = userId;
            Labels = new List<TodoItemLabel>();
        }

        public TodoViewModel(string text)
        {
            // Generates new unique identifier
            Id = Guid.NewGuid();
            // DateTime .Now returns local time , it wont always be what you expect (depending where the server is).
            // We want to use universal (UTC ) time which we can easily convert to local when needed.
            // ( usually done in browser on the client side )
            DateCreated = DateTime.UtcNow;
            Text = text;
        }

        public TodoViewModel()
        {
            // entity framework needs this one
            // not for use :)
        }

        public bool MarkAsCompleted()
        {
            if (!IsCompleted)
            {
                DateCompleted = DateTime.Now;
                return true;
            }
            return false;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != this.GetType())
            {
                return false;
            }

            TodoItem comparisonVar = (TodoItem)obj;
            if (comparisonVar.Id == this.Id)
            {
                return true;
            }
            else
            {
                return false;
            }


        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
