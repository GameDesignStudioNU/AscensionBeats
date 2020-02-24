using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public Transform healthBar;
    public Slider healthFill;
    public float currentHealth;
    public float maxHealth;
    public float healthBarYOffset;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PositionHealthBar();
        if(currentHealth <= 0) {
            Application.LoadLevel(Application.loadedLevel);
        }
    }

    public void ChangeHealth(int amt) {
        currentHealth += amt;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        healthFill.value = currentHealth / maxHealth;
    }

    private void PositionHealthBar() {
        // Vector3 currentPos = transform.position;
        
        // healthBar.position = new Vector3(currentPos.x, currentPos.y + healthBarYOffset, currentPos.z);
        healthBar.LookAt(Camera.main.transform);
    }
}
