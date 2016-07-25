using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    private Rigidbody2D rb;
    private Transform tf;
    private Animator animator;
    private GameController gameController;
    private bool potionGet; // stops player so they can do the hands lift thing
    private bool canMove; // true => player can move

    public Sprite handsUp; // potion get pose
    public GameObject resultPotion;

    public float clampValsMin;
    public float clampValsMax;

    public AudioClip[] walkSounds;
    private AudioSource walkSound;

    private bool pickUp;
    private int walkCounter;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        tf = GetComponent<Transform>();
        animator = GetComponent<Animator>();
        walkSound = GetComponent<AudioSource>();
        walkSound.volume = 0.6f;
        potionGet = false;
        pickUp = false;
        canMove = true;
        walkCounter = 0;

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

    // Move the player, clamped to level restrictions
    void Update () {

        // throw hands up when picking up ingredients
        if (pickUp)
        {
            animator.SetBool("pickup", true);
            pickUp = false;
        } else
        {
            animator.SetBool("pickup", false);
        }

        // throw hands up making potion
        if (gameController.GetHasPotion() && !potionGet)
        {
            potionGet = true;
            canMove = false;
            StartCoroutine(PotionGet());
        }

        if (canMove)
        {
            if (Input.GetKey(KeyCode.A))
            {
                rb.velocity = new Vector2(-5.0f, 0.0f);
                GetComponent<SpriteRenderer>().flipX = true;
                animator.SetBool("playerWalk", true);
                walkCounter++;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                rb.velocity = new Vector2(5.0f, 0.0f);
                GetComponent<SpriteRenderer>().flipX = false;
                animator.SetBool("playerWalk", true);
                walkCounter++;
            }
            else
            {
                animator.SetBool("playerWalk", false);
                rb.velocity = Vector2.zero;
            }

            rb.position = new Vector2(Mathf.Clamp(tf.localPosition.x, clampValsMin, clampValsMax), tf.localPosition.y);
        }

        // higher == means a slower play rate
        if(walkCounter == 19)
        {
            walkCounter = 0;
            System.Random rnd = new System.Random();
            walkSound.clip = walkSounds[rnd.Next(walkSounds.Length)];
            walkSound.Play();
        }

	}
    // throws hands up and spawns a bottle
    IEnumerator PotionGet()
    {
        rb.velocity = Vector2.zero;
        animator.enabled = false;
        GetComponent<SpriteRenderer>().sprite = handsUp;

        GameObject tempPotion = Instantiate(resultPotion, new Vector3(transform.position.x + 0.10f, transform.position.y + 5.0f, transform.position.z), transform.rotation) as GameObject;
        tempPotion.GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, 7.0f);
        yield return new WaitForSeconds(1.0f);
        tempPotion.GetComponent<Rigidbody2D>().gravityScale = 0.0f;
        tempPotion.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        yield return new WaitForSeconds(2.0f);
        Destroy(tempPotion);
        canMove = true;
        animator.enabled = true;
    }

    public void SetCanMove(bool val)
    {
        canMove = val;
    }

    public void SetClamp(float min, float max)
    {
        clampValsMin = min;
        clampValsMax = max;
    }

    public void SetPickup()
    {
        pickUp = true;
    }


    public void SetPotionGet()
    {
        potionGet = false;
    }
}
