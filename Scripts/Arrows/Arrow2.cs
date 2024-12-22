using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class Arrow2 : MonoBehaviour
{
    Rigidbody2D arrowRigidbody;
    [SerializeField] float arrowSpeed;
    public Player2 player;
    float arrowScale;
    BoxCollider2D boxCollider;
    [SerializeField] AudioClip arrowSFX;

    // Start is called before the first frame update
    void Start()
    {
        arrowRigidbody = GetComponent<Rigidbody2D>();
        arrowScale = player.transform.localScale.x * arrowSpeed;
        boxCollider = GetComponent<BoxCollider2D>();

    }

    // Update is called once per frame
    void Update()
    {
        arrowRigidbody.velocity = new Vector2(arrowScale, 0f);
        if (boxCollider.IsTouchingLayers(LayerMask.GetMask("Platform", "enemies", "Hazards", "Arrows")))
        {
            // AudioSource.PlayClipAtPoint(arrowSFX, Camera.main.transform.position);
            Destroy(gameObject, 0.2f);
        }
    }
    void flipSprite()
    {

        transform.localScale = new Vector2((Mathf.Sign(arrowRigidbody.velocity.x)), 1f);

    }

}