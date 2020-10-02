using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class FieldChecker : MonoBehaviour
{
    private int[,] arraySpaces; //2d array game fields 

    //Creates and initializes array with -100 values
    public void InitializeArray(int nbrOfColumns, int nbrOfRows)
    {
        arraySpaces = new int[nbrOfRows, nbrOfColumns];

        for (int i = 0; i < nbrOfColumns; i++)
            for (int j = 0; j < nbrOfRows; j++)
                arraySpaces[j, i] = -100;
    }

    public void ArraySpaceSetter(int yOfClicked, int xOfClicked, int value)
    {
        arraySpaces[yOfClicked, xOfClicked] = value;
    }

    // All checking functions iterate over list of elements in given row/column/diagonal/.
    // In each step sum of every four neighbour elements is checked.
    public bool CheckVerticalFields(int x, int nbrOfRows)
    {
        var check = new List<int>();
        int sum;

        for (int i = 0; i < nbrOfRows; i++)
        {
            check.Add(arraySpaces[i, x]);
        }

        for (int i = 0; i < 3; i++)
        {
            sum = check.Skip(i).Take(4).Sum();
            if (sum == 4 || sum == 44)
            {
                return true;
            }
        }
        return false;
    }


    public bool CheckHorizontalFields(int y, int nbrOfColumns)
    {
        var check = new List<int>();
        int sum;

        for (int i = 0; i < nbrOfColumns; i++)
        {
            check.Add(arraySpaces[y, i]);
        }

        for (int i = 0; i < 4; i++)
        {
            sum = check.Skip(i).Take(4).Sum();
            if (sum == 4 || sum == 44)
            {
                return true;
            }
        }
        return false;
    }

    public bool CheckSlashFields(int x, int y, int nbrOfRows, int nbrOfColumns)
    {
        var check = new List<int>();
        int sum;

        for (int i = 0; i < nbrOfRows; i++)
        {
            int j = x + y - i;
            if (j >= 0 && j < nbrOfColumns)
                check.Add(arraySpaces[i, j]);
        }

        for (int i = 0; i < 3; i++)
        {
            sum = check.Skip(i).Take(4).Sum();
            if (sum == 4 || sum == 44) return true;
        }
        return false;
    }

    public bool CheckBackSlashFields(int x, int y, int nbrOfRows, int nbrOfColumns)
    {
        var check = new List<int>();
        int sum;

        for (int i = 0; i < nbrOfRows; i++)
        {
            int j = x - y + i;
            if (j >= 0 && j < nbrOfColumns)
                check.Add(arraySpaces[i, j]);
        }

        for (int i = 0; i < 3; i++)
        {
            sum = check.Skip(i).Take(4).Sum();
            if (sum == 4 || sum == 44) return true;
        }
        return false;
    }
}
