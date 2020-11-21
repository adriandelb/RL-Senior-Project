using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class PushBlockAgent : Agent
{
    //What does the agent need to keep track of to push the object off the platform?
    //The agent needs to know:
    // - The positon of the object as an observation
    // - Its own position

    //Rewards
    //The agent will receive a negtive reward when:
    // - for each action taken (-0.01)
    // - if the agent falls off the platform (-1)
    // - if it takes too long to push off
    //The agent will receive a positive reward when:
    // - the block is pushed off the edge (1)
    // - When the agent is touching the object(

    //Physics
    //On Collision
    //Agent - Fixed upodate add force to move, Rotation, constant force

    //Environment
    //Generate spawn location for objects

    //Actions
    //Take in a vector3 of action for the direction

    
   
}

