using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotator : MonoBehaviour
{
    // It rotates the powerups around its local axes at a constant speed.
    void Update()
    {
        transform.Rotate(new Vector3 (15,30,45) * Time.deltaTime);
    }
}
