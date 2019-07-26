#pragma warning disable RECS0018 // Comparaison des nombres à virgule flottante avec l’opérateur d’égalité

using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Kit.Unity
{
    public partial class Anim
    {
        static HashSet<Anim> instances = new HashSet<Anim>();
        static DictionarySet<object, Anim> dictionarySet = new DictionarySet<object, Anim>();

        public const string global = "global";

        public readonly object key;

        public float time, duration, delay, timeScale = 1;

        public float Progress { get => time < 0 ? 0 : time / duration; }
        public bool Complete { get => time == duration; }

        public bool Destroyed { get; private set; }

        public Action<Anim> callback;

        bool autoRemoveNullifiedKey;

        public Anim(object key, Action<Anim> callback, float duration = 1, float delay = 0, bool autoRemoveNullifiedKey = true)
        {
            Ticker.Init();

            this.key = key ?? global;

            this.callback = callback;
            this.duration = duration;
            time = -delay;

            this.autoRemoveNullifiedKey = autoRemoveNullifiedKey && key != null;

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

            if (autoRemoveNullifiedKey &&
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
                return;

            if (time > duration)
                time = duration;

            callback(this);

            if (time == duration)
                Destroy();
        }

        static void UpdateAll(float deltaTime)
        {
            foreach (Anim anim in instances.ToArray())
                anim.Update(deltaTime);
        }
    }
}

#pragma warning restore RECS0018 // Comparaison des nombres à virgule flottante avec l’opérateur d’égalité
