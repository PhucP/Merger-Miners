using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundary : MonoBehaviour
{
    public GameObject WinPanel;
    private void OnCollisionEnter(Collision other)
    {
        WinPanel.SetActive(true);
    }
}
