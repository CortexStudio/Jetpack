using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof (Button))]
public class ButtonSound : MonoBehaviour {

	public string clip = "UIButton";

	// Use this for initialization
	void Start () {
        GetComponent<Button>().onClick.AddListener(OnClick);
	}
	
	// Update is called once per frame
	void OnClick () {
         AudioManager.main.Shot(clip);
	}
}
