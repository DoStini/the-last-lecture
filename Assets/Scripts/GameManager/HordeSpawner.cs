using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class HordeSpawner : MonoBehaviour
{
    public new Camera camera;
    public ObjectPool zombiePool;
    public LayerMask spawnGround;
    public List<Zombie> zombiePrefabs;
    public Player player;
    public DamageObserver damageObserver;
    public float hordeTime;
    public float spawnRate;
    
    private float _cameraHeight;
    private float _cameraWidth;
    private float _spawnLine;
    private float _lastHorde;
    private bool _hordeInEffect = false;
    private CameraController _cameraController;

    private IEnumerator SpawnZombies()
    {
        for (int zombies = 0; zombies < 10; zombies++) {
            Vector2 position;
            Vector3 centre = camera.transform.position;
            Vector2 origin = new Vector2(
                centre.x - _cameraWidth / 2,
                centre.z - _cameraHeight / 2);
            
            do
            {
                float linePosition = Random.Range(0f, _spawnLine);
                
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
            } while (!Physics.Raycast(
                         new Vector3(position.x, centre.y, position.y), 
                         Vector3.down,
                         _cameraController.height + 6, 
                         spawnGround));


            zombiePool.GetAndActivate(o =>
            {
                Zombie zombie = o.GetComponent<Zombie>();

                zombie.ResetHealth();
                zombie.target = player;
                zombie.damageObservers.Add(damageObserver);

                o.transform.position = new Vector3(position.x, player.transform.position.y + 1, position.y);
            });

            yield return new WaitForSeconds(spawnRate);
        }

        _lastHorde = Time.time;
        _hordeInEffect = false;
        yield return null;
    }
    
    private void Start()
    {
        _cameraController = camera.GetComponent<CameraController>();
        
        _cameraHeight = 2f * _cameraController.height * Mathf.Tan(camera.fieldOfView * 0.5f * Mathf.Deg2Rad) * 1.3f;
        _cameraWidth = _cameraHeight * camera.aspect * 1.3f;

        _spawnLine = 2 * _cameraHeight + 2 * _cameraWidth;
    }

    private void Update()
    {
        if (_lastHorde + hordeTime > Time.time || _hordeInEffect) return;

        _hordeInEffect = true;
        StartCoroutine(SpawnZombies());
    }
}