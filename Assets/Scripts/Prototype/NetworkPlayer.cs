using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkPlayer : NetworkBehaviour
{
    public List<GameObject> objectsToDestroy = new();
    public List<MonoBehaviour> behavioursToDestroy = new();

    public override void OnNetworkSpawn()
    {
        if (!IsLocalPlayer)
        {
            foreach (var obj in objectsToDestroy)
            {
                Destroy(obj);
            }
            foreach (var behaviour in behavioursToDestroy)
            {
                Destroy(behaviour);
            }
        }
    }
}