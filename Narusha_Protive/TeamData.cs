using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narusha_Protive
{
    class TeamData
    {
        public static long id;
        public static string code; //Team Code
        public static string[] files;
        public static string name; //Team Name
        public static List<Notice> notice;
        public static List<Notice> AddNotices;
        public static List<ToDoList> toDoList;
        public static bool Updated = false;
    }
}
