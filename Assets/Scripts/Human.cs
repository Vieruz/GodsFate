using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : MonoBehaviour {

    public int faith = 0;
    public int loseFaithDelay = 60;
    public int prayDelay = 5;
    public int resourceGainedWhenPrayingValue = 1;
    public int resourceGainedWhenPrayingIndex;

    public int pregnentChance = 20;
    public int pregnentDelay = 60;
    bool isPregnent = false;
    public GameObject childPrefab;

    public int[] resources;
    public int minStartingResource = 20;
    public int maxStartingResource = 100;
    public int maxCollectedResource = 10;
    public int collectResourceDelay = 30;
    public int loseResourceDelay = 5;
    public Ground actualGround;

    public TextMesh requestTextDisplay;
    public string[] requestTexts;

    public Player god;
    public HumanMovement movement;

    const int FAITH_GAINED_BY_POWERS = 10;
    
    const int AIR_INDEX = 0;
    const int FIRE_INDEX = 1;
    const int EARTH_INDEX = 2;
    const int WATER_INDEX = 3;

    const int NB_RESOURCES = 4;

    // Use this for initialization
    void Start () {
        movement = GetComponent<HumanMovement>();
        StartingResources();
        StartCoroutine("LoseResources");
        StartCoroutine("GoToCollect");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void StartingResources()
    {
        resources = new int[NB_RESOURCES];
        for (int i = 0; i < resources.Length; i++)
        {
            int r = Random.Range(minStartingResource, maxStartingResource + 1);
            resources[i] = r;
        }
    }

    IEnumerator LoseResources()
    {
        while (gameObject.activeSelf)
        {
            int randomDelay = Random.Range(1, loseResourceDelay);
            yield return new WaitForSeconds(randomDelay);
            int randomResourceIndex = Random.Range(0, NB_RESOURCES);
            resources[randomResourceIndex]--;
            CheckResourceNeeded();
            CheckHumanDeath();
        }
    }

    IEnumerator GoToCollect()
    {
        while (gameObject.activeSelf)
        {
            int randomCollectDelay = Random.Range(1, collectResourceDelay);
            yield return new WaitForSeconds(randomCollectDelay);
            Collect();
        }
    }

    void Collect()
    {
        movement.Stop();
        movement.animator.SetTrigger("collecting");
        int resourcesCollected = Random.Range(1, maxCollectedResource);
        if (actualGround)
        {
            resourcesCollected = actualGround.GetResourceFromGround(resourcesCollected);
        }
        resources[resourceGainedWhenPrayingIndex] += resourcesCollected;
        CheckResourceNeeded();
    }

    void CheckResourceNeeded()
    {
        for(int i = 0; i < resources.Length; i++)
        {
            if(resources[i] < 20)
            {
                requestTextDisplay.text = requestTexts[i];
                return;
            }
        }
        requestTextDisplay.text = "";
    }

    void CheckHumanDeath()
    {
        foreach(int v in resources)
        {
            if(v <= 0)
            {
                ForgetGod();
                Destroy(gameObject);
            }
        }
    }

    IEnumerator GoToPray()
    {
        while (faith > 0)
        {
            int r = Random.Range(0, 100);
            if(r <= faith)
            {
                Pray();
            }
            yield return new WaitForSeconds(prayDelay);
        }
        ForgetGod();
    }

    void Pray()
    {
        movement.Stop();
        movement.animator.SetTrigger("praying");
        god.AddResources(resourceGainedWhenPrayingIndex, resourceGainedWhenPrayingValue);
        if (god.intro.introIndex == 3 && god.intro.text.CheckFinishWriting())
        {
            god.intro.ValidateIntro(3);
        }
    }

    public void AddFaith(int value)
    {
        if(faith <= 0)
        {
            god = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            god.AddFollower();
            if (god.intro.introIndex == 2 && god.intro.text.CheckFinishWriting())
            {
                god.intro.ValidateIntro(2);
            }
            //Double faith gained if first time.
            faith += value;
            StartCoroutine("GoToPray");
            StartCoroutine("LoseFaith");
        }
        
        faith += value;

    }

    IEnumerator LoseFaith()
    {
        while(faith > 0)
        {
            int randomDelay = Random.Range(1, loseFaithDelay);
            yield return new WaitForSeconds(randomDelay);
            faith--;
        }
    }

    void ForgetGod()
    {
        if(faith <= 0 && god)
        {
            god.LoseFollower();
            god = null;
        }
    }

    IEnumerator Pregnency()
    {
        isPregnent = true;
        yield return new WaitForSeconds(pregnentDelay);
        Instantiate<GameObject>(childPrefab, transform.position, Quaternion.identity, transform.parent);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //This human discover the powers of the gods !
        if(collision.gameObject.tag == "Powers")
        {
            AddFaith(FAITH_GAINED_BY_POWERS);
            Power p = collision.gameObject.GetComponent<Power>();
            resources[p.resourceIndexGiven] += p.resourceQuantityGiven;
            CheckResourceNeeded();

            if (god.intro.introIndex == 5 && god.intro.text.CheckFinishWriting())
            {
                god.intro.ValidateIntro(5);
            }
        }

        //This human talk to another human !
        if(collision.gameObject.tag == "Humans")
        {
            //Make her pregnent ?
            int r = Random.Range(0, 100);
            if(r <= pregnentChance && !isPregnent)
            {
                StartCoroutine("Pregnency");
            }

            //Talk about religion ?
            if(r <= faith)
            {
                Human theOther = collision.gameObject.GetComponent<Human>();
                theOther.AddFaith(r);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //If this human pray, this is the kind of resource he will generate for his god.
        //If he collecte, he will have this kind too.
        if (collision.gameObject.layer == LayerMask.NameToLayer("GroundLayer"))
        {
            resourceGainedWhenPrayingIndex = collision.GetComponent<Ground>().resourceGainedWhenPrayingIndex;
            actualGround = collision.GetComponent<Ground>();
        }

    }
}
