using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.Audio;
using System.IO;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerMovement : MonoBehaviourPunCallbacks
{// lembrete: nome de usuarios iguais buga a mudan�a de cena

	[SerializeField] public GameObject cabecaPivot, pivotCameraInHead, pescocoPivot, pivotTiroBase;
	public Vector3 offset;
	public Camera playerCamera;

	private float horizontalMove, verticalMove;
	public float walkingSpeed = 2.0f;
	public float runningSpeed = 5.0f;
	public float consumoEnergiaPorSegundo = 5.0f;
	public float recuperacaoEnergiaPorSegundo = 2.0f;
	public float pesoGrab = 0.0f;
	public float jumpSpeed = 8.0f;
	public bool isPulando = false;
	public float gravity = 20.0f;
	public float sensivity = 2.0f;
	public float lookXLimit = 90f, lookYLimit = -40f;
	public float intensityFlashlight = 3.0f;
	public CharacterController characterController;
	Vector3 moveDirection = Vector3.zero;
	float rotationX = 0;
	[HideInInspector] public bool canMove = true;
	PhotonView PV;
	public Animator anim;
	PlayerManager playerManager;
	[HideInInspector]
	public float alturaPlayer;

	//cursor
	public Texture2D cursorTexture;
	public CursorMode cursorMode = CursorMode.Auto;
	public Vector2 hotSpot = Vector2.zero;

	public GameController gameController;
	public PlayerController playerController;



	void Awake()
	{
		DontDestroyOnLoad(this.gameObject);

		characterController = GetComponent<CharacterController>();
		PV = GetComponent<PhotonView>();
		anim = GetComponent<Animator>();
		//if (PV != null) playerManager = PhotonView.Find((int)PV.InstantiationData[0]).GetComponent<PlayerManager>(); //se pa tirar isso
		GameObject gc = GameObject.FindGameObjectWithTag("GameController");
		if (gc != null) gameController = gc.GetComponent<GameController>();
		playerController = GetComponent<PlayerController>();
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		alturaPlayer = characterController.height;
	}

	void Start()
	{
		if (PV == null) return;
		if (PV.IsMine)
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
			OnMouseEnter();
		}
		else
		{
			Destroy(GetComponentInChildren<Camera>().gameObject);
			Destroy(characterController);
		}
	}

	void Update()
	{
		if (PV == null) return;
		if (!PV.IsMine)
			return;

		characterController.Move(moveDirection * Time.deltaTime);
		Move();

		if (!characterController.isGrounded)
		{
			anim.SetBool("pulando", true);
			isPulando = true;
		}
		else
		{
			anim.SetBool("pulando", false);
			isPulando = false;
		}

		if (playerController.isMorto) canMove = false;
	}

	void OnMouseEnter()
	{
		Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
	}

	void OnMouseExit()
	{
		Cursor.SetCursor(null, Vector2.zero, cursorMode);
	}

	bool recarregandoEnergia = false;
	void Move()
	{
		// We are grounded, so recalculate move direction based on axes
		Vector3 forward = transform.TransformDirection(Vector3.forward);
		Vector3 right = transform.TransformDirection(Vector3.right);

		// Press Left Shift to run
		bool isRunning = Input.GetKey(KeyCode.LeftShift) && pesoGrab == 0 && !recarregandoEnergia;
        if (isRunning)
        {
			playerController.statsJogador.setarEnergiaAtual(playerController.statsJogador.energiaAtual - consumoEnergiaPorSegundo * Time.deltaTime);
		}
		else
		{
			playerController.statsJogador.setarEnergiaAtual(playerController.statsJogador.energiaAtual + recuperacaoEnergiaPorSegundo * Time.deltaTime);
			if (playerController.statsJogador.energiaAtual > 10) recarregandoEnergia = false;
		}
		if(playerController.statsJogador.energiaAtual <= 0 && !recarregandoEnergia)
        {
			recarregandoEnergia = true;
        }

		float velocidade = (isRunning ? runningSpeed : walkingSpeed);
		velocidade = velocidade - ((pesoGrab * velocidade *10) / 100);
		if (velocidade < 0.6f) velocidade = 0.6f;
		float curSpeedX = canMove ? velocidade * Input.GetAxis("Vertical") : 0;
		float curSpeedY = canMove ? velocidade * Input.GetAxis("Horizontal") : 0;
		float movementDirectionY = moveDirection.y;
		moveDirection = (forward * curSpeedX) + (right * curSpeedY);

		if (canMove)
		{
			horizontalMove = Input.GetAxisRaw("Horizontal");
			verticalMove = Input.GetAxisRaw("Vertical");
			anim.SetFloat("horizontalMove", horizontalMove);
			anim.SetFloat("verticalMove", verticalMove);
			anim.SetBool("correndo", isRunning);
			anim.SetBool("isPlayerParado", (horizontalMove == 0 && verticalMove == 0 && !isRunning));
		}
        else
        {
			anim.SetBool("isPlayerParado", true);
			anim.SetBool("correndo", false);
		}

		if ((Input.GetButton("Horizontal") || Input.GetButton("Vertical")) && canMove)
		{
			anim.SetBool("pegandoItemChao", false);
		}

		if (Input.GetKey(KeyCode.LeftControl) && canMove && characterController.isGrounded)
		{
			anim.SetBool("agachando", true);
			characterController.height = alturaPlayer/2;
			characterController.center = new Vector3(0,alturaPlayer/4,0);
			curSpeedX = 1.0f;
			curSpeedY = 1.0f;
		}
		else
		{
			anim.SetBool("agachando", false);
            characterController.height = alturaPlayer;
            characterController.center = new Vector3(0, alturaPlayer / 2, 0);
		}
		if (Input.GetButtonDown("Jump") && canMove && characterController.isGrounded)
		{
			moveDirection.y = jumpSpeed;
		}
		else
		{
			moveDirection.y = movementDirectionY;
		}

		// Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
		// when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
		// as an acceleration (ms^-2)
		if (!characterController.isGrounded)
		{
			moveDirection.y -= gravity * Time.deltaTime;
		}

		// Player and Camera rotation
		if (canMove)
		{
			rotationX += -Input.GetAxis("Mouse Y") * sensivity;
			rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);

			if (anim.GetCurrentAnimatorStateInfo(0).IsName("Walk") || anim.GetCurrentAnimatorStateInfo(0).IsName("Run") 
				|| anim.GetCurrentAnimatorStateInfo(0).IsName("BlendWalkArmadoMelee") || anim.GetCurrentAnimatorStateInfo(0).IsName("BlendWalkArmadoArco")
				|| anim.GetCurrentAnimatorStateInfo(0).IsName("BlendWalkArmadoCrossbow") || anim.GetCurrentAnimatorStateInfo(0).IsName("atkFerramentaFrente")
				|| anim.GetCurrentAnimatorStateInfo(0).IsName("atkFerramentaMarteloFrente") || anim.GetCurrentAnimatorStateInfo(0).IsName("atkArmaFrente"))
            {
				if (rotationX > lookYLimit)
				{
					pescocoPivot.transform.localRotation = Quaternion.Euler(0, 0, rotationX);
					cabecaPivot.transform.localRotation = Quaternion.Euler(0, 0, 0);
				}
				else
				{
					cabecaPivot.transform.localRotation = Quaternion.Euler(0, 0, rotationX);
					pescocoPivot.transform.localRotation = Quaternion.Euler(0, 0, lookYLimit);
				}
            }
            else
            {
				cabecaPivot.transform.localRotation = Quaternion.Euler(0, 0, 0);
				pescocoPivot.transform.localRotation = Quaternion.Euler(0, 0, 0);
			}

			transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * sensivity, 0);

			// Definir a posi��o da c�mera
			Vector3 desiredPosition = pivotCameraInHead.transform.position + offset;

			// Verificar se h� colis�es entre a posi��o atual da c�mera e o ponto desejado
			RaycastHit hit;
			if (Physics.Linecast(pivotCameraInHead.transform.position, desiredPosition, out hit))
			{
				// Se houver colis�o, ajustar a posi��o da c�mera
				playerCamera.transform.position = hit.point - offset.normalized * 0.2f;
			}
			else
			{
				// Se n�o houver colis�o, posicionar a c�mera normalmente
				playerCamera.transform.position = desiredPosition;
			}

			playerCamera.transform.localRotation = Quaternion.Euler(rotationX, playerCamera.transform.rotation.y, playerCamera.transform.rotation.z);
		}
	}

	void FixedUpdate() //testar update ao inves de fixed
	{
		if (PV == null) return;
		if (!PV.IsMine)
			return;

		if (PlayerPrefs.GetFloat("sensivity") <= 0) sensivity = 2.0f;
		else sensivity = PlayerPrefs.GetFloat("sensivity");

	}

}