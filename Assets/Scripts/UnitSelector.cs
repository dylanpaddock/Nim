using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelector : MonoBehaviour {
    //Takes care of unit selection for a player, computer or human. Draws a box on the GUI and determines if a group and
    //it's stones are within that box. Removes the selected stones if desired.

    private bool isSelecting = false; //Is the player currently making a selection?
    private Vector3 startMousePos; //The player's mouse position when starting the selection.
    private bool paused; //Is the game paused?
    private float computerWaitTime; // The amount of time the computer player has waited before removing the selection.
    public Group selectedGroup; //The current group that the player has selected. Null if empty.
    public List<GameObject> selectedList; //A list of selected stones in the selected group. Null if empty.

    public Pieces pieces; //A list of all pieces on the board
    private List<GameObject> groupsInBounds; //A list of all groups currently within the selection bounds.

    public Turns turnController;
    public ComputerPlayer computerPlayer;
    public Menu menu;

    //initialization
    void Start () {
        Pause();
        groupsInBounds = new List<GameObject>();
	}

    // Update is called once per frame
	void Update () {
        if (paused){
            return;
        }
        if (!turnController.isComputerTurn()){ //human turn
            if (Input.GetMouseButtonDown(0)){ // On click, clear old selection and start a new selection.
                isSelecting = true;
                startMousePos = Input.mousePosition;
                foreach (GameObject gameObject in pieces.groups){
                    Group g = gameObject.GetComponent<Group>();
                    if (g==null){
                        Debug.LogError("Something went wrong with selecting a group");
                    }else if (g.isSelected){
                        //deselect group and its stones
                        g.isSelected = false;
                        foreach (GameObject go in g.stonesList){
                            Stone s = go.GetComponent<Stone>();
                            s.isSelected = false;
                        }
                    }
                }
            }
            if (isSelecting){//While mouse button is held down, select the group and stones.
                //Check each group is within the selection box and update the list of groupsInBounds accordingly.
                foreach (GameObject g in pieces.groups){
                    bool hit = intersectsSelection(g);
                    bool inList = groupsInBounds.Contains(g);
                    if(hit && !inList){ //The group is in selection box and not in list. Add it to the list.
                        groupsInBounds.Add(g);
                    }else if (!hit && inList){//The group is out of selection box but in list. Remove it from the list.
                        g.GetComponent<Group>().isSelected = false;
                        groupsInBounds.Remove(g);
                    }
                }
                //Highlight the group in groupsInBounds that was selected first, if it exists.
                bool found = false;
                for (int i = 0; i<groupsInBounds.Count && !found; i++){
                    Group g = groupsInBounds[i].GetComponent<Group>();
                    if (groupsInBounds[i] != null && g.stonesList.Count > 0){
                        //find first group with active stones
                        selectedGroup = g;
                        selectedGroup.isSelected = true;
                        found = true;
                    }
                }
            }
            if (Input.GetMouseButtonUp(0)){ //On release, add the stones in the selected group to selectedList.
                if (selectedGroup != null){
                    selectedList = new List<GameObject>();
                    foreach (GameObject gameObject in selectedGroup.stonesList){
                        if (isWithinSelection(gameObject)){
                            Stone s = gameObject.GetComponent<Stone>();
                            if (s != null){
                                s.isSelected = true;
                                selectedList.Add(gameObject);
                            }
                        }
                    }
                }
                isSelecting = false; //Selection is complete.
            }
        }else {//computer turn


            if (!isSelecting){//

                computerPlayer.ChooseStones();
                selectedGroup = computerPlayer.selectedGroup;
                selectedGroup.isSelected = true;
                selectedList = computerPlayer.selectedList;
                computerWaitTime = Time.time;
                isSelecting = true;
                foreach (GameObject gameObject in selectedList){
                    Stone s = gameObject.GetComponent<Stone>();
                    if (s != null){
                        s.isSelected = true;
                    }

                }
            }else if (isSelecting && (computerWaitTime + computerPlayer.waitTime < Time.time)){
                removeSelected();
            }
        }
    }

    //OnGUI is called once per frame. Draws a box based on the mouse location.
    void OnGUI(){
        if (isSelecting && !turnController.isComputerTurn()){//draw box
            Rect r = Utils.GetScreenRect(startMousePos, Input.mousePosition);
            Utils.DrawScreenRect(r, new Color(.9f, .0f, .0f, .15f));
            Utils.DrawScreenRectBorder(r, 2, new Color(.9f, .2f, .1f, .9f));
        }
    }

    //Tells if a stone's gameObject's position is inside the selection box. Does not use the mesh information. The
    //center of the object must be in bounds.
    public bool isWithinSelection(GameObject gameObject){
        if (!isSelecting){
            return false;
        }
        Camera camera = Camera.main;
        Bounds bounds = Utils.GetViewportBounds(camera, startMousePos, Input.mousePosition);
        Vector3 diag = bounds.max - bounds.min;
        diag.z = 0;
        return bounds.Contains(camera.WorldToViewportPoint(gameObject.transform.position));
    }

    //Tells if a group's bounding box intersects the selection box.
    public bool intersectsSelection(GameObject gameObject){
        if (!isSelecting){
            return false;
        }
        Camera camera = Camera.main;
        Bounds selectionBounds = Utils.GetViewportBounds(camera, startMousePos, Input.mousePosition);
        Renderer r = gameObject.GetComponent<Renderer>();
        Bounds objBounds = Utils.GetViewportBoundsWorldspace(camera, r.bounds.min, r.bounds.max);//convert to viewport space
        return selectionBounds.Intersects(objBounds);

    }

    //Removes the selected stones from all lists and destroys them.
    public void removeSelected(){
        if (selectedList != null && selectedList.Count > 0){
            foreach (GameObject s in selectedList){
                s.SetActive(false);
                selectedGroup.stonesList.Remove(s);
                Object.Destroy(s);
            }
            if (selectedGroup.Size() == 0){
                pieces.groups.Remove(selectedGroup.gameObject);//remove empty group
                selectedGroup.gameObject.SetActive(false);
                Object.Destroy(selectedGroup.gameObject);
            }

            if (pieces.groups.Count == 0){ // all groups are empty
                menu.enabled = true;
                menu.EndGame(turnController.CurrentPlayer()+" wins!!!");
            }

            turnController.ChangePlayer();
        }
        isSelecting = false; //Deselect the group and stones.
        selectedList = null;
        if (selectedGroup != null){
            selectedGroup.isSelected = false;
        }
        selectedGroup = null;
    }

    //Pause the game.
    public void Pause(){
        paused = true;
    }

    //Unpause the game.
    public void Unpause(){
        paused = false;
    }
}
