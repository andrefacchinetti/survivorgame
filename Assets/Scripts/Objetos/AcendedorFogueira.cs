using UnityEngine;

public class AcendedorFogueira : MonoBehaviour
{

	[SerializeField] public GameObject fogo;

    private void Start()
    {
		ApagarFogo();
    }

    public void AcenderFogo()
	{
		fogo.SetActive(true);
	}

	public void ApagarFogo()
	{
		fogo.SetActive(false);
	}
}
