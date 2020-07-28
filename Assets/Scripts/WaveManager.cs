using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnBatch
{
    public string enemyType;
    public int amountToSpawn;
    public int firstSpawnFrameDelay;
    public int frameDelayBetweenSpawns;

    public EnemySpawnBatch(string enemyType, int amountToSpawn, int firstSpawnFrameDelay, int frameDelayBetweenSpawns)
    {
        this.enemyType = enemyType;
        this.amountToSpawn = amountToSpawn;
        this.firstSpawnFrameDelay = firstSpawnFrameDelay;
        this.frameDelayBetweenSpawns = frameDelayBetweenSpawns;
    }
}

public class Wave
{
    public List<EnemySpawnBatch> enemySpawnInfo { get; private set; }
    public List<EnemySpawnBatch> batchesToSpawn;

    public Wave(List<EnemySpawnBatch> enemySpawnInfo)
    {
        this.enemySpawnInfo = enemySpawnInfo;
    }

    public void Reset()
    {
        batchesToSpawn = new List<EnemySpawnBatch>(enemySpawnInfo);
    }
}

public class WaveManager
{
    public List<Enemy> aliveEnemies;
    public List<Wave> waves;

    public int currentWave;
    private int m_framesSinceWaveStart;

    private List<Spline2DComponent> m_paths;
    private int m_selectedPath;
    private Dictionary<string, Enemy> m_enemyTypes;

    public delegate void OnEnemySpawned(Enemy enemy);
    public event OnEnemySpawned onEnemySpawned;

    public delegate void OnWaveStateChange();
    public event OnWaveStateChange onWaveStarted;
    public event OnWaveStateChange onWaveEnded;

    public WaveManager(List<Spline2DComponent> paths, List<Wave> waves)
    {
        m_paths = paths;
        this.waves = waves;
        currentWave = -1;
        m_framesSinceWaveStart = 0;
        m_enemyTypes = new Dictionary<string, Enemy>();
        aliveEnemies = new List<Enemy>();
        foreach(Wave wave in waves)
        {
            foreach(EnemySpawnBatch batch in wave.enemySpawnInfo)
            {
                m_enemyTypes[batch.enemyType] = Resources.Load<Enemy>(batch.enemyType);
            }
        }
    }

    public void FixedUpdate()
    {
        if(currentWave >=0 && waves[currentWave].batchesToSpawn.Count > 0)
        {
            List<EnemySpawnBatch> batches = new List<EnemySpawnBatch>(waves[currentWave].batchesToSpawn);
            foreach(EnemySpawnBatch batch in batches)
            {
                if(batch.firstSpawnFrameDelay + (batch.frameDelayBetweenSpawns * batch.amountToSpawn) == m_framesSinceWaveStart)
                {
                    waves[currentWave].batchesToSpawn.Remove(batch);
                }
                else if((m_framesSinceWaveStart % batch.frameDelayBetweenSpawns == batch.firstSpawnFrameDelay % batch.frameDelayBetweenSpawns) && (m_framesSinceWaveStart >= batch.firstSpawnFrameDelay))
                {
                    Enemy enemy = GameObject.Instantiate(m_enemyTypes[batch.enemyType]);
                    aliveEnemies.Add(enemy);

                    enemy.Reset(m_paths[(m_selectedPath++) % m_paths.Count]);
                    enemy.onDeath += Enemy_onDeath;
                    onEnemySpawned?.Invoke(enemy);
                }
            }

            m_framesSinceWaveStart++;
        }
    }

    private void Enemy_onDeath(Enemy enemy)
    {
        aliveEnemies.Remove(enemy);
        if(!WaveInProgress())
        {
            onWaveEnded?.Invoke();
        }
    }

    public void StartNextWave()
    {
        if(WaveInProgress())
            return;
        currentWave++;
        m_framesSinceWaveStart = 0;

        waves[currentWave].Reset();
        onWaveStarted?.Invoke();
    }

    public bool WaveInProgress()
    {
        if(currentWave < 0)
            return false;
        return waves[currentWave].batchesToSpawn.Count > 0 || aliveEnemies.Count > 0;
    }
}
