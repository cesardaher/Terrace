using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrownChair : Chair
{
    Vector3 initialPos = new Vector3(-0.54f, 1.94f, 4f);
    Vector3 initialRot = new Vector3(0, 0, 270);

    Vector3 standPos = new Vector3(0.25f, 1.33f, 4f);
    Vector3 standRot = new Vector3(0, 0, 0);


    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override IEnumerator InteractWithObject()
    {
        //move to object if needed
        Task t = new Task(base.InteractWithObject());

        while (t.Running)
            yield return null;

        //pause normal gameplay
        StateMng.instance.CanClick = false;

        //turn on sit down and exit buttons
        sitButton.gameObject.SetActive(true);
        exitButton.gameObject.SetActive(true);
    }

}
