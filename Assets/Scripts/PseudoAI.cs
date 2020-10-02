using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PseudoAI : MonoBehaviour
{
    [SerializeField]
    private List<int> accessibleSpaces;

    public PseudoAI()
    {
        accessibleSpaces = new List<int>();
    }

    //Update accesible field List
    public void UpdateAccessibleFields(int clickedField)
    {
        accessibleSpaces.Remove(clickedField); //erase clicked filed

        if (clickedField < 35)
            accessibleSpaces.Add(clickedField + 7); //add field above clicked field
    }

    //Init accesable fields
    public void InitAccessibleFields()
    {
        //Empty the list
        accessibleSpaces.Clear();
        //first row is active
        for (int i = 0; i < 7; i++)
        {
            accessibleSpaces.Add(i);
        }
    }

    public int AIMove()
    {
        int randomNbr = Random.Range(0, accessibleSpaces.Count); // generate random number
        return accessibleSpaces[randomNbr];
    }
}
