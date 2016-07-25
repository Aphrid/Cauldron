using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour {

    public Sprite highlight; // a "highlighted" or selected sprite
    public GameObject miniVers; // a mini-version of self to be spawned (fluff)
    public float randStart, randEnd;

    private bool inTrigger; // true iff colliders are overlapped
    private bool mouseOver; // true iif mouse is hovered over
    private GameController gameController;
    private Sprite origSprite; // a reference to the original sprite
    private GameObject player;

    private AudioSource itemGet;

    void Start()
    {
        inTrigger = false;
        origSprite = GetComponent<SpriteRenderer>().sprite;

        itemGet = GetComponent<AudioSource>();
        itemGet.volume = 0.3f;

        // Find the player and gameController objects
        player = GameObject.Find("TempPlayer");
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        } else {
            Debug.Log("Cannot find 'GameController' script");
        }
    }

    // On collider overlap, spawn mini version (fluff), and add increase player count
    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Space) && inTrigger) || (mouseOver && inTrigger && Input.GetMouseButtonDown(0)))
        {
            itemGet.Play();
            Spawn();
            player.GetComponent<PlayerMovement>().SetPickup();
        } else if ((Input.GetKeyDown(KeyCode.M) && inTrigger || (mouseOver && inTrigger && Input.GetMouseButtonDown(1))) && (gameController.GetItemCount().ContainsKey(miniVers) && gameController.GetItemCount()[miniVers] > 0)) // removing a copy instead
        {
            Despawn();
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

    // Manage highlighted sprite and collision overlap detection
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name.Equals("TempPlayer"))
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = highlight;
            inTrigger = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.name.Equals("TempPlayer"))
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = origSprite;
            inTrigger = false;
        }
    }

    void Spawn()
    {
        GameObject temp = Instantiate(miniVers, transform.position, transform.rotation) as GameObject;
        temp.AddComponent<MoveTowards>();
        temp.GetComponent<MoveTowards>().SetRandStart(randStart);
        temp.GetComponent<MoveTowards>().SetRandEnd(randEnd);
        gameController.AddItem(miniVers);
    }

    void Despawn()
    {
        GameObject temp = Instantiate(miniVers, player.transform.position, player.transform.rotation) as GameObject;
        temp.AddComponent<MoveAwayFrom>();
        temp.GetComponent<MoveAwayFrom>().SetRandStart(randStart);
        temp.GetComponent<MoveAwayFrom>().SetRandEnd(randEnd);
        gameController.RemoveItem(miniVers);
    }

	

}
