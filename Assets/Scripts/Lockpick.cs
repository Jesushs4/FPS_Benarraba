using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Lockpick : MonoBehaviour
{
    [SerializeField] private Image hitArea;
    [SerializeField] private Image needle;

    [SerializeField] private float rotationSpeed = 50f;

    private bool isResetting = false;

    private int hitCount = 0;
    private int hitNeeded = 4;

    [SerializeField] private Image imageProgress;
    [SerializeField] private Transform cageDoor;
    [SerializeField] private BoxCollider sheepCollider;

    private LayerMask sheepLayer;
    private LayerMask normalLayer;

    private bool isCompleted = false;
    private bool isLockpicking = false;

    public bool IsCompleted { get => isCompleted; set => isCompleted = value; }
    public bool IsLockpicking { get => isLockpicking; set => isLockpicking = value; }

    private void Awake()
    {

        normalLayer = LayerMask.GetMask("Default");
        sheepLayer = LayerMask.GetMask("SheepLayer");

        sheepCollider.enabled = false;
        sheepCollider.gameObject.layer = normalLayer;
    }

    private void Update()
    {
        if (!isResetting)
        {
            RotateNeedle();
        }

        if (hitCount >= hitNeeded)
        {
            gameObject.SetActive(false);
            Vector3 targetRotation = cageDoor.eulerAngles + new Vector3(0, 90, 0);
            cageDoor.DORotate(targetRotation, 1f)
                .SetEase(Ease.InOutSine);
            IsCompleted = true;
            sheepCollider.enabled = true;
            isLockpicking = false;

            sheepCollider.gameObject.layer = LayerMask.NameToLayer("SheepLayer");
        }
    }

    public void StartMinigame()
    {
        SetRandomAngle();
        isLockpicking = true;
    }

    public void HitNeedle()
    {
        if (!isResetting)
        {
            if (IsNeedleInHitArea())
            {
                hitCount++;
                imageProgress.fillAmount = (float)hitCount / hitNeeded;
                AudioManager.Instance.PlayLockpickCorrect();
            } else
            {
                AudioManager.Instance.PlayLockpickFail();
            }

            StartCoroutine(NewLockpick());
        }
    }

    void SetRandomAngle()
    {
        float randomAngle = Random.Range(0f, 360f);

        hitArea.transform.rotation = Quaternion.Euler(0, 0, -randomAngle);
    }

    private void RotateNeedle()
    {
        needle.transform.Rotate(0, 0, -rotationSpeed * Time.deltaTime);
    }


    private bool IsNeedleInHitArea()
    {
        float visibleAngle = 360f * hitArea.fillAmount;

        float hitStartAngle = NormalizeAngle(hitArea.transform.eulerAngles.z - visibleAngle);
        float hitEndAngle = NormalizeAngle(hitStartAngle + visibleAngle);


        float needleAngle = NormalizeAngle(needle.transform.eulerAngles.z);
        if (hitStartAngle < hitEndAngle)
        {
            return needleAngle >= hitStartAngle && needleAngle <= hitEndAngle;
        }
        else
        {
            return needleAngle >= hitStartAngle || needleAngle <= hitEndAngle;
        }
    }

    private float NormalizeAngle(float angle)
    {
        while (angle < 0) angle += 360;
        while (angle >= 360) angle -= 360;
        return angle;
    }

    private IEnumerator NewLockpick()
    {
        isResetting = true;
        yield return new WaitForSeconds(1);
        StartMinigame();
        isResetting = false;
    }

}
