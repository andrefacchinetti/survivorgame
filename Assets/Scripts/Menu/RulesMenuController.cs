using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RulesMenuController : MonoBehaviour
{

    [SerializeField] TMP_Text txDescricaoPgAtual, txPaginacao, txTitulo;
    private List<string> paginas, titulos;
    private int pg;


    private void Awake()
    {
        CarregarPaginas();
    }

    public void CarregarPaginas()
    {
        titulos = new List<string>();
        paginas = new List<string>();
        paginas.Add(primeiraPagina());
        paginas.Add(segundaPagina());
        paginas.Add(terceiraPagina());
        paginas.Add(quartaPagina());
        paginas.Add(quintaPagina());
        paginas.Add(sextaPagina());

        pg = 1;
        atualizarTextosPagina();
    }

    public void AvancarPagina()
    {
        if (pg == paginas.Count) return;
        pg++;
        atualizarTextosPagina();
    }

    public void VoltarPagina()
    {
        if (pg == 1) return;
        pg--;
        atualizarTextosPagina();
    }
    
    private void atualizarTextosPagina()
    {
        txDescricaoPgAtual.text = paginas[pg - 1];
        txPaginacao.text = pg + "/" + paginas.Count;
        txTitulo.text = titulos[pg - 1];
    }

    //----------------------------------------------------------------

    private string primeiraPagina()
    {
        string texto = "";
        if(PlayerPrefs.GetInt("INDEXIDIOMA") == 1)
        {
            titulos.Add("DESCRIÇÃO");
            texto += "Este é um jogo de raciocínio e estratégia do qual podem participar de três a seis jogadores.\n";
            texto += "Nesse jogo, os jogadores irão disputar a sorte nos dados para atacar e defender territórios, quando mais territórios o jogador tiver, mais bonus de soldados por turno ele terá.\n\n";
            texto += "O jogador que que atingir seu objetivo primeiro vencerá.\n";
        }
        else
        {
            titulos.Add("DESCRIPTION");
            texto += "This is a game of reasoning and strategy in which they can participate in three to six players.\n";
            texto += "In this game, the players will compete for luck in the data to attack and defend territories, when more territories the player has, more bonuses of soldiers by shift he will have.\n\n";
            texto += "The player who reaches his goal first will win.\n";
        }
        return texto;
    }

    private string segundaPagina()
    {
        string texto = "";
        if (PlayerPrefs.GetInt("INDEXIDIOMA") == 1)
        {
            titulos.Add("FASES POR TURNO");
            texto += "Durante a primeira rodada, os jogadores poderão apenas distribuir seus soldados disponíveis para fortificar os territórios desejados.\n";
            texto += "A partir da segunda rodada, os jogadores terão 3 fases durante seu turno, sendo elas: Distribuição, Ataque e Remanejo.\n\n";
            texto += "Distribuição: O jogador receberá a quantidade de soldados igual a metade da quantidade de territórios que ele possui.\n\n";
            texto += "Ataque: O jogador escolherá de qual território ele vai atacar e com qual armamento, depois ele escolherá um território inimigo que possua fronteira com o território escolhido para efetuar o ataque.\n\n";
            texto += "Remanejo: O jogador poderá transferir seus exércitos 3 vezes por turno.";
        }
        else
        {
            titulos.Add("PHASES PER SHIFT");
            texto += "During the first round, players will only be able to distribute their available soldiers to fortify the desired territories.\n";
            texto += "From the second round, players will have 3 phases during their turn, namely: Distribution, Attack and Remanagement.\n\n";
            texto += "Distribution: The player will receive the amount of soldiers equal to half the amount of territories he has.\n\n";
            texto += "Attack: The player will choose which territory he will attack from and with which weaponry, then he will choose an enemy territory that has a border with the chosen territory to carry out the attack.\n\n";
            texto += "Relocation: The player can transfer his armies 3 times per turn.";
        }
        return texto;
    }

    private string terceiraPagina()
    {
        string texto = "";
        if (PlayerPrefs.GetInt("INDEXIDIOMA") == 1)
        {
            titulos.Add("CARTAS");
            texto += "O jogador que conquistar pelo menos um território durante sua fase de ataque receberá uma carta no final do turno.\n\n";
            texto += "A carta do Tipo 'MULTIPLICADOR' serve para multiplicar todas as outras cartas selecionadas durante a troca.\n";
            texto += "Caso seja utilizado mais de um multiplicador em uma troca, os multiplicadores serão somados antes de multiplicar pelas outras cartas.\n";
        }
        else
        {
            titulos.Add("CARDS");
            texto += "The player who conquers at least one territory during his attack phase will receive a card at the end of the turn.\n";
            texto += "The 'MULTIPLIER' Type card serves to multiply all other cards selected during the exchange.\n";
            texto += "If more than one multiplier is used in a trade, the multipliers will be added before multiplying by the other cards.\n";

        }
        return texto;
    }

    private string quartaPagina()
    {
        string texto = "";
        if (PlayerPrefs.GetInt("INDEXIDIOMA") == 1)
        {
            titulos.Add("ARMAMENTOS");
            texto += "Os armamentos poderão ser obtidos após efetuar uma troca de cartas.\n\n";
            texto += "Tanque: Possui poder de ataque de 3 dados, quando utilizado contra soldados, os soldados usarão apenas 2 dados, independente da quantidade de soldados existentes no território.\n";
            texto += "Míssil: Possui chance de 3 dados contra 1 de defesa, independente de quantos soldados ou tanques estiverem no território. Caso seja vitorioso, o míssil devastará todo o território inimigo, restando apenas um soldado no território.\n\n";
            texto += "Se um tanque enfrentar outro tanque, serão utilizados 3 dados de ataque contra 3 dados de defesa.\n";
            texto += "Os tanques sempre são prioridade para defender o território.\n";
            texto += "Quando um tanque enfrenta vários soldados e vence, o número do maior dado jogado pelo jogador que possui o tanque será a quantidade de soldados inimigos eliminados. Como os tanques não conseguem conquistar, sempre sobrará pelo menos um soldado território inimigo.";
        }
        else
        {
            titulos.Add("WEAPONS");
            texto += "Weapons can be obtained after exchanging cards.\n\n";
            texto += "Tank: It has an attack power of 3 dice, when used against soldiers, soldiers will only use 2 dice, regardless of the number of soldiers in the territory.\n";
            texto += "Missile: It has a chance of 3 dice against 1 of defense, regardless of how many soldiers or tanks are in the territory. If it is victorious, the missile will devastate the entire enemy territory, leaving only one soldier in the territory.\n\n";
            texto += "If a tank faces another tank, 3 attack dice will be used against 3 defense dice.\n";
            texto += "Tanks are always a priority to defend the territory.\n";
            texto += "When a tank faces several soldiers and wins, the number of the highest dice thrown by the player who owns the tank will be the number of enemy soldiers eliminated. As tanks cannot conquer, there will always be at least one soldier left over enemy territory.";

        }
        return texto;
    }

    private string quintaPagina()
    {
        string texto = "";
        if (PlayerPrefs.GetInt("INDEXIDIOMA") == 1)
        {
            titulos.Add("OBJETIVOS");
            texto += "Conquistar América do Norte e África\n";
            texto += "Conquistar América do Norte e América do Sul\n";
            texto += "Conquistar América do Norte e Oceania\n";
            texto += "Conquistar América do Norte e Europa\n";
            texto += "Conquistar América do Norte e Ásia\n";
            texto += "Conquistar América do Norte e Antártida\n";
            texto += "Conquistar Europa e América do Sul\n";
            texto += "Conquistar Europa e África\n";
            texto += "Conquistar Europa e Ásia\n";
            texto += "Conquistar Europa e Oceania\n";
            texto += "Conquistar Europa e Antártida\n";
        }
        else
        {
            titulos.Add("OBJECTIVES");
            texto += "Conquer North America and Africa\n";
            texto += "Conquer North and South America\n";
            texto += "Conquer North America and Oceania\n";
            texto += "Conquer North America and Europe\n";
            texto += "Conquer North America and Asia\n";
            texto += "Conquer North America and Antarctica\n";
            texto += "Conquer Europe and South America\n";
            texto += "Conquer Europe and Africa\n";
            texto += "Conquer Europe and Asia\n";
            texto += "Conquer Europe and Oceania\n";
            texto += "Conquer Europe and Antarctica\n";
        }
        return texto;
    }

    private string sextaPagina()
    {
        string texto = "";
        if (PlayerPrefs.GetInt("INDEXIDIOMA") == 1)
        {
            titulos.Add("OBJETIVOS");
            texto += "Conquistar Ásia e América do Sul\n";
            texto += "Conquistar Ásia e África\n";
            texto += "Conquistar Ásia e Oceania\n";
            texto += "Conquistar Ásia e Antártida\n";
            texto += "Conquistar América do Sul e África\n";
            texto += "Conquistar América do Sul e Oceania\n";
            texto += "Conquistar África e Oceania\n";
            texto += "Conquistar África e Antártida\n";
        }
        else
        {
            titulos.Add("OBJECTIVES");
            texto += "Conquer Asia and South America\n";
            texto += "Conquer Asia and Africa\n";
            texto += "Conquer Asia and Oceania\n";
            texto += "Conquer Asia and Antarctica\n";
            texto += "Conquer South America and Africa\n";
            texto += "Conquer South America and Oceania\n";
            texto += "Conquer Africa and Oceania\n";
            texto += "Conquer Africa and Antarctica\n";
        }
        return texto;
    }

}
