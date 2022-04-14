using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMover : MonoBehaviour
{
    public Transform pos;

    void Update()
    {
        transform.position = new Vector3(Mathf.Lerp(transform.position.x, pos.transform.position.x, Time.time *1f), transform.position.y, transform.position.z);
    }
}
