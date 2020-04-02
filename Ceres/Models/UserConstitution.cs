using System;

namespace Ceres.Models
{

    public class UserConstitution
    {
        public Constitution Constitution { get; set; }

        public string ModelRecipe
        {
            get
            {
                switch(Constitution)
                {
                    case Constitution.PingheCons:
                        return "431ebfe0a22d802fe85911e72b0ae02.jpg";
                    case Constitution.QixuCons:
                        return "82cb1e805d265c7e02fd13b948ecce6.jpg";
                    case Constitution.QiyuCons:
                        return "e33e244b3b52a8114a1d0743c6b5963.jpg";
                    case Constitution.ShireCons:
                        return "5a2a16044a90aeccb19c70a219587b3.jpg";
                    case Constitution.TanshiCons:
                        return "4d0a7ef392ef0b5cca7e682abd52b58.jpg";
                    case Constitution.TebingCons:
                        return "a29ac51a706b443916e97226ab9a914.jpg";
                    case Constitution.XueyuCons:
                        return "a95dd2fe0a4a0017a5a5e6d596c20f6.jpg";
                    case Constitution.YangxuCons:
                        return "fff4b6e13bc03f4776099e151c3e134.jpg";
                    case Constitution.YinxuCons:
                        return "c0de8982b68b37952666bb53eb8d84b.jpg";
                    default:
                        throw new InvalidOperationException($"当前用户体质数据错误：{Constitution}");
                }
            }
        }

    }
}
