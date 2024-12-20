using UnityEngine;

public class DmgDealer : MonoBehaviour
{
	private float _dmg;
	/// <summary>
	/// The damage that is dealt on hit.
	/// </summary>
	public float Damage
	{
		get { return _dmg; }
		set { _dmg = value; }
	}
}