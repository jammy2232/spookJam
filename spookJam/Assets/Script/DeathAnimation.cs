using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="DeathAnim", menuName ="DeathSequence")]
public class DeathAnimation : ScriptableObject
{

    // Effect to Spawn
    public GameObject effect;

    // The time the whole object should exist
    public float timeToDie;

    // The placeholder Object with no physics will the Character dies
    public GameObject persistanceRenderObject;

    public IEnumerator Die(Vector3 Position, GameObject dieingThing)
    {

        // Instanciate the particle system
        GameObject particle = Instantiate(effect);
        // Place it in the correct position
        particle.transform.position = Position;
        // Get the reference to the object 
        persistanceRenderObject = dieingThing;
        // Wait for the animation to play
        yield return new WaitForSeconds(timeToDie);
        // Delete the objects once thier dead
        Destroy(persistanceRenderObject);

    }

}
