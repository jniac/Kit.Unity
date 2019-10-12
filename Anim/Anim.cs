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

        float timeOld;
    
        // pre-run is the concept of running once the anim, even if delayed, to allow initialization.
        bool preRun;

        public bool Instantaneous => duration == 0;
        public float Progress => Instantaneous ? 1 : time < 0 ? 0 : time / duration;
        public bool Complete => time == duration;
        public bool FirstFrame => !preRun && frame == 0;
        public bool PreRun => preRun;

        public bool TimeUpTrough(float threshold) =>
            time >= threshold && timeOld < threshold;

        public bool ProgressUpThrough(float threshold) =>
            TimeUpTrough(threshold * duration);

        List<Action> _onComplete = new List<Action>();

        public bool Destroyed { get; private set; }

        public readonly Action<Anim> callback;

        bool autoKillNullifiedKey;

        List<Action<Anim>> onUpdate;
        public List<Action<Anim>> OnUpdate => onUpdate ?? (onUpdate = new List<Action<Anim>>());

        List<Action<Anim>> onComplete;
        public List<Action<Anim>> OnComplete => onComplete ?? (onComplete = new List<Action<Anim>>());

        public AwaitableCompletion Completion => new AwaitableCompletion(this);

        public Anim(object key, Action<Anim> callback, 
            float duration = 1, float delay = 0, 
            bool autoKillNullifiedKey = true,
            bool preRun = true)
        {
            Ticker.Init();

            this.key = key ?? global;

            this.callback = callback;
            this.duration = duration;
            time = -delay;

            this.autoKillNullifiedKey = autoKillNullifiedKey && key != null;

            this.preRun = preRun;
            if (preRun)
            {
                callback(this);
                this.preRun = false;
            }

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

            timeOld = time;
            time += deltaTime * timeScale;

            // delayed
            if (time < 0)
                return;

            if (time > duration)
                time = duration;

            callback(this);

            if (onUpdate != null)
                foreach (var action in onUpdate)
                    action(this);

            if (time == duration)
            {
                foreach (var action in _onComplete)
                    action();

                if (onComplete != null)
                    foreach (var action in onComplete)
                        action(this);

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
