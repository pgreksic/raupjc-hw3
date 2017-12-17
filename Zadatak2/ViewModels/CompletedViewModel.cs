using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zadatak2.ViewModels
{
    public class CompletedViewModel
    {

        public List<TodoViewModel> Completed { get; set; }

        public CompletedViewModel()
        {
            Completed = new List<TodoViewModel>();
        }

    }
}
