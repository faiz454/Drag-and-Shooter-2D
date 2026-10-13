using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragShoot2D : MonoBehaviour
{
    public float launchPower = 8f;
    public float maxDragDistance = 2.5f;
    public LineRenderer aimLine;

    private Rigidbody2D rb;
    private Camera mainCamera;
    private Vector2 startPosition;
    private bool isDragging;
    private bool hasShot;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;

        startPosition = rb.position;
        rb.bodyType = RigidbodyType2D.Kinematic;

        if(aimLine != null)
        {
            aimLine.positionCount = 2;
            aimLine.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (hasShot)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hit = Physics2D.OverlapPoint(mousePosition);
            if(hit!= null && hit.gameObject == gameObject)
            {
                isDragging = true;

                if(aimLine != null)
                    aimLine.enabled = true;

            }
        }
        if(isDragging && Input.GetMouseButton(0))
        {
            Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector2 offset = mousePosition - startPosition;
            if (offset.magnitude > maxDragDistance)
                offset = offset.normalized * maxDragDistance;

            rb.position = startPosition + offset;

            if(aimLine!= null)
            {
                aimLine.SetPosition(0, startPosition);
                aimLine.SetPosition(1, rb.position);
            }
        }

        if(isDragging && Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            hasShot = true;

            if (aimLine != null)
                aimLine.enabled = false;

            Vector2 direction = startPosition - rb.position;

            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.AddForce(direction * launchPower, ForceMode2D.Impulse);
        }
    }
}
