﻿namespace OptimaSync.Constants
{
    public static class Messages
    {
        static string SOA_SERVICE = Service.WindowsService.SOA_SERVICE;
        // MessageBox Titles
        public static readonly string BUILD_PATH_CANNOT_BE_EMPTY_TITLE = "[ERROR 0001]";
        public static readonly string SOA_PATH_CANNOT_BE_EMPTY_TITLE = "[ERROR 0002]";
        public static readonly string DEST_PATH_CANNOT_BE_EMPTY_TITLE = "[ERROR 0003]";
        public static readonly string BUILD_PATH_DONT_HAVE_ANY_BUILD_TITLE = "[ERROR 0004]";
        public static readonly string SOA_SERVICE_DONT_EXIST_TITLE = "[ERROR 0005]";
        public static readonly string YOU_HAVE_LATEST_BUILD_TITLE = "[WARNING 0001]";
        // MessageBox Messages
        public static readonly string BUILD_PATH_CANNOT_BE_EMPTY = "Ścieżka kompilacji nie może być pusta!";
        public static readonly string SOA_PATH_CANNOT_BE_EMPTY = "Chcesz wykorzystać SOA. Ścieżka instalacyjna Optimy musi być uzupełniona!";
        public static readonly string DEST_PATH_CANNOT_BE_EMPTY = "Ścieżka docelowa nie może być pusta!";
        public static readonly string BUILD_PATH_DONT_HAVE_ANY_BUILD = "Wskazana ścieżka kompilacji nie zawiera żadnych kompilacji!";
        public static readonly string YOU_HAVE_LATEST_BUILD = "Posiadasz najnowszą kompilację!";
        public static readonly string SOA_SERVICE_DONT_EXIST = "Na stanowisku nie ma " + SOA_SERVICE + ". Zarejestrowanie Optimy z usługą SOA nie jest możliwe";

        // Other Messages
        public static readonly string PATHS_SAVED = "Zapisano ścieżki.";
        public static readonly string OPTIMA_REGISTERED = "Optima została zarejestrowana.";
        public static readonly string OPTIMA_SYNC_STARTED = "Aplikacja została uruchomiona";
    }
}
