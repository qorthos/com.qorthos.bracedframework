using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace BracedFramework
{
    [CreateAssetMenu(fileName = "GameEventChannel", menuName = "Channels/GameEventChannel")]
    public class GameEventChannel : ScriptableObject
    {
        public static GameEventChannel Instance;

        private Dictionary<Type, IBroadcastEvent> _broadcastEventLib;

        private void OnEnable()
        {
            Instance = this;
            _broadcastEventLib = new Dictionary<Type, IBroadcastEvent>();
        }

        private void OnDisable()
        {
            Instance = null;
            foreach (IBroadcastEvent broadcastEvent in _broadcastEventLib.Values)
            {
                broadcastEvent.Clear();
            }

            _broadcastEventLib.Clear();
        }

        public void Broadcast<T>(T args) where T : EventArgs
        {
            var type = typeof(T);

            Debug.Log($"Broadcasting: {type}");
            if (_broadcastEventLib.ContainsKey(type) == false)
            {
                _broadcastEventLib.Add(type, new BroadcastEvent<T>());
            }

            ((BroadcastEvent<T>)_broadcastEventLib[type]).Invoke(args);
        }

        public void RegisterListener<T>(UnityAction<T> callback) where T : EventArgs
        {
            var type = typeof(T);

            if (_broadcastEventLib.ContainsKey(type) == false)
            {
                _broadcastEventLib.Add(type, new BroadcastEvent<T>());
            }

            ((BroadcastEvent<T>)_broadcastEventLib[type]).AddListener(callback);
        }

        public void RemoveListener<T>(UnityAction<T> callback) where T : EventArgs
        {
            var type = typeof(T);

            if (_broadcastEventLib.ContainsKey(type) == false)
            {
                Debug.LogError("Attempting to remove a listener from a BroadcastEvent that never existed in the first place?!");
                return;
            }

            ((BroadcastEvent<T>)_broadcastEventLib[type]).RemoveListener(callback);
        }

    }
}