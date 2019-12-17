using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class G7_L1_inputSidra : MonoBehaviour
{

    public Button BuildButton;
    public Button BackButton;
    public InputField InputText;
    string InputString;
    // Start is called before the first frame update
    void Start()
    {
        BuildButton.onClick.AddListener(userInput);
        BackButton.onClick.AddListener(MainMenu);
    }

    

    private void userInput()
    {
        InputString = InputText.text.Trim();
        Regex regex = new Regex("^[a-b]*$");
        if (!regex.IsMatch(InputString))
        {
            EditorUtility.DisplayDialog("Invalid Input", "Only a,b are allowed in input", "ok");
            InputText.text = string.Empty;
            return;
        }
        else
            SceneManager.LoadScene("G7_L1_machineSidra");
        
    }

    void OnDisable()
    {
        PlayerPrefs.SetString("InputString",InputString);
        PlayerPrefs.SetString("check", "ok");
    }
    private void MainMenu()
    {
        SceneManager.LoadScene("Main_Menu");
    }

}
