using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Caminho : IComparable<Caminho>
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
        this.idCidadeOrigem = int.Parse(linha.Substring(inicioIdCidadeOrigem,tamanhoIdCidadeOrigem));
        this.idCidadeDestino = int.Parse(linha.Substring(inicioIdCidadeDestino,tamanhoIdCidadeDestino));
        this.distancia = int.Parse(linha.Substring(inicioDistancia,tamanhoDistancia));
        this.tempo = int.Parse(linha.Substring(inicioTempo,tamanhoTempo));
        this.custo = int.Parse(linha.Substring(inicioCusto));
    }
    public int CompareTo(Caminho outro)
    {
        return this.CompareTo(outro);
    }
}