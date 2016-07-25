using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class NPC : MonoBehaviour {

    private GameController gameController;

    private bool inTrigger;
    private bool mouseOver;    
    private bool talking; // if we're stuck in talking
    private bool idle; // true iff we're done with the request lines
    private bool idleLine; // to close the idle window
    private bool talkingLine; // so the player cant skip through the text needlessly
    private int count; // which line we're on
    private Animator anim;

    public GameObject player;
    public Image speechBG;
    public Image portrait;
    public Text npcText;

    public string[] reqLines; // when you first meet them
    public string[] idleLines; // when you dont have a potion complete
    public string[] goodLines; // when you finish your potion (success)
    public string[] badLines; // when you fail your potion + tell you to remake it

    private AudioSource talkSound;

    void Start()
    {
        inTrigger = false;
        talking = false;
        idle = false;
        mouseOver = false;
        idleLine = false;
        talkingLine = false;
        count = 0;
        Enable(false);

        anim = GetComponent<Animator>();
        talkSound = GetComponent<AudioSource>();
        talkSound.volume = 0.35f;

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

    void Update()
    {
        if (gameController.GetHasRecipe() && gameController.GetHasPotion() && inTrigger && (Input.GetKeyDown(KeyCode.Space) || (mouseOver && Input.GetMouseButtonDown(0))))
        {
            idle = false;
            player.GetComponent<PlayerMovement>().SetCanMove(false);
            player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            Enable(true);

            bool success = gameController.CheckRecipe();
            if (success)
            {
                StartCoroutine(DisplayLine(goodLines, 0, false, true, true));
            }
            else
            {
                StartCoroutine(DisplayLine(badLines, 0, false, true, false));
            }
        }

        // initial talk, not idle talk and currently not talking
        else if ((Input.GetKeyDown(KeyCode.Space) || (mouseOver && Input.GetMouseButtonDown(0))) && inTrigger && !idle && !talking && !talkingLine)
        {
            talking = true;
            player.GetComponent<PlayerMovement>().SetCanMove(false);
            player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            Enable(true);
            StartCoroutine(DisplayLine(reqLines, count, true, false, false));
        }
        // initial talk, and there's more than the original line of dialogue
        else if (talkingLine && talking && inTrigger && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
        {
            Enable(true);
            talkingLine = false;
            StartCoroutine(DisplayLine(reqLines, count, true, false, false));
        }

        // randomly pick an idle line when you approach him
        else if (!idleLine && idle && inTrigger && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
        {
            Enable(true);
            System.Random rnd = new System.Random();
            StartCoroutine(DisplayLine(idleLines, rnd.Next(idleLines.Length), false, false, false));
            idleLine = true;
        }

        // close the idle box
        else if (idleLine && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
        {
            Enable(false);
            idleLine = false;
        }
    }

    void Enable(bool en)
    {
        if (en)
        {
            npcText.enabled = true;
            speechBG.enabled = true;
            portrait.enabled = true;  
        } else
        {
            npcText.enabled = false;
            speechBG.enabled = false;
            portrait.enabled = false;
        }
    }

    IEnumerator DisplayLine(string[] line, int num, bool countInc, bool leaving, bool good)
    {
        for (int i = 0; i < line[num].Length + 1; i++)
        {
            npcText.text = line[num].Substring(0, i);
            if ((i < line[num].Length) && (i % 2 == 0 && !Char.IsWhiteSpace(line[num][i])))
            {
                talkSound.Play();
            }
            yield return new WaitForSeconds(0.02f);
        }

        talkSound.Stop();

        if (countInc)
        {
            count++;
            if (count < reqLines.Length)
            {
                talking = true;
                talkingLine = true;
            } else if(count == reqLines.Length) // allow movement and generate a recipe on the last line of dialogue where he gives you the recipe
            {
                talking = false;
                idle = true;
                player.GetComponent<PlayerMovement>().SetCanMove(true);
                yield return new WaitForSeconds(0.8f);
                gameController.GenerateRecipe();
                Enable(false);
            }
        }

        if (leaving && good)
        {
            anim.SetBool("finish", true);
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
            GetComponent<Rigidbody2D>().velocity = new Vector2(-4.0f, 0.0f);
            player.GetComponent<PlayerMovement>().SetCanMove(true);
            gameController.RemovePotion();
        } else if (leaving && !good)
        {
            player.GetComponent<PlayerMovement>().SetCanMove(true);
            player.GetComponent<PlayerMovement>().SetPotionGet();
            gameController.RemovePotion();
            idle = true;
        }

        if (leaving)
        {
            yield return new WaitForSeconds(1.5f);
            Enable(false);
        }

        yield return new WaitForSeconds(0.8f);
        
    }

    void OnMouseOver()
    {
        if (!talking)
        {
            mouseOver = true;
            anim.SetBool("inTrigger", true);
        }
    }

    void OnMouseExit()
    {
        if (!talking)
        {
            mouseOver = false;
            anim.SetBool("inTrigger", false);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name.Equals("TempPlayer"))
        {
            inTrigger = true;
            anim.SetBool("inTrigger", true);
        } else if (other.name.Equals("NPC Spawner"))
        {
            Enable(false);
            Destroy(gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.name.Equals("TempPlayer"))
        {
            inTrigger = false;
            anim.SetBool("inTrigger", false);
        }
    }
}
