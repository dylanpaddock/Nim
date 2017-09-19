using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Group : MonoBehaviour {
    public List<GameObject> stonesList;
    private bool _isSelected;
    public bool isSelected{
        get{return _isSelected;}
        set{_isSelected = value;}
    }
    private Behaviour halo;
    private Text sizeLabel;

	// Use this for initialization
	void Start () {
        halo = (Behaviour)GetComponent("Halo");
        halo.enabled = false;
        sizeLabel = this.gameObject.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Text>();
        //this.gameObject.transform.GetChild(0).GetChild(0).localPosition = this.transform.localPosition;

	}

	// Update is called once per frame
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

    public void ResetGroup(int i){
        //instantiate i stones
    }

    public int Size(){
        return stonesList.Count;
    }
}
