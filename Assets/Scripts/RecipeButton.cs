using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

// Opens the menu with the recipe for the potion
public class RecipeButton : MonoBehaviour {

    public Sprite highlight;
    private Sprite original;
    private bool menuOpen;

    public Button nextPageBtn;
    public Button prevPageBtn;

    public Text recipeText; // says recipe
    public Image[] miscElements; // background, 2 frames
    public Image[] itemIcons; // 0 = top item, 1 = bot item
    public Text[] reqAmts; // 0 = top, 1 = bot, displays the fraction
    private int pageCount;
    private int curPage;

    private GameController gameController;
    private AudioSource pageFlip;

    void Start()
    {
        curPage = 0;
        original = GetComponent<Image>().sprite;
        menuOpen = false;

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
        Debug.Log("Number of pages (-1) : " + pageCount.ToString());
        Enable(false);
    }

    // when mouse hovers over button
    public void MouseOverHighlight()
    {
        if (!menuOpen)
        {
            GetComponent<Image>().sprite = highlight;
            GetComponent<RectTransform>().sizeDelta = new Vector2(86.0f, 110.0f);
        }
    }

    // when mouse stops hovering over button
    public void MouseExitHighlight()
    {
        if (!menuOpen)
        {
            GetComponent<Image>().sprite = original;
            GetComponent<RectTransform>().sizeDelta = new Vector2(76.0f, 100.0f);
        }

    }

    // opens and navigates menu via mouse
    public void MouseOpenRecipe()
    {
        if (!menuOpen)
        {
            menuOpen = true;
            Enable(true);
        } else
        {
            menuOpen = false;
            curPage = 0;
            Enable(false);
        }
    }

    // if prev, go to previous page
    public void ButtonPage(bool prev)
    {
        if (prev && curPage > 0)
        {
            curPage--;
        }
        else
        {
            curPage++;
        }
        Enable(true);
    }


    // opens and navigates menu via keyboard buttons
    void Update()
    {

        // menu is closed
        if (Input.GetKeyDown(KeyCode.Tab) && !menuOpen)
        {
            GetComponent<Image>().sprite = highlight;
            GetComponent<RectTransform>().sizeDelta = new Vector2(86.0f, 110.0f);
            menuOpen = true;
            Enable(true);
        }
        // there's another page
        else if (Input.GetKeyDown(KeyCode.Tab) && menuOpen && (curPage < pageCount))
        {
            curPage++;
            Enable(true);
        }
        // go back a page
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            if (Input.GetKeyDown(KeyCode.Tab) && menuOpen && (curPage > 0))
            {
                curPage--;
                Enable(true);
            }
        }
        // last page, close
        else if (Input.GetKeyDown(KeyCode.Tab) && menuOpen && (curPage == pageCount))
        {
            GetComponent<Image>().sprite = original;
            GetComponent<RectTransform>().sizeDelta = new Vector2(76.0f, 100.0f);
            menuOpen = false;
            curPage = 0;
            Enable(false);
        }
    }

    // hides or shows all the elements of the current page of the recipe
    private void Enable(bool en)
    {
        List<Sprite> icons = new List<Sprite>();
        List<int> amounts = new List<int>();

        float tempIconCount = (float)gameController.GetRecipe().Count;
        pageCount = Mathf.CeilToInt(tempIconCount / 2.0f) - 1;

        foreach (KeyValuePair<GameObject, int> kv in gameController.GetRecipe())
        {
            icons.Add(kv.Key.GetComponent<SpriteRenderer>().sprite);
            amounts.Add(kv.Value);
        }

        if (en)
        {
            pageFlip.Play();

            if(curPage == pageCount)
            {
                prevPageBtn.gameObject.SetActive(false);
                nextPageBtn.gameObject.SetActive(false);
            }else if(curPage > 0)
            {
                prevPageBtn.gameObject.SetActive(true);
                nextPageBtn.gameObject.SetActive(false);
            } else if (curPage <= pageCount)
            {
                prevPageBtn.gameObject.SetActive(false);
                nextPageBtn.gameObject.SetActive(true);
            }

            recipeText.enabled = true;
            for (int i = 0; i < miscElements.Length; i++)
            {
                miscElements[i].enabled = true;
            }

            // find the correct icon
            for (int i = 0; i < itemIcons.Length; i++)
            {
                itemIcons[i].enabled = true;
                if (i + (curPage * 2) < icons.Count)
                {
                    itemIcons[i].sprite = icons[i + (curPage * 2)];
                } else
                {
                    itemIcons[i].enabled = false;
                }                
            }

            // Set the proper amount in text
            for (int i = 0; i < reqAmts.Length; i++)
            {
                reqAmts[i].enabled = true;
                if(i + (curPage * 2) < amounts.Count)
                {
                    int gcd = FindGCD(amounts[i + (curPage * 2)], gameController.GetRecipePotSize());
                    reqAmts[i].text = (amounts[i + (curPage * 2)] / gcd).ToString() + "/" + (gameController.GetRecipePotSize() / gcd).ToString();
                } else
                {
                    reqAmts[i].enabled = false;
                }                
            }
        }
        else
        {
            nextPageBtn.gameObject.SetActive(false);
            prevPageBtn.gameObject.SetActive(false);
            recipeText.enabled = false;
            for (int i = 0; i < miscElements.Length; i++)
            {
                miscElements[i].enabled = false;
            }

            for (int i = 0; i < itemIcons.Length; i++)
            {
                itemIcons[i].enabled = false;
            }

            for (int i = 0; i < reqAmts.Length; i++)
            {
                reqAmts[i].enabled = false;
            }
        }
    }

    private int FindGCD(int num1, int num2)
    {
        List<int> div1 = new List<int>();
        List<int> div2 = new List<int>();

        if (num1 % num2 == 0)
        {
            Debug.Log("GDC between" + num1.ToString() + " and " + num2.ToString() + ": " + num2.ToString());
            return num2;
        } else if (num2 % num1 == 0)
        {
            Debug.Log("GDC between" + num1.ToString() + " and " + num2.ToString() + ": " + num1.ToString());
            return num1;
        } else
        {
            int larger;
            larger = num1 > num2 ? num1 : num2;

            for (int i = 2; i < larger; i++)
            {
                if (num1 % i == 0 && i <= num1)
                {
                    div1.Add(i);
                }

                if(num2 % i == 0 && i <= num2)
                {
                    div2.Add(i);
                }
            }
            
            for(int i = div1.Count - 1; i >= 0; i--)
            {
                if (div2.Contains(div1[i]))
                {
                    Debug.Log("GDC between" + num1.ToString() + " and " + num2.ToString() + ": " + div1[i].ToString());
                    return div1[i];
                }
            }
            
        }
        Debug.Log("GDC: 1");
        return 1;
    }

}
