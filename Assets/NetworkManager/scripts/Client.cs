using UnityEngine;
using System.Collections;

public class Client : MonoBehaviour {

	public Transform playerPrefab;
	private bool serverStarted = true;

	void CreatePlayer2D () {
		Transform playerClone = Network.Instantiate (playerPrefab, gameObject.transform.position, gameObject.transform.rotation, 0) as Transform;
		// Cria uma cor randomica para o jogador e envia via RPC para todos conectados neste momento.
		playerClone.networkView.RPC("ChangeColor", RPCMode.All,Random.Range(0,255),Random.Range(0,255),Random.Range(0,255));
		// Atualiza o nome do jogador para todos conectados neste momento.
		playerClone.networkView.RPC("ChangePlayerName", RPCMode.All,GameObject.Find("PlayerNameGUIText").guiText.text);
		
	}

	void OnConnectedToServer() {
		Debug.Log ("OnConnectedToServer");
		CreatePlayer2D();
	}
	
	void OnServerInitialized() {
		Debug.Log ("OnServerInitialized");
		CreatePlayer2D();
	}
	
	void ConnectToServer(HostData data) {
		Debug.Log ("DATA"+data);
	}
	
	
	void OnDisconnectedFromServer() {
		//oldMainCamera.gameObject.SetActive(true);
	}
}
