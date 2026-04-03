using System.Security.Cryptography;

namespace Termo.ConsoleApp;

class Program
{
    static void Main(string[] args)
    {   
        string palavra = SorteioPalavra();                      // metodo para sortear a palavra, retorna palavra sorteada
        IniciarCabecalho();                                     // Inicia o cabeçalho e mostra as regras iniciais
        while(true)                                             // Inicia o jogo com a palavra escolhida, recebe a entrada do jogador e retorna
        {
            string palavraJogador = InicioJogo();               // Retorna a entrada do jogador ja tratada
            
            //ConfereLetraLetra();

            foreach(char caracter in palavraJogador)
            {
                Console.Write($"[{caracter}] ");
            }

            Console.WriteLine($"\n{palavra}");
        
        
            break;
        }
        
    }
    static string SorteioPalavra()
    {
        int indice = RandomNumberGenerator.GetInt32(0, BancoPalavras.palavras.Length);
        return BancoPalavras.palavras[indice];
    }

    static void IniciarCabecalho()
    {
        Console.WriteLine("- = - = - = - = -");
        Console.WriteLine("      Termo      ");
        Console.WriteLine("- = - = - = - = -");
        Console.WriteLine("\nRegras: ");
        Console.WriteLine("  Você tem (5) tentativas");
        Console.WriteLine("  As palavras contem 5 digitos");

        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("     Verde...:");Console.ResetColor();
        Console.WriteLine(" letra correta na posição correta.");
        Console.ResetColor();

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("     Amarelo.:");Console.ResetColor();
        Console.WriteLine(" a letra existe, mas está na posição errada");

        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("     Vermelho:");Console.ResetColor();
        Console.WriteLine(" letra inexistente na palavra");
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

            bool letraVerificada = VerificarLetraEntrada(palavraJogador);

            if (!letraVerificada)
            {
                Console.WriteLine("Digite apenas letras");continue;
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
    
    static void ConfereLetraLetra(string palavraJogador)
    {
        
    }
}

static class BancoPalavras
{
    public static string[] palavras = {"vento", "terra", "livro", "canil", "leite", "carro", 
        "porta", "chave", "peixe", "cobra", "porco", "cinto", "calca", "praia", "areia", "nuvem",
        "chuva", "vento", "calor", "verde", "preto", "amigo", "irmao", "filho", "arroz", "caixa"
    };
}