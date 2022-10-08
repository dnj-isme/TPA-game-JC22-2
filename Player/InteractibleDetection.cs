using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractibleDetection : MonoBehaviour
{
    [SerializeField] private PlayerInteraction playerInteraction;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Weapon"))
        {
            playerInteraction.AddInteractableTarget(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Weapon"))
        {
            playerInteraction.RemoveInteractableTarget(other.gameObject);
        }
    }
}
