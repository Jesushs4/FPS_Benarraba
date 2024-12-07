using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ThunderController : MonoBehaviour {

	public VisualEffect vfx;
	public float offset;

	[Button]
	public void Thunder() {
		if(Physics.Raycast(transform.position, -transform.up, out RaycastHit hitInfo)) {
			vfx.SetFloat("Length", hitInfo.distance - offset);
			vfx.Play();
		}
	}
}
