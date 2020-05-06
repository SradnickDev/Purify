using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Misc
{
	public interface IPoolable
	{
		bool Active();
		void Activate();
		void Deactivate();
	}

	public class ObjectPool<T> where T : Component, IPoolable
	{
		private Stack<T> m_pool = new Stack<T>();
		private T m_objectToPool;
		private Transform m_parent;
		private Stack<T> m_activeObjects = new Stack<T>();

		public void Initialize(int qty, T obj, Transform parent)
		{
			if (obj == null)
			{
				Debug.LogError("Object is null!");
				return;
			}

			m_objectToPool = obj;
			m_parent = parent;

			for (var i = 0; i < qty; i++)
			{
				IncreasePool();
			}
		}

		private void IncreasePool()
		{
			var obj = CreateObject();
			m_pool.Push(obj);
		}

		private T CreateObject()
		{
			return Object.Instantiate(m_objectToPool, new Vector3(0, 5000, 0), Quaternion.identity,
									  m_parent);
		}

		public void PushNonActiveObjects()
		{
			foreach (var obj in m_activeObjects.Where(obj => !obj.Active()))
			{
				m_pool.Push(obj);
			}
		}

		public T Pop()
		{
			if (m_pool.Count == 0) IncreasePool();
			var obj = m_pool.Pop();
			m_activeObjects.Push(obj);
			return obj;
		}

		public void Push(T obj) => m_pool.Push(obj);
	}
}