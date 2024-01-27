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
    // Start is called before the first frame update
    void Start()
    {
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

        if (isFarting && fartBar.value > 0)
        {
            fartBar.value -= fartingSpeed * Time.deltaTime;
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
        }
        else if (fartBar.value < 1)
        {
            fartBar.value += fartingSpeed * Time.deltaTime;
        }
    }

    //The player has lost
    void Lose()
    {

    }
}
