using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astroid : MonoBehaviour
{
    [SerializeField]
    private float _rotateSpeed = 15.0f;
    [SerializeField]
    private float _speed = 4.0f;
    [SerializeField]
    private GameObject _explosionVisualizer;
    private SpawnManager _spawnManager;
    [SerializeField]
    private AudioClip _explosionAudioClip;

    void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * _rotateSpeed * Time.deltaTime);
        //transform.Translate(Vector3.down * _speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            //Destroy laser
            Destroy(other.gameObject);
            DestroyAstroid();
        }
    }

    private void DestroyAstroid()
    {
        _speed = 0;
        GameObject explosion = Instantiate(_explosionVisualizer, transform.position, Quaternion.identity);
        Destroy(this.gameObject, 0.2f);
        Destroy(explosion, 2.4f);
        AudioSource.PlayClipAtPoint(_explosionAudioClip, transform.position, 1);

        //determine level of difficulty
        string currentLevel = PlayerPrefs.GetString("CurrentLevel");
        float enemySpawn = 5f;
        float powerSpawn = 6f;

        if (currentLevel == "MEDIUM")
        {
            enemySpawn = 3f;
            powerSpawn = 8f;
        }
        else if (currentLevel == "HARD")
        {
            enemySpawn = 2f;
            powerSpawn = 10f;
        }

        _spawnManager.StartSpawning(enemySpawn, powerSpawn);
    }
}
