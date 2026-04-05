using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Xml;

namespace Termo.ConsoleApp;

class Program
{
    static void Main(string[] args)
    {   

        foreach (ConsoleColor cor in Enum.GetValues(typeof(ConsoleColor)))
        {
            Console.ForegroundColor = cor;
            Console.WriteLine($"Essa é a cor: {cor}");
        }
        Console.ReadLine();
        
        int jogadas = 0;
        string palavraSorteada = SorteioPalavra();                      // metodo para sortear a palavra, retorna palavra sorteada
        Console.WriteLine($"\nParalavra Sorteada: {palavraSorteada}");
        IniciarCabecalho();                                             // Inicia o cabeçalho e mostra as regras iniciais
        bool JogoAndamento = true;
        while(JogoAndamento)                // Inicia o jogo com a palavra escolhida, recebe a entrada do jogador e retorna
        {
            jogadas++;
            ConferirPontuacao(JogoAndamento, jogadas);                                        // Retorna pontuação vencida/para/continua
            string palavraJogador = InicioJogo();                       // Retorna a entrada do jogador tratada
            
            ConfereLetraLetra(palavraSorteada, palavraJogador);         // Compara cada letra e devolve resultado
            
            bool resultado = ConferePalavraCompleta(palavraJogador, palavraSorteada); // Confere se a palavra é totalmente certa
            if (resultado == true)                                      // Verifica se a entrada é certa
            {
                Derrota(palavraSorteada);
                break;
            }


            Console.WriteLine($"\nParalavra Sorteada: {palavraSorteada}");

            Banner();
            
        }

        Console.WriteLine("Digite ENTER para encerrar");
        Console.ReadLine();
    }

