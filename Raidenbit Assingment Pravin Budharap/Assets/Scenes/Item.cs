using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{

    public GameObject ItemImage;
    public GameObject ItemText;
    public GameObject ItemLockImage;

	public Button yourButton;
    Button btn;

    // Start is called before the first frame update
    void Start()
    {
        btn = yourButton.GetComponent<Button>();
		btn.onClick.AddListener(OpenWebUrl);
    }

	void OpenWebUrl()
    {
		Debug.Log ("You have clicked the button!");
        GameManager.instance.OpenWebUrl(btn);
	}

    // // Update is called once per frame
    // void Update()
    // {
        
    // }
}
