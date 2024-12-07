using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour {

	[Range(0f, 1f)]
	public float time;
	public Transform volumes;
	public float endPoint;
	public float transitionDuration;
	public Ease ease = Ease.Linear;
	public Gradient skyColors;
	public Gradient sunColors;
	public Gradient horizonColors;

	public Material skybox;
	public Light sun;
	private float initPoint;

	public TimeController timeController;

	void Awake() {
		//skybox = Instantiate(RenderSettings.skybox);
		RenderSettings.skybox = skybox;
		sun = RenderSettings.sun;
		initPoint = volumes.localPosition.x;
	}

	// Start is called before the first frame update
	void Start() {
		skybox.SetColor("_Tint", skyColors.Evaluate(time));


		// volumes.DOLocalMoveX(endPoint, transitionDuration).SetLoops(-1, LoopType.Yoyo);
		// skybox.DOColor(nightColor, "_SkyGradientTop", transitionDuration).SetLoops(-1, LoopType.Yoyo);
		// DOTween.To(() => time, x => time = x, 1f, transitionDuration).SetEase(ease).SetLoops(-1, LoopType.Restart);
	}

	// Update is called once per frame
	void Update() {


		if(timeController != null)
			time = timeController.TimeDay();

		if(time >= 1)
			time = 0;

		//Debug.Log(time);

		volumes.localPosition = new Vector3(Mathf.Lerp(initPoint, endPoint, time), volumes.localPosition.y, volumes.localPosition.z);
		skybox.SetColor("_Tint", skyColors.Evaluate(time));
		//skybox.SetColor("_SunHaloColor", sunColors.Evaluate(time));
		//skybox.SetColor("_HorizonLineColor", horizonColors.Evaluate(time));
		sun.transform.parent.rotation = Quaternion.Euler(360f * time, 0f, 0f);
		sun.color = sunColors.Evaluate(time);
	}
}
