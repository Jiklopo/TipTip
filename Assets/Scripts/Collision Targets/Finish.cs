using Events;
using UnityEngine;

public class Finish : MonoBehaviour, ICollisionTarget
{
	public void OnCollision(GameObject other)
	{
		if (other.CompareTag("Player"))
		{
			GameBus.OnLevelCompleted.Invoke();
		}
	}
}