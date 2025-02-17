using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using TMPro;
using FirstDayIn.Network;

public class PlayerObject : NetworkBehaviour {

    public static PlayerObject Local { get; private set; }

    [field: Header("References"), SerializeField] public PlayerMovement Controller { get; private set; }

    [Networked(OnChanged = nameof(UpdatePlayerName))] public NetworkString<_32> PlayerName { get; set; }
    [SerializeField] public TextMeshPro playerLabel;

    [Networked] public PlayerRef Ref { get; set; }
	[Networked] public byte Index { get; set; }

    public void Server_Init(PlayerRef pRef, byte index)
	{
        // Debug.Assert(Runner.IsServer);
        Debug.Log($"PlayerObject Server_Init() Index {index}");

		Ref = pRef;
		Index = index;
	}

    private void Start() {
        Debug.Log("PlayerObject Start()");

        PlayerRegistry.Server_Add(Runner, Object.InputAuthority, this);
        if (Object.HasStateAuthority) {
            PlayerName = GameManager.instance._playerName;
        }

		if (Object.HasInputAuthority)
		{
			Local = this;
		}
	}

    protected static void UpdatePlayerName(Changed<PlayerObject> changed) {
        changed.Behaviour.playerLabel.text = changed.Behaviour.PlayerName.ToString();
    }
}