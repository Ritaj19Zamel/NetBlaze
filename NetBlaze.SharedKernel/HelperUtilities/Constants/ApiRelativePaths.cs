namespace NetBlaze.SharedKernel.HelperUtilities.Constants
{
    public static class ApiRelativePaths
    {
        // Common Api Path
        public const string API_COMMON_PREFIX = "/api";

        // Sample Paths
        public const string SAMPLE_BASE = $"{API_COMMON_PREFIX}/sample";
        public const string SAMPLE_LIST = $"{SAMPLE_BASE}/list";
        public const string SAMPLE_GET = SAMPLE_BASE;
        public const string SAMPLE_ADD = $"{SAMPLE_BASE}/add";
        public const string SAMPLE_UPDATE = $"{SAMPLE_BASE}/update";
        public const string SAMPLE_DELETE = $"{SAMPLE_BASE}/delete";

        // Department Paths
        public const string DEPARTMENT_BASE = $"{API_COMMON_PREFIX}/department";
        public const string DEPARTMENT_GET_ALL = $"{DEPARTMENT_BASE}/GetAllDepartments";
        public const string DEPARTMENT_GET_BY_ID = $"{DEPARTMENT_BASE}/GetDepartmentById";
        public const string DEPARTMENT_CREATE = $"{DEPARTMENT_BASE}/CreateDepartment";
        public const string DEPARTMENT_UPDATE = $"{DEPARTMENT_BASE}/UpdateDepartment";
        public const string DEPARTMENT_DELETE = $"{DEPARTMENT_BASE}/DeleteDepartment";
        public const string DEPARTMENT_TOGGLE_STATUS = $"{DEPARTMENT_BASE}/ToggleDepartmentStatus";

        //User Paths
        public const string USER_BASE = $"{API_COMMON_PREFIX}/user";
        public const string USER_GET_MANAGERS = $"{USER_BASE}/GetManagers";
        public const string USER_GET_EMPLOYEES = $"{USER_BASE}/GetEmployees";
        public const string USER_UPDATE = $"{USER_BASE}/UpdateUser";
        public const string USER_GET_ALL = $"{USER_BASE}/GetAllUsers";
        public const string USER_DELETE = $"{USER_BASE}/DeleteUser";
        public const string USER_EDIT_PROFILE = $"{USER_BASE}/EditUserProfile";
        public const string USER_GET_CURRENT_PROFILE = $"{USER_BASE}/GetCurrentUserProfile";

        //Authentication Paths
        public const string AUTH_BASE = $"{API_COMMON_PREFIX}/auth";
        public const string AUTH_LOGIN = $"{AUTH_BASE}/Login";
        public const string AUTH_REGISTER = $"{AUTH_BASE}/Register";
        public const string AUTH_FORGET_PASSWORD = $"{AUTH_BASE}/ForgetPassword";
        public const string AUTH_RESET_PASSWORD = $"{AUTH_BASE}/ResetPassword";

        //Fideo Paths
        public const string FIDO_BASE = $"{API_COMMON_PREFIX}/fido";
        public const string FIDO_REGISTER_OPTIONS = $"{FIDO_BASE}/FidoRegisterOptions";
        public const string FIDO_REGISTER_COMPLETE = $"{FIDO_BASE}/FidoRegisterComplete";
        public const string FIDO_LOGIN_OPTIONS = $"{FIDO_BASE}/FidoLoginOptions";
        public const string FIDO_LOGIN_COMPLETE = $"{FIDO_BASE}/FidoLoginComplete";

        //Role Paths
        public const string ROLE_BASE = $"{API_COMMON_PREFIX}/role";
        public const string ROLE_GET_ALL = $"{ROLE_BASE}/GetAllRoles";
        public const string ROLE_GET_BY_ID = $"{ROLE_BASE}/GetRoleById";

        //Vacation Paths
        public const string VACATION_BASE = $"{API_COMMON_PREFIX}/vacation";
        public const string VACATION_GET_ALL = $"{VACATION_BASE}/GetVacations";
        public const string VACATION_GET_BY_ID = $"{VACATION_BASE}/GetVacationById";
        public const string VACATION_CREATE = $"{VACATION_BASE}/CreateVacation";
        public const string VACATION_UPDATE = $"{VACATION_BASE}/UpdateVacation";
        public const string VACATION_DELETE = $"{VACATION_BASE}/DeleteVacation";

        //Policy Paths
        public const string POLICY_BASE = $"{API_COMMON_PREFIX}/policy";
        public const string POLICY_GET_ALL = $"{POLICY_BASE}/GetPolicies";
        public const string POLICY_GET_BY_ID = $"{POLICY_BASE}/GetPolicyById";
        public const string POLICY_CREATE = $"{POLICY_BASE}/CreatePolicy";
        public const string POLICY_UPDATE = $"{POLICY_BASE}/UpdatePolicy";
        public const string POLICY_DELETE = $"{POLICY_BASE}/DeletePolicy";

        //Attendance Paths
        public const string ATTENDANCE_BASE = $"{API_COMMON_PREFIX}/attendance";
        public const string ATTENDANCE_ADD = $"{ATTENDANCE_BASE}/Attend";
        public const string ATTENDANCE_APPROVE_POLICY = $"{ATTENDANCE_BASE}/ApprovePolicy";
        public const string ATTENDANCE_GET_REPORT = $"{ATTENDANCE_BASE}/GetAttendanceReport";
        public const string ATTENDANCE_GET_CHECK_VIOLATIONS = $"{ATTENDANCE_BASE}/GetCheckViolations";
        public const string ATTENDANCE_GET_TODAY_ATTENDANCE = $"{ATTENDANCE_BASE}/GetTodayAttendance";

        //RandomChecks Paths
        public const string RANDOMCHECKS_BASE = $"{API_COMMON_PREFIX}/RandomChecks";
        public const string RANDOMCHECK_GET_USER_CHECKS = $"{RANDOMCHECKS_BASE}/GetUserChecks";
        public const string RANDOMCHECK_GENERATE_OTP = $"{RANDOMCHECKS_BASE}/GenerateOTP";
        public const string RANDOMCHECK_VERIFY_OTP = $"{RANDOMCHECKS_BASE}/VerifyOTP";
        public const string RANDOMCHECK_SAVE_AUTO_RANDOMCHECK_CONFIG = $"{RANDOMCHECKS_BASE}/SaveAutoRandomCheckConfig";

    }
}