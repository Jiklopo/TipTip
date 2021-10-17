using Player;
using UnityEngine;


public class Food : MonoBehaviour, ICollisionTarget
{
	public void OnCollision(GameObject other)
	{
		if (other.CompareTag("Player"))
		{
			other.GetComponent<PlayerController>()?.ChangeSize(1);
			Destroy(gameObject);
		}
	}
}