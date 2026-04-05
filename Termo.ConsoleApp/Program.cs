using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Xml;

namespace Termo.ConsoleApp;

class Program
{
    static void Main(string[] args)
    {  
        while (true)        // Inicia o primeiro Looping pra retornar resposta depois
        {
            int jogadas = 0;
            string palavraSorteada = SorteioPalavra();                      // metodo para sortear a palavra, retorna palavra sorteada
            Console.WriteLine($"\nParalavra Sorteada: {palavraSorteada}");
            IniciarCabecalho();                                             // Inicia o cabeçalho e mostra as regras iniciais
            while(true)     // Inicia o jogo com a palavra escolhida, recebe a entrada do jogador e retorna
            {
                Console.Clear();
                Banner();
                jogadas++;
                Console.WriteLine($"Você tem ({6 - jogadas}) jogadas restantes");
                string palavraJogador = InicioJogo();                       // Retorna a entrada do jogador tratada
                ConfereLetraLetra(palavraSorteada, palavraJogador);         // Compara cada letra e printa resultado
                bool resultado = ConfereVitoria(palavraJogador, palavraSorteada); // Confere se a palavra é totalmente certa

                if(jogadas == 5)// Quebra o looping se o jogador atingir numero de tentativas
                {
                    Derrota(palavraSorteada);
                    break;
                }           
                else if (resultado == true)                                      // Verifica se a entrada é certa
                {
                    Vitoria(palavraSorteada);
                    break;
                }

                Console.WriteLine();
                Console.WriteLine("\nDigite ENTER para continuar");
                Console.Write(">");
                Console.ReadLine();
            }

            Console.WriteLine("Deseja Jogar novamente? (s/N)");
            string? ContinuarJogo = Console.ReadLine();
            if (ContinuarJogo.ToUpper() != "S") break;
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
    
            Console.WriteLine("Digite ENTER para iniciar");
            Console.ReadLine();
        }
        static void Banner()
        {
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
            Console.WriteLine();
        }
        static bool ConfereVitoria(string palavraJogador, string palavraSorteada)
        {
            if (palavraJogador == palavraSorteada)
            {
                return true;
            }
            return false;
        }
        static void Vitoria(string palavraSorteada)
        {
            Console.Clear();
            Banner();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(@$"
             _  _  __  ____  __  ____  __   __  
            / )( \(  )(_  _)/  \(  _ \(  ) / _\ 
            \ \/ / )(   )( (  O ))   / )( /    \
             \__/ (__) (__) \__/(__\_)(__)\_/\_/
            ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("A palavra '");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"{palavraSorteada}'");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(" esta CORRETA!");
            Console.ResetColor();
            Console.WriteLine();
        }
        static void Derrota(string palavraSorteada)
        {
            Console.Clear();
            Banner();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(@$"
             ____  ____  ____  ____   __  ____  __  
            (    \(  __)(  _ \(  _ \ /  \(_  _)/ _\ 
             ) D ( ) _)  )   / )   /(  O ) )( /    \
            (____/(____)(__\_)(__\_) \__/ (__)\_/\_/
            ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"Que pena a palavra era ");
            Console.ForegroundColor = ConsoleColor.DarkRed;Console.WriteLine($"'{palavraSorteada}'");
            Console.ResetColor();
            Console.WriteLine();
        }
    }
}

static class BancoPalavras
{
    public static string[] palavras = {"vento", "terra", "livro", "canil", "leite", "carro", 
        "porta", "chave", "peixe", "cobra", "porco", "cinto", "calca", "praia", "areia", "nuvem",
        "chuva", "vento", "calor", "verde", "preto", "amigo", "irmao", "filho", "arroz", "caixa"
    };
}