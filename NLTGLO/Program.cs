using NLTGLO.GeneticAlgorithm;
using NLTGLO.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLTGLO
{
    class Program
    {
        static Scheduler scheduler;
        static void Main(string[] args)
        {
            RunGenetic();
            Console.Read();
        }

        static void RunGenetic()
        {
            var watch = Stopwatch.StartNew();

            FileInfo existingFile = new FileInfo("C:\\Users\\Ádám\\Desktop\\egyetem\\26felev\\dipterv\\NLTGLO\\NLTGLO\\NLTGLO\\Input.xlsx");
            FileInfo auxilaryFile = new FileInfo("C:\\Users\\Ádám\\Desktop\\egyetem\\26felev\\dipterv\\NLTGLO\\NLTGLO\\NLTGLO\\Auxilary_Input.xlsx");

            var context = ExcelHelper.Read(existingFile);
            ExcelHelper.FillAuxialryIntoContext(auxilaryFile, context);

            Console.WriteLine("Every input file processed successfuly!");

            context.Init();
            scheduler = new Scheduler(context);

            var task = scheduler.RunAsync().ContinueWith(scheduleTask =>
            {
                Schedule resultSchedule = scheduleTask.Result;

                string elapsed = watch.Elapsed.ToString();

                SchedulingFitnessVariantA evaluator = new SchedulingFitnessVariantA(context);
                double penaltyScore = evaluator.EvaluateAll(resultSchedule);
                Console.WriteLine("Penalty score: " + penaltyScore);

                ExcelHelper.Write(@"..\..\Results\Done_Ge_" + DateTime.Now.ToString("yyyyMMdd_HHmm") + penaltyScore + ".xlsx", scheduleTask.Result, elapsed, scheduler.GenerationFitness, scheduler.GetFinalScores(resultSchedule, scheduler.Fitness), context);

            }
            );

            while (true)
            {
                if (task.IsCompleted)
                    break;
                var ch = Console.ReadKey();
                if (ch.Key == ConsoleKey.A)
                {
                    scheduler.Cancel();
                }
                Console.WriteLine("Press A to Abort");
            }
            Console.WriteLine();
        }
    }
}
