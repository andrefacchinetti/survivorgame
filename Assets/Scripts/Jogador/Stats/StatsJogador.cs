using Opsive.UltimateCharacterController.Traits;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Opsive.Shared.Events;

public class StatsJogador : MonoBehaviour
{

    [SerializeField] [HideInInspector] public PlayerController playerController;
    [SerializeField] [HideInInspector] public CharacterAttributeManager characterAttributeManager;
    [SerializeField] [HideInInspector] public StatsGeral statsGeral;
    [SerializeField] [HideInInspector] public OverlayController overlayController;
    [SerializeField] HudJogador hudJogador;

    // STATS MAXIMO
    [SerializeField] public float fomeMaxima = 100, sedeMaxima = 100, energiaMaxima = 100;

    //STATS CURRENT
    [SerializeField] public float fomeAtual, sedeAtual, energiaAtual;

    [SerializeField] public float tempoPraDiminuirStatsFomeSedeEmSegundos = 60*1;
    [SerializeField] public float tempoPraDiminuirStatsFeridasInternasEmSegundos = 60*2;
    [SerializeField] public float tempoPraDiminuirStatsDoencasEmSegundos = 60*4;
    [SerializeField] public float tempoPraDiminuirStatsDanoRapidoPorSegundo = 1; //ex: sangramento perde dano a cada x segundos
    [SerializeField] public float valorDaFomeReduzidaPorTempo = 5, valorDaSedeReduzidaPorTempo = 10;
    public float consumoEnergiaPorSegundo = 5.0f;
    public float recuperacaoEnergiaPorSegundo = 2.0f;

