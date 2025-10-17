using UnityEngine;

public class SpawnPlayer : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab;
    GameObject player;
    private void Awake()
    {
        player = Instantiate(playerPrefab, transform.position, Quaternion.identity);
        player.GetComponent<PlayerData>().playerInfo = DataManager.LoadPlayer();
    }

    void Start()
    {
        Vector3 positionPlayer = new Vector3(
            player.GetComponent<PlayerData>().playerInfo.positionX,
            player.GetComponent<PlayerData>().playerInfo.positionY + 0.1f,
            player.GetComponent<PlayerData>().playerInfo.positionZ
        );

        Vector3 rotationPlayer = new Vector3(
            player.GetComponent<PlayerData>().playerInfo.rotationX,
            player.GetComponent<PlayerData>().playerInfo.rotationY,
            player.GetComponent<PlayerData>().playerInfo.rotationZ
        );

        if (PlayerPrefs.HasKey(DataManager.playerKey) && positionPlayer != transform.position)
        {
            player.transform.SetPositionAndRotation(positionPlayer, Quaternion.Euler(rotationPlayer));
        }
    }

}
