using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoList.Shared
{
	public class TodoItem
	{
        public string id { get; set; }
        public string Title { get; set; }
        public bool IsDone { get; set; }
        public string ImageUrl { get; set; }
    }
}
