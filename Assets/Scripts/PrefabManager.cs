using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabManager : MonoBehaviour
{
    public GameObject SpawnPrefab(GameObject prefabToSpawn)
    {
        GameObject prefabInstance = Instantiate(prefabToSpawn, null);
        prefabInstance.SetActive(true);

        return prefabInstance;
    }

    public GameObject badGuy;

    public GameObject enemyDamageText;
    public GameObject playerDamageText;
    public GameObject playerHealthText;

    public GameObject popupTextLarge;
    public GameObject popupText;
}
