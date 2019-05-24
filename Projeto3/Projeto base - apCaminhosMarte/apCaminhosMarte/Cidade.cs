using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Cidade : IComparable<Cidade>
{
    private int idCidade, coordenadaX, coordenadaY;
    private string nomeCidade;

    const int tamanhoId = 3;
    const int inicioNomeCidade = 3;
    const int tamanhoNomeCidade = 15;
    const int inicioX = inicioNomeCidade + tamanhoNomeCidade;
    const int tamanhoCoord = 5;
    const int inicioY = inicioX + tamanhoCoord;

    public Cidade(int id, int x, int y, string nome)
    {
        this.IdCidade = id;
        this.CoordenadaX = x;
        this.CoordenadaY = y;
        this.NomeCidade = nome;
    }

    public Cidade (string linha)
    {
        IdCidade    = int.Parse(linha.Substring(0, tamanhoId));
        NomeCidade = linha.Substring(inicioNomeCidade, tamanhoNomeCidade);
        CoordenadaX = int.Parse(linha.Substring(inicioX, tamanhoCoord));   
        CoordenadaY = int.Parse(linha.Substring(inicioY));
    }

    public int IdCidade { get => idCidade; set => idCidade = value; }
    public int CoordenadaX { get => coordenadaX; set => coordenadaX = value; }
    public int CoordenadaY { get => coordenadaY; set => coordenadaY = value; }
    public string NomeCidade { get => nomeCidade; set => nomeCidade = value; }

    public int CompareTo(Cidade other)
    {
        throw new NotImplementedException();
    }
}
