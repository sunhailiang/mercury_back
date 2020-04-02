using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ceres.Models
{
    public class FoodComposition
    {
        [Key]
        public int Namber { get; set; }
        public string Code { get; set; }
        public string Sort { get; set; }
        public string Coding { get; set; }
        public string SampleName { get; set; }
        public bool Staple { get; set; }
        public double Quantity { get; set; }
        public double Water { get; set; }
        public double CrudeProtein { get; set; }
        public double CrudeFat { get; set; }
        public double AchContent { get; set; }
        public double Carbohydrate { get; set; }
        public double CrudeFibre { get; set; }
        public double DietaryFiber { get; set; }
        public double TheTotalSuger { get; set; }
        public double Sodium { get; set; }
        public double Kalium { get; set; }
        public double Autunite { get; set; }
        public double Magnesium { get; set; }
        public double Phosphorus { get; set; }
        public double Iron { get; set; }
        public double Zinc { get; set; }
        public double selenium { get; set; }
        public double cuprum { get; set; }
        public double manganese { get; set; }
        public double iodine { get; set; }
        public double VitaminB1 { get; set; }
        public double VitaminB2 { get; set; }
        public double VitaminB3 { get; set; }
        public double VitaminB6 { get; set; }
        public double VitaminB12 { get; set; }
        public double VitaminB9 { get; set; }
        public double VitaminC { get; set; }
        public double Equivalent { get; set; }
        public double FattyAcidS { get; set; }
        public double FattyAcidM { get; set; }
        public double FattyAcidP { get; set; }
        public double Cholesterol { get; set; }
        public double VitaminA { get; set; }
        public double RetinoicAcid { get; set; }
        public double αRenieratene { get; set; }
        public double βRenieratene { get; set; }
        public double HydrolyzedAminoAcid { get; set; }
        public double FattyAcid { get; set; }
        public double Alcohol { get; set; }
        public double FixedHeat { get; set; }
        public string PMS { get; set; }
        public double TotalVitaminE { get; set; }
    }
}
