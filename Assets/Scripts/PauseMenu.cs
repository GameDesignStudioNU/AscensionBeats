using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    private EventSystem _ES;

    private Color mainColor = new Color(120, 230, 230);
    private int buttonIndex = 0;
    private Button[] buttonsArray;

    private float cooldown = .25f;
    private float CD;

    private void Awake()
    {
        _ES = EventSystem.current;

        var buttons = transform.Find("Buttons");
        buttonsArray = gameObject.GetComponentsInChildren<Button>();

        var resumebtn = buttons.Find("Resume").GetComponent<Button>();
        resumebtn.onClick.AddListener(ResumeGame);

        var mainbtn = buttons.Find("Main Menu").GetComponent<Button>();
        mainbtn.onClick.AddListener(ReturnToMenu);
    }

    private void ResumeGame()
    {
        FindObjectOfType<GameManager>().NOT_PAUSED = true;
    }

    private void ReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }

    private void Update()
    {
        if (CD > 0)
            CD -= Time.deltaTime;

        _ES.SetSelectedGameObject(buttonsArray[buttonIndex].gameObject);
        
        foreach (Button button in buttonsArray)
        {
            ColorBlock colors = button.colors;
            if (button.gameObject == _ES.currentSelectedGameObject)
            {
                colors.normalColor = button.colors.highlightedColor;

                if (FindObjectOfType<CharacterController2D>().State.JumpButtonPress)
                {
                    if (button.name == "Resume")
                        ResumeGame();
                    if (button.name == "Main Menu")
                        ReturnToMenu();
                }
            }
            else
            {
                colors.normalColor = mainColor;
            }

        }

        if (FindObjectOfType<CharacterController2D>().State.DownButtonHold && CD <= 0)
        {
            buttonIndex++;
            CD = cooldown;
        }
        else if (FindObjectOfType<CharacterController2D>().State.UpButtonHold && CD <= 0)
        {
            buttonIndex--;
            CD = cooldown;
        }
        buttonIndex = Mathf.Abs(buttonIndex) % 2;
    }

}
