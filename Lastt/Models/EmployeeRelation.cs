using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Lastt.Models
{
    public class EmployeeRelation
    {
        [Key]
        public int ID { get; set; }

        public int EmpId1 { get; set; }
        public int EmpId2 { get; set; }

        [ForeignKey("EmpId1")]
        public Employee Employee1 { get; set; }

        [ForeignKey("EmpId2")]
        public Employee Employee2 { get; set; }
    }

}
