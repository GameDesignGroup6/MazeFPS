using UnityEngine;
using System.Collections;

public class TestTarget : MonoBehaviour {

	public void onHit(){
		Debug.Log ("HIT!");
		Color newColor = new Color(Random.Range(0f,1f),Random.Range(0f,1f),Random.Range(0f,1f),1);
		renderer.material.color = newColor;
		newColor.a = 0.5f;
		renderer.material.SetColor("_ReflectColor",newColor);
	}
}
