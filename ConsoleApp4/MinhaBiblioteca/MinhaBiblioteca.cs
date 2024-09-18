using System;

namespace MinhaBiblioteca
{
    public class Person
    {
        public string Nome { get; set; }
        public int Idade { get; set; }
        public string Cidade { get; set; }

        public Person(string nome, int idade, string cidade)
        {
            Nome = nome;
            Idade = idade;
            Cidade = cidade;
        }
    }
}

