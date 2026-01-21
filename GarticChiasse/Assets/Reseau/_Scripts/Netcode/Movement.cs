using Unity.Netcode;
using UnityEngine;

public class Movement : NetworkBehaviour
{

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        print(IsOwner);
    }

    private void Update()
    {
        if(IsOwner)
            transform.position += new Vector3(Input.GetAxis("Horizontal"), transform.position.y, Input.GetAxis("Vertical")) * Time.deltaTime;
    }
}
