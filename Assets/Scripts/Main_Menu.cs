using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Main_Menu : MonoBehaviour
{
    public Button palindrom_btn;
    public Button Sidra_btn;
    public Button Tanveer_btn;
    public Button muhabat_btn;
    public Button close_btn;

    // Start is called before the first frame update
    void Start()
    {
        palindrom_btn.onClick.AddListener(LoadPalidrom);
        Sidra_btn.onClick.AddListener(LoadSidraMachine);
        Tanveer_btn.onClick.AddListener(LoadTanveerMachine);
        muhabat_btn.onClick.AddListener(LoadMuhabatMachine);
        close_btn.onClick.AddListener(closeApp);
    }

    private void closeApp()
    {
        Application.Quit(1);
    }

    private void LoadMuhabatMachine()
    {
        SceneManager.LoadScene("G7_L2_inputMohabbat");
    }

    private void LoadTanveerMachine()
    {
        SceneManager.LoadScene("G7_L3_inputTanveer");
    }

    private void LoadSidraMachine()
    {
        SceneManager.LoadScene("G7_L1_inputSidra");
    }

    private void LoadPalidrom()
    {
        SceneManager.LoadScene("palindrome_input");
    }
}
