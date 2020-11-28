
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
}