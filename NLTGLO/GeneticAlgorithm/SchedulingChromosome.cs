using GeneticSharp.Domain.Chromosomes;
using NLTGLO.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLTGLO.GeneticAlgorithm
{
    public class SchedulingChromosome : ChromosomeBase
    {
        private Context ctx;

        public SchedulingChromosome(Context context) : base(Parameters.TimetableSize)
        {
            this.ctx = context;

            for (int i = 0; i < Parameters.TimetableSize; i++)
            {
                ReplaceGene(i, GenerateGene(i));
            }

        }

        public Schedule Schedule
        {
            get
            {
                Schedule schedule = new Schedule(Parameters.TimetableSize);

                for (int i = 0; i < Parameters.TimetableSize; i++)
                {
                    schedule.FinalExams[i] = (FinalExam)GetGene(i).Value;
                }
                return schedule;
            }
        }

        public override IChromosome CreateNew()
        {
            return new SchedulingChromosome(ctx);
        }


        public override Gene GenerateGene(int geneIndex)
        {
            FinalExam fe = new FinalExam();
            fe.Id = geneIndex;

            fe.Student = ctx.RandStudents[geneIndex];
            fe.Supervisor = fe.Student.Supervisor;
            fe.President = ctx.Presidents[ctx.Rnd.Next(0, ctx.Presidents.Length)];
            fe.Secretary = ctx.Secretaries[ctx.Rnd.Next(0, ctx.Secretaries.Length)];
            fe.Member = ctx.Members[ctx.Rnd.Next(0, ctx.Members.Length)];
            fe.Examiner = fe.Student.ExamCourse.Instructors[ctx.Rnd.Next(0, fe.Student.ExamCourse.Instructors.Length)];

            return new Gene(fe);
        }

    }
}
