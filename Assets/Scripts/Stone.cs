using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour {
    //A Stone is one game piece in the game of Nim. Belongs to a group. Lights
    //up when selected. A player can select any number of stones from one group
    //and then remove them.

    public bool isSelected{get; set;} //Is this stone currently selected?
    private Behaviour halo; //Controls the glow effect for selection.

	//initialization
	void Start () {
        //Store the Behavior to avoid calling each frame.
        halo = (Behaviour)GetComponent("Halo");
        halo.enabled = false;
	}

	//Update is called once per frame
	void Update () {
        if (isSelected){
            halo.enabled = true;
        } else {
            halo.enabled = false;
        }
	}


}
