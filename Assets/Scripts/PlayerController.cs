using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    Rigidbody myRb;  //Player rigidbody
    public Animator anim;   //Character animator
    [SerializeField] GameObject player; // Character game object & particle effect spawn point
    [SerializeField] GameObject normalPlayer, rockStarPlayer, doctorPlayer; //our characters

    public bool isPlaying; // is player moving?
    public bool isFinish;  // is game finish?

    [SerializeField] float forwardSpeed = 12;
    [SerializeField] float sideSpeed = 0.1f;

    public int score; //for character control
    public Slider playerSlider, progressBar; //player's slider
    public TextMeshProUGUI playerTMP;

    [SerializeField] GameObject bookFx, guitarFx;

    [SerializeField]int positiveComboScore, negativeComboScore; //combo score
    [SerializeField] TextMeshProUGUI comboText; //combo score's text
    float nextResetTime = 0f; // using for reset combo 

    bool isChanged; //using for understand character change
    public ManagerScript gameManager;


    Touch touch;
    void Start()
    {
        myRb = GetComponent<Rigidbody>();
        anim = player.GetComponent<Animator>();
        score = 0;

        comboText.gameObject.SetActive(false);
    }

    
    void Update()
    {
        player.transform.eulerAngles = new Vector3(0, player.transform.eulerAngles.y, 0);
        player.transform.position = new Vector3(transform.position.x, 0, transform.position.z);

        playerSlider.value = score;

        CharacterControl();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            isPlaying = true;
            anim.SetBool("isStarted", true);
        }
        if (isPlaying)
        {
            MoveForward();
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                transform.position = new Vector3(Mathf.Clamp(transform.position.x - sideSpeed, -4f, 4f), transform.position.y, transform.position.z);
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                transform.position = new Vector3(Mathf.Clamp(transform.position.x + sideSpeed, -4f, 4f), transform.position.y, transform.position.z);
            }

            if (Input.touchCount > 0)
            {
                touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Moved)
                {
                    transform.position = new Vector3(Mathf.Clamp(transform.position.x + touch.deltaPosition.x * sideSpeed, -4f, 4f), transform.position.y, transform.position.z);
                }
            }
        }

        
        

        if (Time.time > nextResetTime)
        {
            comboText.gameObject.SetActive(false);
            positiveComboScore = 0;
            negativeComboScore = 0;
        }
    }

    //forward movement
    void MoveForward()
    {
       myRb.velocity = Vector3.forward * forwardSpeed;
    }

    //Character change
    void CharacterControl()
    {
        if (score >= -3 && score <= 4)
        {
            player = normalPlayer;
            anim = player.GetComponent<Animator>();

            if (isChanged)
            {
                anim.SetBool("isChanged", true);
            }

            normalPlayer.SetActive(true);
            rockStarPlayer.SetActive(false);
            doctorPlayer.SetActive(false);

            playerTMP.text = "Normal";
            playerTMP.color = new Color(1.0f, 0.64f, 0.0f);
        }

        if (score < -3)
        {
            isChanged = true;
            player = rockStarPlayer;
            anim = player.GetComponent<Animator>();

            normalPlayer.SetActive(false);
            rockStarPlayer.SetActive(true);
            doctorPlayer.SetActive(false);

            playerTMP.text = "RockStar";
            playerTMP.color = new Color(255f, 0f, 0f);
        }

        if (score > 4)
        {
            isChanged = true;
            player = doctorPlayer;
            anim = player.GetComponent<Animator>();

            normalPlayer.SetActive(false);
            rockStarPlayer.SetActive(false);
            doctorPlayer.SetActive(true);

            playerTMP.text = "Doctor";
            playerTMP.color = new Color(0, 210, 0);
        }
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "guitar")
        {
            Destroy(col.gameObject);
            score--;

            GameObject fx = Instantiate(guitarFx, new Vector3(transform.position.x, 1.5f, transform.position.z - 1), Quaternion.identity, transform);
            Destroy(fx, 1);

            nextResetTime = Time.time + 0.5f;
            comboText.gameObject.SetActive(true);
            positiveComboScore = 0;
            negativeComboScore++;
            comboText.text = "-" + negativeComboScore.ToString();
            comboText.color = new Color(255f, 0f, 0f);
        }

        if (col.gameObject.tag == "book")
        {
            Destroy(col.gameObject);
            score++;

            GameObject fx = Instantiate(bookFx, new Vector3(transform.position.x, 1.5f, transform.position.z - 1), Quaternion.identity, transform);
            Destroy(fx, 1);

            positiveComboScore++;
            nextResetTime = Time.time + 0.5f;
            comboText.gameObject.SetActive(true);
            negativeComboScore = 0;
            comboText.text = "+" + positiveComboScore.ToString();
            comboText.color = new Color(0, 210, 0);

            if (score >= 20)
            {
                score = 20;
            }
        }

        if (col.gameObject.tag == "GoodChoice")
        {

            score = score + 5;
            GameObject a = col.transform.gameObject;
            GameObject b = a.transform.parent.gameObject;
            Destroy(b);

            GameObject fx = Instantiate(bookFx, new Vector3(transform.position.x, 1.5f, transform.position.z - 1), Quaternion.identity, transform);
            Destroy(fx, 1);

            nextResetTime = Time.time + 0.5f;
            positiveComboScore = positiveComboScore + 5;
            comboText.gameObject.SetActive(true);
            comboText.text = "+" + positiveComboScore.ToString();
            comboText.color = new Color(0, 210, 0);

            if (score >= 20)
            {
                score = 20;
            }
        }

        if (col.gameObject.tag == "BadChoice")
        {
            score = score - 5;
            GameObject a = col.transform.gameObject;
            GameObject b = a.transform.parent.gameObject;
            Destroy(b);

            GameObject fx = Instantiate(guitarFx, new Vector3(transform.position.x, 1.5f, transform.position.z - 1), Quaternion.identity, transform);
            Destroy(fx, 1);

            nextResetTime = Time.time + 0.5f;
            negativeComboScore = negativeComboScore + 5;
            comboText.gameObject.SetActive(true);
            comboText.text = "-" + negativeComboScore.ToString();
            comboText.color = new Color(255f, 0f, 0f);

            if (score <= -20)
            {
                score = -20;
            }
        }

        if (col.gameObject.tag == "finishLine")
        {
            isPlaying = false;

            if (score < -4)
            {
                gameManager.RockStarEnd();

                anim.SetBool("isFinished", true);

                progressBar.gameObject.SetActive(false);
                playerSlider.gameObject.SetActive(false);
                playerTMP.gameObject.SetActive(false);
            }

            if (score > 4)
            {
                gameManager.DoctorEnd();

                anim.SetBool("isFinished", true);

                progressBar.gameObject.SetActive(false);
                playerSlider.gameObject.SetActive(false);
                playerTMP.gameObject.SetActive(false);
            }

            if(score >= -3 && score <= 4)
            {
                gameManager.NormalEnd();

                anim.SetBool("isFinished", true);

                progressBar.gameObject.SetActive(false);
                playerSlider.gameObject.SetActive(false);
                playerTMP.gameObject.SetActive(false);
            }
        }
    }

}
