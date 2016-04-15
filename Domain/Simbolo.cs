using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Simbolo
    {
        public bool Terminal { get; set; }
        public Simbolo SimboloAcima { get; set; }
        public Simbolo SimboloAbaixo { get; set; }
        public int Ocorrencias { get; set; }
        public string Encaminhamento { get; set; }

        public Simbolo(char encaminhamento, int ocorrencias)
        {
            Encaminhamento = encaminhamento.ToString();
            Ocorrencias = ocorrencias;
            Terminal = true;
        }

        public Simbolo(string encaminhamento)
        {
            Encaminhamento = encaminhamento;
            Terminal = encaminhamento.Length == 1;
        }

        public Simbolo(Simbolo simboloAcima, Simbolo simboloAbaixo)
        {
            SimboloAcima = simboloAcima;
            SimboloAbaixo = simboloAbaixo;
            Ocorrencias = simboloAcima.Ocorrencias + simboloAbaixo.Ocorrencias;
            Encaminhamento = simboloAbaixo.Encaminhamento + simboloAcima.Encaminhamento;
        }

        public string GetByteCodes(string alphabet)
        {
            IDictionary<string, string> map = GetSymbolsCodes();
            StringBuilder sb = new StringBuilder();
            foreach (char c in alphabet)
            {
                sb.Append(map[c.ToString()].PadLeft(8, '0'));
            }
            return sb.ToString();
        }

        public bool IsTerminal()
        {
            return Terminal;
        }

        public static Simbolo MontarArvore(IEnumerable<Simbolo> simbolos)
        {
            IList<Simbolo> listaDeSimbolos = simbolos.ToList();
            while (listaDeSimbolos.Count > 1)
            {
                JuntarSimbolosComMenorOcorrencia(ref listaDeSimbolos);
            }
            return listaDeSimbolos[0];
        }

        private static void JuntarSimbolosComMenorOcorrencia(ref IList<Simbolo> ListaDeSimbolos)
        {
            ListaDeSimbolos = ListaDeSimbolos.OrderBy(s => s.Ocorrencias).ToList();

            Simbolo a = ListaDeSimbolos[0];
            ListaDeSimbolos.RemoveAt(0);
            Simbolo b = ListaDeSimbolos[0];
            ListaDeSimbolos.RemoveAt(0);
            ListaDeSimbolos.Add(new Simbolo(a, b));
        }

        public void PrintPath()
        {
            PrintPath("");
        }

        private void PrintPath(string s)
        {
            if (Terminal)
            {
                Console.WriteLine(Encaminhamento + " " + s);
            }
            else
            {
                SimboloAcima.PrintPath(s + 1);
                SimboloAbaixo.PrintPath(s + 0);
            }
        }
        public IDictionary<string, string> GetSymbolsCodes()
        {
            IDictionary<string, string> map = new Dictionary<string, string>();
            GetSimboloEEncaminhamento("", map);
            return map;
        }

        private void GetSimboloEEncaminhamento(string s, IDictionary<string, string> map)
        {
            if (Terminal)
            {
                map.Add(Encaminhamento, s);
            }
            else
            {
                SimboloAcima.GetSimboloEEncaminhamento((s + 1), map);
                SimboloAbaixo.GetSimboloEEncaminhamento((s + 0), map);
            }
        }

        public void CriarNodo(char rep, string path)
        {
            if (path.Length == 1)
            {
                if (path.Equals("1"))
                {
                    SimboloAcima = new Simbolo(rep.ToString());
                }
                else
                {
                    SimboloAbaixo = new Simbolo(rep.ToString());
                }
            }
            else
            {
                if (path[0] == '1')
                {
                    if (SimboloAcima == null)
                    {
                        SimboloAcima = new Simbolo("");
                    }
                    SimboloAcima.CriarNodo(rep, path.Substring(1, path.Length - 1));
                }
                else
                {
                    if (SimboloAbaixo == null)
                    {
                        SimboloAbaixo = new Simbolo("");
                    }
                    SimboloAbaixo.CriarNodo(rep, path.Substring(1, path.Length - 1));
                }
            }
        }

        public void PrintPs()
        {
            if (Terminal)
            {
                Console.WriteLine(Encaminhamento + " " + Ocorrencias);
            }
            else
            {
                SimboloAcima.PrintPs();
                SimboloAbaixo.PrintPs();
            }
        }

        public string BuscarEncaminhamentos()
        {
            return BuscarEncaminhamentos("");
        }

        private string BuscarEncaminhamentos(string s)
        {
            if (Terminal)
            {
                return (Encaminhamento + s + Codec.Separador).ToByteString();
            }
            else
            {
                return SimboloAcima.BuscarEncaminhamentos(s + "1") + SimboloAbaixo.BuscarEncaminhamentos(s + "0");
            }
        }
    }
}
