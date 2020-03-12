using UnityEngine;

namespace Utilities
{
	public class TransformCache
	{
		public Vector3 Position { get; private set; }
		public Quaternion Rot { get; private set; }
		public Vector3 Scale { get; private set; }
		public int ChildIndex { get; private set; }

		public TransformCache() { }

		public TransformCache(Transform trf)
		{
			Position = trf.position;
			Rot = trf.rotation;
			Scale = trf.localScale;
		}

		public void Set(Transform trf)
		{
			trf.position = Position;
			trf.rotation = Rot;
			trf.localScale = Scale;
		}

		public void Update(Vector3 newPos, Quaternion newRot, Vector3 newScale,int childIndex)
		{
			Position = newPos;
			Rot = newRot;
			Scale = newScale;
			ChildIndex = childIndex;
		}

		public void Save(Transform trf)
		{
			Position = trf.position;
			Rot = trf.rotation;
			Scale = trf.localScale;
			ChildIndex = trf.GetSiblingIndex();
		}
	}
}