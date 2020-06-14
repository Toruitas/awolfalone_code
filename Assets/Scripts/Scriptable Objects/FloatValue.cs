using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://www.youtube.com/watch?v=baaCNZsOECI&list=PL4vbr3u7UKWp0iM1WIfRjCDTI03u43Zfu&index=24

// THis script lives outside the scene, and can't be attached to anything in the scene
// its values can be read by multiple scenes
[CreateAssetMenu]  // allows us to create this as an obj using a right click
public class FloatValue : ScriptableObject, ISerializationCallbackReceiver
{
    // no start or update
    // Values outside the scene. Don't have to reset these values (health)

    public float initialValue;
    // [HideInInspector]  // so can't change runtime val in inspector
    // [NonSerialized]
    public float RuntimeValue;

    // serialization is loading/unloading from memory. When you start the game (serialize) or deserialize (close the game)
    public void OnAfterDeserialize(){
        RuntimeValue = initialValue;
    }

    public void OnBeforeSerialize(){

    }

}
