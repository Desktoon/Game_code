using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CameraScript : MonoBehaviour
{
    //Empty container for Slime Controller
    SlimeController sc;

    // Camera follow
    Vector3 playerPos;
    Vector3 offset = new Vector3(0, 0, -15);
    Camera m_MainCamera;
    Vector3 cameraPos;
    public float timeCheck2 = 0;
    public float cameraSpeed = 0.1f;
    private float cameraSize = 9.5f;
    private float cameraDefaultSize;
    private float cameraZoomOutSize;

    private float cameraCurrentZoomSize;
    private float elapsedZoomOutTime;
    private float elapsedZoomInTime;
    private float zoomingTime = 1f;

    private GameObject followedSlime;
    private bool waitForDoor = false;


    // This tile is used to determine the size of the camera
    [SerializeField] private Sprite tile;

    void Start()
    {
        // Assigns the Slime Controller script
        sc = GetComponent<SlimeController>();
        //Check for which slime is the camera focused on
        sc.cameraFollowCheck = 1;
        m_MainCamera = Camera.main;


    }

    void LateUpdate()
    {
        if (sc.isSplit)
        {
            if (sc.cameraFollowCheck == 1)
            {
                //Camera follows active slime (small blue slime)
                followedSlime = sc.smallSlime1;

                cameraPos = m_MainCamera.transform.position + offset;
                Vector3 smoothFollow = Vector3.Lerp(cameraPos, sc.smallSlime1Pos, cameraSpeed);
                m_MainCamera.transform.position = smoothFollow;
            }
            else if (sc.cameraFollowCheck == 2)
            {
                //Camera follows active slime (small yellow slime)
                followedSlime = sc.smallSlime2;

                cameraPos = m_MainCamera.transform.position + offset;
                Vector3 smoothFollow = Vector3.Lerp(cameraPos, sc.smallSlime2Pos, cameraSpeed);
                m_MainCamera.transform.position = smoothFollow;
            }
        }
        else
        {
            //Camera follows active slime (big slime)
            followedSlime = sc.bigSlime;

            playerPos = sc.bigSlime.transform.position;
            cameraPos = m_MainCamera.transform.position + offset;
            Vector3 smoothFollow = Vector3.Lerp(cameraPos, playerPos, cameraSpeed);
            m_MainCamera.transform.position = smoothFollow;
        }

    }

    //Moves camera towards jump direction when jump is being charged
    private void CameraMove(GameObject activeSlime)
    {
        Vector3 delta = m_MainCamera.ScreenToWorldPoint(Input.mousePosition) - activeSlime.transform.position;

        Vector3 aimingTarget = new Vector3(activeSlime.transform.position.x - Mathf.Clamp(delta.x, -5, 5), activeSlime.transform.position.y - Mathf.Clamp(delta.y, -5, 5), -1);
        playerPos = activeSlime.transform.position;
        cameraPos = m_MainCamera.transform.position + offset;
        m_MainCamera.transform.position = Vector3.Lerp(cameraPos, aimingTarget, cameraSpeed);

    }


    private IEnumerator ReturnCamera()
    {
        yield return new WaitForSeconds(1.5f);
        waitForDoor = false;
    }
}
