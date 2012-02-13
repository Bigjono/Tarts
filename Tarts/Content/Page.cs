using Tarts.Base;

namespace Tarts.Content
{
    public class Page : EntityBase
    {
        public virtual string Name { get; set; }
        public virtual string Content { get; set; }
    }
}
