namespace NetBlaze.SharedKernel.HelperUtilities.Constants
{
    public static class ApiRelativePaths
    {
        // Common Api Path
        public const string API_COMMON_PREFIX = "/api";

        // =========================
        // Sample Paths ✅
        // =========================
        public const string SAMPLE_BASE = $"{API_COMMON_PREFIX}/sample";
        public const string SAMPLE_LIST = $"{SAMPLE_BASE}/list";
        public const string SAMPLE_GET = SAMPLE_BASE;                  // GET ?id=
        public const string SAMPLE_ADD = $"{SAMPLE_BASE}/add";
        public const string SAMPLE_UPDATE = $"{SAMPLE_BASE}/update";
        public const string SAMPLE_DELETE = $"{SAMPLE_BASE}/delete";

        // =========================
        // Attendance Paths ✅
        // =========================
        public const string ATTENDANCE_BASE = $"{API_COMMON_PREFIX}/attendance";
        public const string ATTENDANCE_ADD = $"{ATTENDANCE_BASE}/attend";
        public const string ATTENDANCE_PROCESS_DAILY = ""; // ❌ not implemented in controller

        // =========================
        // Auth Paths ✅
        // =========================
        public const string AUTH_BASE = $"{API_COMMON_PREFIX}/auth";
        public const string AUTH_REGISTER = $"{AUTH_BASE}/register";
        public const string AUTH_LOGIN = $"{AUTH_BASE}/login";
        public const string AUTH_FORGET_PASSWORD = $"{AUTH_BASE}/forgetpassword";
        public const string AUTH_RESET_PASSWORD = $"{AUTH_BASE}/resetpassword";

        // =========================
        // Department Paths ✅
        // =========================
        public const string DEPARTMENT_BASE = "/api/department";
        public const string DEPARTMENT_GET_ALL = $"{DEPARTMENT_BASE}/all";
        public const string DEPARTMENT_GET_BY_ID = DEPARTMENT_BASE;

        // =========================
        // Policy Paths ✅
        // =========================
        public const string POLICY_BASE = $"{API_COMMON_PREFIX}/policy";
        public const string POLICY_CREATE = $"{POLICY_BASE}/createpolicy";
        public const string POLICY_GET_ALL = $"{POLICY_BASE}/getpolicies";
        public const string POLICY_GET_BY_ID = POLICY_BASE;             
        public const string POLICY_UPDATE = POLICY_BASE;               
        public const string POLICY_DELETE = POLICY_BASE;               

        // =========================
        // Random Checks / OTP Paths 
        // =========================
        public const string RANDOM_CHECKS_BASE = $"{API_COMMON_PREFIX}/randomchecks";
        public const string RANDOM_CHECKS_GENERATE_OTP = $"{RANDOM_CHECKS_BASE}/generateotp";
        public const string RANDOM_CHECKS_VERIFY_OTP = $"{RANDOM_CHECKS_BASE}/verifyotp";

        // Role Paths 
        public const string ROLE_BASE = $"{API_COMMON_PREFIX}/role";
        public const string ROLE_GET_ALL = ROLE_BASE;     
        public const string ROLE_GET_BY_ID = ROLE_BASE;

        // =========================
        // User Paths ✅
        // =========================
        public const string USER_BASE = $"{API_COMMON_PREFIX}/user";
        public const string USER_GET_MANAGERS = $"{USER_BASE}/getmanagers";
        public const string USER_UPDATE = $"{USER_BASE}/updateuser";
        public const string USER_ADD = USER_BASE;


        // =========================
        // Vacation Paths ✅
        // =========================
        public const string VACATION_BASE = $"{API_COMMON_PREFIX}/vacation";
        public const string VACATION_CREATE = $"{VACATION_BASE}/createvacation";
        public const string VACATION_GET_ALL = $"{VACATION_BASE}/getvacations";
        public const string VACATION_GET_BY_ID = VACATION_BASE;         // GET /api/vacation/{id}
        public const string VACATION_UPDATE = VACATION_BASE;            // PUT /api/vacation/{id}
        public const string VACATION_DELETE = $"{VACATION_BASE}/deletevacation";
    }
}
