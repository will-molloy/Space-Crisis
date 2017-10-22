using UnityEngine;
using System.Collections;

public class SnakeHiss : MonoBehaviour {

	private GameObject _player1;
	private GameObject _player2;
	private AudioSource _source;
	private bool isPlayed;

	// Use this for initialization
	void Start () {

		foreach(GameObject Obj in GameObject.FindGameObjectsWithTag("Player"))
		{
			if(Obj.name == "Astronaut")
			{
				_player1 = Obj;


			} else if(Obj.name == "Astronaut_2") {
				_player2 = Obj;
			}

		}

		_source = this.gameObject.GetComponent<AudioSource> ();
		isPlayed = false;
	
	}
	
	// Update is called once per frame
	void Update () {

		if(!isPlayed){
			
			float distance1 = Vector3.Distance(this.gameObject.transform.position, _player1.transform.position);
			float distance2 = Vector3.Distance(this.gameObject.transform.position, _player2.transform.position);

			if (distance1 <= 7 || distance2 <= 7)
			{
				if(_source != null){

					if(!_source.isPlaying){
						_source.Play ();
						isPlayed = true;
					}


				}

			}
			
		}
	
	}
}
