using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GroupLabel : MonoBehaviour {
    public int size = 0;
	// Use this for initialization
	void Start () {
        Text t = (Text)GetComponent("Text");
        t.text = ""+size;
	}

	// Update is called once per frame
	void Update () {

	}
}
