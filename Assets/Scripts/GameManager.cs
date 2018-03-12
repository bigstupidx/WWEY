using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public AudioClip fallingClip;

	// Editor-Visible Fields
	public static GameManager instance;
	public GameObject rockEnemyFab;
	public GameObject treeEnemyFab;
	public GameObject saveEnemyFab;
	public GameObject plantEnemyFab;
	public List<GameObject> propDestroyList;
	public List<ResetOnRespawn.DeadGuyToRespawn> respawnList = new List<ResetOnRespawn.DeadGuyToRespawn>();
	[SerializeField]
	private GameObject heroPrefab;
	[SerializeField]
	private GameObject tutorialPrefab;
	[SerializeField]
	private GameObject startScreen;
	[SerializeField]
	private GameObject deathScreen;
	[SerializeField]
	private GameObject blackScreen;
	[SerializeField]
	private GameObject winScreen;
	private SavePoint firstSave;
	[SerializeField]
	private List<SavePoint> allSavePoints;

	// Fields
	private SavePoint currentSave;
	private Damageable currentHeroDamageable;
	private GameObject currentHero;
	private TutorialText currentText;
	public TutorialText CurrentText {
		get { return currentText; }
	}

	// Events
	public delegate void HeroRespawnAction(Hero h);
	public event HeroRespawnAction OnHeroRespawn;

	// Methods
	public void SetSave(SavePoint save) {
		if (currentSave != save) {
			if (currentSave != null) {
				currentSave.DecorateAsNotCurrentSave ();
			}
			currentSave = save;
			currentSave.DecorateAsCurrentSave ();
		}
	}

	Hero.GearUnlockModule lastSavedGear = null;

	public bool OverscreenShown() {
		return startScreen.activeSelf || deathScreen.activeSelf || winScreen.activeSelf;
	}


	public void SpawnHero() {
		

		if (startScreen.activeInHierarchy) {
			startScreen.SetActive (false);
		}

		if (deathScreen.activeInHierarchy) {
			deathScreen.SetActive (false);
		}

		foreach (GameObject prop in propDestroyList) {
			Destroy (prop);
		}

		foreach (ResetOnRespawn.DeadGuyToRespawn guy in respawnList) {
			guy.Respawn ();
		}
		respawnList = new List<ResetOnRespawn.DeadGuyToRespawn>();

		GameObject hero = Instantiate (heroPrefab);
		if (lastSavedGear != null) {
			hero.GetComponent<Hero> ().gear = lastSavedGear;
		}
		lastSavedGear = hero.GetComponent<Hero> ().gear;

		if (OnHeroRespawn != null) {
			OnHeroRespawn (hero.GetComponent<Hero>());
		}
		DropHeartOnDeath.sinceLastDrop = 0;

		hero.transform.position = currentSave.transform.position;
		currentHero = hero;
		currentHeroDamageable = hero.GetComponent<Damageable>();
		currentHeroDamageable.OnDied += SetupDeathScreen;
	}

	public void CallVictory () {
		if (currentHero != null) {
			Destroy (currentHero);
		}
		winScreen.SetActive (true);
	}

	public void ResetGame() {
		StartCoroutine (ResetRoutine());
	}

	private IEnumerator ResetRoutine() {
		winScreen.SetActive (false);
		SetSave (firstSave);
		yield return new WaitForSeconds (1.5f);
		SpawnText ("Did you really think that would work?");
		yield return new WaitForSeconds (3.0f);
		blackScreen.SetActive (true);
		yield return new WaitForSeconds (1.5f);
		SceneManager.LoadScene (SceneManager.GetActiveScene().name);
	}

	public void SetupDeathScreen(Damageable dmgble) {
		
		StartCoroutine (DelayDeathScreen ());
	}

	public IEnumerator DelayDeathScreen() {
		yield return new WaitForSeconds (1.0f);
		StartCoroutine (DeathScreenRoutine());
		currentHeroDamageable.OnDied -= SetupDeathScreen;
	}

	private IEnumerator DeathScreenRoutine() {
		yield return new WaitForSeconds (0.25f);
		deathScreen.SetActive (true);
	}

	public void SpawnText(string whatToSay) {
		GameObject text = Instantiate (tutorialPrefab);
		if (currentText != null) {
			currentText.CleanupText ();
		}
		currentText = text.GetComponent<TutorialText> ();
		currentText.Say (whatToSay);
	}

	// Mono-Behavior Methods
	void Awake () {
		if (instance == null) {
			instance = this;
		} else {
			Destroy (this);
		}

		deathScreen.SetActive (false);
	}

	IEnumerator Start () {
		//Give time for all save points to check in.
		yield return new WaitForEndOfFrame ();
		SetSave (firstSave);
	}

	public void CheckInSavePoint(SavePoint p) {
		if (p.isFirst) {
			firstSave = p;
		}
		allSavePoints.Add (p);
	}
}
