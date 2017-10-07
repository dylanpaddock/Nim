using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Group : MonoBehaviour {
    //A Group contains any number of stones, and lights up when it is selected.
    //Only one group can be selected at a time.

    public List<GameObject> stonesList; //A list of the stones in this group.
    private bool _isSelected;
    public bool isSelected{ //Is this group currently selected?
        get{return _isSelected;}
        set{_isSelected = value;}
    }
    private Behaviour halo; //Controls the glow effect on selection.
    private Text sizeLabel; //Displays the current number of stones.

	//initialization
	void Start () {
        halo = (Behaviour)GetComponent("Halo");
        halo.enabled = false;
        sizeLabel = this.gameObject.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Text>();
        //this.gameObject.transform.GetChild(0).GetChild(0).localPosition = this.transform.localPosition;

	}

	//Update is called once per frame
	void Update () {
        if (_isSelected){
            halo.enabled = true;
        } else {
            halo.enabled = false;
        }
        if (stonesList != null){
            sizeLabel.text = ""+stonesList.Count;
        }
	}

    //Tells the number of stones in the group.
    public int Size(){
        return stonesList.Count;
    }
}
