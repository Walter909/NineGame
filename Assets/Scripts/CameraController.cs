using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float MoveSpeed = 0.2f;

    private Vector3 _velocity = Vector3.zero;
    private float _currentPosY;

    // Update is called once per frame
    void Update()
    {
        Vector3 moveToPosition = new Vector3(transform.position.x, _currentPosY, transform.position.z);
        transform.position = Vector3.SmoothDamp(transform.position, moveToPosition, ref _velocity, MoveSpeed);
    }

    public void MoveToNewRoom(Transform newRoom)
    {
        _currentPosY = newRoom.position.y;
    }
}
