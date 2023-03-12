using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;
using URandom = UnityEngine.Random;

[Serializable]
public struct ZombieSpawnOption
{
    public ObjectPool zombiePool;
    public float zombiePercentage;
}

public class HordeSpawner : MonoBehaviour
{
    public new Camera camera;
    public List<ZombieSpawnOption> zombieSpawnOptions;
    public LayerMask spawnGround;
    public Player player;
    public DamageObserver damageObserver;
    public ObjectPool impactPool;
    public ObjectPool trailPool;
    public float hordeTime;
    public float spawnRate;
    public int initZombiesToSpawn;
    public int zombieIncreaseRate;
    public bool debugMode;
    public AudioSource siren;
    
    private float _cameraHeight;
    private float _cameraWidth;
    private float _spawnLine;
    private float _lastHorde;
    private int _currentZombies;
    private bool _hordeInEffect = false;
    private CameraController _cameraController;
    private readonly Random _random = new Random();

    private IEnumerator SpawnZombies()
    {
        List<int> zombiesToSpawn = zombieSpawnOptions
            .Select(f => Mathf.FloorToInt(f.zombiePercentage * _currentZombies)).ToList();
        List<ObjectPool> zombiePools = zombieSpawnOptions.Select(f => f.zombiePool).ToList();
        
        for (int zombies = 0; zombies < _currentZombies; zombies++) {
            Vector2 position;
            Vector3 centre = camera.transform.position;
            Vector2 origin = new Vector2(
                centre.x - _cameraWidth / 2,
                centre.z - _cameraHeight / 2);
            
            do
            {
                float linePosition = URandom.Range(0f, _spawnLine);
                
                if (linePosition < _cameraHeight)
                {
                    position = origin + new Vector2(0f, linePosition);
                }
                else if (linePosition < _cameraHeight + _cameraWidth)
                {
                    position = origin + new Vector2(linePosition - _cameraHeight, _cameraHeight);
                }
                else if (linePosition < _cameraHeight + _cameraWidth + _cameraHeight)
                {
                    position = origin + new Vector2(_cameraWidth, linePosition - _cameraHeight - _cameraWidth);
                }
                else
                {
                    position = origin + new Vector2(linePosition - _cameraHeight - _cameraWidth - _cameraHeight,
                        0f);
                }

                yield return new WaitForSecondsRealtime(0.05f);
            } while (!Physics.Raycast(
                         new Vector3(position.x, centre.y, position.y), 
                         Vector3.down,
                         _cameraController.height + 6, 
                         spawnGround));

            int index = _random.Next(zombiesToSpawn.Count);
            
            zombiePools[index].GetAndActivate(o =>
            {
                Zombie zombie = o.GetComponent<Zombie>();

                zombie.ResetHealth();
                zombie.target = player;
                zombie.damageObservers.Add(damageObserver);

                o.transform.position = new Vector3(position.x, player.transform.position.y + 1, position.y);
                Weapon weapon = zombie.weapon;

                if (weapon is not FiringWeapon fw) return;
                if (fw.shootingRenderer == null) return;

                fw.shootingRenderer.impactPool = impactPool;
                fw.shootingRenderer.trailPool = trailPool;
            });
            zombiesToSpawn[index]--;
            if (zombiesToSpawn[index] <= 0)
            {
                zombiesToSpawn.RemoveAt(index);
                zombiePools.RemoveAt(index);
            }

            if (zombiesToSpawn.Count == 0) break;
            yield return new WaitForSeconds(spawnRate);
        }

        _lastHorde = Time.time;
        _currentZombies += zombieIncreaseRate;
        _hordeInEffect = false;
        yield return null;
    }
    
    private void Start()
    {
        _cameraController = camera.GetComponent<CameraController>();
        
        _cameraHeight = 2f * _cameraController.height * Mathf.Tan(camera.fieldOfView * 0.5f * Mathf.Deg2Rad) * 1.5f;
        _cameraWidth = _cameraHeight * camera.aspect * 1.3f;

        _spawnLine = 2 * _cameraHeight + 2 * _cameraWidth;
        _currentZombies = initZombiesToSpawn;
        _lastHorde = debugMode ? Time.time - hordeTime : Time.time;
        _hordeInEffect = false;
    }

    private void Update()
    {
        if (_lastHorde + hordeTime > Time.time || _hordeInEffect) return;

        _hordeInEffect = true;
        siren.Play();
        StartCoroutine(SpawnZombies());
    }
}