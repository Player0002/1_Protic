using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narusha_Protive
{
    class ToDoList
    {
        public long id;
        public string teamCode;
        public string message;
        public string when;
        public bool completed;

        public ToDoList(long id, string teamCode, string message, string when, bool completed)
        {
            this.id = id;
            this.teamCode = teamCode;
            this.message = message;
            this.when = when;
            this.completed = completed;
        }
    }
}
