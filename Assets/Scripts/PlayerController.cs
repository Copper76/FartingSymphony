using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private NoteController noteController;

    [SerializeField] private Slider fartBar;

    [SerializeField] private TextMeshProUGUI pointText;
    private float nextReward;
    private float rewardInterval;
    private int points;
    private int rewardPoint;

    private bool isFarting;

    private float fartingSpeed;

    private int currentDir;
    // Start is called before the first frame update
    void Start()
    {
        fartBar.value = 0.5f;
        currentDir = -1;
        isFarting = false;
        fartingSpeed = 0.1f;
        rewardInterval = 0.1f;
        rewardPoint = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fart"))
        {
            isFarting = true;
        }

        if (Input.GetButtonUp("Fart"))
        {
            Debug.Log("I'm done");
            isFarting = false;
        }

        if (Input.GetButtonDown("Left") && currentDir == -1)
        {
            currentDir = 0;
        }

        if (Input.GetButtonUp("Left") && currentDir == 0)
        {
            currentDir = -1;
        }

        if (Input.GetButtonDown("Up") && currentDir == -1)
        {
            currentDir = 1;
        }

        if (Input.GetButtonUp("Up") && currentDir == 1)
        {
            currentDir = -1;
        }

        if (Input.GetButtonDown("Right") && currentDir == -1)
        {
            currentDir = 2;
        }

        if (Input.GetButtonUp("Right") && currentDir == 2)
        {
            currentDir = -1;
        }

        if (Input.GetButtonDown("Down") && currentDir == -1)
        {
            currentDir = 3;
        }

        if (Input.GetButtonUp("Down") && currentDir == 3)
        {
            currentDir = -1;
        }

        if (Time.time > noteController.introTime)
        {
            if (isFarting && fartBar.value > 0)
            {
                if (noteController.canFart())
                {
                    fartBar.value -= fartingSpeed * Time.deltaTime;
                }
                else
                {
                    fartBar.value += fartingSpeed * 1.5f * Time.deltaTime;
                }
                /**
                if (Time.time > nextReward)
                {
                    if (noteController.canFart())
                    {
                        points += rewardPoint;
                        pointText.text = ("Score: " + points.ToString());
                    }
                    else
                    {
                        points -= rewardPoint;
                        points = Mathf.Max(0, points);
                        pointText.text = ("Score: " + points.ToString());
                    }
                    nextReward += rewardInterval;
                }
                **/
            }
            else if (fartBar.value < 1)
            {
                fartBar.value += fartingSpeed * Time.deltaTime;
            }

            if (fartBar.value >= 1.0f)
            {
                Lose();
            }
        }

        if (noteController.MatchDir(currentDir) && Time.time > nextReward)
        {
            if (noteController.ConsumeNote())
            {
                points += rewardPoint * 5;
            }
            else
            {
                points += rewardPoint;
            }
            pointText.text = ("Score: " + points.ToString());
            nextReward += rewardInterval;
        }
    }

    //The player has lost
    void Lose()
    {

    }
}
