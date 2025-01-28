using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BodyPartDetection : MonoBehaviour
{
    public enum BodyPart
    {
        RightHand,
        Head,
        LeftHand,
        Body,
        RightLeg,
        LeftLeg
    }

    public BodyPart assignedBodyPart;
    public TMP_Text bodyPartName;
    public AudioClip audioClip;
    private Transform player;

    private void Start()
    {
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
    }

    private void Update()
    {
        if (bodyPartName != null && player != null)
        {
            // Make the TMP_Text object face the player
            bodyPartName.transform.LookAt(player);

            // Flip the text so it faces the player correctly
            bodyPartName.transform.Rotate(0, 180, 0);
        }
    }
}
