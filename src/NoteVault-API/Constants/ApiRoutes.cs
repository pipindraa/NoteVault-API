namespace NoteVault_API.Constants
{
    public static class ApiRoutes
    {
        public static class Notes
        {
            public const string RoutePrefix = "api/v{version:apiVersion}/notes";
            public const string IdRoute = "{id:guid}";
        }
    }
}
