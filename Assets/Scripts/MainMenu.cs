using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEditor;

public class MainMenu : MonoBehaviour
{
    public Texture2D cursor;
    public Image fade;
    public ControllerState2D State { get; private set; }

    private EventSystem _ES;

    private Color mainColor = new Color(120, 230, 230);
    private int buttonIndex = 0;
    private Button[] buttonsArray;

    private float cooldown = .25f;
    private float CD;

    private void Awake()
    {
        _ES = EventSystem.current;
        State = new ControllerState2D();

        var buttons = transform.Find("Buttons");
        buttonsArray = gameObject.GetComponentsInChildren<Button>();

        var startbtn = buttons.Find("Start").GetComponent<Button>();
        startbtn.onClick.AddListener(() => { StartCoroutine(FadeScreen(0)); });

        var tutorialbtn = buttons.Find("Tutorial").GetComponent<Button>();
        tutorialbtn.onClick.AddListener(() => { StartCoroutine(FadeScreen(1)); });

        var quitbtn = buttons.Find("Quit").GetComponent<Button>();
        quitbtn.onClick.AddListener(QuitGame);

        fade.enabled = false;
        // Cursor.SetCursor(cursor, Vector2.zero, CursorMode.Auto);
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

                if (State.JumpButtonPress)
                {
                    if (button.name == "Start")
                        StartCoroutine(FadeScreen(0));
                    if (button.name == "Tutorial")
                        StartCoroutine(FadeScreen(1));
                    if (button.name == "Quit")
                        QuitGame();
                }
            }
            else
            {
                colors.normalColor = mainColor;
            }

        }

        if (State.DownButtonHold && CD <= 0)
        {
            buttonIndex++;
            CD = cooldown;
        }
        else if (State.UpButtonHold && CD <= 0)
        {
            buttonIndex += 2;
            CD = cooldown;
        }

        buttonIndex %= 3;
    }



    private void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    private IEnumerator FadeScreen(int stage)
    {

        fade.enabled = true;
        float alpha = 0f;
        while (alpha < 1f)
        {
            alpha += Time.deltaTime / 1.5f;
            var c = fade.color;
            c.a = alpha;
            fade.color = c;
            yield return null;
        }
        if (stage == 0) { SceneManager.LoadScene("Level0"); }
        else if (stage == 1) { SceneManager.LoadScene("Tutorial"); }
    }

    private void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
