using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Caminho : IComparable<Caminho>, ICopiavel<Caminho>
{
    private int idCidadeOrigem, idCidadeDestino, distancia, tempo, custo;
    const int inicioIdCidadeOrigem = 0;
    const int tamanhoIdCidadeOrigem = 3;
    const int inicioIdCidadeDestino = inicioIdCidadeOrigem + tamanhoIdCidadeOrigem;
    const int tamanhoIdCidadeDestino = 3;
    const int inicioDistancia = inicioIdCidadeDestino + tamanhoIdCidadeDestino;
    const int tamanhoDistancia = 5;
    const int inicioTempo = inicioDistancia + tamanhoDistancia;
    const int tamanhoTempo = 4;
    const int inicioCusto = inicioTempo + tamanhoTempo;

    public int IdCidadeOrigem { get => idCidadeOrigem; set => idCidadeOrigem = value; }
    public int IdCidadeDestino { get => idCidadeDestino; set => idCidadeDestino = value; }
    public int Distancia { get => distancia; set => distancia = value; }
    public int Tempo { get => tempo; set => tempo = value; }
    public int Custo { get => custo; set => custo = value; }
    

    public Caminho(string linha)
    {
        this.IdCidadeOrigem = int.Parse(linha.Substring(inicioIdCidadeOrigem,tamanhoIdCidadeOrigem));
        this.IdCidadeDestino = int.Parse(linha.Substring(inicioIdCidadeDestino,tamanhoIdCidadeDestino));
        this.Distancia = int.Parse(linha.Substring(inicioDistancia,tamanhoDistancia));
        this.Tempo = int.Parse(linha.Substring(inicioTempo,tamanhoTempo));
        this.Custo = int.Parse(linha.Substring(inicioCusto));
    }

    public Caminho()
    {
        this.IdCidadeOrigem = default(int);
        this.IdCidadeDestino = default(int);
        this.Distancia = default(int);
        this.Tempo = default(int);
        this.Custo = default(int);
    }

    public int CompareTo(Caminho outro)
    {
        return this.CompareTo(outro);
    }

    public Caminho Copia()
    {
        Caminho c = new Caminho();
        c.IdCidadeOrigem = IdCidadeOrigem;
        c.IdCidadeDestino = IdCidadeDestino;
        c.Distancia = Distancia;
        c.Tempo = Tempo;
        c.Custo = Custo;

        return c;
    }
}