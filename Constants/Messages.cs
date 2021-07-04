using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimaSync.Constants
{
    public static class Messages
    {
        // MessageBox Titles
        public static readonly string BUILD_PATH_CANNOT_BE_EMPTY_TITLE = "[ERROR 0001]";
        public static readonly string SOA_PATH_CANNOT_BE_EMPTY_TITLE = "[ERROR 0002]";
        // MessageBox Messages
        public static readonly string BUILD_PATH_CANNOT_BE_EMPTY = "Ścieżka kompilacji nie może być pusta!";
        public static readonly string SOA_PATH_CANNOT_BE_EMPTY = "Chcesz wykorzystać SOA. Ścieżka instalacyjna Optimy musi być uzupełniona!";
    }
}
