using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narusha_Protive
{
    public class Notice
    {
        public long id;
        public string teamCode;
        public string message;
        public string when;
        public List<long> users;

        public Notice(long id, string teamCode, string message, string when, List<long> users)
        {
            this.id = id;
            this.teamCode = teamCode;
            this.message = message;
            this.when = when;
            this.users = users;
        }
    }
}
