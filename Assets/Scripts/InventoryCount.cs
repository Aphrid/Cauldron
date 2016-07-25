using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InventoryCount : MonoBehaviour {

    public Text item; // says item
    public Image[] everything; // includes bg, 6 frames, 6 objects
    public Text[] itemCounts; // 0 = fern, 1 = bag, 2 = berry, 3 = snake, 4 = chem, 5 = mandible
    public GameObject[] keys; // to access the dictionary

    private GameController gameController;
    private bool capsToggle;

    private AudioSource pageFlip;

    void Start()
    {
        capsToggle = false;
        pageFlip = GetComponent<AudioSource>();

        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
   
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }
        else
        {
            Debug.Log("Cannot find 'GameController' script");
        }
        Enable(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.CapsLock))
        {
            capsToggle = !capsToggle;
            if (capsToggle)
            {
                pageFlip.Play();
            }
        }

        if (capsToggle)
        {
            Enable(true);
            for(int i = 0; i < 6; i++)
            {
                if (gameController.GetItemCount().ContainsKey(keys[i]))
                {
                    itemCounts[i].text = gameController.GetItemCount()[keys[i]].ToString();
                } else
                {
                    itemCounts[i].text = "0";
                }
            }
        } else
        {
            Enable(false);
        }
    }


    void Enable(bool en)
    {
        if (en)
        {
            item.enabled = true;
            for (int i = 0; i < everything.Length; i++)
            {
                everything[i].enabled = true;
            }

            for(int i = 0; i < itemCounts.Length; i++)
            {
                itemCounts[i].enabled = true;
            }
        } else
        {
            item.enabled = false;
            for (int i = 0; i < everything.Length; i++)
            {
                everything[i].enabled = false;
            }

            for (int i = 0; i < itemCounts.Length; i++)
            {
                itemCounts[i].enabled = false;
            }
        }
    }
}
