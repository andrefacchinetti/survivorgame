using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EstilhacoFxController : MonoBehaviour
{

	public GameObject ParticleMadeira, ParticlePedra;
	public GameObject bulletholes;
	private List<GameObject> onScreenParticles = new List<GameObject>();

	public Vector3 direction;

	public enum TipoEstilhaco
    {
		Nenhum,
		Sangue,
        Madeira,
        Pedra
    }

    private void Awake()
    {
		StartCoroutine("CheckForDeletedParticles");
	}

	IEnumerator CheckForDeletedParticles()
	{
		while (true)
		{
			yield return new WaitForSeconds(5.0f);
			for (int i = onScreenParticles.Count - 1; i >= 0; i--)
			{
				if (onScreenParticles[i] == null)
				{
					onScreenParticles.RemoveAt(i);
				}
			}
		}
	}

	public void GerarEstilhaco(TipoEstilhaco tipoEstilhaco, Collider colliderAlvo, Vector3 colliderArma)
    {
		Debug.Log("Estilhaço alvo" + colliderAlvo.gameObject.tag);
		GameObject particle = spawnParticle(tipoEstilhaco, colliderAlvo, colliderArma);
    }

	private GameObject obterParticlePorTipo(TipoEstilhaco tipoEstilhaco)
    {
		if (tipoEstilhaco.Equals(TipoEstilhaco.Madeira)) return ParticleMadeira;
		else if (tipoEstilhaco.Equals(TipoEstilhaco.Pedra)) return ParticlePedra;
		else return null;
	}

	public GameObject spawnParticle(TipoEstilhaco tipoEstilhaco, Collider colliderAlvo, Vector3 colliderArma)
	{
		float angle = 0;
		Vector3 spawnPosition = colliderArma;
		Vector3 estilhacoDirection = (colliderArma - spawnPosition).normalized;

		GameObject particleEx = obterParticlePorTipo(tipoEstilhaco);
		GameObject particles = (GameObject)Instantiate(particleEx);
		if (particles == null) return null;
		particles.transform.position = spawnPosition;
		particles.transform.rotation = Quaternion.Euler(0, angle + 90, 0);
		particles.transform.LookAt(spawnPosition + estilhacoDirection, direction);

#if UNITY_3_5
			particles.SetActiveRecursively(true);
#else
		particles.SetActive(true);
#endif

		if (particles.name.StartsWith("WFX_MF"))
		{
			particles.transform.parent = particleEx.transform.parent;
			particles.transform.localPosition = particleEx.transform.localPosition;
			particles.transform.localRotation = particleEx.transform.localRotation;
		}
		else if (particles.name.Contains("Hole"))
		{
			particles.transform.parent = bulletholes.transform;
		}

		ParticleSystem ps = particles.GetComponent<ParticleSystem>();
#if UNITY_5_5_OR_NEWER
		if (ps != null)
		{
			var main = ps.main;
			if (main.loop)
			{
				ps.gameObject.AddComponent<CFX_AutoStopLoopedEffect>();
				ps.gameObject.AddComponent<CFX_AutoDestructShuriken>();
			}
		}
#else
		if(ps != null && ps.loop)
		{
			ps.gameObject.AddComponent<CFX_AutoStopLoopedEffect>();
			ps.gameObject.AddComponent<CFX_AutoDestructShuriken>();
		}
#endif

		onScreenParticles.Add(particles);

		return particles;
	}

}
