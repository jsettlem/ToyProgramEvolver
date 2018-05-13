using System;
using Evolution.AbstractMachines.Brainfuck;

namespace Evolution {
	public struct ProgramWithFitness {
		public BrainfuckProgram machine;
		public int fitness;

		public ProgramWithFitness(BrainfuckProgram machine) {
			this.machine = machine;
			this.fitness = Int32.MaxValue;
		}
	}
}