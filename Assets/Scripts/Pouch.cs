using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Pouch : MonoBehaviour {
    
    public Sprite highlight;

    public Button[] all;
    public List<Sprite> allOrig;

    private GameController gameController;

    void Start()
    {
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }
        else
        {
            Debug.Log("Cannot find 'GameController' script");
        }

        //original = GetComponent<Image>().sprite;
        allOrig = new List<Sprite>();

        for (int i = 0; i < all.Length; i++)
        {
            allOrig.Add(all[i].GetComponent<Image>().sprite);
        }

    }

    // changes the amount to grab and highlight sprite
	public void PouchSelect(int pouchSize)
    {
        for(int i = 0; i < all.Length; i++)
        {
            all[i].GetComponent<Image>().sprite = allOrig[i];
        }
        gameController.SetPouchSize(pouchSize);
        GetComponent<Image>().sprite = highlight;
    }
}
