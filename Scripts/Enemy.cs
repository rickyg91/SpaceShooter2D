using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;
    private float randomX = 0;
    private Player _player;
    //private Animator _animator;
    [SerializeField]
    private AudioClip _explosionAudioClip;
    private bool _alreadyDead = false;
    private SpawnManager _SpawnManager;
    [SerializeField]
    private GameObject _enemyLaserPrefab;
    [SerializeField]
    private GameObject _explosionVisualizer;

    void Start()
    {
        _SpawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _player = GameObject.Find("Player").GetComponent<Player>();
        //_animator = GetComponent<Animator>();
        //if (_animator == null)
        //{
        //    Debug.Log("Enemy animator is null");
        //}

        string currentLevel = PlayerPrefs.GetString("CurrentLevel");
        if (currentLevel == "MEDIUM")
        {
            StartCoroutine(FireLazerRoutine(4f));
        }
        else if (currentLevel == "HARD")
        {
            StartCoroutine(FireLazerRoutine(3f));
        }
        
    }

    IEnumerator FireLazerRoutine(float spawnTime)
    {
        yield return new WaitForSeconds(3f);

        while (true)
        {
            Instantiate(_enemyLaserPrefab, transform.position + new Vector3(0, -1.6f, 0), Quaternion.identity);

            yield return new WaitForSeconds(spawnTime);

        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        
        if (transform.position.y < -5)
        {
            randomX = Random.Range(-8f, 8f);
            transform.position = new Vector3(randomX, 6.8f, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!_alreadyDead)
        {
            if (other.tag == "Player")
            {
                DestroyEnemy();

                Player player = other.transform.GetComponent<Player>();
                if (player != null)
                {
                    player.Damage();
                }

            }
            else if (other.tag == "Laser")
            {
                DestroyEnemy();

                //Destroy laser
                Destroy(other.gameObject);

                if (_player != null)
                {
                    _player.UpdateScore();
                }

            }
        }
    }

    private void DestroyEnemy()
    {
        _alreadyDead = true;
        _speed = 0;
        _SpawnManager.OnEnemyDeath();
        //_animator.SetTrigger("OnEnemyDeath");
        GameObject explosion = Instantiate(_explosionVisualizer, transform.position, Quaternion.identity);
        Destroy(this.gameObject, 0.2f);
        Destroy(explosion, 2.4f);
        AudioSource.PlayClipAtPoint(_explosionAudioClip, transform.position, 1);
    }
}
