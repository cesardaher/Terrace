using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatMove : ObjMove {
    
    public List<Vector3> posList;
    int pos;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        posList.Add(new Vector3(10f, -6.5f, -5));
        posList.Add(new Vector3(16f, -6.7f, -5));
        posList.Add(new Vector3(10.8f, -5.4f, -5)); //needs to be reversed
        posList.Add(new Vector3(-14f, -6.3f, -5)); //needs to be reversed
    }

    private void OnDisable()
    {
        ClickMng.instance.ReScan();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnCat()
    {
        int newPos = Random.Range(0, posList.Count);

        //make sure cat is alwayus at a diffrent position
        while (newPos == pos)
        {
           newPos = Random.Range(0, posList.Count);
        }

        pos = newPos;

        transform.position = posList[pos];

        if (pos == 2 || pos == 3)
        {
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
            return;
        }

        transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
    }
}
