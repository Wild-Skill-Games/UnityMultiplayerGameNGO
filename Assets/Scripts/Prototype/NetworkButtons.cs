using UnityEngine;
using Unity.Netcode;

namespace Prototype
{
    public class NetworkButtons : MonoBehaviour
    {
        private void OnGUI()
        {
            if (GUI.Button(new Rect(10, 10, 100, 30), "Start Host"))
            {
                NetworkManager.Singleton.StartHost();
            }
            if (GUI.Button(new Rect(10, 50, 100, 30), "Start Server"))
            {
                NetworkManager.Singleton.StartServer();
            }
            if (GUI.Button(new Rect(10, 90, 100, 30), "Start Client"))
            {
                NetworkManager.Singleton.StartClient();
            }
        }
    }
}