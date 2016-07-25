using UnityEngine;
using System.Collections;

// a little fountain + falling effect
public class MoveAwayFrom : MonoBehaviour {

    private Rigidbody2D rb;
    public float randStart, randEnd;

    // Use this for initialization
    void Start () {
        // Randomly scale the size of the object (fluff)
        float randVal = Random.Range(randStart, randEnd);
        transform.localScale = new Vector2(randVal, randVal);

        Destroy(gameObject, 2.0f);
        rb = gameObject.GetComponent<Rigidbody2D>();
        rb.gravityScale = 1.0f;

        // "throw the object randomly        
        float xForce = 0.0f;        
        float randDir = Random.Range(0.0f, 1.0f);
        float yForce = Random.Range(50.0f, 150.0f);
        if (randDir >= 0.5f)
        {
            xForce = Random.Range(50.0f, 150.0f);
        } else
        {
            xForce = Random.Range(50.0f, 150.0f) * -1.0f;
        }

        rb.AddForce(new Vector2(xForce, yForce));
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
}
