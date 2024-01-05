using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exemple.Domain.Models
{
    public class Client
    {
        public Client(string nume, string prenume, long numarDeTelefon, string adresa)
        {
            this.clientId = Guid.NewGuid();
            this.nume = nume;
            this.prenume = prenume;
            this.numarDeTelefon = numarDeTelefon;
            this.adresa = adresa;
        }
        public Guid clientId { get; }
        public string nume { get; set; }
        public string prenume { get; set; }
        public long numarDeTelefon { get; set; }
        public string adresa { get; set; }
    }
}
