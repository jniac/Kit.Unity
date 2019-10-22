using System;
using System.Runtime.CompilerServices;

namespace Kit
{
    public partial class Anim
    {
        /*
         * thanks to:
         * https://www.markopapic.com/csharp-under-the-hood-async-await/
         *         
         * Usage: 
         * await Anim.During(duration, anim => { ... }).Completion            
         * 
         */

        public class AwaitableCompletion
        {
            readonly Anim anim;
            public AwaitableCompletion(Anim anim) => this.anim = anim;
            public CompletionAwaiter GetAwaiter() => new CompletionAwaiter(anim);
        }

        public class CompletionAwaiter : INotifyCompletion
        {
            readonly Anim anim;
            public CompletionAwaiter(Anim anim) => this.anim = anim;

            public void GetResult() { }

            public bool IsCompleted { get => anim.Complete; }

            public void OnCompleted(Action continuation) =>
                anim._onComplete.Add(continuation);
        }
    }
}
