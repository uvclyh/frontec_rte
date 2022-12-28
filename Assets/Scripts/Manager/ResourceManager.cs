
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Noah;

namespace Noah
{


public class ResourceManager
{
		public Dictionary<string, Sprite> _sprites = new Dictionary<string, Sprite>();
	
		public void Init()
		{
		}

		public T Load<T>(string path) where T : Object
		{
			if (typeof(T) == typeof(Sprite))
			{
				if (_sprites.TryGetValue(path, out Sprite sprite))
					return sprite as T;

				Sprite sp = Resources.Load<Sprite>(path);
				_sprites.Add(path, sp);
				return sp as T;
			}

			// original에 들고 있으면 바로 사용 <오브젝트풀링>
            if (typeof(T) == typeof(GameObject))
            {
				string name = path;
				int index = name.LastIndexOf('/');
				if(index > 0)
					name = name.Substring(index + 1);

				GameObject go = Managers.Pool.GetOriginal(name);
				// 풀에 담겨 있으면 그냥 그거 반환 
				if (go != null)
					return go as T;
            }


            return Resources.Load<T>(path);
		}

		public GameObject Instantiate(string path, Transform parent = null)
		{
			GameObject original = Load<GameObject>($"Prefabs/{path}");
			if (original == null)
			{
				Debug.Log($"Failed to load prefab : {path}");
				return null;
			}

			// 혹시 풀링된 오브젝트가 있다면
			if (original.GetComponent<Poolable>() != null)
				return Managers.Pool.Pop(original, parent).gameObject;

			return Instantiate(original, parent);
		}

		public GameObject Instantiate(GameObject prefab, Transform parent = null)
		{
			GameObject go = Object.Instantiate(prefab, parent);
			go.name = prefab.name;
			return go;
		}

		public void Destroy(GameObject go)
		{
			if (go == null)
				return;

			// if Poolabe -> PoolManager will manage that object
			Poolable poolable = go.GetComponent<Poolable>();
			if(poolable != null)
			{
				Managers.Pool.Push(poolable);
				return;
			}

			Object.Destroy(go);
		}
	}
}
