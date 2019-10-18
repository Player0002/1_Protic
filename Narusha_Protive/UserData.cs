using Narusha_Protive.CustomControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using WebSocketSharp;

namespace Narusha_Protive
{
    class UserData
    {
        public static WebSocket socket;

        public static bool memoUpdated;
        public static bool fileUpdated;
        public static bool noticeUpdated;

        public static long id;
        public static string username;
        public static string email;
        public static string teamCode;
        public static string password;
        public static int[] memos;
        public static int updateMemos;
        public static Dashboard board;


        public static bool isStop = false;
        public static bool isEnter = false;
        public static int cnt = 0;
        public static bool isOutsideClick = false;
        public static Frame last = null;
        public static Frame showd = null;
        public static MemoUI clickedUI = null;

        public static string[] userMemos;
    }
}
