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
    private int hitNeeded = 5;

    [SerializeField] private Image imageProgress;

    private void Update()
    {
        if (!isResetting)
        {
            RotateNeedle();
        }

        if (hitCount >= hitNeeded)
        {
            gameObject.SetActive(false);
        }
    }

    public void StartMinigame()
    {
        SetRandomAngle();
    }

    public void HitNeedle()
    {
        if (!isResetting)
        {
            if (IsNeedleInHitArea())
            {
                hitCount++;
                imageProgress.fillAmount = (float)hitCount / hitNeeded;
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
