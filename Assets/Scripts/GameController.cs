using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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

    public Animator WindowAnimator;

    public GameObject ThieveObject;

    public Animator ThieveAnimator;

    public GameObject ThieveLeanObject;

    public GameObject ArmObjects;

    public GameObject NormalFaceObject;

    public GameObject HappyFaceObject;

    public GameObject SadFaceObject;

    public ParticleSystem LootParticles;

    public GameObject ThieveHandLoot;

    public bool isArmOnPosition;

    public GameObject ArrowObj;

    public List<GameObject> MapInfoObjectsToDisable;

    void Start()
    {
        StartCoroutine(StartLevel());
        ArrowObj.transform.DOMoveY(1.8f, 0.8f).SetLoops(-1,LoopType.Yoyo).SetEase(Ease.InOutSine);
    }

    public IEnumerator StartLevel()
    {

        yield return new WaitForSeconds(4f);
        WindowAnimator.SetTrigger("Open");
        cameraFollower.GoToPosSetter(CameraFPSPos, CameraFPSRot, 0);
        foreach (GameObject item in MapInfoObjectsToDisable)
        {
            item.SetActive(false);
        }
        yield return new WaitForSeconds(1f);
        
        ArmObjects.SetActive(true);

        Vector3 startingPos = ArmObjects.transform.position;
        Vector3 finalPos = new Vector3(ArmObjects.transform.position.x, ArmObjects.transform.position.y,0f);
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

        ThieveObject.SetActive(false);
        ThieveLeanObject.SetActive(true);
    }

    public void EndLevelTPV()
    {
        cameraFollower.GoToPosSetter(CameraTPSPos, CameraTPSRot, 1);

        
    }

    public void CameraSendToCharPos(bool CharMood)
    {
        cameraFollower.GoToPosSetter(CameraCharPos, CameraCharRot, 1);
        ArmObjects.SetActive(false);
        ThieveLeanObject.SetActive(false);
        NormalFaceObject.SetActive(false);
        ThieveObject.SetActive(true);
        ThieveObject.transform.rotation = Quaternion.Euler(new Vector3(0f, 130f, 0f));

        if (CharMood)
        {
            
            LootParticles.Play();
            WindowAnimator.SetTrigger("Close");
            ThieveHandLoot.SetActive(true);
            HappyFaceObject.SetActive(true);
            ThieveAnimator.SetTrigger("Loot");
            //GameManager.Instance.LevelSucceded();
        }
        else
        {
            WindowAnimator.SetTrigger("Close");
            SadFaceObject.SetActive(true);
            ThieveAnimator.SetTrigger("Sad");
            //GameManager.Instance.LevelFailed();
        }
    }

    

    

}
