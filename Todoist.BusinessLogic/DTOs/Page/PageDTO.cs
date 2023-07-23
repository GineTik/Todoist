using Todoist.Data.Models.Page;

namespace Todoist.BusinessLogic.DTOs.Page
{
    public class PageDTO<TContent>
    {
        public PageMetadata PageMetadata { get; set; } = default!;
        public IEnumerable<TContent> Content { get; set; } = default!;
    }
}
