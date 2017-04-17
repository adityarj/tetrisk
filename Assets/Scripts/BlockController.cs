using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BlockController : NetworkBehaviour {

	private Rigidbody2D rb;
	private bool spawnSame = false;
	private bool spawnNext = false;
	private bool hasRun = false;
	[SerializeField] private GameObject winBar;
	[SerializeField] private GameObject shadow;

	private float maxy = -6f; // highest point of block (range is b/w -6 to 10)
	public float gravityBias = 1f;
	public float gravityCoeffcient = 2f;

	private GameObject[] shadows = new GameObject[4];

	public override void OnStartAuthority() {
		int i = 0;
		foreach (Transform childTransform in gameObject.transform) {
			shadows [i] = Instantiate (shadow);
			shadows [i].transform.position = childTransform.position - new Vector3 (0, 12, 0);
			i++;
		}

	}

	void Start () {
		rb = this.GetComponent<Rigidbody2D> ();
	}

	void Update() {
		
		if (hasAuthority) {
			int i = 0;
			List<Transform> subList = new List<Transform> ();
			foreach (Transform childTransform in gameObject.transform) {
				
				if (!gameObject.CompareTag ("DeadBlock")) {
					shadows [i].transform.position = childTransform.position - new Vector3 (0, 12, 0);

					shadows [i].GetComponent<SpriteRenderer> ().color = new Color (1f, 1f, 1f, 0.3f);

					foreach (Transform sub in subList) {
						if (System.Math.Round(shadows [i].transform.position.x,2) == System.Math.Round(sub.transform.position.x,2)) {
							shadows [i].GetComponent<SpriteRenderer> ().color = new Color (1f, 1f, 1f, 0f);
						}
					}
					subList.Add (shadows [i].transform);
				} else {
					Destroy (shadows [i]);

				}
				i++;
			}
		}

		if (gameObject.CompareTag("DeadBlock")) {
			if (rb.velocity.y > -1f) { 
			foreach (Transform childTransform in gameObject.transform) {
				if (childTransform.position.y > maxy) {
					maxy = childTransform.position.y;
				}
			}
			// make maxy postive
			float maxynormal = maxy + 6f;
			rb.gravityScale = gravityBias + maxynormal*gravityCoeffcient;
			} else {
				rb.gravityScale = gravityBias;
			}
		}
	}

	void OnDisable() {
		for (int i=0; i < shadows.Length; i++) {
			Destroy(shadows[i]);
		}
	}

	private void setScaling() {
		foreach (Transform childTransform in gameObject.transform) {
			if (childTransform.position.x - 0.5 < shadow.transform.position.x - 0.5) {
				shadow.transform.localScale += new Vector3 (0.5f, 0, 0);
				shadow.transform.position += new Vector3 (-0.5f, 0, 0);
			} 

			if (childTransform.position.x + 0.5 > shadow.transform.position.x + 0.5) {
				shadow.transform.localScale += new Vector3 (0.5f, 0, 0);
			}
		}
	}
		
	public void SetSpawnNext(bool boolean) {
		this.spawnNext = boolean;
	}

	public bool GetSpawnNext() {
		return spawnNext;
	}

	public void setVelocity(Vector3 vel) {
		this.rb.velocity = vel;
	}

	public Vector3 getVelocity() {
		return rb.velocity;
	}

	void OnCollisionEnter2D(Collision2D collision) {
		if (gameObject.CompareTag("fallingrain")) {
			if (collision.collider.transform.parent.CompareTag("DeadBlock")) {
				gameObject.tag = "DeadBlock";
				return;
			} else {
				return;
			}
		}


		if (collision.collider.CompareTag ("wall")) {
			return;
		} else if (gameObject.CompareTag ("falling")) {
			gameObject.tag = "DeadBlock";
			SetSpawnNext (true);
		} else {
			return;
		}
	}

	//Method called when downward arrow is pressed.
	public void applyDownwardForce() {
		this.rb.AddForce (new Vector3 (0, -1000, 0));
	}
		
}
