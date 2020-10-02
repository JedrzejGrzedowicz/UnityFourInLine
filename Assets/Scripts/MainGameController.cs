using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MainGameController : MonoBehaviour
{
    //field related fields 

    public Button[] fieldSpaces; // Game field represented by 1D array.Grid 7x6 - rows from bottom to top.
    public Sprite emptyIcon; // icon of emty field
    public Sprite[] playersIcons; // array holding different sprites for different players

    private int xOfClicked, yOfClicked; // x and y to coordinates of clicked field
    const int nbrOfColumns = 7;
    const int nbrOfRows = 6;

    // game management related fields

    private int whoseTurn; // 0 - players turn; 1 - AI's turn
    private int turnCount; // stores number of turns
    private bool gameOver; 

    // items of Score Pannel

    public GameObject ScorePannel; // Pannel to be displayed after finnishing the game
    public int playersScore;
    public int aiScore;
    public Text playersScoreText;
    public Text aiScoreText;
    public Text winnerText;

    //Handlers for other scripts

    public FieldChecker fieldChecker;
    public PseudoAI pseudoAI;

    //Method called after clicking each field
    public void FieldClicked(int clickedField)
    {
        // do not continue if the game has ended - its preventing AI move after Player has won
        if (gameOver) return; 

        fieldSpaces[clickedField].image.sprite = playersIcons[whoseTurn];
        fieldSpaces[clickedField].interactable = false;

        ActivateField(clickedField);        
        pseudoAI.UpdateAccessibleFields(clickedField);

        //convert index from 1D array to 2D x and y 
        xOfClicked = clickedField % nbrOfColumns;
        yOfClicked = clickedField / (nbrOfRows+1);

        //field is being assigned with 1(player) or 11(AI) value
        fieldChecker.ArraySpaceSetter(yOfClicked, xOfClicked, 10 * whoseTurn + 1);
        CheckForWinners();

        turnCount++; // next turn
        if (turnCount == 42) Draw(); //check for draw
        if (whoseTurn == 0) whoseTurn = 1; //change player
        else whoseTurn = 0;
    }

    //Method to activate next playable field
    private void ActivateField(int clickedField)
    {
        if(clickedField < 35)// cannot activate field that is out of arrays range
        fieldSpaces[clickedField+7].interactable = true;
    }

    //invoke movement of AI
    public void AIMove()
    {
        FieldClicked(pseudoAI.AIMove());  
    }

    private void GameInitialize()
    {
        fieldChecker.InitializeArray(nbrOfColumns, nbrOfRows);//initialize checking array
        whoseTurn = Random.Range(0,2); //Choose the first player randomly
        turnCount = 0;
        gameOver = false;

        //reset field icons
        for (int i = 0; i < fieldSpaces.Length; i++)
        {
            fieldSpaces[i].GetComponent<Image>().sprite = emptyIcon;
        }

        // activate first row of fields
        for (int i = 0; i < fieldSpaces.Length; i++)
        {
            if(i < nbrOfColumns) fieldSpaces[i].interactable = true;
                else fieldSpaces[i].interactable = false;
        }

        pseudoAI.InitAccessibleFields(); //initialize accessible spaces
        if (whoseTurn == 1) FieldClicked(pseudoAI.AIMove()); // if AI was choosen than it should start with move
    }

    // Start is called before the first frame update
    private void Start()
    {
        fieldChecker = GetComponent<FieldChecker>();
        pseudoAI = GetComponent<PseudoAI>();
        GameInitialize();
    }

    private void CheckForWinners()
    {
        if (turnCount > 5) //check if there was at least 8 turns played
        {
            if (
                    fieldChecker.CheckVerticalFields(xOfClicked,nbrOfRows)
                    || fieldChecker.CheckHorizontalFields(yOfClicked, nbrOfColumns)
                    || fieldChecker.CheckBackSlashFields(xOfClicked, yOfClicked, nbrOfRows, nbrOfColumns)
                    || fieldChecker.CheckSlashFields(xOfClicked, yOfClicked, nbrOfRows, nbrOfColumns)
                ) GameWon();
        }
    }

    //update score text and value of both players
    private void Draw()
    {
        gameOver = true;
        winnerText.text = "It's a draw!";
        playersScore++;
        aiScore++;
        aiScoreText.text = aiScore.ToString();
        playersScoreText.text = playersScore.ToString();
        ScorePannel.gameObject.SetActive(true);
    }

    //update score text and value for the winner
    private void GameWon()
    {
        gameOver = true;
        if (whoseTurn == 0)
        {
            playersScore++;
            playersScoreText.text = playersScore.ToString();
            winnerText.text = "Player Wins!";
        }
        else if (whoseTurn == 1)
        {
            aiScore++;
            aiScoreText.text = aiScore.ToString();
            winnerText.text = "AI Wins!";
        }
        ScorePannel.gameObject.SetActive(true);
    }

    public void PlaAgainClicked()
    {
        GameInitialize();
        ScorePannel.gameObject.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
