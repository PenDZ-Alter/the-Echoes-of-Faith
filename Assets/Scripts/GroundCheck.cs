using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    PlayerLogic logicMovement;
    private void Start()
    {
        logicMovement = this.GetComponentInParent<PlayerLogic>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Touch the ground!");
        logicMovement.groundedChanger();
    }
    
    void Update()
    {

    }
}
