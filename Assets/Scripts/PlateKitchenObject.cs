using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Netcode;

public class PlateKitchenObject : KitchenObject
{
    public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;
    public class OnIngredientAddedEventArgs : EventArgs
    {
        public KitchenObjectsSO kitchenObjectSO;
    }

    [SerializeField] private List<KitchenObjectsSO> validKitchenObjectSOList; 

    List<KitchenObjectsSO> kitchenObjectSOList;

    protected override void Awake()
    {
        base.Awake();
        kitchenObjectSOList = new List<KitchenObjectsSO>();
    }
    public bool TryAddIngrdient(KitchenObjectsSO kitchenObjectSO)
    {
        if (!validKitchenObjectSOList.Contains(kitchenObjectSO))
        {
            // not a valid Type
            return false;
        }

        if (kitchenObjectSOList.Contains(kitchenObjectSO))
        {
            // All Ready Has This Type
            return false;
        }
        else
        {
            AddIngrdientServerRpc(KitchenGameMultiplayer.Instance.GetKitcheObjectSOIndex(kitchenObjectSO));
            
            return true;
        }
        
    }

    [ServerRpc(RequireOwnership = false)]
    private void AddIngrdientServerRpc(int kitchenObjectSOIndex)
    {
        AddIngrdientClientRpc(kitchenObjectSOIndex);
    }
    [ClientRpc]
    private void AddIngrdientClientRpc(int kitchenObjectSOIndex)
    {
        KitchenObjectsSO kitchenObjectSO = KitchenGameMultiplayer.Instance.GetKitchenObjectSOFromIndex(kitchenObjectSOIndex);
        kitchenObjectSOList.Add(kitchenObjectSO);
        OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs { kitchenObjectSO = kitchenObjectSO });
    }

    public List<KitchenObjectsSO> GetKitchenObjectSOList()
    {
        return kitchenObjectSOList;
    }
}
