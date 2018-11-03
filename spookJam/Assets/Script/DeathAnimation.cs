using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="DeathAnim", menuName ="DeathSequence")]
public class DeathAnimation : ScriptableObject
{

    // Effect to Spawn
    public ParticleSystem effect;

    // The time the whole object should exist
    public float timeToDie;

    // The placeholder Object with no physics will the Character dies 
    public GameObject persistanceRenderObject;

    public void Execute(Vector3 Position, GameObject dieingThing)
    {



    }



}
