using UnityEngine;

public interface ICollisionTarget
{
	public abstract void OnCollision(GameObject other);
}