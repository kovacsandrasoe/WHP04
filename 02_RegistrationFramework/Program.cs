using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02_RegistrationFramework
{
    class Program
    {
        static void Main(string[] args)
        {
            DataRegister.xml_file_path = "train.xml";
            DataRegister.CollectData();
        }
    }
}
