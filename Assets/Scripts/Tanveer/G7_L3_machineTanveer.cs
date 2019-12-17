using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class G7_L3_machineTanveer : MonoBehaviour
{
   
    public Button ResetButton;
    public GameObject HeadDisplay;
    public TextMesh CurrentStateText;
    public GameObject TapeHead;
    public AudioClip headMovementSound;
    public AudioSource headAudioSource;
    public AudioSource CubeRotationSoundSource;
    public AudioClip CubeRotationSound;
    public AudioClip AcceptedSound;
    public AudioSource AcceptedSoundSource;
    public AudioSource RejectedSoundSource;
    public AudioClip RejectedSound;
    private GameObject cube;
    private float positionCube;
    private string current_state;
    private List<char> Word = new List<char>();
    private int HeadPosition = 1;
    private char symbol;
    private string inputText;
    private int MovementSpeed;
    private GameObject CurrentCube;
    bool isAnimationStoped = true;
    bool IsMovementStoped = true;


    public GameObject accpetParticle1;
    public GameObject accpetParticle2;
    public GameObject accpetParticle3;
    public GameObject accpetParticle4;
    public GameObject accpetParticle5;

    public GameObject rejectParticle;

    void Start()
    {

        
        headAudioSource.clip = headMovementSound;
        CubeRotationSoundSource.clip = CubeRotationSound;
        AcceptedSoundSource.clip = AcceptedSound;
        RejectedSoundSource.clip = RejectedSound;
        positionCube = 0;
        MovementSpeed = 8;
        symbol = 'Δ';
        HeadPosition = 1;
        current_state = "q0";
        accpetParticle1.SetActive(false);
        accpetParticle2.SetActive(false);
        accpetParticle3.SetActive(false);
        accpetParticle4.SetActive(false);
        accpetParticle5.SetActive(false);
        rejectParticle.SetActive(false);
        InitializeMachine();
        ResetButton.onClick.AddListener(ResetMachine);
        ResetButton.gameObject.SetActive(false);
        


    }

    void OnEnable()
    {
        inputText = PlayerPrefs.GetString("InputString");
        if (PlayerPrefs.GetString("check") == "")
            SceneManager.LoadScene("s_input");
    }

    private void InitializeMachine()
    {


        string input = symbol.ToString() + inputText + symbol.ToString();
        Word = ConvertToCharList(input);
        CreateMachine(Word);
        current_state = "q0";
        CurrentStateText.GetComponent<TextMesh>().text = "Initial State : " + current_state;
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
    public void ResetMachine()
    {
        SceneManager.LoadScene("G7_L3_inputTanveer");
    }
    bool isHalt = false;
    void Update()
    {


        if (Input.GetKeyDown(KeyCode.Space) && IsMovementStoped && !isHalt)
        {
            IsMovementStoped = false;
            headAudioSource.Play();
            System.Threading.Thread.Sleep(200);
            isHalt = Next();
            
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
   

    private char getCurrentChar()
    {
        return Word[HeadPosition];
    }
    enum Movement
    {
        R = 1,
        L = -1,
        H = 0
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


    private bool Next()
    {

        switch (current_state)
        {
            case "q0":
                switch (getCurrentChar())
                {
                    case '0':
                        PerformTransaction('a', Movement.R, "q1");
                        break;

                    case '1':
                        PerformTransaction('b', Movement.R, "q7");
                        break;

                    case '#':
                        PerformTransaction('#', Movement.R, "q13");
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
                    case '1':
                        PerformTransaction('1', Movement.R, "q1");
                        break;


                    case '0':
                        PerformTransaction('0', Movement.R, "q1");
                        break;

                    case '#':
                        PerformTransaction('#', Movement.R, "q2");
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
                    case 'd':
                        PerformTransaction('d', Movement.R, "q2");
                        break;

                    case '0':
                        PerformTransaction('0', Movement.R, "q2");
                        break;

                    case '1':
                        PerformTransaction('1', Movement.R, "q2");
                        break;
                    case 'c':
                        PerformTransaction('c', Movement.R, "q2");
                        break;
                    case '#':
                        PerformTransaction('#', Movement.L, "q3");
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
                    case 'd':
                        PerformTransaction('d', Movement.L, "q3");
                        break;

                    case 'c':
                        PerformTransaction('c', Movement.L, "q3");
                        break;

                    case '0':
                        PerformTransaction('c', Movement.R, "q4");
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
                    case 'd':
                        PerformTransaction('d', Movement.R, "q4");
                        break;

                    case 'c':
                        PerformTransaction('c', Movement.R, "q4");
                        break;

                    case '#':
                        PerformTransaction('#', Movement.R, "q5");
                        break;

                   
                    default:
                        UpdateHeadDisplay(Color.red, " String Rejected", false);
                        RejectedSoundSource.Play();
                        return true;
                }
                break;

            case "q5":
                switch (getCurrentChar())
                {
                    case 'f':
                        PerformTransaction('f', Movement.R, "q5");
                        break;

                    case 'e':
                        PerformTransaction('e', Movement.R, "q5");
                        break;

                    case '0':
                        PerformTransaction('e', Movement.L, "q6");
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
                    case 'd':
                        PerformTransaction('d', Movement.L, "q6");
                        break;

                    case 'e':
                        PerformTransaction('e', Movement.L, "q6");
                        break;

                    case 'c':
                        PerformTransaction('c', Movement.L, "q6");
                        break;

                    case 'f':
                        PerformTransaction('f', Movement.L, "q6");
                        break;
                    case '#':
                        PerformTransaction('#', Movement.L, "q6");
                        break;

                    case '0':
                        PerformTransaction('0', Movement.L, "q6");
                        break;

                    case '1':
                        PerformTransaction('1', Movement.L, "q6");
                        break;
                    case 'a':
                        PerformTransaction('a', Movement.R, "q0");
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
                    case '0':
                        PerformTransaction('0', Movement.R, "q7");
                        break;

                    case '1':
                        PerformTransaction('1', Movement.R, "q7");
                        break;

                    case '#':
                        PerformTransaction('#', Movement.R, "q8");
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
                    case 'd':
                        PerformTransaction('d', Movement.R, "q8");
                        break;

                    case 'c':
                        PerformTransaction('c', Movement.R, "q8");
                        break;

                    case '1':
                        PerformTransaction('1', Movement.R, "q8");
                        break;

                    case '0':
                        PerformTransaction('0', Movement.R, "q8");
                        break;
                    case '#':
                        PerformTransaction('#', Movement.L, "q9");
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
                    case 'd':
                        PerformTransaction('d', Movement.L, "q9");
                        break;

                    case 'c':
                        PerformTransaction('c', Movement.L, "q9");
                        break;

                    case '1':
                        PerformTransaction('d', Movement.R, "q10");
                        break;

                    default:
                        UpdateHeadDisplay(Color.red, " String Rejected", true);
                        RejectedSoundSource.Play();
                        return true;
                }
                break;

            case "q10":
                switch (getCurrentChar())
                {
                    case 'd':
                        PerformTransaction('d', Movement.R, "q10");
                        break;

                    case 'c':
                        PerformTransaction('c', Movement.R, "q10");
                        break;


                    case '#':
                        PerformTransaction('#', Movement.R, "q11");
                        break;

                   

                    default:
                        UpdateHeadDisplay(Color.red, " String Rejected", true);
                        RejectedSoundSource.Play();
                        return true;
                }
                break;
            case "q11":
                switch (getCurrentChar())
                {
                    case 'e':
                        PerformTransaction('e', Movement.R, "q11");
                        break;

                    case 'f':
                        PerformTransaction('f', Movement.R, "q11");
                        break;


                    case '1':
                        PerformTransaction('f', Movement.L, "q12");
                        break;



                    default:
                        UpdateHeadDisplay(Color.red, " String Rejected", true);
                        RejectedSoundSource.Play();
                        return true;
                }
                break;
            case "q12":
                switch (getCurrentChar())
                {
                    case 'd':
                        PerformTransaction('d', Movement.L, "q12");
                        break;

                    case 'e':
                        PerformTransaction('e', Movement.L, "q12");
                        break;

                    case 'c':
                        PerformTransaction('c', Movement.L, "q12");
                        break;

                    case 'f':
                        PerformTransaction('f', Movement.L, "q12");
                        break;
                    case '#':
                        PerformTransaction('#', Movement.L, "q12");
                        break;

                    case '0':
                        PerformTransaction('0', Movement.L, "q12");
                        break;

                    case '1':
                        PerformTransaction('1', Movement.L, "q12");
                        break;
                    case 'b':
                        PerformTransaction('b', Movement.R, "q0");
                        break;



                    default:
                        UpdateHeadDisplay(Color.red, " String Rejected", true);
                        RejectedSoundSource.Play();
                        return true;
                }
                break;
            case "q13":
                switch (getCurrentChar())
                {

                    case 'b':
                        PerformTransaction('b', Movement.R, "q13");
                        break;
                    case 'd':
                        PerformTransaction('d', Movement.R, "q13");
                        break;

                    case 'c':
                        PerformTransaction('c', Movement.R, "q13");
                        break;

                    case 'a':
                        PerformTransaction('a', Movement.R, "q13");
                        break;

                    case 'e':
                        PerformTransaction('e', Movement.R, "q13");
                        break;

                    case 'f':
                        PerformTransaction('f', Movement.R, "q13");
                        break;

                    case '#':
                        PerformTransaction('#', Movement.R, "q13");
                        break;

                    case 'Δ':
                        PerformTransaction('Δ', Movement.H, "q14");
                        break;



                    default:
                        UpdateHeadDisplay(Color.red, " String Rejected", true);
                        RejectedSoundSource.Play();
                        return true;
                }
                break;

            case "q14":
                getCurrentChar();
                UpdateHeadDisplay(Color.green, " String Accepted", false);
                AcceptedSoundSource.Play();
                return true;

            
        }
        return false;
    }
    private void UpdateHeadDisplay(Color screenColor, String displayText, bool isRejected)
    {
        HeadDisplay.GetComponent<Renderer>().material.SetColor("_Color", screenColor);
        CurrentStateText.GetComponent<TextMesh>().text = displayText;
        ResetButton.gameObject.SetActive(true);

        if (isRejected)
        {
            rejectParticle.SetActive(true);
        }
        else
        {

            accpetParticle1.SetActive(true);
            accpetParticle2.SetActive(true);
            accpetParticle3.SetActive(true);
            accpetParticle4.SetActive(true);
            accpetParticle5.SetActive(true);
        }
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
        current_state = NextState;
        CurrentStateText.GetComponent<TextMesh>().text = "Current State : " + current_state;
    }

    public void write()
    {

        int currentPosition = Convert.ToInt32(TapeHead.transform.position.x);
        int newPosition = Convert.ToInt32(HeadPosition * 300);
        if (currentPosition < newPosition)
        {
            currentPosition += MovementSpeed;
            TapeHead.transform.position = new Vector3(currentPosition, 60f, 0);

            if (currentPosition + MovementSpeed >= newPosition)
            {
                TapeHead.transform.position = new Vector3(newPosition, 60f, 0);
            }

        }
        else if (currentPosition > newPosition)
        {

            currentPosition -= MovementSpeed;
            TapeHead.transform.position = new Vector3(currentPosition, 60f, 0);
            if (newPosition >= currentPosition - MovementSpeed)
            {
                TapeHead.transform.position = new Vector3(newPosition, 60f, 0);
            }
        }
        else
        {
            headAudioSource.Stop();
            IsMovementStoped = true;
        }



    }
}
