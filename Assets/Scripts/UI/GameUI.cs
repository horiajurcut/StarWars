﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour {

    public Image fadePlane;
    public GameObject gameOverUI;

    public RectTransform newWaveBanner;
    public Text newWaveTitle;
    public Text newWaveEnemyCount;
    public Text scoreUI;
    public Text gameOverScoreUI;
    public RectTransform healthBar;

    Player player;
    Spawner spawner;

    private void Awake()
    {
        spawner = FindObjectOfType<Spawner>();
        spawner.OnNewWave += OnNewWave;
    }

    private void Start () {
        player = FindObjectOfType<Player>();
        player.OnDeath += OnGameOver;
	}

    private void Update()
    {
        scoreUI.text = ScoreManager.score.ToString("D6");

        float healthPercent = 0f;
        if (player != null)
        {
            healthPercent = player.health / player.initialHealth;
        }
        healthBar.localScale = new Vector3(healthPercent, 1f, 1f);
    }

    private void OnNewWave(int waveNumber)
    {
        string[] numbers = { "ONE", "TWO", "THREE", "FOUR", "FIVE" };
        newWaveTitle.text = "WAVE " + numbers[waveNumber - 1];
        string enemyCountString = spawner.waves[waveNumber - 1].infinite ? "Infinite" : spawner.waves[waveNumber - 1].enemyCount.ToString();
        newWaveEnemyCount.text = "Enemies: " + enemyCountString;

        // Animate new wave banner
        StopCoroutine("AnimateNewWaveBanner");
        StartCoroutine("AnimateNewWaveBanner");
    }
	
    private void OnGameOver()
    {
        Cursor.visible = true;
        StartCoroutine(Fade(Color.clear, new Color(0, 0, 0, .7f), 2f));
        gameOverScoreUI.text = scoreUI.text;
        scoreUI.transform.parent.gameObject.SetActive(false);
        healthBar.transform.parent.gameObject.SetActive(false);
        gameOverUI.SetActive(true);
    }

    private IEnumerator Fade(Color from, Color to, float time)
    {
        float speed = 1 / time;
        float percent = 0f;

        while (percent < 1)
        {
            percent += Time.deltaTime * speed;
            fadePlane.color = Color.Lerp(from, to, percent);
            yield return null;
        }
    }

    private IEnumerator AnimateNewWaveBanner()
    {
        float delayTime = 2f;
        float speed = 2.5f;
        float percent = 0f;
        int direction = 1;

        float endDelayTime = Time.time + 1f / speed + delayTime;

        while (percent >= 0f)
        {
            percent += Time.deltaTime * speed * direction;

            if (percent >= 1f)
            {
                percent = 1f;
                if (Time.time > endDelayTime)
                {
                    direction = -1;
                }
            }

            newWaveBanner.anchoredPosition = Vector2.up * Mathf.Lerp(-40, 120, percent);
            yield return null;
        }
    }

    // UI Input
    public void StartNewGame()
    {
        SceneManager.LoadScene("Main");
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
