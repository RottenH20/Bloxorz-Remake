using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CharacterController : MonoBehaviour {

	public float rotationPeriod = 0.3f;		
	Vector3 scale;

	public InputManager InputManager;
	public AudioSource blockMove;

	// Make sure object called smokePuff is in scene
	private ParticleSystem smokePuff;

	bool isRotate = false;					
	float directionX = 0;					
	float directionZ = 0;

	[HideInInspector]
	public float x = 0f;
	[HideInInspector]
	public float y = 0f;
	[HideInInspector]
	public int numberOfMoves = 0;
	[HideInInspector]
	public bool Falling = false;

	public CanvasHandlerLevel CanvasHandlerLevel;

	float startAngleRad = 0;				
	Vector3 startPos;						
	float rotationTime = 0;					
	float radius = 1;						
	Quaternion fromRotation;				
	Quaternion toRotation;					

	// Use this for initialization
	void Start () {
		scale = transform.lossyScale;
		//Debug.Log ("[x, y, z] = [" + scale.x + ", " + scale.y + ", " + scale.z + "]");
		smokePuff = GameObject.Find("smokePuff").GetComponent<ParticleSystem>();
	}

	// Update is called once per frame
	void Update () {

		x = InputManager.xManager;
		y = InputManager.yManager;

		if ((x != 0 || y != 0) && !isRotate) {
			InputManager.xManager = 0;
			InputManager.yManager = 0;
			//Debug.Log(x + " X");
			//Debug.Log(y + " Y");
			directionX = y;																
			directionZ = x;
			startPos = transform.position;												
			fromRotation = transform.rotation;											
			transform.Rotate (directionZ * 90, 0, directionX * 90, Space.World);		
			toRotation = transform.rotation;											
			transform.rotation = fromRotation;											
			setRadius();																
			rotationTime = 0;
			blockMove.Play(0);
			isRotate = true;
			numberOfMoves++; // Used to keep track of amount of moves made
			CanvasHandlerLevel.updateMoveCounter();
		}
	}

	void FixedUpdate() {

		if (isRotate) {

			rotationTime += Time.fixedDeltaTime;									
			float ratio = Mathf.Lerp(0, 1, rotationTime / rotationPeriod);			

		
			float thetaRad = Mathf.Lerp(0, Mathf.PI / 2f, ratio);					
			float distanceX = -directionX * radius * (Mathf.Cos (startAngleRad) - Mathf.Cos (startAngleRad + thetaRad));		
			float distanceY = radius * (Mathf.Sin(startAngleRad + thetaRad) - Mathf.Sin (startAngleRad));						
			float distanceZ = directionZ * radius * (Mathf.Cos (startAngleRad) - Mathf.Cos (startAngleRad + thetaRad));			
			transform.position = new Vector3(startPos.x + distanceX, startPos.y + distanceY, startPos.z + distanceZ);			

		
			transform.rotation = Quaternion.Lerp(fromRotation, toRotation, ratio);		

			
			if (ratio == 1) {
				isRotate = false;
				directionX = 0;
				directionZ = 0;
				rotationTime = 0;
				if (this.transform.position.y > 1.9f && this.transform.position.y < 2.1f)
					smokePuff.transform.position = this.transform.position + new Vector3(0, -1f, 0);
				else
					smokePuff.transform.position = this.transform.position + new Vector3(0, -0.5f, 0);
				smokePuff.Play(smokePuff);
			}
		}
	}

	void setRadius() {

		Vector3 dirVec = new Vector3(0, 0, 0);			
		Vector3 nomVec = Vector3.up;					// (0,1,0)

		
		if (directionX != 0) {							
			dirVec = Vector3.right;						// (1,0,0)
		} else if (directionZ != 0) {					
			dirVec = Vector3.forward;					// (0,0,1)
		} 

		if (Mathf.Abs (Vector3.Dot (transform.right, dirVec)) > 0.99) {						
			if (Mathf.Abs (Vector3.Dot (transform.up, nomVec)) > 0.99) {					
				radius = Mathf.Sqrt(Mathf.Pow(scale.x/2f,2f) + Mathf.Pow(scale.y/2f,2f));	
				startAngleRad = Mathf.Atan2(scale.y, scale.x);								
			} else if (Mathf.Abs (Vector3.Dot (transform.forward, nomVec)) > 0.99) {		
				radius = Mathf.Sqrt(Mathf.Pow(scale.x/2f,2f) + Mathf.Pow(scale.z/2f,2f));
				startAngleRad = Mathf.Atan2(scale.z, scale.x);
			}

		} else if (Mathf.Abs (Vector3.Dot (transform.up, dirVec)) > 0.99) {					
			if (Mathf.Abs (Vector3.Dot (transform.right, nomVec)) > 0.99) {					
				radius = Mathf.Sqrt(Mathf.Pow(scale.y/2f,2f) + Mathf.Pow(scale.x/2f,2f));
				startAngleRad = Mathf.Atan2(scale.x, scale.y);
			} else if (Mathf.Abs (Vector3.Dot (transform.forward, nomVec)) > 0.99) {		
				radius = Mathf.Sqrt(Mathf.Pow(scale.y/2f,2f) + Mathf.Pow(scale.z/2f,2f));
				startAngleRad = Mathf.Atan2(scale.z, scale.y);
			}
		} else if (Mathf.Abs (Vector3.Dot (transform.forward, dirVec)) > 0.99) {			
			if (Mathf.Abs (Vector3.Dot (transform.right, nomVec)) > 0.99) {					
				radius = Mathf.Sqrt(Mathf.Pow(scale.z/2f,2f) + Mathf.Pow(scale.x/2f,2f));
				startAngleRad = Mathf.Atan2(scale.x, scale.z);
			} else if (Mathf.Abs (Vector3.Dot (transform.up, nomVec)) > 0.99) {				
				radius = Mathf.Sqrt(Mathf.Pow(scale.z/2f,2f) + Mathf.Pow(scale.y/2f,2f));
				startAngleRad = Mathf.Atan2(scale.y, scale.z);
			}
		}
		//Debug.Log (radius + ", " + startAngleRad);
	}

	public bool fallingCheck()
    {
		return Falling;
    }

	public void setFallingTrue()
    {
		Falling = true;
    }
}