using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Canon : MonoBehaviour
{
    
    
    /// <summary>
    /// Set Variables
    /// </summary>
    
    
    // Define Floats
    public float power = 0.5f;
    public float minPower, maxPower;
    public float minAngle, maxAngle;
    public float fillPowerSpeed = 1f;
    public float rotationSpeed = 1f;
    public float minimumForceToScore = 30f;

    public float score;
    // Define UI
    public Text scoreText;
    
    public Image powerBar;
    
    //Define Objects (Transforms,Rigidbodies
    public Transform canonBody;
    public Transform ragdollTransform;
    public Transform canonBarrel;
    public Transform firePoint;
    public Rigidbody rb;

    // CharacterJoint Array
    public CharacterJoint[] characterJoints;
    public bool hasFired;
    

    private void Awake()
    {
        characterJoints = ragdollTransform.GetComponentsInChildren<CharacterJoint>();
    }
    
    /// <summary>
    /// Rotates Canon to Left
    /// </summary>
    public void OnRotateLeftButtonPress()
    {
        canonBody.transform.Rotate(Vector3.up,Time.deltaTime * -rotationSpeed);
    }
    
    /// <summary>
    /// Rotates Canon to Right
    /// </summary>
    public void OnRotateRightButtonPress()
    {
        canonBody.transform.Rotate(Vector3.up,Time.deltaTime * rotationSpeed);
    }
    
    /// <summary>
    /// Rotates Canon Upwards
    /// </summary>
    public void OnPivotUpButtonPress()
    {
        canonBarrel.transform.Rotate(Vector3.right,Time.deltaTime * -rotationSpeed);
    }
    
    /// <summary>
    /// Rotates Canon Downwards
    /// </summary>
    public void OnPivotDownButtonPress()
    {
        canonBarrel.transform.Rotate(Vector3.right,Time.deltaTime * rotationSpeed);
    }
    
    /// <summary>
    /// Increases Canon power and changes power bar UI
    /// </summary>
    public void OnPowerUpButtonPress()
    {
        power = Mathf.Clamp01(power + Time.deltaTime * fillPowerSpeed);
        powerBar.fillAmount = power;
        
    }
    /// <summary>
    /// Decreases Canon power and changes power bar UI
    /// </summary>
    public void OnPowerDownButtonPress()
    {
        power = Mathf.Clamp01(power - Time.deltaTime * fillPowerSpeed);
        powerBar.fillAmount = power;
    }
    
    /// <summary>
    /// Fires the ragdoll
    /// </summary>
    public void OnFireButtonPress()
    {
        hasFired = true;
        score = 0;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.AddForce(canonBarrel.forward* Mathf.Lerp(minPower,maxPower,power),ForceMode.Impulse);
        foreach(Rigidbody childrb in ragdollTransform.GetComponentsInChildren<Rigidbody>())
        {
            childrb.velocity = Vector3.zero;
            childrb.angularVelocity = Vector3.zero;
            childrb.AddForce(canonBarrel.forward* Mathf.Lerp(minPower,maxPower,power),ForceMode.Impulse);
        }
        ragdollTransform.position = firePoint.position;
        ragdollTransform.rotation = firePoint.rotation;
        

    }
    // Start is called before the first frame update
    void Start()
    {
        powerBar.fillAmount = power;
    }

    /// <summary>
    /// During FixedUpdate, run through each joint and check force being applied, add that force to the score
    /// </summary>
    void FixedUpdate()
    {
        if(!hasFired)
        {
            return;
        }

        foreach(CharacterJoint characterJoint in characterJoints)
        {
            if(characterJoint.currentForce.magnitude < minimumForceToScore)
            {
                continue;
            }

            score += characterJoint.currentForce.magnitude;
        }

        scoreText.text = "Score: " + score.ToString();


    }
}
