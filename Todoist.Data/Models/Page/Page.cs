using Todoist.Data.Models.Base;

namespace Todoist.Data.Models.Page
{
    public class Page<TModel>
        where TModel : class, IBaseModel
    {
        public PageMetadata PageMetadata { get; set; } = default!;
        public IEnumerable<TModel> Content { get; set; } = default!;
    }
}
