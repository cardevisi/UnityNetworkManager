using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {

	int serverPort = 8632;
	string registerGameName = "MASTER_SERVER_TEST";
	//bool isRefreshing = false;
	float refreshRequestLenght = 3.0f;
	HostData[] hostData;
	bool connected = false;


	private void StartServer() {
		Network.InitializeServer(16, serverPort, true);
		//MasterServer.RegisterHost(registerGameName, "SpaceShipGameServer", "First room on server");
	}

	void OnServerInitialized() {
		MasterServer.RegisterHost(registerGameName, "SpaceShipGameServer", "First room on server");
		Debug.Log("Server has been initialied!");
		connected = true;
	}

	void OnConnectedToServer() {
		connected = true;
	}

	void OnDisconnectedFromServer(NetworkDisconnection info) {
		if (Network.isServer)
			Debug.Log("Local server connection disconnected");
		else if (info == NetworkDisconnection.LostConnection)
				Debug.Log("Lost connection to the server");
		else
			Debug.Log("Successfully diconnected from the server");


		connected = false;
	}

	void OnMasterServerEvent(MasterServerEvent masterServerEvent) {
		if (masterServerEvent == MasterServerEvent.RegistrationSucceeded) {
			Debug.Log ("Registration successfull!");
		}
	}

	public IEnumerator RefreshHostList() {
		Debug.Log ("Refreshing...");
		MasterServer.RequestHostList(registerGameName);
		float timeStarted = Time.time;
		float timeEnd = timeStarted + refreshRequestLenght;

		while (Time.time < timeEnd) {
			hostData = MasterServer.PollHostList ();
			yield return new WaitForEndOfFrame();
		}

		if (hostData == null || hostData.Length == 0) {
				Debug.Log ("No active servers have been found");
		} else {
				Debug.Log (hostData.Length + " have been found!");
		}

	}

	void OnGUI () {

		print ("COPNE"+connected);

		if (Network.isServer)
			GUILayout.Label("Running as a server");
		else if (Network.isClient)
			GUILayout.Label("Running as a client");


			if (!connected) {

			/*if (Network.isClient || Network.isServer) {
				return;
			}*/

				if (GUI.Button (new Rect (25f, 25f, 150f, 30f), "Start new Server")) {
						StartServer ();
				}

				if (GUI.Button (new Rect (25f, 65f, 150f, 30f), "Refresh Server List")) {
						StartCoroutine ("RefreshHostList");
				}

				if (hostData != null) {
						for (int i = 0; i < hostData.Length; i++) {
								if (GUI.Button (new Rect (Screen.width / 2, 65f + (30f * i), 300f, 30f), hostData [i].gameName)) {
										Network.Connect ("127.0.0.1", serverPort);
										print ("yay" + NetworkPeerType.Client);
								}
						}
				}

		} else {
				GUILayout.Label ("Connections: " + Network.connections.Length.ToString ());
		}

	}
}