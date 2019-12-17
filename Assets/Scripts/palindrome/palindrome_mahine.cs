using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class palindrome_mahine : MonoBehaviour
{
    public GameObject TapeHead;
    public AudioClip headMovementSound;
    public AudioSource headAudioSource;
    public AudioSource CubeRotationSoundSource;
    public AudioClip CubeRotationSound;
    public AudioClip AcceptedSound;
    public AudioSource AcceptedSoundSource;
    public AudioSource RejectedSoundSource;
    public AudioClip RejectedSound;
    public Button ResetButton;
    public GameObject HeadDisplay;
    public TextMesh CurrentStateText;

    private GameObject cube;
    private float positionCube;
    private string CurrentState;
    private List<char> Word = new List<char>();
    private int HeadPosition = 1;
    private char symbol;
    private string inputText;
    private int MovementSpeed;
    private GameObject CurrentCube;
    bool isAnimationStoped = true;
    bool IsMovementStoped = true;

    void Start()
    {

        positionCube = 0;
        MovementSpeed = 8;
        symbol = 'Δ';
        HeadPosition = 1;
        CurrentState = "q0";
        headAudioSource.clip = headMovementSound;
        CubeRotationSoundSource.clip = CubeRotationSound;
        AcceptedSoundSource.clip = AcceptedSound;
        RejectedSoundSource.clip = RejectedSound;

        ResetButton.onClick.AddListener(ResetMachine);
        ResetButton.gameObject.SetActive(false);
        InitializeMachine();


    }

    void OnEnable()
    {
        inputText = PlayerPrefs.GetString("InputString");
        if (PlayerPrefs.GetString("check") == "")
            SceneManager.LoadScene("palindrome_input");
    }

    private void InitializeMachine()
    {


        string input = symbol.ToString() + inputText + symbol.ToString();
        Word = ConvertToCharList(input);
        CreateMachine(Word);

        CurrentStateText.GetComponent<TextMesh>().text = "Initial State : " + CurrentState;
    }

    public void ResetMachine()
    {
        SceneManager.LoadScene("palindrome_input");
    }

    private List<char> ConvertToCharList(string input)
    {
        List<char> list = new List<char>();
        foreach (char c in input)
        {
            list.Add(c);
        }

        return list;
    }

    void CreateMachine(List<char> list)
    {
        foreach (char c in list)
        {
            AddBlock(c.ToString());
        }
        GameObject.FindGameObjectWithTag("cube").SetActive(false);
    }

    void AddBlock(string text)
    {
        CreateCube(text, positionCube);
        positionCube += 300;
    }

    void CreateCube(string _cubeText, float position_X)
    {
        cube = Instantiate(GameObject.FindGameObjectWithTag("cube")) as GameObject;
        cube.transform.position = new Vector3(position_X, 0f, 0f);
        cube.transform.localScale = new Vector3(150f, 150f, 150f);
        cube.name = "CubeMachine" + _cubeText;
        cube.tag = "cubeMachine";
        TextMesh cubeText = cube.GetComponentInChildren<TextMesh>();
        cubeText.text = _cubeText;
    }

    bool isHalt = false;
    void Update()
    {


        if (Input.GetKeyDown(KeyCode.Space) && IsMovementStoped && !isHalt)
        {
            IsMovementStoped = false;
            headAudioSource.Play();
            System.Threading.Thread.Sleep(200);
            isHalt = GetNextState();

        }
        try
        {
            isAnimationStoped = CurrentCube.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("idle");
        }
        catch
        {

        }
        if (isAnimationStoped)
            write();
    }


    private void UpdateHeadDisplay(Color screenColor, String displayText)
    {
        HeadDisplay.GetComponent<Renderer>().material.SetColor("_Color", screenColor);
        CurrentStateText.GetComponent<TextMesh>().text = displayText;
        ResetButton.gameObject.SetActive(true);
    }

    public void write()
    {

        int currentPosition = Convert.ToInt32(TapeHead.transform.position.x);
        int newPosition = Convert.ToInt32(HeadPosition * 300);
        if (currentPosition < newPosition)
        {
            currentPosition += MovementSpeed;
            TapeHead.transform.position = new Vector3(currentPosition, -206, 0);

            if (currentPosition + MovementSpeed >= newPosition)
            {
                TapeHead.transform.position = new Vector3(newPosition, -206, 0);
            }

        }
        else if (currentPosition > newPosition)
        {

            currentPosition -= MovementSpeed;
            TapeHead.transform.position = new Vector3(currentPosition, -206, 0);
            if (newPosition >= currentPosition - MovementSpeed)
            {
                TapeHead.transform.position = new Vector3(newPosition, -206, 0);
            }
        }
        else
        {
            headAudioSource.Stop();
            IsMovementStoped = true;
        }



    }

    private char getCurrentChar()
    {


        return Word[HeadPosition];
    }

    private void addExtraCube(int position_X, string text)
    {
        cube = Instantiate(GameObject.FindGameObjectWithTag("cubeMachine")) as GameObject;
        cube.transform.position = new Vector3(position_X, 0f, 0f);
        cube.transform.localScale = new Vector3(150f, 150f, 150f);
        cube.name = "CubeMachine" + text;
        cube.tag = "cubeMachine";
        TextMesh cubeText = cube.GetComponentInChildren<TextMesh>();
        cubeText.text = text;
    }


    public bool GetNextState()
    {

        switch (CurrentState)
        {
            case "q0":
                switch (getCurrentChar())
                {
                    case '0':
                        PerformTransaction('Δ', Movement.R, "q1");
                        break;
                    case '1':
                        PerformTransaction('Δ', Movement.R, "q4");
                        break;

                    case 'Δ':
                        PerformTransaction('Δ', Movement.H, "q6");
                        UpdateHeadDisplay(Color.green, " String Accepted");
                        AcceptedSoundSource.Play();
                        return true;
                      

                    default:
                        UpdateHeadDisplay(Color.red, " String Rejected");
                        RejectedSoundSource.Play();
                        return true;
                }

                break;

            case "q1":
                switch (getCurrentChar())
                {
                    case '0':
                        PerformTransaction('0', Movement.R, "q1");
                        break;
                    case '1':
                        PerformTransaction('1', Movement.R, "q1");
                        break;

                    case 'Δ':
                        PerformTransaction('Δ', Movement.L, "q2");
                        break;

                    default:
                        UpdateHeadDisplay(Color.red, " String Rejected");
                        RejectedSoundSource.Play();
                        return true;
                }
                break;

            case "q2":
                switch (getCurrentChar())
                {
                    case '0':
                        PerformTransaction('Δ', Movement.L, "q3");
                        break;

                    case 'Δ':
                        PerformTransaction('Δ', Movement.H, "q6");
                        break;

                    default:
                        UpdateHeadDisplay(Color.red, " String Rejected");
                        RejectedSoundSource.Play();
                        return true;
                }
                break;

            case "q3":
                switch (getCurrentChar())
                {
                    case '0':
                        PerformTransaction('0', Movement.L, "q3");
                        break;
                    case '1':
                        PerformTransaction('1', Movement.L, "q3");
                        break;

                    case 'Δ':
                        PerformTransaction('Δ', Movement.R, "q0");
                        break;

                    default:
                        UpdateHeadDisplay(Color.red, " String Rejected");
                        RejectedSoundSource.Play();
                        return true;
                }
                break;

            case "q4":
                switch (getCurrentChar())
                {
                    case '0':
                        PerformTransaction('0', Movement.R, "q4");
                        break;
                    case '1':
                        PerformTransaction('1', Movement.R, "q4");
                        break;

                    case 'Δ':
                        PerformTransaction('Δ', Movement.L, "q5");
                        break;

                    default:
                        UpdateHeadDisplay(Color.red, " String Rejected");
                        RejectedSoundSource.Play();
                        return true;
                }
                break;

            case "q5":
                switch (getCurrentChar())
                {
                    case '1':
                        PerformTransaction('Δ', Movement.L, "q3");
                        break;

                    case 'Δ':
                        PerformTransaction('Δ', Movement.H, "q6");
                        break;

                    default:
                        UpdateHeadDisplay(Color.red, " String Rejected");
                        RejectedSoundSource.Play();
                        return true;
                }
                break;
            
            case "q6":
                getCurrentChar();
                UpdateHeadDisplay(Color.green, " String Accepted");
                AcceptedSoundSource.Play();
                return true;

        }
        return false;
    }

    private void PerformTransaction(char replaceChar, Movement movement, string NextState)
    {
        char tempChar = Word[HeadPosition];
        Word[HeadPosition] = replaceChar;

        Ray ray = new Ray(new Vector3(transform.position.x, transform.position.y - 2, transform.position.z), Vector3.forward); //trying to single select
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.red);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            CurrentCube = hit.collider.gameObject;

            if (Word[HeadPosition] != tempChar)
            {
                CurrentCube.GetComponent<Animator>().Play("WriteCube");
                CubeRotationSoundSource.Play();
            }
            CurrentCube.GetComponentInChildren<TextMesh>().text = replaceChar.ToString();

        }

        HeadPosition += (int)movement;
        if (HeadPosition <= 0)
        {
            addExtraCube(HeadPosition * 300 - 300, symbol.ToString());
            Word.Insert(0, symbol);
            addExtraCube(HeadPosition * 300 - 300, symbol.ToString());
        }
        else if (HeadPosition >= Word.Count - 2)
        {
            addExtraCube(HeadPosition * 300 + 300, symbol.ToString());
            Word.Add(symbol);
        }
        CurrentState = NextState;
        CurrentStateText.GetComponent<TextMesh>().text = "Current State : " + CurrentState;
    }

    enum Movement
    {
        R = 1,
        L = -1,
        H = 0
    }
}
