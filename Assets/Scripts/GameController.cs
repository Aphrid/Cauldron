using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    // tracks item -> item count
    private Dictionary<GameObject, int> itemCount;

    // for when the user makes a potion
    private Dictionary<GameObject, int> itemCountCopy;

    // all the item objects available in a game to be used as ingredients, copy is for random selection
    public GameObject[] ingredients;

    // the required amount 
    private Dictionary<GameObject, int> recipe;

    // to be added later 
    public int[] potSizes; // different sizes of pots available
    private int potSizeRecipe; // pot size needed in the recipe
    //private int potSizeChosen; // pot size chosen by the player

    // each addition adds this amount
    private int pouchSize;

    private bool hasPotion; // a potion is "in" the inventory
    private bool hasRecipe; // true iff the recipe has been generated

    // put all recipe loading here since it is called before start
    void Awake()
    {
        hasPotion = false;
        hasRecipe = false;
        pouchSize = 1;

        itemCount = new Dictionary<GameObject, int>();
        //ingredientsCopy = new List<GameObject>();

        recipe = new Dictionary<GameObject, int>();
    }

    // tmep
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            //recipe = new Dictionary<GameObject, int>();
            //GenerateRecipe();
            RemovePotion();
        }

    }

    // later change return type to int to judge "level" of correctness
    public bool CheckRecipe()
    {
        if (itemCount.Count == 0)
        {
            Debug.Log("Potion fail - empty inventory");
            return false;
        }
        else
        {
            foreach (KeyValuePair<GameObject, int> entry in itemCountCopy)
            {
                // do something with entry.Value or entry.Key
                if (!recipe.ContainsKey(entry.Key) || (recipe.ContainsKey(entry.Key) && recipe[entry.Key] != entry.Value))
                {
                    Debug.Log("Potion fail");
                    return false;
                }
            }
            Debug.Log("Potion true");
            return true;
        }
    }

    public void SetPouchSize(int amt)
    {
        pouchSize = amt;
    }

    // Adds the count of an item
    public void AddItem(GameObject item)
    {
        if (itemCount.ContainsKey(item))
        {
            itemCount[item] += pouchSize;
        } else
        {
            itemCount.Add(item, pouchSize);
        }

        Debug.Log("Item: " + item + " | Count: " + itemCount[item].ToString());
    }

    // Remove a count of an item (guaranteed to exist)
    public void RemoveItem(GameObject item)
    {
        itemCount[item]--;
        Debug.Log(item.name + " " + itemCount[item].ToString());
    }

    // Return the dictionary to check values for that item
    public Dictionary<GameObject, int> GetItemCount()
    {
        return itemCount;
    }

    public Dictionary<GameObject, int> GetRecipe()
    {
        return recipe;
    }

    /*public void SetPlayerPotSize(int potSize)
    {
        potSizeChosen = potSize;
    }*/

    public int GetRecipePotSize()
    {
        return potSizeRecipe;
    }

    // either the player just made or threw away the potion + checking
    public void AddPotion()
    {
        Debug.Log("Potion added!");
        hasPotion = true;
        itemCountCopy = new Dictionary<GameObject, int>(itemCount); // keep track of what's inside the potion
    }

    public void RemovePotion()
    {
        hasPotion = false;
    }
    // ------------------

    public bool GetHasPotion()
    {
        return hasPotion;
    }

    public bool GetHasRecipe()
    {
        return hasRecipe;
    }

    // TO BE IMPLEMENTED properly **********************************************************************
    public void GenerateRecipe()
    {
        hasRecipe = true;
        System.Random rnd = new System.Random();
        potSizeRecipe = potSizes[rnd.Next(potSizes.Length)];
        List<GameObject> ing = new List<GameObject>(ingredients); // the ingredients to be used
        List<int> ingCount = new List<int>(); // the count of each ingredient
        int numIng;
        
        if(potSizeRecipe == 36)
        {
            numIng = rnd.Next(2, 5); // randomly choose 2 to 4 ingredients if pot size 36
        }
        else
        {
            numIng = rnd.Next(2, 4); // randomly choose 2 to 3 ingredients else
        }

        // Randomly remove ingredients that aren't needed
        for(int i = 6 - numIng; i > 0; i--)
        {
            ing.RemoveAt(rnd.Next(ing.Count));
        }

        Debug.Log("Pot size is: " + potSizeRecipe.ToString());

        // pseudorandom for recipe
        switch (numIng)
        {
            case 2:
                if(potSizeRecipe == 36)
                {
                    switch (rnd.Next(2))
                    {
                        case 0:
                            ingCount.Add(16);
                            ingCount.Add(20);
                            break;
                        case 1:
                            ingCount.Add(10);
                            ingCount.Add(26);
                            break;
                    }
                } else
                {
                    switch (rnd.Next(2))
                    {
                        case 0:
                            ingCount.Add(4);
                            ingCount.Add(12);
                            break;
                        case 1:
                            ingCount.Add(8);
                            ingCount.Add(8);
                            break;
                    }
                }
                break;
            case 3:
                if (potSizeRecipe == 36)
                {
                    switch (rnd.Next(2))
                    {
                        case 0: // 2/3, 1/12, 1/4
                            ingCount.Add(24);
                            ingCount.Add(3);
                            ingCount.Add(9);
                            break;
                        case 1: //  2/9, 1/6, 11/18
                            ingCount.Add(8);
                            ingCount.Add(6);
                            ingCount.Add(22);
                            break;
                    }
                }
                else
                {
                    switch (rnd.Next(2))
                    {
                        case 0: // 1/4, 3/8, 3/8
                            ingCount.Add(4);
                            ingCount.Add(6);
                            ingCount.Add(6);
                            break;
                        case 1: // 1/8, 1/2, 3/8
                            ingCount.Add(2);
                            ingCount.Add(8);
                            ingCount.Add(6);
                            break;
                    }
                }
                break;
            case 4:
                switch (rnd.Next(2))
                {
                    case 0: // 1/6, 1/9, 5/9, 1/36
                        ingCount.Add(6);
                        ingCount.Add(20);
                        ingCount.Add(9);
                        ingCount.Add(1);
                        break;
                    case 1: //  1/9, 5/18, 2/9, 7/18
                        ingCount.Add(9);
                        ingCount.Add(10);
                        ingCount.Add(8);
                        ingCount.Add(7);
                        break;
                }
                break;
        }

        while(ing.Count != 0)
        {
            int rand = rnd.Next(0, ing.Count);
            recipe.Add(ing[rand], ingCount[rand]);
            ing.RemoveAt(rand);
            ingCount.RemoveAt(rand);
        }

    }
    // ****************************************************************************************
}
