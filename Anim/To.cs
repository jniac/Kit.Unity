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
        static MethodInfo Matching(PropertyInfo property, string name, Type paramType1, Type paramType2)
        {
            try
            {
                return property.PropertyType.GetMethods()
                    .Where(m => m.Name == name)
                    .Single(m =>
                    {
                        var ps = m.GetParameters();

                        return ps.Length == 2
                            && ps[0].ParameterType == paramType1
                            && ps[1].ParameterType == paramType2;
                    });
            }
            catch
            {
                throw new Exception($"Oups, on ({property.PropertyType.Name})\"{property.Name}\" " +
                	$"no method: {name}({paramType1.Name}, {paramType2.Name})");
            }
        }

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
            bool autoKillSimilarTarget = true, bool autoKillNullifiedTarget = true, bool preRun = true)
        {
            Type targetType = target.GetType();

            List<Action<float>> actions = new List<Action<float>>();

            var key = target;
            var ease = Ease.Out3;
            var delay = 0f;

            // speed
            float speed = float.NaN, distance = float.NaN;

            foreach (var (property, index) in props.GetType().GetProperties().ItemIndex())
            {
                var type = property.PropertyType;
                var name = property.Name;

                if (name == "duration")
                {
                    duration = Convert.ToSingle(property.GetValue(props));
                    continue;
                }

                if (name == "delay")
                {
                    delay = Convert.ToSingle(property.GetValue(props));
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

                // by the default, 'key' is 'target', but that value can be overrided
                if (name == "key")
                {
                    key = property.GetValue(props);
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

                if (from.GetType() != to.GetType())
                {
                    throw new Exception($"oups, ({from.GetType()})\"from\" & ({to.GetType()})\"to\" do not match the same type!");
                }

                Action<float> action;

                if (from is Quaternion fromQ && to is Quaternion toQ)
                {
                    action = t => propertyTarget.SetValue(target,
                        Quaternion.SlerpUnclamped(fromQ, toQ, t));
                }
                else if (from is float fromF && to is float toF)
                {
                    action = t => propertyTarget.SetValue(target,
                        fromF + (toF - fromF) * t);
                }
                else
                {
                    var add = Matching(property, "op_Addition", type, type);
                    var sub = Matching(property, "op_Subtraction", type, type);
                    var mul = Matching(property, "op_Multiply", type, typeof(float));

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

            return new Anim(key, anim =>
            {
                float t = ease(anim.Progress);

                foreach (var action in actions)
                    action(t);

            }, duration, delay, autoKillNullifiedTarget, preRun);
        }

        public static Anim To(object target, object props,
            bool autoKillSimilarTarget = true, bool autoKillNullifiedTarget = true, bool preRun = true) =>
            To(target, 1, props, autoKillSimilarTarget, autoKillNullifiedTarget, preRun);

    }
}
