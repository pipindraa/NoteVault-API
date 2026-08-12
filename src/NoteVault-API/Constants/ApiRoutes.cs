namespace NoteVault_API.Constants
{
    internal static class ApiRoutes
    {
        internal static class Notes
        {
            internal const string RoutePrefix = "api/v{version:apiVersion}/notes";
            internal const string IdRoute = "{id:guid}";
        }

        internal static class Auth
        {
            internal const string RoutePrefix = "api/v{version:apiVersion}/auth";
            internal const string Register = "register";
            internal const string Login = "login";
        }
    }
}
