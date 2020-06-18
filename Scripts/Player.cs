using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5f;
    private float _speedMultiplier = 2;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _trippleShotPrefab;
    [SerializeField]
    private float _fireRate = 0.15f;
    private float _nextFire = -1.0f;
    [SerializeField]
    private int _lives = 3;
    private SpawnManager _SpawnManager;
    private bool _trippleShotActive = false;
    private bool _shieldIsActive = false;
    [SerializeField]
    private GameObject _shieldVisualizer;
    private UIManager _uiManager;
    [SerializeField]
    private GameObject _damageLeft;
    [SerializeField]
    private GameObject _damageRight;
    [SerializeField]
    private AudioClip _laserAudioClip;
    [SerializeField]
    private AudioClip _explosionAudioClip;
    [SerializeField]
    private GameObject _explosionVisualizer;

    // Start is called before the first frame update
    void Start()
    {
        //take current position = new position (0, 0, 0)
        transform.position = new Vector3(0, 0, 0);
        _SpawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        if (_SpawnManager == null)
        {
            Debug.LogError("Spawn Manager is null");
        }

        if (_uiManager == null)
        {
            Debug.LogError("UI Manager is null");
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        CreateBoundaries();

        //Note: Time.time = total running time of game
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _nextFire)
        {
            FireLazer();
        }   
    }

    private void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // 1 vector = 1 meter in the real world
        //transform.Translate(Vector3.right * horizontalInput * _speed * Time.deltaTime);
        //transform.Translate(Vector3.up * verticalInput * _speed * Time.deltaTime);

        Vector3 direction = new Vector3(horizontalInput, verticalInput);
        transform.Translate(direction * _speed * Time.deltaTime);
    }

    private void CreateBoundaries()
    {
        float verticalPosition = transform.position.y;
        float horizontalPosition = transform.position.x;

        //if (verticalPosition >= 0)
        //{
        //    transform.position = new Vector3(horizontalPosition, 0);
        //}
        //else if (verticalPosition <= -3.8f)
        //{
        //    transform.position = new Vector3(horizontalPosition, -3.8f);
        //}

        transform.position = new Vector3(horizontalPosition, Mathf.Clamp(verticalPosition, -3.8f, 0));

        if (horizontalPosition >= 9f)
        {
            transform.position = new Vector3(-9f, verticalPosition);
        }
        else if (horizontalPosition < -9f)
        {
            transform.position = new Vector3(9f, verticalPosition);
        }
    }

    private void FireLazer()
    {
        //Debug.Log("Space key pressed");

        _nextFire = Time.time + _fireRate;
        
        if (_trippleShotActive)
        {
            Instantiate(_trippleShotPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
        }

        AudioSource.PlayClipAtPoint(_laserAudioClip, transform.position, 1);
    }

    public void Damage()
    {
        if (_shieldIsActive)
        {
            _shieldIsActive = false;
            _shieldVisualizer.SetActive(false);
            return;
        }

        _lives -= 1;
        _uiManager.UpdateLives(_lives);

        if (_lives == 2)
        {
            _damageLeft.SetActive(true);
            AudioSource.PlayClipAtPoint(_explosionAudioClip, transform.position, 1);
        }
        if (_lives == 1)
        {
            _damageRight.SetActive(true);
            AudioSource.PlayClipAtPoint(_explosionAudioClip, transform.position, 1);
        }
        if (_lives < 1)
        {
            _uiManager.CheckBestScore();
            _SpawnManager.OnPlayerDeath();

            GameObject explosion = Instantiate(_explosionVisualizer, transform.position, Quaternion.identity);
            Destroy(this.gameObject, 0.2f);
            Destroy(explosion, 2.4f);
            AudioSource.PlayClipAtPoint(_explosionAudioClip, transform.position, 1);
        }
    }

    public void trippleShotActivate()
    {
        _trippleShotActive = true;

        StartCoroutine(TrippleShotPowerDown());
    }

    IEnumerator TrippleShotPowerDown()
    {
        yield return new WaitForSeconds(5.0f);

        _trippleShotActive = false;
    }

    public void speedPowerUpActivate()
    {
        _speed = _speed * _speedMultiplier;

        StartCoroutine(SpeedPowerDown());
    }

    IEnumerator SpeedPowerDown()
    {
        yield return new WaitForSeconds(5.0f);

        _speed = _speed / _speedMultiplier;
    }

    public void shieldActivate()
    {
        _shieldIsActive = true;
        _shieldVisualizer.SetActive(true);
    }

    public void UpdateScore()
    {
        _uiManager.updateScore(1);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "EnemyLaser")
        {
            //Destroy laser
            Destroy(other.gameObject);
            Damage();
        }
    }
}
