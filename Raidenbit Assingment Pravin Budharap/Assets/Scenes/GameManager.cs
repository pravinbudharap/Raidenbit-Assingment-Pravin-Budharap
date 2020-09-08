using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Newtonsoft.Json;

public class GameManager : MonoBehaviour
{

    public static GameManager instance = null;		//Static instance of GameManager which allows it to be accessed by any other script.
    
    // Reference to the Prefab. Drag a Prefab into this field in the Inspector.
    public GameObject ItemPanel;
    public string MainUrl = "https://api.jsonbin.io/b/5f56071c4d8ce411138a7d14" ;
    PlayerData deserializedPlayerData = new PlayerData();
    public Text MessageOfTheDayText;
    RectTransform rectTransform;



    // public RawImage DemoImage;

	//Awake is always called before any Start functions
	void Awake()
	{
		//Check if instance already exists
		if (instance == null)
		{
			Input.multiTouchEnabled = false;

			//if not, set instance to this
			instance = this;
		}
		//If instance already exists and it's not this:
		else if (instance != this)
		{
			//Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
			Destroy(gameObject);   
		}
    }

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        // A correct website page.
        StartCoroutine(GetText());
    
        // StartCoroutine(GetTexture());

        // int Lenght = 10;
        // for (int i = 0; i < Lenght ; ++i)
        // {
        //     // Instantiate at position (0, 0, 0) and zero rotation.
        //     GameObject ItemObject =  Instantiate(ItemPanel, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;

        //     ItemObject.transform.SetParent(transform, false);
        // }

    }

    IEnumerator GetText() 
    {
        UnityWebRequest www = UnityWebRequest.Get(MainUrl);

        yield return www.SendWebRequest();

        if(www.isNetworkError || www.isHttpError) 
        {
            Debug.Log(www.error);
        }
        else 
        {
            // Show results as text
            Debug.Log(www.downloadHandler.text);

            // Or retrieve results as binary data
            byte[] results = www.downloadHandler.data;

            Debug.Log(www.downloadHandler.data);

            deserializedPlayerData = JsonConvert.DeserializeObject<PlayerData>(www.downloadHandler.text);

            // string messageOfTheDay = deserializedPlayerData.messageOfTheDay;
            // string secondTitle = deserializedPlayerData.items[1].title;
            // int itemlenght = deserializedPlayerData.items.Length;

            // Debug.Log(messageOfTheDay);
            // Debug.Log(secondTitle);
            // Debug.Log(itemlenght);

            MessageOfTheDayText.text = deserializedPlayerData.messageOfTheDay;

            int itemlenght = deserializedPlayerData.items.Length;

            rectTransform.sizeDelta = new Vector2(1018, 250 * itemlenght);

            // Debug.LogError(new Rect(20, 20, 150, 80), "Rect : " + rectTransform.rect);

            for (int i = 0; i < itemlenght ; ++i)
            {
                // Instantiate at position (0, 0, 0) and zero rotation.
                GameObject ItemObject =  Instantiate(ItemPanel, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;

                ItemObject.transform.SetParent(transform, false);

                ItemObject.name = i.ToString();

                ItemObject.transform.GetChild(1).GetComponent<Text>().text = deserializedPlayerData.items[i].title;

                if(deserializedPlayerData.items[i].available == true)
                {
                    ItemObject.transform.GetChild(2).gameObject.SetActive(false);   
                }
                else
                {
                    ItemObject.transform.GetChild(2).gameObject.SetActive(true);   
                }

                string PassedimageUrl = deserializedPlayerData.items[i].image;
                StartCoroutine(GetTexture(ItemObject, PassedimageUrl));
            }

        }
    }

    // IEnumerator GetTexture() 
    IEnumerator GetTexture(GameObject ItemObject, string imageUrl) 
    {
        string CompleteImageUrl = deserializedPlayerData.imagePath + imageUrl;

        // Debug.Log(CompleteImageUrl);

        // UnityWebRequest www = UnityWebRequestTexture.GetTexture("https://via.placeholder.com/150/000000/FFFFFF/?text=2");
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(CompleteImageUrl);

        yield return www.SendWebRequest();

        if(www.isNetworkError || www.isHttpError) 
        {
            Debug.Log(www.error);
        }
        else 
        {
            Texture myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;

            // DemoImage.GetComponent<RawImage>().texture = myTexture;
            ItemObject.transform.GetChild(0).GetComponent<RawImage>().texture = myTexture;
        }
    }


    public void OpenWebUrl(Button clickedbutton)
    {
        int indexnumber  = Convert.ToInt32(clickedbutton.name);

        if(deserializedPlayerData.items[indexnumber].available == true)
        {
            string url =  deserializedPlayerData.items[indexnumber].address;  
            Application.OpenURL(url);
        }
        else
        {
            Debug.Log("Item Locked.!");
        }

    }

    // // Update is called once per frame
    // void Update()
    // {
        
    // }
    
}

[Serializable]
public class PlayerData
{
    public string messageOfTheDay;
    public string imagePath;
    public ItemData[] items;
}

[Serializable]
public class ItemData
{
    public string title;
    public string image;
    public bool available;
    public string address;
    
}