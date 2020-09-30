using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathScript : MonoBehaviour
{
    void OnCollisionEnter(Collision col) {
        if(col.collider.CompareTag("Player")) {
            Debug.Log("I'm dead!");
        }
    }
}
