using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Populations;
using GeneticSharp.Domain.Reinsertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLTGLO.GeneticAlgorithm
{
    public class SchedulingElitistReinsertion : ElitistReinsertion
    {
        protected override IList<IChromosome> PerformSelectChromosomes(IPopulation population, IList<IChromosome> offspring, IList<IChromosome> parents)
        {
            var diff = population.MaxSize - offspring.Count;

            if (diff > 0)
            {
                var bestParents = parents.OrderByDescending(p => p.Fitness).Take(diff).ToList();

                for (int i = 0; i < bestParents.Count; i++)
                {
                    offspring.Add(bestParents[i]);
                }
            }
            return offspring;
        }
    }
}
