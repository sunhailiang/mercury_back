using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ceres.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ceres.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CersController : Controller
    {
        private readonly CersContent cersContent;
        public CersController(CersContent db)
        {
            cersContent = db;
        }

        #region
        /// <summary>
        /// 返回一个人每日所需的总能量
        /// </summary>
        /// <param name="height">身高m</param>
        /// <param name="weight">体重kg</param>
        /// /// <param name="activity">活动水平分级</param>
        [HttpPost]
        public IActionResult Energy(double height, double weight, string activity)
        {
            double physique = 0;
            physique = weight / (height * height);//体制指数
            List<double> kcal = new List<double>();
            if (physique < 18.5)//消瘦
            {
                if (activity == "极轻")
                {
                    kcal.Add(((height * 100) - 105) * 30);
                }
                else if (activity == "轻")
                {
                    kcal.Add(((height * 100) - 105) * 35);
                }
                else if (activity == "中")
                {
                    kcal.Add(((height * 100) - 105) * 40);
                }
                else if (activity == "重")
                {
                    kcal.Add(((height * 100) - 105) * 45);
                }
            }
            else if (physique >= 18.5 && physique <= 23.9)//正常
            {
                if (activity == "极轻")
                {
                    kcal.Add(((height * 100) - 105) * 25);
                }
                else if (activity == "轻")
                {
                    kcal.Add(((height * 100) - 105) * 30);
                }
                else if (activity == "中")
                {
                    kcal.Add(((height * 100) - 105) * 35);
                }
                else if (activity == "重")
                {
                    kcal.Add(((height * 100) - 105) * 40);
                }
            }
            else if (physique >= 24.0)//超重
            {
                if (activity == "极轻")
                {
                    kcal.Add(((height * 100) - 105) * 20);
                }
                else if (activity == "轻")
                {
                    kcal.Add(((height * 100) - 105) * 25);
                }
                else if (activity == "中")
                {
                    kcal.Add(((height * 100) - 105) * 30);
                }
                else if (activity == "重")
                {
                    kcal.Add(((height * 100) - 105) * 35);
                }
            }
            return Json(kcal);

        }
        #endregion
        /// <summary>
        /// 计算三种营养素分别为多少 
        /// </summary>
        /// <param name="height">身高m</param>
        /// <param name="weight">体重kg</param>
        /// <param name="activity">活动水平分级  极轻  轻  中  重</param>
        /// <returns></returns>
        public IActionResult NutrientRatio(double height, double weight, string activity, double supply)
        {
            double energy = Convert.ToDouble(Energy(height, weight, activity));//能量的总量kcal


            double morningprotein = ((energy * supply) * 0.3) / 4;//早餐的蛋白质g
            double morningfats = ((energy * supply) * 0.3) / 9;//早餐的脂肪g
            double morningcarbohydrates = ((energy * supply) * 0.3) / 4;//早餐的碳水化合物g

            double aftennoonprotein = ((energy * supply) * 0.4) / 4;//中餐的蛋白质g
            double aftennoonfats = ((energy * supply) * 0.4) / 9;//中餐的脂肪g
            double aftennooncarbohydrates = ((energy * supply) * 0.4) / 4;//中餐的碳水化合物g

            double eveningprotein = ((energy * supply) * 0.3) / 4;//晚餐的蛋白质g
            double eveningfats = ((energy * supply) * 0.3) / 9;//晚餐的脂肪g
            double eveningcarbohydrates = ((energy * supply) * 0.3) / 4;//晚餐的碳水化合物g

            return null;

        }

        /// <summary>
        /// 实现食物输入重量传出每一个营养素的重量
        /// </summary>
        /// <param name="food">食物名称</param>
        /// <param name="foodWeight">食物重量g</param>
        [HttpPost]
        public IActionResult FoodComposition(string food, double foodWeight)
        {
            var result = (from s in cersContent.FoodComposition
                          where s.SampleName.Contains(food)
                          select s).FirstOrDefault();
            List<FoodComposition> foodCompositions = new List<FoodComposition>();
            FoodComposition foodComposition = new FoodComposition
            {
                Namber = result.Namber,
                Code = result.Code,
                Sort=result.Sort,
                Coding=result.Coding,
                SampleName=result.SampleName,
                Staple=result.Staple,
                Quantity = result.Quantity * (foodWeight/100),
                Water = result.Water * (foodWeight/100),
                CrudeProtein = result.CrudeProtein * (foodWeight/100),
                CrudeFat = result.CrudeFat * (foodWeight/100),
                AchContent = result.AchContent * (foodWeight/100),
                Carbohydrate = result.Carbohydrate * (foodWeight/100),
                CrudeFibre = result.CrudeFibre * (foodWeight/100),
                DietaryFiber = result.DietaryFiber * (foodWeight/100),
                TheTotalSuger = result.TheTotalSuger * (foodWeight/100),
                Sodium = result.Sodium * (foodWeight/100),
                Kalium = result.Kalium * (foodWeight/100),
                Autunite = result.Autunite *(foodWeight/100),
                Magnesium = result.Magnesium * (foodWeight/100),
                Phosphorus = result.Phosphorus * (foodWeight/100),
                Iron = result.Iron * (foodWeight/100),
                Zinc = result.Zinc * (foodWeight/100),
                selenium = result.selenium * (foodWeight/100),
                cuprum = result.cuprum * (foodWeight/100),
                manganese = result.manganese * (foodWeight/100),
                iodine = result.iodine * (foodWeight/100),
                VitaminB1 = result.VitaminB1 * (foodWeight/100),
                VitaminB2 = result.VitaminB2 * (foodWeight/100),
                VitaminB3 = result.VitaminB3 * (foodWeight/100),
                VitaminB6 = result.VitaminB6 * (foodWeight/100),
                VitaminB12 = result.VitaminB12 * (foodWeight/100),
                VitaminB9 = result.VitaminB9 * (foodWeight/100),
                VitaminC = result.VitaminC * (foodWeight/100),
                Equivalent = result.Equivalent * (foodWeight/100),
                FattyAcidM = result.FattyAcidM * (foodWeight/100),
                FattyAcidP = result.FattyAcidP * (foodWeight/100),
                Cholesterol = result.Cholesterol * (foodWeight/100),
                VitaminA = result.VitaminA * (foodWeight/100),
                RetinoicAcid = result.RetinoicAcid * (foodWeight/100),
                αRenieratene = result.αRenieratene * (foodWeight/100),
                βRenieratene = result.βRenieratene * (foodWeight/100),
                HydrolyzedAminoAcid = result.HydrolyzedAminoAcid * (foodWeight/100),
                FattyAcid = result.FattyAcid * (foodWeight/100),
                Alcohol = result.Alcohol * (foodWeight/100),
                FixedHeat = result.FixedHeat * (foodWeight/100),
                PMS = result.PMS,
                TotalVitaminE = result.TotalVitaminE * (foodWeight / 100),
            };
            foodCompositions.Add(foodComposition);
            return Json(foodCompositions);

        }
    }
}