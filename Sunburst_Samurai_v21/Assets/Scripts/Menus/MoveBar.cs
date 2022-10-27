using RPG.Control;
using RPG.Display;
using RPG.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveBar : MonoBehaviour
{
    // This system of using a bool for each letter is definitely not efficient
    // Think about this one and make it better

    GameObject hud;

    public Image qMoveLoadImage;
    public Image wMoveLoadImage;
    public Image eMoveLoadImage;
    public Image rMoveLoadImage;

    [SerializeField] float qReloadTime = 5.0f;
    [SerializeField] float wReloadTime = 5.0f;

    bool qReset = false;
    bool wReset = false;
    bool eReset = false;
    bool rReset = false;

    float qCounter = 0f;
    float wCounter = 0f;
    float eCounter = 0f;
    float rCounter = 0f;

    float qCounterRatio = 0f;
    float wCounterRatio = 0f;
    float eCounterRatio = 0f;
    float rCounterRatio = 0f;

    bool qBeginCountdown;
    bool wBeginCountdown;
    bool eBeginCountdown;
    bool rBeginCountdown;

    // Start is called before the first frame update
    void Start()
    {
        hud = GameObject.FindWithTag("HUD");
    }

    // Update is called once per frame
    void Update()
    {
        CheckForCount();
    }

    public void StartReloadVisual(string letter)
    {
        if (letter == "Q")
        {
            qReset = true;
        }

        if (letter == "W")
        {
            wReset = true;
        }
    }

    void CheckForCount()
    {
        if (qReset)
        {
            qCounter = 0f;
            qBeginCountdown = true;
            qReset = false;

            // Player cannot spin anymore
            GameObject.FindWithTag("Player").GetComponent<PlayerController>().playerCanSpin = false;
        }

        if (wReset)
        {
            wCounter = 0f;
            wBeginCountdown = true;
            wReset = false;
        }

        //----------------------------------------------//

        if (qBeginCountdown)
        {
            if (!hud.GetComponent<MenuManager>().AnyMenuOpen())
            {
                qCounter += Time.deltaTime;
            }
            qCounterRatio = qCounter / qReloadTime;
            qMoveLoadImage.fillAmount = qCounterRatio;
        }

        if (wBeginCountdown)
        {
            if (!hud.GetComponent<MenuManager>().AnyMenuOpen())
            {
                wCounter += Time.deltaTime;
            }
            wCounterRatio = wCounter / wReloadTime;
            wMoveLoadImage.fillAmount = wCounterRatio;
        }

        //----------------------------------------------//

        if (qCounter >= qReloadTime)
        {
            qBeginCountdown = false;
            qMoveLoadImage.fillAmount = 0f;
            GameObject.FindWithTag("Player").GetComponent<PlayerController>().playerCanSpin = true;
        }

        if (wCounter >= wReloadTime)
        {
            wBeginCountdown = false;
            wMoveLoadImage.fillAmount = 0f;
            GameObject.FindWithTag("Player").GetComponent<PlayerController>().playerCanSpeedBoost = true;
        }
    }
}
