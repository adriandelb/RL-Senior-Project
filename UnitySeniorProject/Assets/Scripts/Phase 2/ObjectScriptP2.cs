using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectScriptP2 : MonoBehaviour
{
    public AgentScriptP2 agent;
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Agent"))
        {
            agent.HitObject();
        }
    }
}
