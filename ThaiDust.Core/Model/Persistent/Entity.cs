using ReactiveUI;

namespace ThaiDust.Core.Model.Persistent
{
    public abstract class Entity : ReactiveObject
    {
        public int Id { get; set; }
    }
}
