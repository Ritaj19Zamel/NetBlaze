namespace NetBlaze.SharedKernel.HelperUtilities.General
{
    public static class AuthorizationPolicies
    {
        public const string SuperAdmin = nameof(SuperAdmin);
        public const string HRAndManager = nameof(HRAndManager);
        public const string Employee = nameof(Employee);
        public const string AnyAuthenticated = nameof(AnyAuthenticated);
    }
}
