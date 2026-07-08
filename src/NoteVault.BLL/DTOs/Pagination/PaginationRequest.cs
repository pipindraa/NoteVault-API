namespace NoteVault.BLL.DTOs.Pagination
{
    public class PaginationRequest
    {
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 10;

        public PaginationRequest(int pageNumber = 1, int pageSize = 10)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}
