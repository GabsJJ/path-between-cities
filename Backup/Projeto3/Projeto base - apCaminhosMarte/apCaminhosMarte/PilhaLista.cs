using System;

public class PilhaLista<Dado> : IStack<Dado>, ICopiavel<PilhaLista<Dado>>, IComparable<PilhaLista<Dado>> where Dado : IComparable<Dado>, ICopiavel<Dado>
{
    private NoLista<Dado> topo;
    private int tamanho;
    public PilhaLista()
    { // construtor
        topo = null;
        tamanho = 0;
    }
    public int Tamanho()
    {
        return tamanho;
    }
    public bool EstaVazia()
    {
        return (topo == null);
    }
    public void Empilhar(Dado o)
    {
        NoLista<Dado> novoNo = new NoLista<Dado>(o, topo);
        topo = novoNo; // topo passa a apontar o novo nó
        tamanho++; // atualiza número de elementos na pilha
    }
    public Dado OTopo()
    {
        Dado o;
        if (EstaVazia())
            throw new PilhaVaziaException("Underflow da pilha");
        o = topo.Info;
        return o;
    }
    public Dado Desempilhar()
    {
        if (EstaVazia())
            throw new PilhaVaziaException("Underflow da pilha");
        Dado o = topo.Info; // obtém o objeto do topo
        topo = topo.Prox; // avança topo para o nó seguinte
        tamanho--; // atualiza número de elementos na pilha
        return o; // devolve o objeto que estava no topo
    }

    public PilhaLista<Dado> Copia()
    {
        PilhaLista<Dado> s1 = new PilhaLista<Dado>();
        PilhaLista<Dado> s2 = new PilhaLista<Dado>();
        while (!this.EstaVazia())
            s1.Empilhar(this.Desempilhar());
        while (!s1.EstaVazia())
        {
            var obj = s1.Desempilhar();
            this.Empilhar(obj);
            s2.Empilhar(obj.Copia());
        }
        return s2;
    }

    public int CompareTo(PilhaLista<Dado> outra)
    {
        return this.CompareTo(outra);
    }
}