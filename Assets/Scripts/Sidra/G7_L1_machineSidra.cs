using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
using UnityEditor;

public class G7_L1_machineSidra : MonoBehaviour
{
    
    public Button ResetButton;
    public GameObject HeadDisplay;
    public TextMesh CurrentStateText;
    public GameObject AcceptedParticles;
    private GameObject cube;
    private float positionCube;
    private string CurrentState;
    private List<char> tape = new List<char>();
    private int HeadPosition = 1;
    public GameObject TapeHead;
    public AudioClip headMovementSound;
    public AudioSource headAudioSource;
    public AudioSource CubeRotationSoundSource;
    public AudioClip CubeRotationSound;
    public AudioClip AcceptedSound;
    public AudioSource AcceptedSoundSource;
    public AudioSource RejectedSoundSource;
    public AudioClip RejectedSound;
    private GameObject CurrentCube;
    bool isAnimationStoped = true;
    bool IsMovementStoped = true;
    private char symbol;
    private string inputText;
    private int MovementSpeed;


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
        AcceptedParticles.gameObject.SetActive(false);
        onStartMachine();


    }

    void OnEnable()
    {
        inputText = PlayerPrefs.GetString("InputString");
        if (PlayerPrefs.GetString("check")=="")
            SceneManager.LoadScene("G7_L1_inputSidra");
    }

    private void onStartMachine()
    {


        string input = symbol.ToString() + inputText + symbol.ToString();
        tape = ConvertToCharList(input);
        CreateMachine(tape);
        
        CurrentStateText.GetComponent<TextMesh>().text = "Initial State : " + CurrentState;
    }

    public void ResetMachine()
    {
        SceneManager.LoadScene("G7_L1_inputSidra");
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
        try
        {
            cube = Instantiate(GameObject.FindGameObjectWithTag("cube")) as GameObject;
            cube.transform.position = new Vector3(position_X, 0f, 0f);
            cube.transform.localScale = new Vector3(150f, 150f, 150f);
            cube.name = "CubeMachine" + _cubeText;
            cube.tag = "cubeMachine";
            TextMesh cubeText = cube.GetComponentInChildren<TextMesh>();
            cubeText.text = _cubeText;
        }
        catch { }
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
        writeOnMachine();
    }


    private void UpdateHeadDisplay(Color screenColor, String displayText, bool isRejected)
    {
        HeadDisplay.GetComponent<Renderer>().material.SetColor("_Color", screenColor);
        CurrentStateText.GetComponent<TextMesh>().text = displayText;
        ResetButton.gameObject.SetActive(true);
        if(isRejected)
        {

            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("cubeMachine"))
            {
                obj.AddComponent<Rigidbody>().AddForce(Vector3.back * 500f);
            }
           
        }
        else
        {
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("SmokeFire"))
            {
                obj.SetActive(false);
            }
            AcceptedParticles.gameObject.SetActive(true);
        }

        GameObject.Find("BackgroundMusic").SetActive(false);
    }

    private char getCurrentChar()
    {
        

        return tape[HeadPosition];
    }

    private void cubeAdd(int position_X, string text)
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
                    case 'a':
                        changeOnMachine('n', Movement.R, "q1");
                        break;

                    case 'b':
                        changeOnMachine('j', Movement.R, "q4");
                        break;

                    case 'Δ':
                        changeOnMachine('Δ', Movement.H, "q11");
                        break;

                    default:
                        UpdateHeadDisplay(Color.red, " String Rejected", true);
                        RejectedSoundSource.Play();
                        return true;
                }

                break;

            case "q1":
                switch (getCurrentChar())
                {
                    case 'a':
                        changeOnMachine('a', Movement.R, "q1");
                        break;


                    case 'b':
                        changeOnMachine('b', Movement.R, "q2");
                        break;

                    case 'Δ':
                        changeOnMachine('Δ', Movement.H, "q11");
                        break;

                    default:
                        UpdateHeadDisplay(Color.red, " String Rejected", true);
                        RejectedSoundSource.Play();
                        return true;
                }
                break;

            case "q2":
                switch (getCurrentChar())
                {
                    case 'a':
                        changeOnMachine('k', Movement.L, "q3");
                        break;

                    case 'b':
                        changeOnMachine('b', Movement.R, "q2");
                        break;

                    case 'k':
                        changeOnMachine('k', Movement.R, "q2");
                        break;

                    default:
                        UpdateHeadDisplay(Color.red, " String Rejected", true);
                        RejectedSoundSource.Play();
                        return true;
                }
                break;

            case "q3":
                switch (getCurrentChar())
                {
                    case 'a':
                        changeOnMachine('a', Movement.L, "q3");
                        break;

                    case 'b':
                        changeOnMachine('b', Movement.L, "q3");
                        break;

                    case 'k':
                        changeOnMachine('k', Movement.L, "q3");
                        break;

                    case 'n':
                        changeOnMachine('n', Movement.R, "q0");
                        break;

                    default:
                        UpdateHeadDisplay(Color.red, " String Rejected", true);
                        RejectedSoundSource.Play();
                        return true;
                }
                break;

            case "q4":
                switch (getCurrentChar())
                {
                    case 'a':
                        changeOnMachine('k', Movement.R, "q5");
                        break;

                    case 'k':
                        changeOnMachine('k', Movement.R, "q6");
                        break;

                    case 'b':
                        changeOnMachine('b', Movement.R, "q4");
                        break;

                    case 'Δ':
                        changeOnMachine('Δ', Movement.H, "q11");
                        break;

                    default:
                        UpdateHeadDisplay(Color.red, " String Rejected", true);
                        RejectedSoundSource.Play();
                        return true;
                }
                break;

            case "q5":
                switch (getCurrentChar())
                {
                    case 'a':
                        changeOnMachine('k', Movement.R, "q5");
                        break;

                    case 'b':
                        changeOnMachine('l', Movement.L, "q7");
                        break;

                    case 'k':
                        changeOnMachine('k', Movement.R, "q9");
                        break;

                    default:
                        UpdateHeadDisplay(Color.red, " String Rejected", true);
                        RejectedSoundSource.Play();
                        return true;
                }
                break;

            case "q6":
                switch (getCurrentChar())
                {
                    case 'k':
                        changeOnMachine('k', Movement.R, "q6");
                        break;

                    case 'b':
                        changeOnMachine('l', Movement.L, "q7");
                        break;

                    case 'a':
                        changeOnMachine('k', Movement.R, "q6");
                        break;

                    case 'j':
                        changeOnMachine('j', Movement.R, "q10");
                        break;

                    default:
                        UpdateHeadDisplay(Color.red, " String Rejected", true);
                        RejectedSoundSource.Play();
                        return true;
                }
                break;

            case "q7":
                switch (getCurrentChar())
                {
                    case 'b':
                        changeOnMachine('j', Movement.R, "q8");
                        break;

                    case 'k':
                        changeOnMachine('k', Movement.L, "q7");
                        break;

                    case 'l':
                        changeOnMachine('l', Movement.R, "q7");
                        break;

                    case 'j':
                        changeOnMachine('j', Movement.R, "q10");
                        break;

                    default:
                        UpdateHeadDisplay(Color.red, " String Rejected", true);
                        RejectedSoundSource.Play();
                        return true;
                }
                break;

            case "q8":
                switch (getCurrentChar())
                {
                    case 'k':
                        changeOnMachine('k', Movement.L, "q8");
                        break;

                    case 'b':
                        changeOnMachine('b', Movement.L, "q8");
                        break;

                    case 'l':
                        changeOnMachine('l', Movement.L, "q8");
                        break;

                    case 'j':
                        changeOnMachine('j', Movement.R, "q5");
                        break;

                    default:
                        UpdateHeadDisplay(Color.red, " String Rejected", true);
                        RejectedSoundSource.Play();
                        return true;
                }
                break;

            case "q9":
                switch (getCurrentChar())
                {
                    case 'k':
                        changeOnMachine('k', Movement.R, "q9");
                        break;

                    case 'l':
                        changeOnMachine('l', Movement.R, "q9");
                        break;

                    case 'b':
                        changeOnMachine('l', Movement.R, "q11");
                        break;

                    default:
                        UpdateHeadDisplay(Color.red, " String Rejected" , true);
                        RejectedSoundSource.Play();
                        return true;
                }
                break;

            case "q10":
                switch (getCurrentChar())
                {
                    case 'b':
                        changeOnMachine('l', Movement.R, "q10");
                        break;

                    case 'l':
                        changeOnMachine('l', Movement.R, "q10");
                        break;

                    case 'k':
                        changeOnMachine('k', Movement.R, "q10");
                        break;

                    case 'Δ':
                        changeOnMachine('Δ', Movement.H, "q11");
                        break;

                    default:
                        UpdateHeadDisplay(Color.red, " String Rejected", true);
                        RejectedSoundSource.Play();
                        return true;
                }
                break;

            case "q11":
                getCurrentChar();
                UpdateHeadDisplay(Color.green, " String Accepted", false);
                AcceptedSoundSource.Play();
                return true;


        }
       
        return false;
    }
    enum Movement
    {
        R = 1,
        L = -1,
        H = 0
    }
    public void writeOnMachine()
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

    private void changeOnMachine(char replaceChar, Movement movement, string NextState)
    {
        char tempChar = tape[HeadPosition];
        tape[HeadPosition] = replaceChar;

        Ray ray = new Ray(new Vector3(transform.position.x, transform.position.y - 2, transform.position.z), Vector3.forward); //trying to single select
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.red);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            CurrentCube = hit.collider.gameObject;

            if (tape[HeadPosition] != tempChar)
            {
                CurrentCube.GetComponent<Animator>().Play("WriteCube");
                CubeRotationSoundSource.Play();
            }
            CurrentCube.GetComponentInChildren<TextMesh>().text = replaceChar.ToString();

        }

        HeadPosition += (int)movement;
        if (HeadPosition <= 0)
        {
            cubeAdd(HeadPosition * 300 - 300, symbol.ToString());
            tape.Insert(0, symbol);
            
        }
        else if (HeadPosition >= tape.Count - 2)
        {
            cubeAdd(HeadPosition * 300 + 300, symbol.ToString());
            tape.Add(symbol);
        }
        CurrentState = NextState;
        CurrentStateText.GetComponent<TextMesh>().text = "Current State : " + CurrentState;
    }

}
