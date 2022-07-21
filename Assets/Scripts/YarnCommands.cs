using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using UnityEngine.UI;
using TMPro;

public class YarnCommands : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] Animator playerAnimator;
    [SerializeField] DialogueRunner dialogueRunner;
    [SerializeField] DialogueUI dialogueUI;
    [SerializeField] InMemoryVariableStorage variableStorage;


    Vector2 starterPos = new Vector2(-5f, -5f);
    Vector3 friendChatPos = new Vector3(-6.8f, -6.2f, -8);

    private void Awake()
    {
        dialogueRunner.AddCommandHandler("move", YarnMoveTo);
        dialogueRunner.AddCommandHandler("sit", SitDown);
        dialogueRunner.AddCommandHandler("getUp", GetUp);
        dialogueRunner.AddCommandHandler("zoomIn", ZoomIn);
        dialogueRunner.AddCommandHandler("zoomOut", ZoomOut);
        dialogueRunner.AddCommandHandler("normalZoom", NormalZoom);
        dialogueRunner.AddCommandHandler("fadeIn", CallFadeIn);
        dialogueRunner.AddCommandHandler("fadeOut", CallFadeOut);
        dialogueRunner.AddCommandHandler("close", Close);
        dialogueRunner.AddCommandHandler("passTime", CallPassTime);
        dialogueRunner.AddCommandHandler("plant", Plant);
        dialogueRunner.AddCommandHandler("cutscene", ToggleScene);  //MIGHT WANT TO TURN THIS INTO A BUILT IN DIALOGUE RUNNER FUCTION
        dialogueRunner.AddCommandHandler("interacting", ToggleInteraction);
        dialogueRunner.AddCommandHandler("flipChar", FlipCharacter);
        dialogueRunner.AddCommandHandler("dryPlant", DryPlant);
        dialogueRunner.AddCommandHandler("getSeed", GetSeed);
        dialogueRunner.AddCommandHandler("getPlant", GetPlant);

        // story specifics
        dialogueRunner.AddCommandHandler("showDiary", DiaryCanvas);
        dialogueRunner.AddCommandHandler("showSparrows", ShowSparrows);
        dialogueRunner.AddCommandHandler("flyAway", FlyAway);
        dialogueRunner.AddCommandHandler("endIntro", EndIntro);
        dialogueRunner.AddCommandHandler("fadeTitle", CallFadeTitle);
        dialogueRunner.AddCommandHandler("prepareTea", CallPrepareTea);
        dialogueRunner.AddCommandHandler("endTea", EndTea);
        dialogueRunner.AddCommandHandler("showCatSeed", CallShowCatSeed);
        dialogueRunner.AddCommandHandler("hideCatSeed", CallHideCatSeed);


        // functions 
        // still need to understand this better;
        dialogueRunner.Dialogue.library.RegisterFunction("hasPlant", 1, delegate (Yarn.Value[] parameters) {
            int id = 0;
            string plantName = parameters[0].AsString;

            if (plantName == "mint")
            {
                id = 1;
            }
            else if (plantName == "valerian")
            {
                id = 2;
            }

            if (id == 0)
            {
                Debug.LogError("Trying to check a plant that doesn't exist.");
            }
            else
            {
                foreach (int plant in InventoryMng.instance.plantPage)
                {
                    if (plant == id)
                    {
                        return new Yarn.Value(true);
                    }
                }
            }
            return new Yarn.Value(false);
        });
    }

    public void Close(string[] parameters)
    {
        dialogueUI.dialogueContainer.SetActive(false);
    }
    public void NormalZoom(string[] parameters, System.Action onComplete)
    {
        StartCoroutine(ClickMng.instance.NormalZoom(onComplete));
    }

    public void ZoomIn(string[] parameters, System.Action onComplete)
    {
        StartCoroutine(ClickMng.instance.ZoomIn(onComplete));
    }

    public void ZoomOut(string[] parameters, System.Action onComplete) 
    {
        StartCoroutine(ClickMng.instance.ZoomOut(onComplete));
    }

    public void SitDown(string[] parameters)
    {
       playerAnimator.SetBool("isSitting", true);
    }

    public void GetUp(string[] parameters)
    {
        StateMng.instance.IsSitting = false;
        playerAnimator.SetBool("isSitting", false);
    }

    public void CallFadeIn(string[] parameters, System.Action onComplete)
    {
        float fadeTime = 3f;

        if (parameters.Length != 0)
            fadeTime = float.Parse(parameters[0], System.Globalization.CultureInfo.InvariantCulture);

        StartCoroutine(FadeIn(fadeTime, onComplete));
    }

    public IEnumerator FadeIn(float fadeTime, System.Action onComplete)
    {
        Task t = new Task(TimeMng.instance.FadeIn(fadeTime));

        while (t.Running)
            yield return null;

        onComplete();
    }

    public void CallFadeOut(string[] parameters, System.Action onComplete)
    {
        float fadeTime = 3f;

        if (parameters.Length != 0)
            fadeTime = float.Parse(parameters[0], System.Globalization.CultureInfo.InvariantCulture);

        StartCoroutine(FadeOut(fadeTime, onComplete));
    }

    public IEnumerator FadeOut(float fadeTime, System.Action onComplete)
    {
        Task t = new Task(TimeMng.instance.FadeOut(fadeTime));

        while (t.Running)
            yield return null;

        onComplete();
    }

    public void CallPassTime(string[] parameters, System.Action onComplete)
    {
        StartCoroutine(PassTime(onComplete));
    }

    public void CallPrepareTea(string[] parameters, System.Action onComplete)
    {
        StartCoroutine(StoryMng.instance.PrepareTeaScene(onComplete));
    }
    public void EndTea(string[] parameters)
    {
        StoryMng.instance.teaSet.SetActive(false);
        StoryMng.instance.friend.gameObject.SetActive(false);
    }

    public IEnumerator PassTime(System.Action onComplete)
    {
        Task t = new Task(TimeMng.instance.PassTime());

        while (t.Running)
            yield return null;

        onComplete();
    }

    public void YarnMoveTo(string[] destination, System.Action onComplete)
    {
        Vector3 vdestination = new Vector2(0,0);


        if (destination[0] == "starterPos")
        {
            vdestination = starterPos;

        } else if (destination[0] == "friendChatPos")
        {
            vdestination = friendChatPos;
        }

        StartCoroutine(ClickMng.instance.MoveCharacterExact(vdestination, onComplete));

    }

    public void GetSeed(string[] parameters)
    {
        InventoryMng.instance.seedsPage.Add(int.Parse(parameters[0]));
        SoundList.instance.CollectSound(ClickMng.instance.player);
    }

    public void GetPlant(string[] parameters)
    {
        InventoryMng.instance.plantPage.Add(int.Parse(parameters[0]));
        SoundList.instance.CollectSound(ClickMng.instance.player);
    }

    public void Plant(string[] parameters)
    {
        StateMng.instance.CanClick = false;
        InventoryMng.instance.plantMenu.SetActive(true);
        InventoryMng.instance.ShowSeedDescription(true, true);
    }

    public void ToggleScene(string[] parameters) //MIGHT WANT TO TURN THIS INTO A BUILT IN DIALOGUE RUNNER FUCTION
    {
        if(parameters[0] == "on")
        {
            StateMng.instance.cutscene = true;
            InteractableObject.DisableCanvas();

        } else if (parameters[0] == "off")
        {
            StateMng.instance.cutscene = false;
        }
    }

    public void DiaryCanvas(string[] parameters)
    {
        StoryMng.instance.diary.GetComponent<Diary>().objCanvas.gameObject.SetActive(true);
    }

    public void CallShowCatSeed(string[] parameters, System.Action onComplete)
    {
        StartCoroutine(ShowCatSeed(parameters, onComplete));
    }

    public void CallHideCatSeed(string[] parameters, System.Action onComplete)
    {
        StartCoroutine(HideCatSeed(parameters, onComplete));
    }

    public IEnumerator ShowCatSeed(string[] parameters, System.Action onComplete)
    {
        float time = float.Parse(parameters[0], System.Globalization.CultureInfo.InvariantCulture);
        
        StoryMng.instance.mysterySeed.SetActive(true);

        Task t = new Task(StoryMng.instance.FadeObject(StoryMng.instance.mysterySeed, time, true));

        while (t.Running)
            yield return null;

        onComplete();
    }

    public IEnumerator HideCatSeed(string[] parameters, System.Action onComplete)
    {
        float time = float.Parse(parameters[0], System.Globalization.CultureInfo.InvariantCulture);

        Task t = new Task(StoryMng.instance.FadeObject(StoryMng.instance.mysterySeed, time, false));

        while (t.Running)
            yield return null;

        StoryMng.instance.mysterySeed.SetActive(false);

        onComplete();
    }

    public void FlipCharacter(string[] parameters)
    {
        ClickMng.instance.SpriteFlip();
    }

    public void ShowSparrows(string[] parameters)
    {
        StartCoroutine(StoryMng.instance.SparrowSounds());
        StoryMng.instance.valerianSeeds.SetActive(true);
        StoryMng.instance.mintSeeds.SetActive(true);
    }
    
    public void FlyAway(string[] parameters)
    {
        StoryMng.instance.sparrowAnim.SetBool("flyAway", true);
    }

    public void DryPlant(string[] parameters)
    {
        int id = 0;

        if(parameters[0] == "mint")
        {
            id = 1;
        } else if(parameters[0] == "valerian")
        {
            id = 2;
        }

        if(id == 0)
        {
            Debug.LogError("Trying to dry a plant that does not exist.");

        } else ((Hanger)InteractableObject.currentObj).DryPlant(id);
    }

    public void EndIntro(string[] parameters)
    {

    }

    public void CallFadeTitle(string[] parameters, System.Action onComplete)
    {
        StartCoroutine(FadeTitle(parameters, onComplete));
    }

    public IEnumerator FadeTitle(string[] parameters, System.Action onComplete) // coroutine on top of coroutine
    {
        float time = float.Parse(parameters[0], System.Globalization.CultureInfo.InvariantCulture);

        StoryMng.instance.title.SetActive(true);

        Task t = new Task(StoryMng.instance.FadeObject(StoryMng.instance.title, time, true));

        while (t.Running)
            yield return null;

        yield return new WaitForSeconds(2 * time);

        t = new Task(StoryMng.instance.FadeObject(StoryMng.instance.title, time, false));

        while (t.Running)
            yield return null;


        StoryMng.instance.title.SetActive(false);


        onComplete();
    }

    public void ToggleInteraction(string[] parameters)
    {
        if(parameters[0] == "on")
        {
            StateMng.instance.interacting = true;
            return;
        }

        if(parameters[0] == "off")
        {
            StateMng.instance.interacting = false;
            return;
        }
    }

    /*
    public Yarn.Function HasPlant(params string[] parameters)
    {
        int id = 0;

        if (parameters[0] == "mint")
        {
            id = 1;
        }
        else if (parameters[0] == "valerian")
        {
            id = 2;
        }

        if (id == 0)
        {
            Debug.LogError("Trying to check a plant that doesn't exist.");
        }
        else
        {
            foreach(int plant in InventoryMng.instance.plantPage)
            {
                if(plant == id)
                {
                    return new Yarn.Value(true);
                }
            }
        }

        return new Yarn.Value(false);
    }
    */
}
