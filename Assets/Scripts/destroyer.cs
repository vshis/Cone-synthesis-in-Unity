using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyer : MonoBehaviour
{
    //public Renderer rend;
    public float lifeTime = 1f;

    //void Start()
    //{
    //    rend = GetComponent<Renderer>();
    //    rend.enabled = true;
    //}

    // Update is called once per frame
    void Update()
    {
        //bool oddeven = Mathf.FloorToInt(Time.time) % 2 == 0;
        //rend.enabled = oddeven;

        if(lifeTime > 0)
        {
            lifeTime -= Time.deltaTime;
            if(lifeTime <= 0)
            {
                Destruction();
            }
        }

        if(this.transform.position.y <= -20)
        {
            Destruction();
        }

    }

    void OnCollisionEnter(Collision coll)
    {
        if(coll.gameObject.name == "destroyer")
        {
            Destruction();
        }
    }

    public void Destruction()
    {
        Destroy(this.gameObject);
    }
}
