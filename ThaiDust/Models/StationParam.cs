using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace ThaiDust.Models
{
    public class StationParam : ReactiveObject
    {
        [Reactive] public string Param { get; set; }
        [Reactive] public string Name { get; set; }
    }
}