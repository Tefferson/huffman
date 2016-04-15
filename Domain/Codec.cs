using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Codec
    {
        public const char Separador = (char)0;
        private string txt;

        public Codec() { }

        public Codec(string txt) {
            this.txt = txt;
        }

        public bool ByteArrayToFile(string fileName, byte[] byteArray)
        {
            try
            {
                FileStream fileStream =
                   new FileStream(fileName, FileMode.Create, FileAccess.Write);
                fileStream.Write(byteArray, 0, byteArray.Length);
                fileStream.Close();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception caught in process: {0}",
                                  ex.ToString());
            }
            return false;
        }

        public IDictionary<byte, int> MapearSimbolos(byte[] bytesLidos)
        {
            IDictionary<byte, int> simbolosMapeados = new Dictionary<byte, int>();
            foreach (byte b in bytesLidos)
            {
                IncrementarOcorrencias(simbolosMapeados, b);
            }
            return simbolosMapeados;
        }

        private void IncrementarOcorrencias(IDictionary<byte, int> map, byte c)
        {
            if (map.ContainsKey(c))
            {
                map[c]++;
            }
            else
            {
                map.Add(c, 1);
            }
        }

        public IEnumerable<Simbolo> CriarSimbolos(IDictionary<byte, int> simbolosEncontrados)
        {
            foreach (KeyValuePair<byte, int> chaveValor in simbolosEncontrados)
            {
                yield return new Simbolo((char)chaveValor.Key, chaveValor.Value);
            }
        }

        public byte[] CodificarTexto(string caminho, string nomeDoArquivo)
        {
            byte[] bytesLidos = File.ReadAllBytes(caminho + nomeDoArquivo);

            return CodificarTexto(bytesLidos);
        }

        public byte[] CodificarTexto()
        {
            if(txt == null) return new byte[' '];

            byte[] bytesDoTexto = txt.ToByteArray();

            return CodificarTexto(bytesDoTexto);
        }

        public byte[] CodificarTexto(byte[] bytesParaProcessar)
        {
            Simbolo arvore = MontarArvore(bytesParaProcessar);

            return CodificarBytesLidos(bytesParaProcessar, arvore).ToByteArray();
        }

        private Simbolo MontarArvore(byte[] bytesLidos)
        {
            IDictionary<byte, int> simbolosEncontrados = MapearSimbolos(bytesLidos);
            IEnumerable<Simbolo> simbolosTerminais = CriarSimbolos(simbolosEncontrados);
            return Simbolo.MontarArvore(simbolosTerminais);
        }

        private string GetRepresentationsAndCodesAsString(IDictionary<string, string> represetationsAndCodes, byte[] respresentations)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte respresentation in respresentations)
            {
                sb.Append(represetationsAndCodes[((char)respresentation).ToString()]);
            }
            return sb.ToString();
        }

        public BitArray CodificarBytesLidos(byte[] bytesLidos, Simbolo arvore)
        {
            string textoCodificado = GetRepresentationsAndCodesAsString(arvore.GetSymbolsCodes(), bytesLidos);
            string bitsNaoUtilizadosNoFinal = Convert.ToString(textoCodificado.Length % 8, 2).PadLeft(4, '0');
            string encaminhamentos = arvore.BuscarEncaminhamentos();
            string tamanhoReservadoParaOAlfabeto = Convert.ToString(encaminhamentos.Length / 8, 2).PadLeft(16, '0');

            return (bitsNaoUtilizadosNoFinal + tamanhoReservadoParaOAlfabeto + encaminhamentos + textoCodificado).ToBitArray();
        }

        public string Decodificar() {
            if (txt == null) return string.Empty;

            byte[] bytesDoTexto = txt.ToByteArray();

            return Decodificar(bytesDoTexto);
        }

        public string Decodificar(byte[] bytes)
        {
            BitArray bits = new BitArray(bytes);
            string dados = bits.ToDigitString();
            int offset = Convert.ToInt32(dados.Substring(4, 16), 2);
            int endOffset = Convert.ToInt16(dados.Substring(0, 4), 2);
            offset *= 8;

            string[] encaminhamentosESimbolos = dados.Substring(20, offset).BytesToString().Split(Separador);

            Simbolo tree = new Simbolo("");
            foreach (string encaminhamentoESimbolo in encaminhamentosESimbolos)
            {
                tree.CriarNodo(encaminhamentoESimbolo[0], encaminhamentoESimbolo.Substring(1, encaminhamentoESimbolo.Length - 1));
            }

            int offsetWithSizeLength = offset + 20;
            return Decodificar(tree, dados.Substring(offsetWithSizeLength, dados.Length - offsetWithSizeLength));
        }

        private string Decodificar(Simbolo arvore, string conteudoCodificado)
        {
            StringBuilder sb = new StringBuilder();
            Simbolo atual = arvore;
            foreach (char zeroOuUm in conteudoCodificado)
            {
                if (zeroOuUm.ToString().Equals("1"))
                {
                    atual = atual.SimboloAcima;
                }
                else
                {
                    atual = atual.SimboloAbaixo;
                }

                if (atual.Terminal)
                {
                    sb.Append(atual.Encaminhamento);
                    atual = arvore;
                }
            }

            return sb.ToString();
        }
    }
}
