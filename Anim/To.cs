using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Kit.Utils;
using UnityEngine;

namespace Kit.Unity
{
    public partial class Anim
    {
        // Fetch the right method (name and params should match).
        static MethodInfo Matching(MethodInfo[] methods, string name, Type paramType1, Type paramType2) =>
            methods
            .Where(m => m.Name == name)
            .Single(m =>
            {
                var ps = m.GetParameters();

                return ps.Length == 2
                    && ps[0].ParameterType == paramType1
                    && ps[1].ParameterType == paramType2;
            });

        static bool EvaluateDistance(object delta, out float distance)
        {
            if (delta is float)
            {
                distance = (float)delta;
                return true;
            }

            if (delta is Vector3)
            {
                distance = ((Vector3)delta).magnitude;
                return true;
            }

            distance = float.NaN;
            return false;
        }

        public static Anim To(object target, float duration, object props, 
            bool autoKillSimilarTarget = true, bool autoKillNullifiedTarget = true)
        {
            Type targetType = target.GetType();

            List<Action<float>> actions = new List<Action<float>>();

            Ease.Del ease = Ease.Linear;
            float delay = 0;

            // speed
            float speed = float.NaN, distance = float.NaN;

            foreach (var (property, index) in props.GetType().GetProperties().ItemIndex())
            {
                var type = property.PropertyType;
                var name = property.Name;

                if (name == "ease")
                {
                    ease = property.GetValue(props) as Ease.Del;
                    continue;
                }

                if (name == "speed")
                {
                    speed = (float)property.GetValue(props);
                    continue;
                }

                var propertyFrom = targetType.GetProperty(name);

                bool propertyTypesMatch = propertyFrom.PropertyType == type;

                if (!propertyTypesMatch)
                    throw new Exception($"properties do not match: {name} {type} {propertyFrom.PropertyType}");

                var methods = property.PropertyType.GetMethods();

                var add = Matching(methods, "op_Addition", type, type);
                var sub = Matching(methods, "op_Subtraction", type, type);
                var mul = Matching(methods, "op_Multiply", type, typeof(float));

                //// NOTE: May check for add, sub & mul existence

                object from = propertyFrom.GetValue(target);
                object to = property.GetValue(props);
                object delta = sub.Invoke(null, new object[] { to, from });

                if (EvaluateDistance(delta, out float distanceEvaluation))
                    distance = distanceEvaluation;

                Action<float> action = t => propertyFrom.SetValue(target,
                    add.Invoke(null, new object[] { from, mul.Invoke(null, new object[] { delta, t }) }));

                actions.Add(action);
            }

            if (!float.IsNaN(speed) && !float.IsNaN(distance))
                duration = speed * distance;

            if (autoKillSimilarTarget)
                Kill(target);

            return new Anim(target, anim => 
            {
                float t = ease(anim.Progress);

                foreach (var action in actions)
                    action(t);

            }, duration, delay, autoKillNullifiedTarget);
        }

        public static Anim To(object target, object props,
            bool autoKillSimilarTarget = true, bool autoKillNullifiedTarget = true) =>
            To(target, 1, props, autoKillSimilarTarget, autoKillNullifiedTarget);

    }
}
