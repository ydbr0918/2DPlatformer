using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class TilemapDisabler : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<TilemapRenderer>().enabled = false;
    }








}
