using System;
using Evolution.AbstractMachines.Brainfuck;

namespace Evolution {
	public class Runner {

		private int genNumber;
		private Generation currentGen;
		private int bestFitness = Int32.MaxValue;

		public Runner() {
			genNumber = 0;
			var machines = new BrainfuckProgram[100];
			for (var i = 0; i < 100; i++) {
				machines[i] = new BrainfuckProgram(BrainfuckProgram.RandomProgram(100));
			}

			currentGen = new Generation(machines);
		}

		public void runGeneration() {
			currentGen.runAll();
			var bestMachines = currentGen.getBest(10);
			if (bestMachines[0].fitness < bestFitness) {
				Console.WriteLine(
					$"Generation {genNumber} - Best fitness: {bestMachines[0].fitness}, \"{bestMachines[0].machine.output}\"  {bestMachines[0].machine.program}");
				bestFitness = bestMachines[0].fitness;
			}

			var nextGeneration = new BrainfuckProgram[100];

			for (var i = 0; i < 100; i++) {
				int parent = i / 10;
				if (i % 10 == 0) {
					nextGeneration[i] = bestMachines[parent].machine.clone();
				}
				else {
					nextGeneration[i] = getMutant(bestMachines[parent].machine);
				}
			}

			genNumber++;
			currentGen = new Generation(nextGeneration);
		}

		private BrainfuckProgram getMutant(BrainfuckProgram parent) {
			return new BrainfuckProgram(parent.mutate());
		}
	}
}