using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    //Dictionary< InventorySlot , ItemID >
    public Dictionary<int, int> inventory = new Dictionary<int, int>();

    class InventoryItem
    {
        public string name;
        public int itemID;
        public int inventorySlot;
        public string rawEffects;
        public Dictionary<string, float> decodedEffects;
    }

    public int placeholder = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int slot = 0; slot < placeholder; slot++) //wow placeholder
        {
            inventory.Add(slot, -1); //no item will have the ID -1
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
