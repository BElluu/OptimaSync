namespace OptimaSync.Constant
{
    public static class Messages
    {
        static string SOA_SERVICE = Service.WindowsService.SOA_SERVICE;
        // MessageBox Titles
        public static readonly string ERROR_TITLE = "Bład!";
        public static readonly string WARNING_TITLE = "Ostrzeżenie!";
        public static readonly string INFORMATION_TITLE = "Informacja";
        // MessageBox Messages
        public static readonly string SOA_PATH_CANNOT_BE_EMPTY = "Chcesz wykorzystać SOA. Ścieżka instalacyjna Optimy musi być uzupełniona!";
        public static readonly string DEST_PATH_CANNOT_BE_EMPTY = "Ścieżka docelowa nie może być pusta!";
        public static readonly string YOU_HAVE_LATEST_BUILD = "Posiadasz najnowszą kompilację!";
        public static readonly string SOA_SERVICE_DONT_EXIST = "Na stanowisku nie ma " + SOA_SERVICE + ". Zarejestrowanie Optimy z usługą SOA nie jest możliwe";
        public static readonly string SOA_SERVICE_NOT_STOPPED = "Nie udało się zatrzymać usługi SOA lub jej stan nie jest poprawny. Spróbuj zatrzymać usługę SOA ręcznie.";
        public static readonly string REGISTER_OPTIMA_ERROR = "Rejestracja Optimy nie powiodła się. Zerknij w logi lub spróbuj zarejestrować ją ręcznie.";
        public static readonly string LOGS_DIRECTORY_NOT_EXIST = "Folder z logami nie istnieje!";
        public static readonly string ACCESS_TO_HOST_ERROR = "Brak dostępu do natalie! Sprawdź czy masz internet lub połączenie VPN.";

        // ProgressLabel and Notification Messages

        public static readonly string ERROR_CHECK_LOGS = "Błąd! Sprawdź logi.";
        public static readonly string REGISTER_OPTIMA_SUCCESSFUL = "Kompilacja gotowa do pracy :)";
        public static readonly string REGISTER_OPTIMA_INPROGRESS = "Rejestrowanie kompilacji...";
        public static readonly string SEARCHING_FOR_BUILD = "Wyszukiwanie kompilacji.";
        public static readonly string DOWNLOADING_BUILD = "Pobieranie kompilacji";
        public static readonly string OSA_READY_TO_WORK = "OSA gotowa do pracy :)";
        public static readonly string STOPPING_SOA_SERVICE = "Zatrzymywanie usługi SOA.";
        public static readonly string OSA_WORKING_IN_BACKGROUND = "OptimaSync działa w tle";

        // Other Messages
        public static readonly string SETTINGS_SAVED = "Zapisano ustawienia.";
        public static readonly string OPTIMA_REGISTERED = "Optima została zarejestrowana.";
        public static readonly string OPTIMA_SYNC_STARTED = "Aplikacja została uruchomiona";
        public static readonly string SOA_SERVICE_IS_STOPPED = "Usługa SOA jest zatrzymana";
        public static readonly string SOA_SERVICE_UNKNOWN_STATUS = "Stan usługi SOA jest nieznany!";
    }
}
