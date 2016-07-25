using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class OnButtonSelect : MonoBehaviour, ISelectHandler, IDeselectHandler {

    private RectTransform rt;

    void Start()
    {
        rt = GetComponent<RectTransform>();
    }

	public void OnSelect(BaseEventData eventData)
    {
        Debug.Log(this.gameObject.name + " was selected");
        rt.localScale = new Vector3(1.0f, 1.0f, 0.0f);        
    }

    public void OnDeselect(BaseEventData eventData)
    {
        Debug.Log(this.gameObject.name + " was deselected");
        rt.localScale = Vector3.zero;
    }

}
