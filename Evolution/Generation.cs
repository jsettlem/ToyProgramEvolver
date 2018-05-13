using System;
using System.Linq;
using System.Threading.Tasks;
using Evolution.AbstractMachines.Brainfuck;

namespace Evolution {
	public class Generation {
		private ProgramWithFitness[] members;

		public Generation(BrainfuckProgram[] machines) {
			members = new ProgramWithFitness[machines.Length];
			for (var i = 0; i < machines.Length; i++) {
				members[i] = new ProgramWithFitness(machines[i]);
			}
		}
		
		public void runAll() {
			Parallel.For (0, members.Length, i => {
				var member = members[i];
				var machine = member.machine;

				var dmem = new int[] {0};
				machine.InitialMemory = dmem;

				var cycles = 0;

				while (!machine.Done && cycles < 1000) {
					machine.Execute();
					cycles++;
				}

				var fitness = stringDistance(machine.output, "Jordan");//Math.Abs(dmem[0] - 127);
				if (fitness == 0) {
					fitness = -10000 + machine.program.Length;
				}

				members[i].fitness = fitness;
			});
		}

		public ProgramWithFitness[] getBest(int count) {
			Array.Sort(members, (x, y) => x.fitness.CompareTo(y.fitness));
			return members.Take(count).ToArray();
		}

		private int stringDistance(string s1, string s2) {
			var longstring = s1.Length < s2.Length ? s2 : s1;
			var shortstring = s1.Length < s2.Length ? s1 : s2;
			var distance = 0;
			var i = 0;
			for (; i < shortstring.Length; i++) {
				distance += Math.Abs(longstring[i] - shortstring[i]);
			}

			for (; i < longstring.Length; i++) {
				distance += longstring[i];
			}

			return distance;
		}
	}
}