using System;
using System.Collections.Generic;
using System.Text;

namespace DTO
{
    public class BanqueDTO
    {
        public string name { get; set; }
        public Guid id { get; set; }
        public Guid default_account { get; set; }


        public BanqueDTO(string name)
        {
            this.name = name;
        }

        public override bool Equals(object obj)
        {
            return obj is BanqueDTO banque &&
                   name == banque.name;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(name);
        }
        public override string ToString()
        {
            return this.name;
        }
    }
}
