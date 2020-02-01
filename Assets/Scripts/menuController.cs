using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class menuController : MonoBehaviour {

	[Tooltip("_sceneToLoadOnPlay is the name of the scene that will be loaded when users click play")]
	public string sceneToLoadOnPlay = "main";
	[Tooltip("_soundButtons define the SoundOn[0] and SoundOff[1] Button objects.")]
	public Button[] soundButtons;
	[Tooltip("_audioClip defines the audio to be played on button click.")]
	public AudioClip audioClip;
	[Tooltip("_audioSource defines the Audio Source component in this scene.")]
	public AudioSource audioSource;
	
	void Awake () {
		if(!PlayerPrefs.HasKey("_Mute")){
			PlayerPrefs.SetInt("_Mute", 0);
		}
	}
	
	public void PlayGame () {
		audioSource.PlayOneShot(audioClip);
		SceneManager.LoadScene(sceneToLoadOnPlay);
	}
	
	public void Mute () {
		audioSource.PlayOneShot(audioClip);
		soundButtons[0].interactable = true;
		soundButtons[1].interactable = false;
		PlayerPrefs.SetInt("_Mute", 1);
	}
	
	public void Unmute () {
		audioSource.PlayOneShot(audioClip);
		soundButtons[0].interactable = false;
		soundButtons[1].interactable = true;
		PlayerPrefs.SetInt("_Mute", 0);
	}
	
	public void QuitGame () {
		audioSource.PlayOneShot(audioClip);
		#if !UNITY_EDITOR
			Application.Quit();
		#endif
		
		#if UNITY_EDITOR
			EditorApplication.isPlaying = false;
		#endif
	}
}
