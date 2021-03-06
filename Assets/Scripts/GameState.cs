﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Path;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameState : MonoBehaviour
{
    public int currency { get; private set; }
    public int lives { get; private set; }

    [SerializeField]
    List<Spline2DRoadComponent> m_paths;
    WaveManager m_waveManager;

    public GameObject objectStretch;

    public static GameState instance { get; private set; }

    public List<Tower> towers { get; private set; }

    private void Awake()
    {
        if(instance)
            Destroy(gameObject);
        instance = this;
        SceneManager.sceneUnloaded += SceneManager_sceneUnloaded;

        lives = 100;
        currency = 300;

        towers = new List<Tower>(Resources.LoadAll<Tower>("Tower/"));

        List<Wave> waves = new List<Wave>();
        List<EnemySpawnBatch> enemySpawnBatches = new List<EnemySpawnBatch>();
        enemySpawnBatches.Add(new EnemySpawnBatch("Test Enemy", 10, 0, 20));
        enemySpawnBatches.Add(new EnemySpawnBatch("Test Enemy", 10, 420, 13));

        waves.Add(new Wave(enemySpawnBatches));

        m_waveManager = new WaveManager(m_paths, waves);
        m_waveManager.onEnemySpawned += waveManager_onEnemySpawned;
    }

    private void waveManager_onEnemySpawned(Enemy enemy)
    {
        enemy.onPathFinished += Enemy_onPathFinished;
        enemy.onDeath += Enemy_onDeath;
    }

    private void Enemy_onDeath(Enemy enemy)
    {
        AddCurrency(enemy.money);
        enemy.onDeath -= Enemy_onDeath;
        enemy.onPathFinished -= Enemy_onPathFinished;
    }

    private void Enemy_onPathFinished(Enemy enemy)
    {
        lives -= enemy.damage;
        enemy.onDeath -= Enemy_onDeath;
        enemy.onPathFinished -= Enemy_onPathFinished;
    }

    private void SceneManager_sceneUnloaded(Scene arg0)
    {
        instance = null;
    }

    public void AddCurrency(int amount)
    {
        currency += amount;
    }

    public bool SpendCurrency(int amount)
    {
        if(currency < amount)
            return false;

        currency -= amount;
        return true;
    }

    // Start is called before the first frame update
    void Start()
    {
        //Debug
        m_waveManager.StartNextWave();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        m_waveManager.FixedUpdate();
    }
}
