#pragma warning disable RECS0018 // Comparaison des nombres à virgule flottante avec l’opérateur d’égalité

using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace Kit.Unity
{
    public partial class Anim
    {
        static HashSet<Anim> instances = new HashSet<Anim>();
        static DictionarySet<object, Anim> dictionarySet = new DictionarySet<object, Anim>();

        public const string global = "global";

        public readonly object key;

        public float time, duration, delay, timeScale = 1;
        public int frame;
    
        // pre-run is the concept of running once the anim, even if delayed, to allow initialization.
        bool preRun;

        public bool Instantaneous => duration == 0;
        public float Progress => Instantaneous ? 1 : time < 0 ? 0 : time / duration;
        public bool Complete => time == duration;
        public bool FirstFrame => !preRun && frame == 0;
        public bool PreRun => preRun;

        List<Action> onComplete = new List<Action>();

        public bool Destroyed { get; private set; }

        public Action<Anim> callback;

        bool autoKillNullifiedKey;

        public AwaitableCompletion Completion { get => new AwaitableCompletion(this); }

        public Anim(object key, Action<Anim> callback, 
            float duration = 1, float delay = 0, 
            bool autoKillNullifiedKey = true,
            bool preRunDelayedAnim = true)
        {
            Ticker.Init();

            this.key = key ?? global;

            this.callback = callback;
            this.duration = duration;
            time = -delay;

            this.autoKillNullifiedKey = autoKillNullifiedKey && key != null;

            preRun = preRunDelayedAnim && delay > 0;

            instances.Add(this);
            dictionarySet.Add(this.key, this);
        }

        public void Destroy()
        {
            Destroyed = true;

            instances.Remove(this);
            dictionarySet.Remove(key, this);
        }

        void Update(float deltaTime)
        {
            if (Destroyed)
                return;

            if (autoKillNullifiedKey &&
                (key == null) ||
                // because Unity can say a object is "null", but not equal to null... (nullifiedObject != null)
                ((key is Component) && !((key as Component) ?? false)) ||
                ((key is GameObject) && !((key as GameObject) ?? false)))
            {
                Destroy();
                return;
            }

            time += deltaTime * timeScale;

            // delayed
            if (time < 0)
            {
                if (preRun)
                {
                    callback(this);
                    preRun = false;
                }

                return;
            }

            if (time > duration)
                time = duration;

            callback(this);

            if (time == duration)
            {
                foreach (var action in onComplete)
                    action();

                Destroy();
            }

            frame++;
        }

        static void UpdateAll(float deltaTime)
        {
            foreach (Anim anim in instances.ToArray())
                anim.Update(deltaTime);
        }
    }
}

#pragma warning restore RECS0018 // Comparaison des nombres à virgule flottante avec l’opérateur d’égalité