    //Feridas internas
    public bool isFraturado = false, isAbstinencia = false, isSangrando = false;
    public bool isIndigestao = false, isInfeccionado = false;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        statsGeral = GetComponent<StatsGeral>();
        characterAttributeManager = GetComponent<CharacterAttributeManager>();
        overlayController = GetComponent<OverlayController>();
    }

    private void Start()
    {
        AtualizarImgVida();
        AtualizarImgArmor();
        setarFomeAtual(fomeMaxima);
        setarSedeAtual(sedeMaxima);
        setarEnergiaAtual(energiaMaxima);
        ResetarStatsFeridasInternas();
        InvokeRepeating("DiminuirStatsPorTempo", 0, tempoPraDiminuirStatsFomeSedeEmSegundos);
        InvokeRepeating("VerificarStatsFeridasInternas", 0, tempoPraDiminuirStatsFeridasInternasEmSegundos);
        InvokeRepeating("VerificarStatsDoencas", 0, tempoPraDiminuirStatsDoencasEmSegundos);
        InvokeRepeating("VerificarStatsDanoRapido", 0, tempoPraDiminuirStatsDanoRapidoPorSegundo);
    }

    public void ResetarStatsFeridasInternas()
    {
        isSangrando = false;
        isFraturado = false;
        isAbstinencia = false;
        isIndigestao = false;
        isInfeccionado = false;
        AtualizarImgSangrando();
        AtualizarImgAbstinencia();
        AtualizarImgFraturado();
        AtualizarImgIndigestao();
        AtualizarImgInfeccionado();
    }

    void DiminuirStatsPorTempo()
    {
        setarFomeAtual(fomeAtual - valorDaFomeReduzidaPorTempo);
        setarSedeAtual(sedeAtual - valorDaSedeReduzidaPorTempo);
        if(sedeAtual <= 0 || fomeAtual <= 0)
        {
            TakeDamageHealth(10, false, false);
        }
    }

    public void TakeDamageHealth(float value, bool isPodeCausarSangramento, bool armorPodeTankar)
    {
        float danoRecebido = armorPodeTankar ? value - ((value * playerController.characterAttributeManager.GetAttribute("Armor").Value) / 100) : value;
        if (isPodeCausarSangramento)
        {
            if (!isSangrando)
            {
                int randomSangramento = Random.Range(0, 100);
                if (randomSangramento < 30)
                {
                    isSangrando = true;
                }
                AtualizarImgSangrando();
            }
            if (isSangrando && !isInfeccionado)
            {
                int randomInfeccao = Random.Range(0, 100);
                isInfeccionado = randomInfeccao < 30;
                AtualizarImgInfeccionado();
            }
        }
        
        playerController.characterHealth.Damage(danoRecebido);
        AtualizarImgVida();
    }

    public void TakeHealHealth(float value)
    {
        playerController.characterHealth.Heal(value);
        AtualizarImgVida();
    }

    public void AumentarArmorJogador(float value)
    {
        playerController.characterAttributeManager.GetAttribute("Armor").Value += value;
        hudJogador.atualizarImgArmor(playerController.characterAttributeManager.GetAttribute("Armor").Value);
    }

    public void DiminuirArmorJogador(float value)
    {
        playerController.characterAttributeManager.GetAttribute("Armor").Value -= value;
        hudJogador.atualizarImgArmor(playerController.characterAttributeManager.GetAttribute("Armor").Value);
    }

    private void AtualizarImgArmor()
    {
        hudJogador.atualizarImgArmor(playerController.characterAttributeManager.GetAttribute("Armor").Value);
    }

    public void AtualizarImgVida()
    {
        float vidaAtual = playerController.characterHealth.HealthValue;
        float vidaMaxima = ObterVidaMaximaHealth();
        hudJogador.atualizarImgVida(vidaAtual, vidaMaxima);
        overlayController.TakeDamageOverlay(vidaAtual, vidaMaxima);
    }

    public void AtualizarImgAbstinencia()
    {
        hudJogador.atualizarImgAbstinencia(isAbstinencia);
        overlayController.AtualizarAbstinenciaOverlay(isAbstinencia);
    }

    public void AtualizarImgSangrando()
    {
        hudJogador.atualizarImgSangrando(isSangrando); //Se pa que nem precisa desse indicador na hud, pois ja vai ter na overlay
        overlayController.AtualizarBloodOverlay(isSangrando);
    }

    public void AtualizarImgFraturado()
    {
        hudJogador.atualizarImgFraturado(isFraturado);
    }

    public void AtualizarImgIndigestao()
    {
        hudJogador.atualizarImgIndigestao(isIndigestao);
    }

    public void AtualizarImgInfeccionado()
    {
        hudJogador.atualizarImgInfeccionado(isInfeccionado);
    }

    public float ObterVidaMaximaHealth()
    {
        return playerController.characterAttributeManager.GetAttribute("Health").MaxValue;
    }

    public void setarFomeAtual(float valor)
    {
        if (valor > fomeMaxima) valor = fomeMaxima;
        if (valor < 0) valor = 0;
        fomeAtual = valor;
        hudJogador.atualizarImgFome(fomeAtual, fomeMaxima);
    }

    public void setarSedeAtual(float valor)
    {
        if (valor > sedeMaxima) valor = sedeMaxima;
        if (valor < 0) valor = 0;
        sedeAtual = valor;
        hudJogador.atualizarImgSede(sedeAtual, sedeMaxima);
    }

    public void setarEnergiaAtual(float valor)
    {
        if (valor > energiaMaxima) valor = energiaMaxima;
        if (valor < 0) valor = 0;
        energiaAtual = valor;
        hudJogador.atualizarImgEnergia(energiaAtual, energiaMaxima);
    }

    public void AcoesMorreu()
    {
        //TODO: DROPAR MOCHILA
        playerController.canMove = false;
    }

    public void AcoesReviveu()
    {
        playerController.StartarAbility(playerController.reviveAbility);
        playerController.canMove = true;
        playerController.corpoDissecando = null;
        playerController.animalCapturado = null;
        TakeHealHealth(playerController.characterAttributeManager.GetAttribute(playerController.characterHealth.HealthAttributeName).MaxValue * 0.20f); //CURA 20% DA VIDA MAXIMA
        setarFomeAtual(fomeMaxima*0.25f);
        setarSedeAtual(sedeMaxima * 0.25f);
        setarEnergiaAtual(energiaMaxima);
        ResetarStatsFeridasInternas();
    }

    // INICIO VERIFICAR STATS FERIDAS E DOENCAS
    void VerificarStatsFeridasInternas()
    {
        verificarAbstinencia();
        if (isAbstinencia)
        {
            TakeDamageHealth(10, false, false);
        }
        if (isFraturado)
        {
            TakeDamageHealth(10, false, false);
        }
    }

    void VerificarStatsDoencas()
    {
        if (isIndigestao)
        {
            vomitar();
        }
        if (isInfeccionado)
        {
            TakeDamageHealth(20, false, false);
        }
    }

    void VerificarStatsDanoRapido()
    {
        if (isSangrando)
        {
            TakeDamageHealth(2, false, false);
        }
    }

    int countAbstinencia = 0;
    private void verificarAbstinencia()
    {
        if (isAbstinencia) return; //Ja esta com abstinencia
        if (fomeAtual <= fomeMaxima * 0.15f || sedeAtual <= sedeMaxima * 0.15f)
        {
            countAbstinencia++;
        }
        if (countAbstinencia >= 3) //Causa abstinencia
        {
            isAbstinencia = true;
            countAbstinencia = 0;
            AtualizarImgAbstinencia();
        }
    }
    
    int countVomitos = 0;
    private void vomitar()
    {
        TakeDamageHealth(10, false, false);
        setarFomeAtual(fomeAtual - 40);
        setarSedeAtual(sedeAtual - 40);
        countVomitos++;
        if (countVomitos >= 3)
        {
            isIndigestao = false;
        }
        Debug.Log("Jogador vomitou"); //TODO: Som de vomito e animacao (animacao nao pode bugar habilidades em uso)
    }

    // FIM VERIFICAR STATS FERIDAS E DOENCAS
    //INICIO FERIDAS E DOENÇAS
    public void FraturarJogador()
    {
        playerController.speedChangeAbility.MaxSpeedChangeValue = playerController.maxSpeedChangeValue * 0.3f;
        playerController.jumpAbility.Force = 0.05f;
        isFraturado = true;
        AtualizarImgFraturado();
    }

    public void CurarFraturaJogador()
    {
        playerController.speedChangeAbility.MaxSpeedChangeValue = playerController.maxSpeedChangeValue;
        playerController.jumpAbility.Force = playerController.jumpForceValue;
        isFraturado = false;
        AtualizarImgFraturado();
    }

    public void AplicarIndigestao()
    {
        isIndigestao = true;
        AtualizarImgIndigestao();
    }

    public void CurarIndigestao()
    {
        isIndigestao = false;
        AtualizarImgIndigestao();
    }

    public void CurarInfeccao()
    {
        isInfeccionado = false;
        AtualizarImgInfeccionado();
    }

    public void CurarFratura()
    {
        isFraturado = false;
        AtualizarImgFraturado();
    }

    public void CurarSangramento()
    {
        isSangrando = false;
        AtualizarImgSangrando();
    }
    //FIM FERIDAS E DOENÇAS
}
