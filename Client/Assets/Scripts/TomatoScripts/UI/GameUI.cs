using UnityEngine;
using System.Collections;

public class GameUI : MonoBehaviour {
	public virtual void Appear()
	{
		gameObject.SetActive(true);
	}
	
	public virtual void Init()
	{
		
	}	
	
	
	public virtual void DisAppear()
	{
		gameObject.SetActive(false);	
	}
}
