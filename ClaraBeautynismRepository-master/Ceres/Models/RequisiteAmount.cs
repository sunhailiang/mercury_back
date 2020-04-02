using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ceres.Models
{
    public class RequisiteAmount
    {
        [Key]
        public bool Sex { get; set; }
        public double Calcium { get; set; }
        public double Phosphor { get; set; }
        public double Magnesium { get; set; }
        public double Iron { get; set; }
        public double Iodine { get; set; }
        public double Zinc { get; set; }
        public double Selenium { get; set; }
        public double Copper { get; set; }
        public double Aluminum { get; set; }
        public double VitaminA { get; set; }
        public double VitaminD { get; set; }
        public double VitaminB1 { get; set; }
        public double VitaminB2 { get; set; }
        public double VitaminB6 { get; set; }
        public double VitaminB12 { get; set; }
        public double Folicacid { get; set; }
        public double Nicotinicacid { get; set; }
        public double VitaminC { get; set; }

    }
}
