using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(AudioSource))]
public class LaserGun : Weapon {
	//	public Transform muzzle;
	private LineRenderer line;
	public AudioClip fireSound,emptySound;
	private int beamTime = 0;
	public int maxBeamTime = 10;
	public Light innerLight;
	private float defaultLightRange;
	public GameObject lightPrefab;
	private GameObject[] lights;
	public float lineDensity = 1.0f;
	private float lineLightIntensity;
	
	// Use this for initialization
	void Start () {
		defaultLightRange = innerLight.range;
		lineLightIntensity = lightPrefab.light.intensity;
		line = GetComponentInChildren<LineRenderer>();
	}
	
	// Update is called once per frame
	override public void UpdateCallback () {
		if(Player.Dead)return;
		if(Input.GetKeyDown (KeyCode.F3)){
			InventoryManager.numLaserGunAmmo+=10;
		}
		if(beamTime>0){
			beamTime--;
			for(int i = 0; i<lights.Length;i++){
				lights[i].light.intensity = Mathf.Lerp(0f,lineLightIntensity,(float)beamTime/(float)maxBeamTime);
//					lineLightIntensity*(((float)(maxBeamTime))/(float)(maxBeamTime-beamTime+1));
			}
			innerLight.range = defaultLightRange*(((float)(maxBeamTime))/(float)(maxBeamTime-beamTime+1));
			return;
		}
		if(beamTime==0){
			line.enabled = false;
			innerLight.range = defaultLightRange;
			if(lights!=null){
				for(int i = 0; i<lights.Length;i++){
					Destroy(lights[i]);
				}
				lights = null;
//				Destroy(lights);
			}
		}

	}
	
	override public void Fire(){
		if(beamTime>0)return;
		if(InventoryManager.numLaserGunAmmo<=0){
			audio.PlayOneShot(emptySound);
			return;
		}
		InventoryManager.numLaserGunAmmo--;
		audio.PlayOneShot(fireSound);
		Vector3 fwd = muzzle.TransformDirection(Vector3.forward);
		RaycastHit hit;
		if(Physics.Raycast(muzzle.position,fwd,out hit)){
			hit.collider.gameObject.SendMessage("onHit",SendMessageOptions.DontRequireReceiver);
			lights = new GameObject[(int)(hit.distance/lineDensity)];
			Vector3 delta = (hit.point-muzzle.position)/lights.Length;
			for(int i = 0; i<lights.Length;i++){
				lights[i] = Instantiate(lightPrefab,muzzle.position+(delta*i),Quaternion.identity) as GameObject;
			}
			line.SetPosition(1,transform.InverseTransformPoint(hit.point));
			line.enabled = true;
			beamTime = maxBeamTime;
		}
		
	}
	public override void PutAway ()
	{
//		throw new System.NotImplementedException ();
	}

}
