using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour {

	private Rigidbody2D rb;
	private bool spawnSame = false;
	private bool spawnNext = false;
	private bool hasRun = false;
	[SerializeField] private GameObject winBar;
	[SerializeField] private GameObject shadow;

	private GameObject[] shadows = new GameObject[4];

	void Start () {
		rb = this.GetComponent<Rigidbody2D> ();
		gameObject.tag = "falling";
		int i = 0;
		foreach (Transform childTransform in gameObject.transform) {
			shadows[i] = Instantiate(shadow);
			shadows[i].transform.position = childTransform.position - new Vector3(0,12,0);
			i++;
		}
	}

	void Update() {
		int i = 0;
		List<Transform> subList = new List<Transform> (); 

		foreach (Transform childTransform in gameObject.transform) {
			if (!gameObject.CompareTag ("Untagged")) {
				shadows [i].transform.position = childTransform.position - new Vector3(0,12,0);

				shadows [i].GetComponent<SpriteRenderer> ().color = new Color (1f, 1f, 1f, 0.3f);

				foreach (Transform sub in subList) {
					if (shadows [i].transform.position.x == sub.transform.position.x) {
						shadows [i].GetComponent<SpriteRenderer> ().color = new Color (1f, 1f, 1f, 0f);
					}
				}
				subList.Add (shadows [i].transform);
			} else {
				Destroy (shadows[i]);
			}
			i++;
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
			
		if (collision.collider.CompareTag("wall")) {
			return;
		}
		if (!collision.collider.CompareTag("Untagged")) {
			return;
		}
		if (gameObject.CompareTag("falling")) {
			gameObject.tag = "Untagged";
			SetSpawnNext(true);
		}
	}

	//Method called when downward arrow is pressed.
	public void applyDownwardForce() {
		this.rb.AddForce (new Vector3 (0, -1000, 0));
	}
		
}
