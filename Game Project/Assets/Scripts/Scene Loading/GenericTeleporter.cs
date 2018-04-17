using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GenericTeleporter : MonoBehaviour
{

	public GameObject teleportAnim;
	public GameObject teleportSound;
	public string newSceneName;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			teleportSound.SetActive(true);
			teleportAnim.SetActive(true);
			StartCoroutine(LoadAfterAnim());
		}
	}

	public IEnumerator LoadAfterAnim()
	{
		yield return new WaitForSeconds(0.6f);
		float fadeTime = GetComponent<Fading>().BeginFade(1);
		yield return new WaitForSeconds(fadeTime);
		SceneManager.LoadScene(newSceneName);
	}
}


