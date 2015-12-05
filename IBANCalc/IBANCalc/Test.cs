using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBANCalc
{
    static class Test
    {
        public static bool mTest(int accountnumber)
        {
            string acn = accountnumber.ToString();
            //if (acn.Length != 9)
            //{
            //    Console.WriteLine("Invalid Accountnumber length.");
            //    return false;
            //}

            int sum = 0;
            for(int i = 0; i<9; i++)
            {
                sum += (int)char.GetNumericValue(acn[i]) * (9 - i);
            }

            if (sum % Program.M == 0)
                return true;

            return false;
        }
    }
}
