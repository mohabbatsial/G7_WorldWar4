using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class G7_L3_inputTanveer : MonoBehaviour
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
    void OnDisable()
    {
        PlayerPrefs.SetString("InputString", InputString);
        PlayerPrefs.SetString("check", "ok");
    }
    private void GetInputFromUser()
    {
        InputString = InputText.text.Trim();
        Regex regex = new Regex("^[0,1,#]*$");
        if (!regex.IsMatch(InputString))
        {
            EditorUtility.DisplayDialog("Invalid Input", "Combinations of 0 ,1 and # are allowed in input only.", "ok");
            InputText.text = string.Empty;
            return;
        }
        else
            SceneManager.LoadScene("G7_L3_machineTanveer");

    }

    
}
