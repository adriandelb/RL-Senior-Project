using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectScript : MonoBehaviour
{
    private AgentScript agent;
    void OnCollisionEnter(Collision col)
    {
        agent = FindObjectOfType<AgentScript>();
        if (col.gameObject.CompareTag("Agent"))
        {
            agent.HitObject();
        }
    }
}
