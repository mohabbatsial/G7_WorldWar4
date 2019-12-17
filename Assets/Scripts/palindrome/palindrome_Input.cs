using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class palindrome_Input : MonoBehaviour
{
    public Button BuildButton;
    public Button BackButton;
    public InputField InputText;
    string InputString;
    // Start is called before the first frame update
    void Start()
    {
        BuildButton.onClick.AddListener(GetInputFromUser);
        BackButton.onClick.AddListener(GoBackToMenu);
    }

    private void GoBackToMenu()
    {
        SceneManager.LoadScene("Main_Menu");
    }

    private void GetInputFromUser()
    {
        InputString = InputText.text.Trim();
        Regex regex = new Regex("^[0-1]*$");
        if (!regex.IsMatch(InputString))
        {
            EditorUtility.DisplayDialog("Invalid Input", "Only 0s and 1s are allowed in input", "ok");
            InputText.text = string.Empty;
            return;
        }
        else
            SceneManager.LoadScene("palindrome_machine");

    }

    void OnDisable()
    {
        PlayerPrefs.SetString("InputString", InputString);
        PlayerPrefs.SetString("check", "ok");
    }
}
