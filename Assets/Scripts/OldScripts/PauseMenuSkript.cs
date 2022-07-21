using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuSkript : MonoBehaviour
{
    public GameObject PauseMenu;
	
	public bool isPaused;

	// Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
			if(Input.GetKeyDown(KeyCode.Escape))
			{
				isPaused = !isPaused;
			}
			
			if (isPaused)
			{
				ActivateMenu();
			}
			
			else
			{
				DeactivateMenu();
			}
    }
	
	
	public void ActivateMenu()
	{
			Time.timeScale = 0;
			PauseMenu.SetActive(true);
	}
	
	public void DeactivateMenu()
	{
			Time.timeScale = 1;
			PauseMenu.SetActive(false);
			isPaused = false;
	}
}

	