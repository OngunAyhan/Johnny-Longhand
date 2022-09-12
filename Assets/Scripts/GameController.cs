using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public bool IsGameStarted;

    public Vector3 CameraTPSPos;

    public Vector3 CameraFPSPos;

    public Vector3 CameraTPSRot;

    public Vector3 CameraFPSRot;

    public Vector3 CameraCharPos;

    public Vector3 CameraCharRot;

    public CameraFollower cameraFollower;

    public GameObject ArmObjects;

    public GameObject NormalFaceObject;

    public GameObject HappyFaceObject;

    public GameObject SadFaceObject;

    public GameObject ThieveHandledObject;

    public bool isArmOnPosition;

    void Start()
    {
        StartCoroutine(StartLevel());
    }

    public IEnumerator StartLevel()
    {

        yield return new WaitForSeconds(2f);
        cameraFollower.GoToPosSetter(CameraFPSPos, CameraFPSRot, 0);
        yield return new WaitForSeconds(1f);
        ArmObjects.SetActive(true);

        Vector3 startingPos = ArmObjects.transform.position;
        Vector3 finalPos = Vector3.zero;
        float elapsedTime = 0;
        float time = 0.5f;

        while (elapsedTime < time)
        {
            ArmObjects.transform.position = Vector3.Lerp(startingPos, finalPos, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        cameraFollower.SetCameraOffset();
        cameraFollower.CanGo = false;
        IsGameStarted = true;
    }

    public void EndLevelTPV()
    {
        cameraFollower.GoToPosSetter(CameraTPSPos, CameraTPSRot, 1);

        
    }

    public void CameraSendToCharPos(bool CharMood)
    {
        cameraFollower.GoToPosSetter(CameraCharPos, CameraCharRot, 1);

        //NormalFaceObject.SetActive(false);

        if (CharMood)
        {
            //HappyFaceObject.SetActive(true);
        }
        else
        {
            //SadFaceObject.SetActive(true);
        }
    }

    public void EndLevelFail()
    {

    }

    

}
