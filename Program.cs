using System;

namespace LocadoraDeMidias
{
    // CLASSE PAI
}
    abstract class Midia
    {
        private string titulo;
        private int anoLancamento;
        private decimal precoBase;

        public string Titulo
        {
            get { return titulo; }
        }

        public int AnoLancamento
        {
            get { return anoLancamento; }
        }

        public decimal PrecoBase
        {
            get { return precoBase; }
        }

        public bool Disponivel { get; private set; }
        public int DiasAlugados { get; private set; }
        public DateTime DataPrevistaDevolucao { get; private set; }

        public Midia(string titulo, int ano, decimal preco)
        {
            if (string.IsNullOrWhiteSpace(titulo))
                throw new ArgumentException("Título inválido.");

            if (ano <= 0)
                throw new ArgumentException("Ano inválido.");

            if (preco <= 0)
                throw new ArgumentException("Preço deve ser positivo.");

            this.titulo = titulo;
            anoLancamento = ano;
            precoBase = preco;
            Disponivel = true;
        }

        // Cada classe filha terá seu próprio cálculo.
        public abstract decimal CalcularValorAluguel();

        public bool Alugar(int dias)
        {
            if (!Disponivel)
            {
                Console.WriteLine(titulo + " não está disponível.");
                return false;
            }

            if (dias <= 0)
            {
                Console.WriteLine("A quantidade de dias deve ser positiva.");
                return false;
            }

            DiasAlugados = dias;
            DataPrevistaDevolucao = DateTime.Today.AddDays(dias);
            Disponivel = false;

            Console.WriteLine("\nAluguel realizado: " + titulo);
            Console.WriteLine("Valor: R$ " +
            CalcularValorAluguel().ToString("F2"));

            Console.WriteLine("Devolver até: " +
            DataPrevistaDevolucao.ToString("dd/MM/yyyy"));

            return true;
        }

        public decimal Devolver(DateTime dataDevolucao)
        {
            if (Disponivel)
            {
                Console.WriteLine("Esta mídia não está alugada.");
                return 0;
            }

            int diasAtraso = Math.Max(
            0, (dataDevolucao.Date -
            DataPrevistaDevolucao.Date).Days);

            // Multa de 10% do preço-base por dia de atraso.
            decimal multa = diasAtraso * PrecoBase * 0.10m;

            Disponivel = true;
            DiasAlugados = 0;

            Console.WriteLine("\nDevolução: " + Titulo);
            Console.WriteLine("Dias de atraso: " + diasAtraso);
            Console.WriteLine("Multa: R$ " + multa.ToString("F2"));

            return multa;
        }

        public virtual void ExibirInformacoes()
        {
            Console.WriteLine(
            $"{Titulo} | Ano: {AnoLancamento} | " +
            $"Preço-base: R$ {PrecoBase:F2} | " +
            $"Disponível: {(Disponivel ? "Sim" : "Não")}");
        }
    }

// CLASSE FILHA: FILME
class Filme : Midia
{
public Filme(string titulo, int ano, decimal preco)
: base(titulo, ano, preco)
{
}

public override decimal CalcularValorAluguel()
{
return PrecoBase * DiasAlugados;
}

public override void ExibirInformacoes()
{
Console.WriteLine("CATEGORIA: FILME");
base.ExibirInformacoes();
}
}

// CLASSE FILHA: JOGO
class Jogo : Midia
{
public Jogo(string titulo, int ano, decimal preco)
: base(titulo, ano, preco)
{
}

public override decimal CalcularValorAluguel()
{
return PrecoBase * DiasAlugados * 1.5m;
}

public override void ExibirInformacoes()
{
Console.WriteLine("CATEGORIA: JOGO");
base.ExibirInformacoes();
}
}

// CLASSE FILHA: SÉRIE
class Serie : Midia
{
public Serie(string titulo, int ano, decimal preco)
: base(titulo, ano, preco)
{
}

public override decimal CalcularValorAluguel()
{
return PrecoBase * DiasAlugados * 0.8m;
}

public override void ExibirInformacoes()
{
Console.WriteLine("CATEGORIA: SÉRIE");
base.ExibirInformacoes();
}
}

// PROGRAMA PRINCIPAL
class Program
{
static void Main(string[] args)
{
Midia filme = new Filme(
"Vingadores", 2019, 10.00m);

Midia jogo = new Jogo(
"FIFA", 2025, 15.00m);

Midia serie = new Serie(
"Stranger Things", 2022, 8.00m);

Console.WriteLine("=== LOCADORA DE MÍDIAS ===");

filme.ExibirInformacoes();
jogo.ExibirInformacoes();
serie.ExibirInformacoes();

// Teste dos aluguéis
filme.Alugar(3);
jogo.Alugar(2);
serie.Alugar(5);

// Teste de operação inválida
Console.WriteLine("\n=== TESTE DE VALIDAÇÃO ===");
filme.Alugar(2); // Filme já alugado
jogo.Alugar(0); // Dias inválidos

// Teste de devolução com atraso
Console.WriteLine("\n=== DEVOLUÇÕES ===");

filme.Devolver(
DateTime.Today.AddDays(5));

jogo.Devolver(
DateTime.Today.AddDays(2));

serie.Devolver(
DateTime.Today.AddDays(7));

Console.WriteLine("\n=== FIM DOS TESTES ===");
}
}