using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Program
    {
        static void Main(string[] args)
        {
            Codec codec = new Codec();
            string pastaDoArquivo = @"C:\Users\tefferson.guterres\Desktop";
            string nomeDoArquivo = @"\teste2.txt";
            string extensaoDoArquivoCodificado = ".teff";
            string destinoDoArquivoCodificado = pastaDoArquivo + nomeDoArquivo + extensaoDoArquivoCodificado;

            byte[] arquivoCodificado = codec.CodificarTexto(pastaDoArquivo, nomeDoArquivo);
            Console.WriteLine(
                codec.ByteArrayToFile(destinoDoArquivoCodificado, arquivoCodificado)
            );

            byte[] arquivoCodificadoLido = File.ReadAllBytes(destinoDoArquivoCodificado);
            string arquivoLidoDecodificado = codec.Decodificar(arquivoCodificadoLido);
            File.WriteAllText(pastaDoArquivo + nomeDoArquivo, arquivoLidoDecodificado);

            Console.WriteLine("fim");

            Console.Read();
        }
    }
}
