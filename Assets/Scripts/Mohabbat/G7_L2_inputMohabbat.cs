using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class G7_L2_inputMohabbat : MonoBehaviour
{
    public Button BuildButton;
    public Button BackButton;
    public InputField InputText;
    string InputString;
    // Start is called before the first frame update
    void Start()
    {
        BuildButton.onClick.AddListener(inputString);
        BackButton.onClick.AddListener(mainMenu);
    }

    private void mainMenu()
    {
        SceneManager.LoadScene("Main_Menu");
    }

    private void inputString()
    {
        InputString = InputText.text.Trim();
        Regex regex = new Regex("^[a-b]*$");
        if (!regex.IsMatch(InputString))
        {
            EditorUtility.DisplayDialog("Invalid Input", "String must contain only a and b", "ok");
            InputText.text = string.Empty;
            return;
        }
        else
            SceneManager.LoadScene("G7_L2_machineMohabbat");
    }

    void OnDisable()
    {
        PlayerPrefs.SetString("InputString", InputString);
        PlayerPrefs.SetString("check", "ok");
    }
}
