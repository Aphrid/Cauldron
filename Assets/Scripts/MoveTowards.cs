using UnityEngine;
using System.Collections;

public class MoveTowards : MonoBehaviour {

    private GameObject player;
    private Vector3 velocity = Vector3.zero;
    public float randStart, randEnd;

    void Start()
    {
        // Randomly scale the size of the object (fluff)
        float randVal = Random.Range(randStart, randEnd);
        transform.localScale = new Vector2(randVal, randVal);

        // Find the player object
        player = GameObject.Find("TempPlayer");
    }

    // to change the size of the random range based on each item
    public void SetRandStart(float val)
    {
        this.randStart = val;
    }

    public void SetRandEnd(float val)
    {
        this.randEnd = val;
    }

    // Smoothly move towards the player
	void Update()
    {
        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, new Vector3(player.transform.position.x, player.transform.position.y - 0.5f, transform.localPosition.z), ref velocity, 0.2f);
    }

    // On collision with the player, destroy itself
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name.Equals("TempPlayer"))
        {
            Destroy(gameObject);
        }        
    }
}
