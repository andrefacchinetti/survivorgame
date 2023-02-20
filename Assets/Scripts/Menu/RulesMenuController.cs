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
            titulos.Add("DESCRI��O");
            texto += "Este � um jogo de racioc�nio e estrat�gia do qual podem participar de tr�s a seis jogadores.\n";
            texto += "Nesse jogo, os jogadores ir�o disputar a sorte nos dados para atacar e defender territ�rios, quando mais territ�rios o jogador tiver, mais bonus de soldados por turno ele ter�.\n\n";
            texto += "O jogador que que atingir seu objetivo primeiro vencer�.\n";
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
            texto += "Durante a primeira rodada, os jogadores poder�o apenas distribuir seus soldados dispon�veis para fortificar os territ�rios desejados.\n";
            texto += "A partir da segunda rodada, os jogadores ter�o 3 fases durante seu turno, sendo elas: Distribui��o, Ataque e Remanejo.\n\n";
            texto += "Distribui��o: O jogador receber� a quantidade de soldados igual a metade da quantidade de territ�rios que ele possui.\n\n";
            texto += "Ataque: O jogador escolher� de qual territ�rio ele vai atacar e com qual armamento, depois ele escolher� um territ�rio inimigo que possua fronteira com o territ�rio escolhido para efetuar o ataque.\n\n";
            texto += "Remanejo: O jogador poder� transferir seus ex�rcitos 3 vezes por turno.";
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
            texto += "O jogador que conquistar pelo menos um territ�rio durante sua fase de ataque receber� uma carta no final do turno.\n\n";
            texto += "A carta do Tipo 'MULTIPLICADOR' serve para multiplicar todas as outras cartas selecionadas durante a troca.\n";
            texto += "Caso seja utilizado mais de um multiplicador em uma troca, os multiplicadores ser�o somados antes de multiplicar pelas outras cartas.\n";
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
            texto += "Os armamentos poder�o ser obtidos ap�s efetuar uma troca de cartas.\n\n";
            texto += "Tanque: Possui poder de ataque de 3 dados, quando utilizado contra soldados, os soldados usar�o apenas 2 dados, independente da quantidade de soldados existentes no territ�rio.\n";
            texto += "M�ssil: Possui chance de 3 dados contra 1 de defesa, independente de quantos soldados ou tanques estiverem no territ�rio. Caso seja vitorioso, o m�ssil devastar� todo o territ�rio inimigo, restando apenas um soldado no territ�rio.\n\n";
            texto += "Se um tanque enfrentar outro tanque, ser�o utilizados 3 dados de ataque contra 3 dados de defesa.\n";
            texto += "Os tanques sempre s�o prioridade para defender o territ�rio.\n";
            texto += "Quando um tanque enfrenta v�rios soldados e vence, o n�mero do maior dado jogado pelo jogador que possui o tanque ser� a quantidade de soldados inimigos eliminados. Como os tanques n�o conseguem conquistar, sempre sobrar� pelo menos um soldado territ�rio inimigo.";
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
            texto += "Conquistar Am�rica do Norte e �frica\n";
            texto += "Conquistar Am�rica do Norte e Am�rica do Sul\n";
            texto += "Conquistar Am�rica do Norte e Oceania\n";
            texto += "Conquistar Am�rica do Norte e Europa\n";
            texto += "Conquistar Am�rica do Norte e �sia\n";
            texto += "Conquistar Am�rica do Norte e Ant�rtida\n";
            texto += "Conquistar Europa e Am�rica do Sul\n";
            texto += "Conquistar Europa e �frica\n";
            texto += "Conquistar Europa e �sia\n";
            texto += "Conquistar Europa e Oceania\n";
            texto += "Conquistar Europa e Ant�rtida\n";
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
            texto += "Conquistar �sia e Am�rica do Sul\n";
            texto += "Conquistar �sia e �frica\n";
            texto += "Conquistar �sia e Oceania\n";
            texto += "Conquistar �sia e Ant�rtida\n";
            texto += "Conquistar Am�rica do Sul e �frica\n";
            texto += "Conquistar Am�rica do Sul e Oceania\n";
            texto += "Conquistar �frica e Oceania\n";
            texto += "Conquistar �frica e Ant�rtida\n";
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
