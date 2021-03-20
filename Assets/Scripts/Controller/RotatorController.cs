using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatorController : MonoBehaviour
{
    public float speed = 3;

    // Update is called once per frame
    private void Update()
    {
        transform.Rotate(Vector3.up, speed * Time.deltaTime);
    }
}
