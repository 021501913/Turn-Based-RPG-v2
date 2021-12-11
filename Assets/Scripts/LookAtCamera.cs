using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;

    }

    private void Update()
    {
        transform.LookAt(new Vector3(mainCamera.transform.position.x, this.transform.position.y, mainCamera.transform.position.z), Vector3.up);
    }
}
