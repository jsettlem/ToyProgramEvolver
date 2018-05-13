using System;
using System.Linq;
using Evolution.AbstractMachines.Brainfuck;

namespace Evolution {
	internal class Program {
		

		public static Random rnd = new Random();
		
		public static void Main(string[] args) {
			var runner = new Runner();
			while (true) {
				runner.runGeneration();
			}
		}
	}
}