using UnityEngine;

public class Finish : MonoBehaviour, ICollisionTarget
{
	public void OnCollision(GameObject other)
	{
		if (other.CompareTag("Player"))
		{
			Debug.Log("Level Completed =)");
		}
	}
}