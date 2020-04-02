namespace MercurySurvey.Models.TraditionalMedicalConstitution
{
    public class Regime
    {
        /// <summary>
        /// 体质状态
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 转化分
        /// </summary>
        public double CurrentCent { get; set; }
        /// <summary>
        /// 标准分
        /// </summary>
        public int StandCent { get; set; }
        /// <summary>
        /// 建议
        /// </summary>
        public string DescriptionText
        {
            get
            {
                switch (Title)
                {
                    case "倾向阳虚质":
                        return "您的体质类型倾向于阳虚质,请注意日常调养，防止体质出现过度偏颇。";
                    case "阳虚质":
                        return "您的体质表现为阳虚质，易患痰饮、肿胀、泄泻等病。感邪易从寒化。女性易形成寒滞，宫寒。";
                    case "倾向阴虚质":
                        return "您的体质倾向于阴虚质，建议合理饮食，起居有时，防止体质出现更大的偏颇。";
                    case "阴虚质":
                        return "您的体质表现为阴虚质，易患虚劳、失精、不寐，感邪易从热化。";
                    case "倾向气虚质":
                        return "您的体质倾向于气虚质，建议合理饮食，起居有时，防止体质出现更大的偏颇。";
                    case "气虚质":
                        return "您的体质表现为气虚质，易感冒、内脏下垂，病后康复缓慢。";
                    case "倾向痰湿质":
                        return "您的体质倾向于痰湿质，建议合理饮食，起居有时，防止体质出现更大的偏颇。";
                    case "痰湿质":
                        return "您的体质表现为痰湿质，易患消渴、中风、胸痹、高血脂。";
                    case "倾向湿热质":
                        return "您的体质倾向于湿热质，建议合理饮食，起居有时，防止体质出现更大的偏颇。";
                    case "湿热质":
                        return "您的体质表现为湿热质，易患疮疖、黄疸、热淋等病。";
                    case "倾向血瘀质":
                        return "您的体质倾向于血瘀质，建议合理饮食，起居有时，防止体质出现更大的偏颇。";
                    case "血瘀质":
                        return "您的体质表现为血瘀质，易患冠心病、中风、痛症等。";
                    case "倾向气郁质":
                        return "您的体质倾向于气郁质，建议合理饮食，保持良好的心情，多与朋友沟通，及时倾诉不良情绪。";
                    case "气郁质":
                        return "您的体质表现为气郁质，易患脏躁、郁证、失眠、抑郁症、神经官能症等。建议解除自我封闭状态，多结交朋友，及时向朋友倾诉不良情绪。";
                    case "倾向特禀质":
                        return "建议保持室内清洁，被褥床单经常洗晒，室内装修后不宜立即居住。春季减少室外活动，预防花粉过敏。不宜养宠物，起居规律，积极参加各种体育锻炼，避免情绪紧张。";
                    case "特禀质":
                        return "如为过敏体质者，易患哮喘、荨麻疹、花粉症及药物过敏。另可见于遗传性疾病、胎传性疾病等。";
                    case "倾向平和质":
                        return "您的体质倾向于平和质，平和体质若不注意后天调养，亦可变为偏颇体质。";
                    case "平和质":
                        return "您的体质表现为平和质，平和体质若不注意后天调养，亦可变为偏颇体质。";
                    default:
                        return "服务器计算错误，计算结果中出现了九大辩证体质以外的信息。";
                }
            }
        }
        public string UserName { get; set; }
    }

}
