using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SurveysManagement.Web.Enums
{
    public class Enums
    {
        public enum QuestionTypes : int
        {
            Checkbox = 1,
            Multiplechoice = 2,
            Text = 3,
            Percentage = 4,
            Range = 5,
            Number = 6,
            Dropdown = 7,
            Slider = 8,
            Date = 9,
            Rate = 10
        }

        public struct RoleTypes
        {
            public static string User = "User";
            public static string Admin = "Admin";
            public static string Client = "Client";
        }


        public enum SurveyMode
        {
            ReadOnly,
            Entry
        }

    }
}