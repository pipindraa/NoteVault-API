namespace NoteVault_API.Extensions.DI
{
    public static class EndpointExtensions
    {
        public static WebApplication MapApplicationEndpoints(this WebApplication app)
        {
            app.MapControllers();

            return app;
        }
    }
}
