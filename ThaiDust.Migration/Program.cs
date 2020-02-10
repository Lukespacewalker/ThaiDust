using System;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace ThaiDust.Migration
{
    class Program
    {
        static void Main(string[] args)
        {
            var initial = Observable.FromAsync(() => Task.FromResult(11)).Replay().RefCount();
            var sub = initial.Select(r =>
            {
                var a = new int[] {r + 0, r + 1, r + 2, r + 3, r + 4, r + 5};
                return a.ToObservable().Select(r => Observable.Return(r + 1)).Merge(2);
            }).Switch();
            initial.Merge(sub).Subscribe(r => { Console.WriteLine(r); });
        }
    }
}
