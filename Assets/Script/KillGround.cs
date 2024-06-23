using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillGround : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Eraser")
        {
            Destroy(this.gameObject);
        }
    }
 
}
