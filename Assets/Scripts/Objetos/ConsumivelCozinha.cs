using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumivelCozinha : MonoBehaviour
{

    [SerializeField] public Fogo fogo;
    [SerializeField] public SlotConsumivelPanela slotConsumivelPanela;
    [SerializeField] public ItemDrop itemDropConsumivel;

    public float temperatureLevel = 0.0f; // Nível de temperatura atual do objeto
    public float maxTemperatureLevel = 100.0f; // Nível máximo de temperatura permitido
    public ItemDrop consumivelCozido; // Objeto para o estado quente

    private void Update()
    {
        if (fogo == null) return;
        if (fogo.isFogoAceso)
        {
            // Atualiza a temperatura do objeto a cada frame
            temperatureLevel += Time.deltaTime;
        }
        if (temperatureLevel >= maxTemperatureLevel)
        {
            // Muda para o estado quente
            ChangeState(consumivelCozido);
        }
    }

    private void ChangeState(ItemDrop newState)
    {
        // Desativa o objeto atual
        gameObject.SetActive(false);
        // Ativa o novo objeto
        newState.gameObject.SetActive(true);
        slotConsumivelPanela.nomeItemNoSlot = newState.nomeItem;
    }

}
