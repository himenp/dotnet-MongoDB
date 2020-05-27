using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace dotnetfundaMongoDB.Models
{
    public class Utilities
    {
        public static string ConvertStringArrayToString(string[] array)
        {
            StringBuilder builder = new StringBuilder();
            foreach (string value in array)
            {
                builder.Append(value);
                builder.Append(',');
            }
            return builder.ToString();
        }
    }
}