using UnityEngine;

public class Spike : MonoBehaviour, ICollisionTarget
{
    public void OnCollision(GameObject other)
    {
        other.GetComponent<PlayerController>()?.ChangeSize(-1);
    }
}
