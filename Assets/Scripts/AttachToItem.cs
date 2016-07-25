using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// Attaches the menu to the coal pit
public class AttachToItem : MonoBehaviour {

    public Image buttonBG; // drawn under the buttons
    public Image[] buttonCauldrons;
    public Button[] buttons; // the buttons that we're going to show
    public Camera cam; // the camera to track relative to (we only have 1 camera anyways)
    public GameObject cauldron; // cauldron prefab to spawn on top of the logs
    public Text[] sizes; // lazy man's way of showing size of each cauldron
    public Text[] sizesLabel; // says Size over the numbers
    public Text[] title; // ye olde and potShoppe
    public AudioClip[] potPlace;
    public AudioClip potionGet;
    public AudioClip firePit;
    private AudioSource firePitAS;

    private bool inTrigger;
    private bool tracking; // tells update to keep tracking
    private bool cauldronExists; // true iff a cauldron was spawned already (prevents menu from respawning in subsequent presses)
    private GameController gameController;
    private AudioSource audioSource;

    private bool mouseOver;
    public Sprite highlight;
    private Sprite origSprite;


    void Start()
    {
        inTrigger = false;
        tracking = false;
        cauldronExists = false;
        origSprite = GetComponent<SpriteRenderer>().sprite;
        audioSource = GetComponent<AudioSource>();

        firePitAS = gameObject.AddComponent<AudioSource>() as AudioSource;
        firePitAS.volume = 0.5f;
        firePitAS.clip = firePit;
        firePitAS.loop = true;

        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }
        else
        {
            Debug.Log("Cannot find 'GameController' script");
        }

    }
    

    // set up and play the fire pit audio
    void FirePitSound(bool visible)
    {
        if (visible)
        {
            firePitAS.Play();
        } else
        {
            firePitAS.Stop();
        }

    }

    void OnBecameVisible()
    {
        FirePitSound(true);
    }

    void OnBecameInvisible()
    {
        FirePitSound(false);
    }

    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.M) && inTrigger) || (mouseOver && inTrigger && Input.GetMouseButtonDown(1)) && cauldronExists)
        {
            GameObject tempCauldron = GameObject.Find("Cauldron(Clone)");
            Destroy(tempCauldron);
            cauldronExists = false;
        } else if ((Input.GetKeyDown(KeyCode.R) || mouseOver && Input.GetMouseButtonDown(0)) && inTrigger && cauldronExists && !gameController.GetHasPotion())
        {
            gameController.AddPotion();
            audioSource.clip = potionGet;
            audioSource.volume = 0.7f;
            audioSource.Play();
        }

        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0) && mouseOver) && !cauldronExists && inTrigger)
        {
            Debug.Log("Got here bro");
            tracking = true;
            for(int i = 0; i < buttons.Length; i++)
            {
                buttons[i].interactable = true;
            }
            buttons[0].Select();
        } else if (!inTrigger || cauldronExists)
        {
            // move the menu elsewhere
            tracking = false;
            buttonBG.enabled = false;
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].interactable = false;
                buttons[i].enabled = false;
                buttonCauldrons[i].enabled = false;
                sizes[i].enabled = false;
                sizesLabel[i].enabled = false;
                title[i].enabled = false;
            }
        }

        // attach the menu to the gameObject
        if (tracking)
        {
            buttonBG.enabled = true;
            buttonBG.transform.position = cam.WorldToScreenPoint(new Vector3(transform.position.x - 3.0f, transform.position.y + 2.9f, transform.position.z));

            for (int i = 0; i < buttons.Length; i++)
            {
                title[i].enabled = true;
                title[i].transform.position = cam.WorldToScreenPoint(new Vector3(transform.position.x - 3.0f, transform.position.y + 4.9f - (i * 0.8f), transform.position.z));

                sizes[i].enabled = true;
                sizes[i].transform.position = cam.WorldToScreenPoint(new Vector3(transform.position.x - 2.2f, transform.position.y + 3.2f - (i * 1.6f), transform.position.z));

                sizesLabel[i].enabled = true;
                sizesLabel[i].transform.position = cam.WorldToScreenPoint(new Vector3(transform.position.x - 2.5f, transform.position.y + 3.6f - (i * 1.6f), transform.position.z));

                buttons[i].enabled = true;
                buttons[i].transform.position = cam.WorldToScreenPoint(new Vector3(transform.position.x - 4.5f, transform.position.y + 3.1f - (i * 1.6f), transform.position.z));

                buttonCauldrons[i].enabled = true;
                buttonCauldrons[i].transform.position = cam.WorldToScreenPoint(new Vector3(transform.position.x - 3.6f, transform.position.y + 3.1f - (i * 1.6f), transform.position.z));
            }
        }
    }

    // when mouse hovers over button
    void OnMouseOver()
    {
        GetComponent<SpriteRenderer>().sprite = highlight;
        mouseOver = true;
    }

    // when mouse stops hovering over button
    void OnMouseExit()
    {
        GetComponent<SpriteRenderer>().sprite = origSprite;
        mouseOver = false;
    }

    // Called by the buttons
    public void SpawnCauldron(int size)
    {        
        if (size == 36)
        {
            cauldron.transform.localScale = new Vector2(8.0f, 8.0f);
            Instantiate(cauldron, new Vector3(transform.position.x, transform.position.y + 1.60f, transform.position.z), transform.rotation);
        } else
        {
            cauldron.transform.localScale = new Vector2(6.0f, 6.0f);
            Instantiate(cauldron, new Vector3(transform.position.x, transform.position.y + 1.30f, transform.position.z), transform.rotation);
        }
        //gameController.SetPlayerPotSize(size);            
        cauldronExists = true;

        System.Random rnd = new System.Random();
        audioSource.clip = potPlace[rnd.Next(potPlace.Length)];
        audioSource.volume = 1f;
        audioSource.Play();

        GameObject myEventSystem = GameObject.Find("EventSystem");
        myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        inTrigger = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        inTrigger = false;
    }

}
