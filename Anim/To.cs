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

            var ease = Ease.Out3;
            float delay = 0;

            // speed
            float speed = float.NaN, distance = float.NaN;

            foreach (var (property, index) in props.GetType().GetProperties().ItemIndex())
            {
                var type = property.PropertyType;
                var name = property.Name;

                if (name == "duration")
                {
                    duration = (float)property.GetValue(props);
                    continue;
                }

                if (name == "ease")
                {
                    ease = property.GetValue(props) as Func<float, float>;
                    continue;
                }

                if (name == "speed")
                {
                    speed = (float)property.GetValue(props);
                    continue;
                }

                var propertyTarget = targetType.GetProperty(name);

                if (propertyTarget == null)
                    throw new Exception($"the property \"{name}\" does not exist on target ({target})!");

                bool propertyTypesMatch = propertyTarget.PropertyType == type;

                if (!propertyTypesMatch)
                    throw new Exception($"properties \"{name}\" do not match: " +
                    	$"\nfrom: {type}, to: {propertyTarget.PropertyType}");

                object from = propertyTarget.GetValue(target);
                object to = property.GetValue(props);

                Action<float> action;

                if (from is Quaternion fromQ && to is Quaternion toQ)
                {
                    action = t => propertyTarget.SetValue(target, 
                        Quaternion.SlerpUnclamped(fromQ, toQ, t));
                }
                else
                {
                    var methods = property.PropertyType.GetMethods();
                    var add = Matching(methods, "op_Addition", type, type);
                    var sub = Matching(methods, "op_Subtraction", type, type);
                    var mul = Matching(methods, "op_Multiply", type, typeof(float));

                    object delta = sub.Invoke(null, new object[] { to, from });

                    if (EvaluateDistance(delta, out float distanceEvaluation))
                        distance = distanceEvaluation;

                    action = t => propertyTarget.SetValue(target,
                        add.Invoke(null, new object[] { from, mul.Invoke(null, new object[] { delta, t }) }));
                }

                actions.Add(action);
            }

            if (!float.IsNaN(speed) && !float.IsNaN(distance))
                duration = distance / speed;

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
