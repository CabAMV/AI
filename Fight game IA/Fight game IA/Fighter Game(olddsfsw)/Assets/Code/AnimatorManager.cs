using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Controller for the animations of the characters
public class AnimatorManager : MonoBehaviour 
{

	[SerializeField]
	private Animator RyuAnimator;
	[SerializeField]
	private Animator KenAnimator;

	// Check if any other state is going on except idle for all the characters
	public bool isPlayingAnim()
	{
		if (!this.RyuAnimator.GetCurrentAnimatorStateInfo (0).IsName("Ryu_idle") || !this.KenAnimator.GetCurrentAnimatorStateInfo (0).IsName("Ken_idle"))
			return true;
		else
			return false;
	
	}

	// Checks if one character is KO
	public bool getKO()
	{
		if (RyuAnimator.GetBool("KO"))
			return true;
		else if (KenAnimator.GetBool("KO"))
			return true;
		else
			return false;
	}

	// Change Ry animations
	public void launchRyuAnim(Movements move)
	{
		switch (move)
		{
		case Movements.AttackMedium:
			
			RyuAnimator.SetTrigger ("MidAttack");
			break;

		case Movements.AttackLow:
			RyuAnimator.SetTrigger ("LowAttack");
			break;

		case Movements.DefenseMedium:
			RyuAnimator.SetTrigger ("MidBlock");
			break;

		case Movements.DefenseLow:
			RyuAnimator.SetTrigger ("LowBlock");
			break;
		}
		
	}

	// Changes Ken animations
	public void launchKenAnim(Movements move)
	{
		switch (move)
		{
		case Movements.AttackMedium:
			KenAnimator.SetTrigger ("MidAttack");
			break;

		case Movements.AttackLow:
			KenAnimator.SetTrigger ("LowAttack");
			break;

		case Movements.DefenseMedium:
			KenAnimator.SetTrigger ("MidBlock");
			break;

		case Movements.DefenseLow:
			KenAnimator.SetTrigger ("LowBlock");
			break;
		}
	}

	// Set KO to Ryu
	public void launchRyuKO(bool ko)
	{
		RyuAnimator.SetBool ("KO",ko);
	}

	// Set KO to Ken
	public void launchKenKO(bool ko)
	{
		KenAnimator.SetBool ("KO",ko);

	}

	// Doble KO 
	public void doubleKO(bool state)
	{
		launchRyuKO (state);
		launchKenKO (state);
	}
}
