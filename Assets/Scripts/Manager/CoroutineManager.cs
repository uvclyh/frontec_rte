using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Noah;
using System;

namespace Noah
{


    public class CoroutineManager : Singleton<CoroutineManager>
    {
        static Dictionary<string, IEnumerator> _routines = new Dictionary<string, IEnumerator>(100);

        public static Coroutine Start(IEnumerator routine) => Instance.StartCoroutine(routine);
        public static Coroutine Start(IEnumerator routine, string id)
        {
            var coroutine = Instance.StartCoroutine(routine);
            if (!_routines.ContainsKey(id)) _routines.Add(id, routine);
            else
            {
                Instance.StopCoroutine(_routines[id]);
                _routines[id] = routine;
            }
            return coroutine;
        }
        public static void Stop(IEnumerator routine) => Instance.StopCoroutine(routine);
        public static void Stop(string id)
        {
            if (_routines.TryGetValue(id, out var routine))
            {
                Instance.StopCoroutine(routine);
                _routines.Remove(id);
            }
            else Debug.LogWarning($"coroutine '{id}' not found");
        }
        public static void StopAll() => Instance.StopAllCoroutines();



    }

}