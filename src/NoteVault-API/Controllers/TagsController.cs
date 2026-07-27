using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using NoteVault.BLL.DTOs.Tags;
using NoteVault.BLL.Interfaces;
using NoteVault_API.Constants;
using NoteVault_API.Extensions.Results;

namespace NoteVault_API.Controllers
{
    [ApiController]
    [ApiVersion(ApiVersions.V1)]
    [Route(ApiRoutes.Tags.RoutePrefix)]
    public class TagsController : ControllerBase
    {
        private readonly ITagService _tagService;

        public TagsController(ITagService tagService)
        {
            _tagService = tagService;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyCollection<TagDto>>> GetAll(CancellationToken cancellationToken)
        {
            var result = await _tagService.GetAllAsync(cancellationToken);
            return result.ToActionResult();
        }

        [HttpPost]
        public async Task<ActionResult<TagDto>> Create([FromBody] TagCreateDto request, CancellationToken cancellationToken)
        {
            var result = await _tagService.CreateAsync(request, cancellationToken);
            return result.ToActionResult();
        }

        [HttpDelete(ApiRoutes.Tags.IdRoute)]
        public async Task<ActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var result = await _tagService.DeleteAsync(id, cancellationToken);
            return result.ToActionResult();
        }
    }
}