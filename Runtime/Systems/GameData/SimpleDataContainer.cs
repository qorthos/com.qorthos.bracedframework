using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn;

namespace BracedFramework
{
    public abstract class SimpleDataContainer : GameDataContainer
    {
        protected Dictionary<string, Value> lib = new Dictionary<string, Value>();
        protected Dictionary<string, SimpleDataContainer> children = new Dictionary<string, SimpleDataContainer>();

        protected abstract Value GetLocalValue(string variableName);
        protected abstract void SetLocalValue(string variableName, Value value);
        protected abstract SimpleDataContainer GetLocalChild(string name);

        public void RaiseNotify(string name)
        {
            this.Notify(name);
        }

        public void AddChild(string name, SimpleDataContainer container)
        {
            children.Add(name, container);
        }

        public sealed override Value GetValue(string variableName)
        {
            var split = variableName.Split(new[] { '_' }, 2);

            if (split.Length > 1)
            {
                if (children.ContainsKey(split[0]))
                {
                    return children[split[0]].GetValue(split[1]);
                }
                else
                {
                    Debug.LogError($"child not set in datacontainer {split[0]}");
                    return Value.NULL;
                }
            }

            if (lib.ContainsKey(variableName))
            {
                return lib[variableName];
            }
            else
            {
                return GetLocalValue(variableName);
            }
        }

        public sealed override void SetValue(string variableName, Value value)
        {
            var split = variableName.Split(new[] { '_' }, 2);

            if (split.Length > 1)
            {
                if (children.ContainsKey(split[0]))
                {
                    children[split[0]].SetValue(split[1], value);
                }
                else
                {
                    SetLocalValue(variableName, value);
                    return;
                }
            }

            if (lib.ContainsKey(variableName))
            {
                lib[variableName] = value;
                Notify(variableName);
            }
            else
            {
                SetLocalValue(variableName, value);
            }
        }

    }
}