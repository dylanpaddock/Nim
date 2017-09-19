using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour {
    private bool _isSelected;
    public bool isSelected{
        get{return _isSelected;}
        set{_isSelected = value;}
    }
    private Behaviour halo;

	// Use this for initialization
	void Start () {
        halo = (Behaviour)GetComponent("Halo");
        halo.enabled = false;
	}

	// Update is called once per frame
	void Update () {
        if (_isSelected){
            halo.enabled = true;
        } else {
            halo.enabled = false;
        }
	}


}
