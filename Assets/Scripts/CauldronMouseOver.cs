using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CauldronMouseOver : MonoBehaviour {

    public Button topButton;
    public Button botButton;

    public void MouseHighlightEnter(bool top)
    {
        if (top)
        {
            topButton.Select();            
        } else
        {
            botButton.Select();
        }
    }

}
