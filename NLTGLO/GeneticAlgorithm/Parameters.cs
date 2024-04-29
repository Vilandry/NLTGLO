using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLTGLO.GeneticAlgorithm
{
    public class Parameters
    {
        public const int MinPopulationSize = 5000;
        public const int MaxPopulationSize = 7500;

        public const int StagnationTermination = 20;

        public const bool GetInfo = true;

        public static bool Finish = false;

        public const int NumberOfGenesForSelection = 2;
        public const int TimetableSize = 100;
    }
}
