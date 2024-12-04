using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Lever : MonoBehaviour
{
    private bool isRotating = false;
    private bool isReversing = false;
    private bool wellCompleted = false;
    private Coroutine reverseCoroutine;
    private Coroutine continuousReverseCoroutine;
    private float lastInteractionTime;
    [SerializeField] private GameObject wellPanel;
    private Slider wellProgress;

    private void Start()
    {
        wellProgress = wellPanel.transform.GetChild(0).GetComponent<Slider>();
        wellPanel.SetActive(false);
    }
    private void Update()
    {
        if (wellProgress.value >= 100)
        {
            wellCompleted = true;
            wellPanel.SetActive(false);
        }
        if (wellProgress.value > 0) {
            wellPanel.SetActive(true);
        } else
        {
            wellPanel.SetActive(false);
        }
    }

    public void PutLever(Transform placeholderTransform)
    {
        transform.position = placeholderTransform.position;
        transform.rotation = placeholderTransform.rotation;
        gameObject.layer = LayerMask.GetMask("Default");
        placeholderTransform.GetComponent<Renderer>().enabled = false;
    }

    public void RotateLever()
    {
        if (isRotating || wellCompleted) return;

        if (isReversing)
        {
            StopReversing();
        }

        isRotating = true;

        lastInteractionTime = Time.time;

        float initialValue = wellProgress.value;
        float targetValue = Mathf.Min(100f, wellProgress.value + 10f);

        DOVirtual.Float(initialValue, targetValue, 0.3f, value =>
        {
            wellProgress.value = value;
        });

        transform.DORotate(
            new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z - 360),
            0.3f,
            RotateMode.FastBeyond360
        ).OnComplete(() =>
        {
            isRotating = false;

            if (reverseCoroutine == null)
            {
                reverseCoroutine = StartCoroutine(CheckForReverse());
            }
        });
    }

    private IEnumerator CheckForReverse()
    {
        while (true)
        {
            yield return null;

            if (Time.time - lastInteractionTime >= 0.5f && !isReversing && !wellCompleted)
            {
                isReversing = true;
                continuousReverseCoroutine = StartCoroutine(ContinuousReverse());
                break;
            }
        }
    }


    private IEnumerator ContinuousReverse()
    {
        while (isReversing)
        {
            float initialValue = wellProgress.value;
            float targetValue = Mathf.Max(0, wellProgress.value - 5f);

            DOVirtual.Float(initialValue, targetValue, 0.2f, value =>
            {
                wellProgress.value = value;
            });

            yield return transform.DORotate(
                new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z + 360),
                0.2f,
                RotateMode.FastBeyond360
            ).WaitForCompletion();

            if (wellProgress.value <= 0)
            {
                StopReversing();
            }
        }
    }

    private void StopReversing()
    {
        if (reverseCoroutine != null)
        {
            StopCoroutine(reverseCoroutine);
            reverseCoroutine = null;
        }

        if (continuousReverseCoroutine != null)
        {
            StopCoroutine(continuousReverseCoroutine);
            continuousReverseCoroutine = null;
        }

        isReversing = false;
    }
}