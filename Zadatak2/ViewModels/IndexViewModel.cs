using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zadatak2.ViewModels
{
    public class IndexViewModel
    {

        public List<TodoViewModel> Items { get; set; }

        public IndexViewModel()
        {
            Items = new List<TodoViewModel>();
        }
    }
}
