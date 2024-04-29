using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLTGLO.GeneticAlgorithm;

namespace NLTGLO.Model
{
    static class WorkloadCalculator
    {

        static public double CalculateWorkLoad(Schedule sch, Instructor inst)
        {
            double Workload = 0;

            foreach (FinalExam finalExam in sch.FinalExams)
            {
                bool worksOnCurrent = false;

                if (finalExam.Member.Name.Equals(inst.Name))
                {
                    worksOnCurrent = true;
                    Workload += Scores.WorksAsMember;
                }

                if (finalExam.Examiner.Name.Equals(inst.Name))
                {
                    worksOnCurrent = true;
                    Workload += Scores.WorksAsExaminer;
                }

                if (finalExam.Secretary.Name.Equals(inst.Name))
                {
                    worksOnCurrent = true;
                    Workload += Scores.WorksAsSecretary;
                }

                if (finalExam.President.Name.Equals(inst.Name))
                {
                    worksOnCurrent = true;
                    Workload += Scores.WorksAsPresident;
                }

                if (finalExam.Supervisor.Name.Equals(inst.Name))
                {
                    worksOnCurrent = true;
                    Workload += Scores.WorksAsSupervisor;
                }

                if(worksOnCurrent)
                {
                    Workload += Scores.WorksAsAnything;
                }
            }


            return Workload;
        }
    }
}