    static string SorteioPalavra()
    {
        int indice = RandomNumberGenerator.GetInt32(0, BancoPalavras.palavras.Length);
        return BancoPalavras.palavras[indice];
    }
    static void IniciarCabecalho()
    {
        Banner();
        Console.WriteLine("\nRegras: ");
        Console.WriteLine(" Você tem (5) tentativas");
        Console.WriteLine(" As palavras contem 5 digitos");

        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("  Verde...:");Console.ResetColor();
        Console.WriteLine(" letra correta na posição correta.");
        Console.ResetColor();

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("  Amarelo.:");Console.ResetColor();
        Console.WriteLine(" a letra existe, mas está na posição errada");

        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("  Vermelho:");Console.ResetColor();
        Console.WriteLine(" letra inexistente na palavra\n");
    }
    static void Banner()
    {
        //Console.Clear();
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write(@"████████╗ ");Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("███████╗");Console.ForegroundColor = ConsoleColor.White; Console.Write(" ██████╗  ");Console.ForegroundColor = ConsoleColor.Red;Console.Write("███╗   ███╗ "); Console.ResetColor(); Console.ForegroundColor = ConsoleColor.Green; Console.WriteLine(" ██████╗ ");
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write(@"╚══██╔══╝ ");Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("██╔════╝");Console.ForegroundColor = ConsoleColor.White; Console.Write(" ██╔══██╗ ");Console.ForegroundColor = ConsoleColor.Red;Console.Write("████╗ ████║ "); Console.ResetColor(); Console.ForegroundColor = ConsoleColor.Green; Console.WriteLine("██╔═══██╗");
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write(@"   ██║    ");Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("█████╗  ");Console.ForegroundColor = ConsoleColor.White; Console.Write(" ██████╔╝ ");Console.ForegroundColor = ConsoleColor.Red;Console.Write("██╔████╔██║ "); Console.ResetColor(); Console.ForegroundColor = ConsoleColor.Green; Console.WriteLine("██║   ██║");
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write(@"   ██║    ");Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("██╔══╝  ");Console.ForegroundColor = ConsoleColor.White; Console.Write(" ██╔══██╗ ");Console.ForegroundColor = ConsoleColor.Red;Console.Write("██║╚██╔╝██║ "); Console.ResetColor(); Console.ForegroundColor = ConsoleColor.Green; Console.WriteLine("██║   ██║");
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write(@"   ██║    ");Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("███████╗");Console.ForegroundColor = ConsoleColor.White; Console.Write(" ██║  ██║ ");Console.ForegroundColor = ConsoleColor.Red;Console.Write("██║ ╚═╝ ██║ "); Console.ResetColor(); Console.ForegroundColor = ConsoleColor.Green; Console.WriteLine("╚██████╔╝");
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write(@"   ╚═╝    ");Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("╚══════╝");Console.ForegroundColor = ConsoleColor.White; Console.Write(" ╚═╝  ╚═╝ ");Console.ForegroundColor = ConsoleColor.Red;Console.Write("╚═╝     ╚═╝ "); Console.ResetColor(); Console.ForegroundColor = ConsoleColor.Green; Console.WriteLine(" ╚═════╝ ");
        Console.ResetColor();
    }
    static string InicioJogo()
    {
        string? palavraJogador;

        while (true)
        {   
            Console.Write(">");
            palavraJogador = Console.ReadLine();
            
            if (string.IsNullOrWhiteSpace(palavraJogador))
            {
                Console.WriteLine("Digite algo válido!");
                continue;                
            }
            
            palavraJogador = palavraJogador.Trim().ToLower();
            palavraJogador = palavraJogador.Replace(" ", "");

            bool letraVerificada = VerificarLetraEntrada(palavraJogador);           // Retorna a verificação se é apenas letras

            if (!letraVerificada)
            {
                Console.WriteLine("Digite apenas letras");
                continue;
            }

            if (palavraJogador.Length != 5)
            {
                Console.WriteLine("Digite uma palavra com 5 letras");
                continue;
            }

            return palavraJogador;
        }
    }
    static bool VerificarLetraEntrada(string palavraJogador)
    {
        bool LetraVerificada = true;

        foreach (char letra in palavraJogador)
        {
            if (!char.IsLetter(letra))
            {
                LetraVerificada = false;
                break;
            }
        }
        return LetraVerificada;
    }
    static void ConfereLetraLetra(string palavraSorteada, string palavraJogador)
    {
        for (int i = 0; i < palavraSorteada.Length; i++)
        {
            if (palavraSorteada[i] == palavraJogador[i])
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write($"[{palavraJogador[i]}] ");
                Console.ResetColor();
            }
            else if (palavraSorteada.Contains(palavraJogador[i]))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write($"[{palavraJogador[i]}] ");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write($"[{palavraJogador[i]}] ");
                Console.ResetColor();
            }
        }
    }
    static bool ConferePalavraCompleta(string palavraJogador, string palavraSorteada)
    {
        if (palavraJogador == palavraSorteada)
        {
            return true;
        }
        return false;
    }
    static bool ConferirPontuacao(bool JogoAndamento, int jogadas)
    {
        if(jogadas == 5)
        {
            JogoAndamento = false;
        }
        return false;
    }
    static void Vitoria(string palavraSorteada)
    {
        Console.Clear();
        Banner();
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"A palavra '{palavraSorteada}' esta CORRETA!");
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine(@$"
         _  _  __  ____  __  ____  __   __  
        / )( \(  )(_  _)/  \(  _ \(  ) / _\ 
        \ \/ / )(   )( (  O ))   / )( /    \
         \__/ (__) (__) \__/(__\_)(__)\_/\_/
        ");

        Console.ResetColor();
    }

    static void Derrota(string palavraSorteada)
    {
        Console.Clear();
        Banner();
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write($"Que pena a palavra era ");
        Console.ForegroundColor = ConsoleColor.DarkRed;Console.WriteLine($"'{palavraSorteada}'");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(@$"
         ____  ____  ____  ____   __  ____  __  
        (    \(  __)(  _ \(  _ \ /  \(_  _)/ _\ 
         ) D ( ) _)  )   / )   /(  O ) )( /    \
        (____/(____)(__\_)(__\_) \__/ (__)\_/\_/
        ");
        Console.ResetColor();
    }
    
}

static class BancoPalavras
{
    public static string[] palavras = {"vento", "terra", "livro", "canil", "leite", "carro", 
        "porta", "chave", "peixe", "cobra", "porco", "cinto", "calca", "praia", "areia", "nuvem",
        "chuva", "vento", "calor", "verde", "preto", "amigo", "irmao", "filho", "arroz", "caixa"
    };
}