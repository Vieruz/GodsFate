using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour {

    public int[] resources;
    public int followers;

    public float countDownDelay = 10; //In seconds
    public int[] powersCosts;
    public int[] powerDuration; // In seconds

    public GameController gc;
    public IntroManager intro;
    public Text[] resourcesDisplay;
    public Text[] resourcesAnimation;
    public Text followersDisplay;
    public Text followerAnimation;
    public GameObject actionButtons;
    public GameObject[] powersPrefabs;

    const int AIR_INDEX = 0;
    const int FIRE_INDEX = 1;
    const int EARTH_INDEX = 2;
    const int WATER_INDEX = 3;

    const int RAIN_INDEX = 0;
    const int STORM_INDEX = 1;
    const int TREE_INDEX = 2;
    const int WIND_INDEX = 3;

    // Use this for initialization
    void Start () {
        UpdateResourceDisplay();
        StartCoroutine("CountDown");
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            DisplayActions();
        }
    }

    void DisplayActions()
    {
        actionButtons.transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0);
        actionButtons.SetActive(true);
    }

    public void BringDownTheRain()
    {
        actionButtons.SetActive(false);
        GameObject rain = Instantiate<GameObject>(powersPrefabs[RAIN_INDEX],
            new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0),
            Quaternion.identity);
        Destroy(rain, powerDuration[RAIN_INDEX]);
        LoseResources(WATER_INDEX, powersCosts[RAIN_INDEX]);
        if (intro.introIndex == 1 && intro.text.CheckFinishWriting())
        {
            intro.ValidateIntro(1);
        }
        if (intro.introIndex == 4 && intro.text.CheckFinishWriting())
        {
            intro.ValidateIntro(4);
        }
    }

    public void CauseAStorm()
    {
        actionButtons.SetActive(false);
        GameObject storm = Instantiate<GameObject>(powersPrefabs[STORM_INDEX],
            new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0),
            Quaternion.identity);
        Destroy(storm, powerDuration[STORM_INDEX]);
        LoseResources(FIRE_INDEX, powersCosts[STORM_INDEX]);
        if (intro.introIndex == 1 && intro.text.CheckFinishWriting())
        {
            intro.ValidateIntro(1);
        }
        if (intro.introIndex == 4 && intro.text.CheckFinishWriting())
        {
            intro.ValidateIntro(4);
        }
    }

    public void PlantTrees()
    {
        actionButtons.SetActive(false);
        GameObject trees = Instantiate<GameObject>(powersPrefabs[TREE_INDEX],
            new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0),
            Quaternion.identity);
        Destroy(trees, powerDuration[TREE_INDEX]);
        LoseResources(EARTH_INDEX, powersCosts[TREE_INDEX]);
        if (intro.introIndex == 1 && intro.text.CheckFinishWriting())
        {
            intro.ValidateIntro(1);
        }
        if (intro.introIndex == 4 && intro.text.CheckFinishWriting())
        {
            intro.ValidateIntro(4);
        }

    }

    public void MakeWind()
    {
        actionButtons.SetActive(false);
        GameObject wind = Instantiate<GameObject>(powersPrefabs[WIND_INDEX],
            new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0),
            Quaternion.identity);
        Destroy(wind, powerDuration[WIND_INDEX]);
        LoseResources(AIR_INDEX, powersCosts[WIND_INDEX]);
        if (intro.introIndex == 1 && intro.text.CheckFinishWriting())
        {
            intro.ValidateIntro(1);
        }
        if (intro.introIndex == 4 && intro.text.CheckFinishWriting())
        {
            intro.ValidateIntro(4);
        }

    }

    public void AddFollower()
    {
        followers++;
        followerAnimation.text = "+1";
        followerAnimation.GetComponent<Animator>().SetTrigger("win");
        UpdateFollowersDisplay();
    }

    public void LoseFollower()
    {
        followers--;
        followerAnimation.text = "-1";
        followerAnimation.GetComponent<Animator>().SetTrigger("lose");
        UpdateFollowersDisplay();
        if (intro.introIndex == 6 && intro.text.CheckFinishWriting())
        {
            intro.ValidateIntro(6);
        }
    }

    public void AddResources(int index, int value)
    {
        resources[index] += value;
        resourcesAnimation[index].text = "+" + (value).ToString();
        resourcesAnimation[index].GetComponent<Animator>().SetTrigger("win");
        resourcesDisplay[index].text = resources[index].ToString();
    }

    void LoseResources(int index, int value)
    {
        resources[index] -= value;
        CheckResources();
        resourcesAnimation[index].text = "-" + (value).ToString();
        resourcesAnimation[index].GetComponent<Animator>().SetTrigger("lose");
        resourcesDisplay[index].text = resources[index].ToString();
    }

    bool CheckResources()
    {
        foreach(int v in resources)
        {
            if(v <= 0)
            {
                gc.LaunchEndingScene();
            }
        }

        return true;
    }

    IEnumerator CountDown()
    {
        while (CheckResources())
        {
            int resourceLostIndex = Random.Range(0, resources.Length);
            int resourceLostValue = Random.Range(0, followers + 1);
            LoseResources(resourceLostIndex, resourceLostValue);
            yield return new WaitForSeconds(countDownDelay);
        }
    }

    public void UpdateResourceDisplay()
    {
        for(int i = 0; i < resources.Length; i++)
        {
            resourcesDisplay[i].text = resources[i].ToString();
        }
    }

    public void UpdateFollowersDisplay()
    {
        followersDisplay.text = followers.ToString();
    }
}
