using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {

    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, Screen.width - 20, Screen.height - 20));

        if (GUILayout.Button("Start Server", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true)))
        {
            //start server
            BoltLauncher.StartServer(UdpKit.UdpEndPoint.Parse("127.0.0.1:27000"));
            BoltNetwork.LoadScene("Test");
        }

        if (GUILayout.Button("Start Client", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true)))
        {
            //Start Client 
            BoltLauncher.StartClient();
            BoltNetwork.Connect(UdpKit.UdpEndPoint.Parse("127.0.0.1:27000"));
        }

        GUILayout.EndArea();
    }
}
