namespace NoteVault_API.Constants
{
    internal static class ApiRoutes
    {
        internal static class Notes
        {
            internal const string RoutePrefix = "api/v{version:apiVersion}/notes";
            internal const string IdRoute = "{id:guid}";
        }

        internal static class Tags
        {
            internal const string RoutePrefix = "api/v{version:apiVersion}/tags";
            internal const string IdRoute = "{id:guid}";
        }
    }
}
