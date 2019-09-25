using System;

namespace DTO
{
    public class SousClassificationDTO
    {
        public SousClassificationDTO(string id, string name)
        {
            this.id = id;
            this.name = name;
        }

        public string id { get; set; }
        public string name { get; set; }
        public Guid id_classification { get; set; }
        public override bool Equals(object obj)
        {
            if (((SousClassificationDTO)obj).id == this.id) return true;
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(id);
        }
    }
}
