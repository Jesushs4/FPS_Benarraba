using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Core.Easing;
using DG.Tweening.Core.Enums;
using DG.Tweening.Plugins.Core;
using DG.Tweening.Plugins.Options;
using System;
using UnityEngine;

/// <summary>
/// Custom DOTween plugin example.
/// This one tweens a Range value, but you can also create plugins just to do weird stuff, other than to tween custom objects
/// </summary>
public class RangePlugin : ABSTweenPlugin<Range, Range, NoOptions> {

	private static RangePlugin instance;
	public static RangePlugin Plugin {

		get {
			if(instance == null) {
				instance = new RangePlugin();
			}
			return instance;
		}
	}

	// Leave this empty
	public override void Reset(TweenerCore<Range, Range, NoOptions> t) { }

	// Sets the values in case of a From tween
	public override void SetFrom(TweenerCore<Range, Range, NoOptions> t, bool isRelative) {
		Range prevEndVal = t.endValue;
		t.endValue = t.getter();
		t.startValue = isRelative ? t.endValue + prevEndVal : prevEndVal;
		t.setter(t.startValue);
	}

	// Sets the values in case of a From tween with a specific from value
	public override void SetFrom(TweenerCore<Range, Range, NoOptions> t, Range fromValue, bool setImmediately, bool isRelative) {
		t.startValue = fromValue;
		if(setImmediately) t.setter(fromValue);
	}

	// Used by special plugins, just let it return the given value
	public override Range ConvertToStartValue(TweenerCore<Range, Range, NoOptions> t, Range value) {
		return value;
	}

	// Determines the correct endValue in case this is a relative tween
	public override void SetRelativeEndValue(TweenerCore<Range, Range, NoOptions> t) {
		t.endValue = t.startValue + t.changeValue;
	}

	// Sets the overall change value of the tween
	public override void SetChangeValue(TweenerCore<Range, Range, NoOptions> t) {
		t.changeValue = t.endValue - t.startValue;
	}

	// Converts a regular duration to a speed-based duration
	public override float GetSpeedBasedDuration(NoOptions options, float unitsXSecond, Range changeValue) {
		// Not implemented in this case (but you could implement your own logic to convert duration to units x second)
		return unitsXSecond;
	}

	// Calculates the value based on the given time and ease
	public override void EvaluateAndApply(NoOptions options, Tween t, bool isRelative, DOGetter<Range> getter, DOSetter<Range> setter, float elapsed, Range startValue, Range changeValue, float duration, bool usingInversePosition, UpdateNotice updateNotice) {
		float easeVal = EaseManager.Evaluate(t, elapsed, duration, t.easeOvershootOrAmplitude, t.easePeriod);

		// Here I use startValue directly because CustomRange a struct, so it won't reference the original.
		// If CustomRange was a class, I should create a new one to pass to the setter
		startValue = new Range(startValue.Min + changeValue.Min * easeVal, startValue.Max + changeValue.Max * easeVal);
		/*
        startValue.Min += changeValue.Min * easeVal;
        startValue.Max += changeValue.Max * easeVal;
		*/
		setter(startValue);
	}
}
