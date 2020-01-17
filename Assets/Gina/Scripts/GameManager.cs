using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    internal static int maxLevel = 80;

    // Start is called before the first frame update
    void Start()
    {
        WorldItem.Create(Database.GetItemByID("gina:broad_sword"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
