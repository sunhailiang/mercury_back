using System;
using System.ComponentModel.DataAnnotations;

namespace MercurySurvey.Models.TraditionalMedicalConstitution
{
    public class UserInformationRecord
    {
        [Key]
        public int RecordID { get; set; }

//#error 尚未进行db migration
        public string Province { get; set; }

        public string City { get; set; }

        public double Height { get; set; }

        public string PhoneNumber { get; set; }

        public Gender UserGender { get; set; }

        public Guid UserGuid { get; set; }

        public string UserName { get; set; }

        public double Weight { get; set; }

        public int Age { get; set; }

        public enum Gender { Man, Woman }

    }
}