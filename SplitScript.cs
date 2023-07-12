using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Services.Analytics;

public class SplitScript : MonoBehaviour
{
    //Empty container for Slime Controller
    SlimeController sc;
    private Button splitButton;

    public float slimeConnectRange = 2;
    public float smallSlime1PosX;
    public float smallSlime1PosY;
    public float smallSlime2PosX;
    public float smallSlime2PosY;

    public Vector2 smallSlimeFix;
    private Animator m_Anim;

    public bool active;

    void Awake()
    {
        // Assigns the Slime Controller script
        sc = GetComponent<SlimeController>();

        splitButton = GameObject.Find("Split").GetComponent<Button>();
    }

    void Update()
    {
        if (sc.isSplit && sc.timeCheck < sc.timer && active)
        {
            smallSlime1PosX = sc.smallSlime1.transform.position.x;
            smallSlime1PosY = sc.smallSlime1.transform.position.y;
            smallSlime2PosX = sc.smallSlime2.transform.position.x;
            smallSlime2PosY = sc.smallSlime2.transform.position.y;

            if (smallSlime1PosX + slimeConnectRange >= smallSlime2PosX && smallSlime1PosY + slimeConnectRange >= smallSlime2PosY && smallSlime1PosX - slimeConnectRange <= smallSlime2PosX && smallSlime1PosY - slimeConnectRange <= smallSlime2PosY)
            {
                splitButton.GetComponent<Button>().interactable = true;
            }
            else
            {
                splitButton.GetComponent<Button>().interactable = false;
            }
        }
        else if (!sc.isSplit && sc.timeCheck < sc.timer && active)
        {
            splitButton.GetComponent<Button>().interactable = true;
        }
        else
        {
            splitButton.GetComponent<Button>().interactable = false;
        }
    }

    public void Split()
    {

        if (sc.isSplit == false && sc.timeCheck < sc.timer)
        {

            Debug.Log("is Split: " + sc.isSplit + " should be false");
            m_Anim.SetTrigger("Split");

            StartCoroutine(DelayedSplit(m_Anim.GetCurrentAnimatorStateInfo(0).length));

        }
        if (sc.isSplit == true && sc.timeCheck < sc.timer)
        {
            smallSlime1PosX = sc.smallSlime1.transform.position.x;
            smallSlime1PosY = sc.smallSlime1.transform.position.y;
            smallSlime2PosX = sc.smallSlime2.transform.position.x;
            smallSlime2PosY = sc.smallSlime2.transform.position.y;

            if (smallSlime1PosX + slimeConnectRange >= smallSlime2PosX && smallSlime1PosY + slimeConnectRange >= smallSlime2PosY && smallSlime1PosX - slimeConnectRange <= smallSlime2PosX && smallSlime1PosY - slimeConnectRange <= smallSlime2PosY)
            {
                sc.bigSlime.transform.position = sc.slimeTarget;
                Debug.Log("is Split: " + sc.isSplit + " should be true");
                sc.timeCheck = sc.timer + 2;
                Debug.Log("Time: " + sc.timer);
                sc.smallSlime1.SetActive(false);
                sc.smallSlime2.SetActive(false);
                sc.bigSlime.SetActive(true);
                sc.isSplit = false;
                Dictionary<string, object> mergeParameters = new Dictionary<string, object>()
                    {
                        { "Merge", "Is split: " + sc.isSplit },
                        { "MergeTime", sc.timeCheck },
                    };
                AnalyticsService.Instance.CustomData("Merge", mergeParameters);
                AnalyticsService.Instance.Flush();

            }
        }
    }
    IEnumerator DelayedSplit(float _delay = 0)
    {
        yield return new WaitForSeconds(_delay / 3);
        sc.smallSlime1.transform.position = sc.bigSlimePos + smallSlimeFix;
        sc.smallSlime2.transform.position = sc.bigSlimePos - smallSlimeFix;

        sc.timeCheck = sc.timer + 2;
        Debug.Log("Time: " + sc.timer);
        sc.smallSlime1.SetActive(true);
        sc.smallSlime2.SetActive(true);
        sc.bigSlime.SetActive(false);
        sc.isSplit = true;

    }
}
