using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02_RegistrationFramework
{
    [DataRegister.ModelAnnotation]
    class Car
    {
        public string Name { get; set; }

        public double Weight { get; set; }

        public DateTime ConstructionDate { get; set; }
    }
}
