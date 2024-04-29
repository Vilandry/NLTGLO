using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Fitnesses;
using NLTGLO.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLTGLO.GeneticAlgorithm
{
    public class SchedulingFitnessVariantA : IFitness
    {
        private Context ctx;

        public readonly List<Func<Schedule, double>> CostFunctions;


        public SchedulingFitnessVariantA(Context context)
        {
            ctx = context;
            CostFunctions = new List<Func<Schedule, double>>()
            {
                GetStudentDuplicatedScore,
                GetPresidentNotAvailableScore,
                GetSecretaryNotAvailableScore,
                GetExaminerNotAvailableScore,
                GetMemberNotAvailableScore,
                GetSupervisorNotAvailableScore,

                GetPresidentChangeScore,
                GetSecretaryChangeScore,

                GetAvgDifference,
                GetMinDifference,
                GetMaxDifference,

                GetPresidentSelfStudentScore,
                GetSecretarySelfStudentScore,
                GetExaminerNotPresidentScore

           };
        }


        public double EvaluateAll(Schedule sch)
        {
            int score = 0;


            sch.Details = new FinalExamDetail[Parameters.TimetableSize];

            var tasks = CostFunctions.Select(cf => Task.Run(() => cf(sch))).ToArray();
            Task.WaitAll(tasks);
            foreach (var task in tasks)
            {
                score -= (int)task.Result;
            }

            return score;
        }

        public double Evaluate(IChromosome chromosome)
        {
            int score = 0;

            Schedule sch = new Schedule(Parameters.TimetableSize);
            sch.Details = new FinalExamDetail[Parameters.TimetableSize];
            for (int i = 0; i < Parameters.TimetableSize; i++)
            {
                sch.FinalExams[i] = ((FinalExam)chromosome.GetGene(i).Value);
            }

            var tasks = CostFunctions.Select(cf => Task.Run(() => cf(sch))).ToArray();
            Task.WaitAll(tasks);
            foreach (var task in tasks)
            {
                score -= (int)task.Result;
            }

            return score;
        }

        public double GetStudentDuplicatedScore(Schedule sch)
        {
            double score = 0;
            List<Student> studentBefore = new List<Student>();
            int[] count = new int[Parameters.TimetableSize];
            foreach (var fe in sch.FinalExams)
            {
                count[fe.Student.Id]++;
            }
            for (int i = 0; i < Parameters.TimetableSize; i++)
            {
                if (count[i] > 1)
                {
                    score += (count[i] - 1) * Scores.StudentDuplicated;

                }
            }
            return score;
        }

        public double GetPresidentNotAvailableScore(Schedule sch)
        {
            double score = 0;
            for (int i = 0; i < sch.FinalExams.Length; i++)
            {
                if (sch.FinalExams[i].President.Availability[i] == false)
                {
                    score += Scores.PresidentNotAvailable;
                    if (ctx.FillDetails)
                    {
                        sch.Details[i].PresidentComment += $"President not available: {Scores.PresidentNotAvailable}\n";
                        sch.Details[i].PresidentScore += Scores.PresidentNotAvailable;
                    }
                }
            }
            return score;
        }

        public double GetSecretaryNotAvailableScore(Schedule sch)
        {
            double score = 0;
            for (int i = 0; i < sch.FinalExams.Length; i++)
            {
                if (sch.FinalExams[i].Secretary.Availability[i] == false)
                {
                    score += Scores.SecretaryNotAvailable;
                    if (ctx.FillDetails)
                    {
                        sch.Details[i].SecretaryComment += $"Secretary not available: {Scores.SecretaryNotAvailable}\n";
                        sch.Details[i].SecretaryScore += Scores.SecretaryNotAvailable;
                    }
                }
            }
            return score;
        }

        public double GetExaminerNotAvailableScore(Schedule sch)
        {
            double score = 0;
            for (int i = 0; i < sch.FinalExams.Length; i++)
            {
                if (sch.FinalExams[i].Examiner.Availability[i] == false)
                {
                    score += Scores.ExaminerNotAvailable;
                    if (ctx.FillDetails)
                    {
                        sch.Details[i].ExaminerComment += $"Examiner not available: {Scores.ExaminerNotAvailable}\n";
                        sch.Details[i].ExaminerScore += Scores.ExaminerNotAvailable;
                    }
                }


            }
            return score;
        }

        public double GetMemberNotAvailableScore(Schedule sch)
        {
            double score = 0;
            for (int i = 0; i < sch.FinalExams.Length; i++)
            {
                if (sch.FinalExams[i].Member.Availability[i] == false)
                {
                    score += Scores.MemberNotAvailable;
                    if (ctx.FillDetails)
                    {
                        sch.Details[i].MemberComment += $"Member not available: {Scores.MemberNotAvailable}\n";
                        sch.Details[i].MemberScore += Scores.MemberNotAvailable;
                    }
                }



            }
            return score;
        }

        public double GetSupervisorNotAvailableScore(Schedule sch)
        {
            double score = 0;
            for (int i = 0; i < sch.FinalExams.Length; i++)
            {
                if (sch.FinalExams[i].Supervisor.Availability[i] == false)
                {
                    score += Scores.SupervisorNotAvailable;
                    if (ctx.FillDetails)
                    {
                        sch.Details[i].SupervisorComment += $"Supervisor not available: {Scores.SupervisorNotAvailable}\n";
                        sch.Details[i].SupervisorScore += Scores.SupervisorNotAvailable;
                    }
                }


            }
            return score;
        }

        public double GetPresidentChangeScore(Schedule sch)
        {
            double score = 0;

            for (int i = 0; i < sch.FinalExams.Length; i += 5)
            {
                if (sch.FinalExams[i].President != sch.FinalExams[i + 1].President)
                {
                    score += Scores.PresidentChange;
                    if (ctx.FillDetails)
                    {
                        sch.Details[i + 1].PresidentComment += $"President changed: {Scores.PresidentChange}\n";
                        sch.Details[i + 1].PresidentScore += Scores.PresidentChange;
                    }
                }
                if (sch.FinalExams[i + 1].President != sch.FinalExams[i + 2].President)
                {
                    score += Scores.PresidentChange;
                    if (ctx.FillDetails)
                    {
                        sch.Details[i + 2].PresidentComment += $"President changed: {Scores.PresidentChange}\n";
                        sch.Details[i + 2].PresidentScore += Scores.PresidentChange;
                    }
                }
                if (sch.FinalExams[i + 2].President != sch.FinalExams[i + 3].President)
                {
                    score += Scores.PresidentChange;
                    if (ctx.FillDetails)
                    {
                        sch.Details[i + 3].PresidentComment += $"President changed: {Scores.PresidentChange}\n";
                        sch.Details[i + 3].PresidentScore += Scores.PresidentChange;
                    }
                }
                if (sch.FinalExams[i + 3].President != sch.FinalExams[i + 4].President)
                {
                    score += Scores.PresidentChange;
                    if (ctx.FillDetails)
                    {
                        sch.Details[i + 4].PresidentComment += $"President changed: {Scores.PresidentChange}\n";
                        sch.Details[i + 4].PresidentScore += Scores.PresidentChange;
                    }
                }
            }
            return score;
        }

        public double GetSecretaryChangeScore(Schedule sch)
        {
            double score = 0;

            for (int i = 0; i < sch.FinalExams.Length; i += 5)
            {
                if (sch.FinalExams[i].Secretary != sch.FinalExams[i + 1].Secretary)
                {
                    score += Scores.SecretaryChange;
                    if (ctx.FillDetails)
                    {
                        sch.Details[i + 1].SecretaryComment += $"Secretary changed: {Scores.SecretaryChange}\n";
                        sch.Details[i + 1].SecretaryScore += Scores.SecretaryChange;
                    }
                }
                if (sch.FinalExams[i + 1].Secretary != sch.FinalExams[i + 2].Secretary)
                {
                    score += Scores.SecretaryChange;
                    if (ctx.FillDetails)
                    {
                        sch.Details[i + 2].SecretaryComment += $"Secretary changed: {Scores.SecretaryChange}\n";
                        sch.Details[i + 2].SecretaryScore += Scores.SecretaryChange;
                    }
                }
                if (sch.FinalExams[i + 2].Secretary != sch.FinalExams[i + 3].Secretary)
                {
                    score += Scores.SecretaryChange;
                    if (ctx.FillDetails)
                    {
                        sch.Details[i + 3].SecretaryComment += $"Secretary changed: {Scores.SecretaryChange}\n";
                        sch.Details[i + 3].SecretaryScore += Scores.SecretaryChange;
                    }
                }
                if (sch.FinalExams[i + 3].Secretary != sch.FinalExams[i + 4].Secretary)
                {
                    score += Scores.SecretaryChange;
                    if (ctx.FillDetails)
                    {
                        sch.Details[i + 4].SecretaryComment += $"Secretary changed: {Scores.SecretaryChange}\n";
                        sch.Details[i + 4].SecretaryScore += Scores.SecretaryChange;
                    }
                }

            }

            return score;
        }

        public double GetAvgDifference(Schedule sch)
        {
            if(this.ctx.Instructors.Length == 0)
            {
                return 0;
            }

            double diff = 0;

            foreach (Instructor inst in this.ctx.Instructors) {
                diff += WorkloadCalculator.CalculateWorkLoad(sch, inst);

            }

            return diff/this.ctx.Instructors.Length;
        }

        public double GetMinDifference(Schedule sch)
        {
            double diff = 0;

            foreach (Instructor inst in this.ctx.Instructors)
            {
                double wl = WorkloadCalculator.CalculateWorkLoad(sch, inst);

                diff = wl < diff ? wl : diff;
            }


            return diff;
        }

        public double GetMaxDifference(Schedule sch)
        {
            double diff = 0;

            foreach (Instructor inst in this.ctx.Instructors)
            {
                double wl = WorkloadCalculator.CalculateWorkLoad(sch, inst);

                diff = wl > diff ? wl : diff;
            }


            return diff;
        }

        public double GetPresidentSelfStudentScore(Schedule sch)
        {
            double score = 0;
            foreach (var fi in sch.FinalExams)
            {
                if ((fi.Supervisor.Roles & Roles.President) == Roles.President && fi.Supervisor != fi.President)
                {
                    score += Scores.PresidentSelfStudent;
                    if (ctx.FillDetails)
                    {
                        sch.Details[Array.IndexOf(sch.FinalExams, fi)].SupervisorComment += $"Not President: {Scores.PresidentSelfStudent}\n";
                        sch.Details[Array.IndexOf(sch.FinalExams, fi)].SupervisorScore += Scores.PresidentSelfStudent;
                    }
                }
            }
            return score;
        }

        public double GetSecretarySelfStudentScore(Schedule sch)
        {
            double score = 0;
            foreach (var fi in sch.FinalExams)
            {
                if ((fi.Supervisor.Roles & Roles.Secretary) == Roles.Secretary && fi.Supervisor != fi.Secretary)
                {
                    score += Scores.SecretarySelfStudent;
                    if (ctx.FillDetails)
                    {
                        sch.Details[Array.IndexOf(sch.FinalExams, fi)].SupervisorComment += $"Not Secretary: {Scores.SecretarySelfStudent}\n";
                        sch.Details[Array.IndexOf(sch.FinalExams, fi)].SupervisorScore += Scores.SecretarySelfStudent;
                    }
                }
            }
            return score;
        }

        public double GetExaminerNotPresidentScore(Schedule sch)
        {
            double score = 0;
            foreach (var fi in sch.FinalExams)
            {
                if ((fi.Examiner.Roles & Roles.President) == Roles.President && fi.Examiner != fi.President)
                {
                    score += Scores.ExaminerNotPresident;
                    if (ctx.FillDetails)
                    {
                        sch.Details[Array.IndexOf(sch.FinalExams, fi)].ExaminerComment += $"Not President: {Scores.ExaminerNotPresident}\n";
                        sch.Details[Array.IndexOf(sch.FinalExams, fi)].ExaminerScore += Scores.ExaminerNotPresident;
                    }
                }
            }
            return score;
        }


    }

}
