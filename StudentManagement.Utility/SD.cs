using System;
using System.Collections.Generic;
using System.Text;

namespace StudentManagement.Utility
{
    public class SD
    {
        public const string Role_Admin = "Admin";
        public const string Role_Manager = "Manager";
        public const string Role_Teacher = "Teacher";
        public const string Role_Student = "Student";

        public static float? GetAverageScore(float? score1, float? score2, float? score3)
        {
            return (score1 + score2 * 2 + score3 * 3) / 6;
        }
    }
}
