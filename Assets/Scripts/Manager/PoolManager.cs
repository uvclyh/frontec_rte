using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Noah;

namespace Noah
{
    public class PoolManager
    {
        #region Pool
        // Pool매니저에서는 여러개의 풀을 가지고 있다. 
        class Pool
        {
            public GameObject Original { get; private set; }
            public Transform Root { get; set; }
            Stack<Poolable> _poolStack = new Stack<Poolable>();


            public void Init(GameObject original, int count = 1)
            {
                Original = original;
                Root = new GameObject().transform;
                Root.name = $"{original.name}_Root";

                for (int i = 0; i < count; i++)
                    Push(Create());

            }

            Poolable Create()
            {
                GameObject go = Object.Instantiate<GameObject>(Original);
                go.name = Original.name;
                return go.GetOrAddComponent<Poolable>();
            }

            public void Push(Poolable poolable)
            {
                if(poolable == null) return;
                
                poolable.transform.parent = Root;
                poolable.gameObject.SetActive(false);
                poolable.isUsing = false;

                _poolStack.Push(poolable);
            }

            public Poolable Pop(Transform parent)
            {
                Poolable poolable;

                if(_poolStack.Count > 0)
                    poolable = _poolStack.Pop();
                else
                    poolable = Create();

                poolable.gameObject.SetActive(true);

                // DontDestroyOnload 해제용도
                if (parent == null)
                    poolable.transform.parent = Managers.Scene.CurrentScene.transform;

                poolable.transform.parent = parent;
                poolable.isUsing = true;
                
                return poolable;
            }

        }
        // 풀 목록을 딕셔너리로 저장
        #endregion


        Dictionary<string, Pool> _pool = new Dictionary<string, Pool>();


        Transform _root;

        public void Init()
        {
            if (_root == null)
            {
                _root = new GameObject { name = "@Pool_root" }.transform;
                Object.DontDestroyOnLoad(_root);
            }
        }

        

        public void Push(Poolable poolable)
        {
            string name = poolable.gameObject.name;

            if(_pool.ContainsKey(name) == false)
            {
                GameObject.Destroy(poolable.gameObject);
                return;
            }

            _pool[name].Push(poolable);
        }


        // Pool 에 오브젝트가 담겨있으면 사용
        public Poolable Pop(GameObject original, Transform parent = null)
        {
            // Pool 이 없으면 생성
            if (_pool.ContainsKey(original.name) == false)
                CreatePool(original);

            return _pool[original.name].Pop(parent);
        }

        public void CreatePool(GameObject original, int count = 5)
        {
            Pool pool = new Pool();
            pool.Init(original, count);
            pool.Root.parent = _root;

            _pool.Add(original.name, pool);

        }

        public GameObject GetOriginal(string name)
        {
            if (_pool.ContainsKey(name) == false)
                return null;

            return _pool[name].Original;
        }


        public void Clear()
        {
            foreach (Transform child in _root)
                GameObject.Destroy(child.gameObject);
            

            _pool.Clear();
        }
    }
}

