using Player;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class PlayerCameraFollow : MonoBehaviour
{
    private Transform target => PlayerController.ParentPlayer.transform;
    private Camera camera;

    private void Awake()
    {
        camera = Camera.main;
    }

    private void LateUpdate()
    {
        var position = new Vector3(target.position.x, target.position.y, camera.transform.position.z);
        camera.transform.position = position;
    }
}
