using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.0f;
    [SerializeField]
    private int powerUpId;
    [SerializeField]
    private AudioClip _powerUpAudioClip;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -5)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                switch (powerUpId)
                {
                    case 0:
                        player.trippleShotActivate();
                        break;
                    case 1:
                        player.speedPowerUpActivate();
                        break;
                    case 2:
                        player.shieldActivate();
                        break;
                    default:
                        Debug.Log("Unknown power up ID");
                        break;
                }
            }

            AudioSource.PlayClipAtPoint(_powerUpAudioClip, transform.position, 1);
            Destroy(this.gameObject);
        }
    }
}
