
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectScriptP1 : MonoBehaviour
{
    public AgentScriptP1 agent;
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Agent"))
        {
            agent.HitObject();
        }
    }

    void OnCollisionStay(Collision collision)
    {
        //Check to see if the Collider's name is "Chest"
        if (collision.collider.name == "Agent")
        {
            agent.HitObject();
        }
    }
}