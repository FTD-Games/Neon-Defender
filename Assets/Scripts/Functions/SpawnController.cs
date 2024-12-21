using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class SpawnController : MonoBehaviour
{
    /// <summary>
    /// The time for sending monsters inside monster state. Example: 0.5f ->
    /// Every 0.5 second a monster is spawned around player.
    /// </summary>
    private float _spawnRate = 0.5f;
    /// <summary>
    /// How long the current state is active in seconds.
    /// </summary>
    private float _currentStateTime;
    /// <summary>
    /// The current time to track the spawn rate. Counts upwards.
    /// </summary>
    private float _currentSpawnRate;
    private SpawnStateData _currentSpawnState;
    /// <summary>
    /// The set difficulty for this spawner.
    /// </summary>
    private Enums.E_Difficulty _difficulty;
    private Transform _playersTransform;
    private List<SpawnStateData> _spawnStatesData = new List<SpawnStateData>();
    private ObjectPooling _pools;

    public void SetupSpawner(Transform player)
    {
        _pools = GameControl.control.GetComponent<ObjectPooling>();
        _pools.ClearAllPools();
        _playersTransform = player;
        SetupDifficulty();
        _currentSpawnState = _spawnStatesData.FirstOrDefault(s => s.state == Enums.E_SpawnState.Monster);
    }

    private void SetupDifficulty()
    {
        _difficulty = GameControl.control.selectedDifficulty;
        var statesCount = Enum.GetNames(typeof(Enums.E_SpawnState)).Length;
        var difficultyCount = Enum.GetNames(typeof(Enums.E_Difficulty)).Length;
        for (int i = 0; i < statesCount; i++)
        {
            var newStateData = new SpawnStateData();
            newStateData.state = (Enums.E_SpawnState)i;
            switch (_difficulty)
            {
                case Enums.E_Difficulty.Easy:
                    switch (newStateData.state)
                    {
                        case Enums.E_SpawnState.Idle:
                            newStateData.chance = 20;
                            break;
                        case Enums.E_SpawnState.Monster:
                            newStateData.chance = 70;
                            break;
                        case Enums.E_SpawnState.Boss:
                            newStateData.chance = 10;
                            break;
                    }
                    break;
                case Enums.E_Difficulty.Normal:
                    switch (newStateData.state)
                    {
                        case Enums.E_SpawnState.Idle:
                            newStateData.chance = 15;
                            break;
                        case Enums.E_SpawnState.Monster:
                            newStateData.chance = 72.5f;
                            break;
                        case Enums.E_SpawnState.Boss:
                            newStateData.chance = 12.5f;
                            break;
                    }
                    break;
                case Enums.E_Difficulty.Hard:
                    switch (newStateData.state)
                    {
                        case Enums.E_SpawnState.Idle:
                            newStateData.chance = 10;
                            break;
                        case Enums.E_SpawnState.Monster:
                            newStateData.chance = 75f;
                            break;
                        case Enums.E_SpawnState.Boss:
                            newStateData.chance = 15f;
                            break;
                    }
                    break;
                case Enums.E_Difficulty.Insane:
                    switch (newStateData.state)
                    {
                        case Enums.E_SpawnState.Idle:
                            newStateData.chance = 5;
                            break;
                        case Enums.E_SpawnState.Monster:
                            newStateData.chance = 77.5f;
                            break;
                        case Enums.E_SpawnState.Boss:
                            newStateData.chance = 17.5f;
                            break;
                    }
                    break;
            }
            _spawnStatesData.Add(newStateData);
        }
    }

    private void Update()
    {
        var deltaTime = Time.deltaTime;
        if (_currentSpawnState.IsEnding(_currentStateTime))
        {
            _currentSpawnState = GetNextSpawnState();
            _currentSpawnState.SetNewDuration(_difficulty);
            _currentStateTime = 0f;
        }
        else
        {
            _currentStateTime += deltaTime;
            DoAction(deltaTime);
        }
    }

    private void DoAction(float deltaTime)
    {
        switch (_currentSpawnState.state)
        {
            case Enums.E_SpawnState.Idle:
                break;
            case Enums.E_SpawnState.Monster:
                if (_currentSpawnRate >= _spawnRate)
                {
                    SpawnMonster();
                    _currentSpawnRate = 0;
                }
                else
                {
                    _currentSpawnRate += deltaTime;
                }
                break;
            case Enums.E_SpawnState.Boss:
                break;
            default:
                Debug.LogWarning("Spawner no state?!");
                break;
        }
    }

    /// <summary>
    /// Gets the next target spawn state by the chance or milestones.
    /// </summary>
    private SpawnStateData GetNextSpawnState()
    {
        var randomValue = UnityEngine.Random.value; // Value between 0 - 1
        var currSumChance = 0f; // Summed up chance to evaluate
        foreach (var state in _spawnStatesData)
        {
            currSumChance += (state.chance / 100);
            if (randomValue <= currSumChance)
                return state;
        }
        return _spawnStatesData.FirstOrDefault(s => s.state == Enums.E_SpawnState.Idle);
    }

    private void SpawnMonster()
    {
        var randomMonster = (Enums.E_Monster)UnityEngine.Random.Range(1, Enum.GetNames(typeof(Enums.E_Monster)).Length);
        var randomPosition = new Vector3(UnityEngine.Random.Range(16, 24), UnityEngine.Random.Range(12, 18));
        randomPosition.x *= UnityEngine.Random.value >= 0.5f ? -1 : 1;
        randomPosition.y *= UnityEngine.Random.value >= 0.5f ? -1 : 1;
        var newMonster = _pools.GetAvailableObject(Enums.E_RequestableObject.Monster, randomMonster);
        newMonster.transform.SetPositionAndRotation((_playersTransform.position + randomPosition), Quaternion.identity);
        newMonster.transform.parent = null;
        newMonster.GetComponent<Monster>().SetupMonster(_playersTransform, _pools);
    }

    /// <summary>
    /// Here the chances for each rarity can be entered in the unity inspector.
    /// </summary>
    [Serializable]
    public class SpawnStateData
    {
        public Enums.E_SpawnState state;
        /// <summary>
        /// Chance in % for example: 25
        /// </summary>
        public float chance;
        /// <summary>
        /// How long the state is active.
        /// </summary>
        public float duration;

        public bool IsEnding(float currentTime) => currentTime >= duration;

        public void SetNewDuration(Enums.E_Difficulty difficulty)
        {
            switch (state)
            {
                case Enums.E_SpawnState.Idle:
                    switch (difficulty)
                    {
                        case Enums.E_Difficulty.Easy:
                            duration = UnityEngine.Random.Range(3, 8);
                            break;
                        case Enums.E_Difficulty.Normal:
                            duration = UnityEngine.Random.Range(3, 7);
                            break;
                        case Enums.E_Difficulty.Hard:
                            duration = UnityEngine.Random.Range(2, 6);
                            break;
                        case Enums.E_Difficulty.Insane:
                            duration = UnityEngine.Random.Range(2, 4);
                            break;
                    }
                    break;
                case Enums.E_SpawnState.Monster:
                    switch (difficulty)
                    {
                        case Enums.E_Difficulty.Easy:
                            duration = UnityEngine.Random.Range(18, 23);
                            break;
                        case Enums.E_Difficulty.Normal:
                            duration = UnityEngine.Random.Range(19, 24);
                            break;
                        case Enums.E_Difficulty.Hard:
                            duration = UnityEngine.Random.Range(21, 26);
                            break;
                        case Enums.E_Difficulty.Insane:
                            duration = UnityEngine.Random.Range(23, 28);
                            break;
                    }
                    break;
                case Enums.E_SpawnState.Boss:
                    switch (difficulty)
                    {
                        case Enums.E_Difficulty.Easy:
                            duration = UnityEngine.Random.Range(50, 54);
                            break;
                        case Enums.E_Difficulty.Normal:
                            duration = UnityEngine.Random.Range(48, 52);
                            break;
                        case Enums.E_Difficulty.Hard:
                            duration = UnityEngine.Random.Range(42, 46);
                            break;
                        case Enums.E_Difficulty.Insane:
                            duration = UnityEngine.Random.Range(36, 40);
                            break;
                    }
                    break;
            }
        }
    }
}
