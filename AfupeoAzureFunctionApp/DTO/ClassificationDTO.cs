using System;
using System.Collections.Generic;
using System.Text;

namespace DTO
{
    public class ClassificationDTO
    {
        public string id { get; set; }
        public string name { get; set; }
        public List<SousClassificationDTO> sousClassifications { get; set; }
        public override bool Equals(object obj)
        {
            if(((ClassificationDTO)obj).id==this.id)return true;
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(id);
        }

        public ClassificationDTO(string id, string name)
        {
            this.id = id;
            this.name = name;
            this.sousClassifications = new List<SousClassificationDTO>();
        }

        public ClassificationDTO(string id)
        {
            this.id = id;
        }
        public ClassificationDTO()
        {

        }
        public override string ToString()
        {
            return name;
        }
    }
}
