using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class G7_L2_machineMohabbat : MonoBehaviour
{
    
    public AudioClip RejectedSound;
    public Button ResetButton;
    public GameObject HeadDisplay;
    public TextMesh CurrentStateText;
    public GameObject Plane;
    public GameObject TapeHead;
    public AudioClip headMovementSound;
    public AudioSource headAudioSource;
    public AudioSource CubeRotationSoundSource;
    public AudioClip CubeRotationSound;
    public AudioClip AcceptedSound;
    public AudioSource AcceptedSoundSource;
    public AudioSource RejectedSoundSource;
    private GameObject cube;
    private float positionCube;
    private string CurrentState;
    private List<char> Word = new List<char>();
    private int HeadPosition = 2;
    private char symbol;
    private string inputText;
    private int MovementSpeed;
    private GameObject CurrentCube;
    bool isAnimationStoped = true;
    bool IsMovementStoped = true;

    public GameObject acceptSmoke1;
    public GameObject acceptSmoke2;
    public GameObject acceptSmoke3;
    public GameObject CubeFire;

    void Start()
    {

        acceptSmoke1.SetActive(false);
        acceptSmoke2.SetActive(false);
        acceptSmoke3.SetActive(false);
        CubeFire.SetActive(false);
        positionCube = 0;
        MovementSpeed = 8;
        symbol = 'Δ';
        HeadPosition = 2;
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
            SceneManager.LoadScene("G7_L2_machineMohabbat");
    }

    private void InitializeMachine()
    {


        string input = symbol.ToString() + symbol.ToString() + inputText + symbol.ToString()+ symbol.ToString();
        Word = ConvertToCharList(input);
        initiateTM(Word);
        CurrentState = "q0";
        CurrentStateText.GetComponent<TextMesh>().text = "Initial State : " + CurrentState;
    }

    public void ResetMachine()
    {
        SceneManager.LoadScene("G7_L2_inputMohabbat");
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

    void initiateTM(List<char> list)
    {
        foreach (char c in list)
        {
            addCube(c.ToString());
        }
        GameObject.FindGameObjectWithTag("cube").SetActive(false);
    }

    void addCube(string text)
    {
        CreateCube(text, positionCube);
        positionCube += 300;
    }

    void CreateCube(string _cubeText, float position_X)
    {
        cube = Instantiate(GameObject.FindGameObjectWithTag("cube")) as GameObject;
        cube.transform.position = new Vector3(position_X, 120f, 0f);
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


    private void UpdateHeadDisplay(Color screenColor, string displayText, bool isRejected)
    {
        Plane.GetComponent<Renderer>().material.SetColor("_Color", screenColor);
        CurrentStateText.GetComponent<TextMesh>().text = displayText;
        ResetButton.gameObject.SetActive(true);

        if (isRejected)
        {
            CubeFire.SetActive(true);
           

        }
        else
        {
            
            acceptSmoke1.SetActive(true);
            acceptSmoke2.SetActive(true);
            acceptSmoke3.SetActive(true);
        }
    }

    public void write()
    {

        int currentPosition = Convert.ToInt32(TapeHead.transform.position.x);
        int newPosition = Convert.ToInt32(HeadPosition * 300);
        if (currentPosition < newPosition)
        {
            currentPosition += MovementSpeed;
            TapeHead.transform.position = new Vector3(currentPosition, 200, 0);

            if (currentPosition + MovementSpeed >= newPosition)
            {
                TapeHead.transform.position = new Vector3(newPosition, 200, 0);
            }

        }
        else if (currentPosition > newPosition)
        {

            currentPosition -= MovementSpeed;
            TapeHead.transform.position = new Vector3(currentPosition, 200, 0);
            if (newPosition >= currentPosition - MovementSpeed)
            {
                TapeHead.transform.position = new Vector3(newPosition, 200, 0);
            }
        }
        else
        {
            headAudioSource.Stop();
            IsMovementStoped = true;
        }



    }

    private char machineCurrentPosition()
    {
        return Word[HeadPosition];
    }


    enum Movement
    {
        R = 1,
        L = -1,
        H = 0
    }

    public bool GetNextState()
    {

        switch (CurrentState)
        {
            case "q0":
                switch (machineCurrentPosition())
                {
                    case 'a':
                        PerformTransaction('x', Movement.R, "q1");
                        break;

                    case 'y':
                        PerformTransaction('y', Movement.R, "q4");
                        break;

                    case 'Δ':
                        PerformTransaction('Δ', Movement.H, "q12");
                        break;

                    default:
                        UpdateHeadDisplay(Color.red, " String Rejected", true);
                        RejectedSoundSource.Play();
                        return true;
                }

                break;

            case "q1":
                switch (machineCurrentPosition())
                {
                    case 'a':
                        PerformTransaction('a', Movement.R, "q1");
                        break;

                    case 'y':
                        PerformTransaction('y', Movement.R, "q1");
                        break;

                    case 'b':
                        PerformTransaction('y', Movement.R, "q2");
                        break;

                    default:
                        UpdateHeadDisplay(Color.red, " String Rejected", true);
                        RejectedSoundSource.Play();
                        return true;
                }
                break;

            case "q2":
                switch (machineCurrentPosition())
                {
                    case 'b':
                        PerformTransaction('b', Movement.R, "q2");
                        break;

                    case 'c':
                        PerformTransaction('c', Movement.R, "q2");
                        break;

                    case 'Δ':
                        PerformTransaction('c', Movement.L, "q3");
                        break;


                    default:
                        UpdateHeadDisplay(Color.red, " String Rejected", true);
                        RejectedSoundSource.Play();
                        return true;
                }
                break;

            case "q3":
                switch (machineCurrentPosition())
                {
                    case 'a':
                        PerformTransaction('a', Movement.L, "q3");
                        break;

                    case 'b':
                        PerformTransaction('b', Movement.L, "q3");
                        break;

                    case 'y':
                        PerformTransaction('y', Movement.L, "q3");
                        break;

                    case 'c':
                        PerformTransaction('c', Movement.L, "q3");
                        break;

                    case 'x':
                        PerformTransaction('x', Movement.R, "q0");
                        break;

                    default:
                        UpdateHeadDisplay(Color.red, " String Rejected", true);
                        RejectedSoundSource.Play();
                        return true;
                }
                break;

            case "q4":
                switch (machineCurrentPosition())
                {
                    case '#':
                        PerformTransaction('#', Movement.R, "q4");
                        break;

                    case 'b':
                        PerformTransaction('b', Movement.R, "q4");
                        break;

                    case 'y':
                        PerformTransaction('y', Movement.R, "q4");
                        break;

                    case 'c':
                        PerformTransaction('#', Movement.R, "q5");
                        break;


                    default:
                        UpdateHeadDisplay(Color.red, " String Rejected", true);
                        RejectedSoundSource.Play();
                        return true;
                }
                break;

            case "q5":
                switch (machineCurrentPosition())
                {
                    case 'c':
                        PerformTransaction('c', Movement.L, "q6");
                        break;

                    case 'Δ':
                        PerformTransaction('Δ', Movement.L, "q11");
                        break;

                    default:
                        UpdateHeadDisplay(Color.red, " String Rejected", true);
                        RejectedSoundSource.Play();
                        return true;
                }
                break;

            case "q6":
                switch (machineCurrentPosition())
                {
                    case '#':
                        PerformTransaction('#', Movement.L, "q6");
                        break;

                    case 'b':
                        PerformTransaction('b', Movement.L, "q6");
                        break;

                    case 'y':
                        PerformTransaction('y', Movement.L, "q6");
                        break;

                    case 'x':
                        PerformTransaction('a', Movement.L, "q7");
                        break;

                    default:
                        UpdateHeadDisplay(Color.red, " String Rejected", true);
                        RejectedSoundSource.Play();
                        return true;
                }
                break;

            case "q7":
                switch (machineCurrentPosition())
                {
                    case 'x':
                        PerformTransaction('a', Movement.L, "q7");
                        break;

                    case 'Δ':
                        PerformTransaction('Δ', Movement.R, "q8");
                        break;

                    default:
                        UpdateHeadDisplay(Color.red, " String Rejected", true);
                        RejectedSoundSource.Play();
                        return true;
                }
                break;

            case "q8":
                switch (machineCurrentPosition())
                {
                    case 'a':
                        PerformTransaction('x', Movement.R, "q9");
                        break;

                    case 'y':
                        PerformTransaction('y', Movement.R, "q4");
                        break;

                    default:
                        UpdateHeadDisplay(Color.red, " String Rejected", true);
                        RejectedSoundSource.Play();
                        return true;
                }
                break;

            case "q9":
                switch (machineCurrentPosition())
                {
                    case 'a':
                        PerformTransaction('a', Movement.R, "q9");
                        break;

                    case 'y':
                        PerformTransaction('y', Movement.R, "q9");
                        break;

                    case 'b':
                        PerformTransaction('y', Movement.L, "q10");
                        break;

                    default:
                        UpdateHeadDisplay(Color.red, " String Rejected", true);
                        RejectedSoundSource.Play();
                        return true;
                }
                break;

            case "q10":
                switch (machineCurrentPosition())
                {
                    case 'a':
                        PerformTransaction('a', Movement.L, "q10");
                        break;

                    case 'y':
                        PerformTransaction('y', Movement.L, "q10");
                        break;

                    case 'x':
                        PerformTransaction('x', Movement.R, "q8");
                        break;

                    default:
                        UpdateHeadDisplay(Color.red, " String Rejected", true);
                        RejectedSoundSource.Play();
                        return true;
                }
                break;

            case "q11":
                switch (machineCurrentPosition())
                {
                    case '#':
                        PerformTransaction('#', Movement.L, "q11");
                        break;

                    case 'y':
                        PerformTransaction('y', Movement.L, "q11");
                        break;

                    case 'x':
                        PerformTransaction('x', Movement.L, "q11");
                        break;

                    case 'Δ':
                        PerformTransaction('Δ', Movement.H, "q12");
                        break;

                    default:
                        UpdateHeadDisplay(Color.red, " String Rejected", true);
                        RejectedSoundSource.Play();
                        return true;
                }
                break;

            case "q12":
                machineCurrentPosition();
                UpdateHeadDisplay(Color.green, " String Accepted", false);
                AcceptedSoundSource.Play();
                return true;
        }
        return false;
    }

    private void addExtraCube(int position_X, string text)
    {
        cube = Instantiate(GameObject.FindGameObjectWithTag("cubeMachine")) as GameObject;
        cube.transform.position = new Vector3(position_X, 120f, 0f);
        cube.transform.localScale = new Vector3(150f, 150f, 150f);
        cube.name = "CubeMachine" + text;
        cube.tag = "cubeMachine";
        TextMesh cubeText = cube.GetComponentInChildren<TextMesh>();
        cubeText.text = text;
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

        }
        else if (HeadPosition >= Word.Count-1)
        {
            addExtraCube(HeadPosition * 300 + 300, symbol.ToString());
            Word.Add(symbol);
        }
        CurrentState = NextState;
        CurrentStateText.GetComponent<TextMesh>().text = "Current State : " + CurrentState;
    }

    
}
