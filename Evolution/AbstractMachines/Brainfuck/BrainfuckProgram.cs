using System;
using System.Collections.Generic;

namespace Evolution.AbstractMachines.Brainfuck {
	public class BrainfuckProgram : IAbstractProgram {
		
		public static char[] usefulInstructions = new char[] {'>', '<', '+', '-', '.', '[', ']'};
		
		public bool Done => pc >= imem.Count || pc < 0;
		public int[] InitialMemory { get; set; }
		public string output => buffer;
		public string program => new string(imem.ToArray());

		private string buffer;
		private List<char> imem;
		private byte[] tape = new byte[30000];
		private int pc = 0;
		private int dp = 0;

		public BrainfuckProgram(IEnumerable<char> instructions, int[] initialMemory = null) {
			this.imem = new List<char>(instructions);
			InitialMemory = initialMemory ?? new int[0];
			for (var i = 0; i < InitialMemory.Length && i < tape.Length; i++) {
				tape[i] = (byte) InitialMemory[i];
			}

			buffer = "";
		}

		public void Execute() {
			var inst = imem[pc];
			switch (inst) {
				case '>':
					dp++;
					if (dp > tape.Length - 1) {
						dp = 0;
					} 
					break;
				case '<':
					dp--;
					if (dp < 0) {
						dp = tape.Length - 1;
					}
					break;
				case '+':
					tape[dp]++;
					if (dp < InitialMemory.Length) {
						InitialMemory[dp] = tape[dp];
					}
					break;
				case '-':
					tape[dp]--;
					if (dp < InitialMemory.Length) {
						InitialMemory[dp] = tape[dp];
					}
					break;
				case '.':
					buffer += (char) tape[dp];
					break;
				case ',':
					throw new NotImplementedException(",");
					break;
				case '[':
					if (tape[dp] == 0) {
						var lcount = 0;
						pc++;
						while (pc < imem.Count && (lcount != 0 || imem[pc] != ']')) {
							if (imem[pc] == '[') {
								lcount += 1;
							} else if (imem[pc] == ']') {
								lcount -= 1;
							}
							pc++;
						}
					}
					break;
				case ']':
					if (tape[dp] != 0) {
						var rcount = 0;
						pc--;
						while (pc >= 0 && (rcount != 0 || imem[pc] != '[')) {
							if (imem[pc] == ']') {
								rcount += 1;
							}
							else if (imem[pc] == '[') {
								rcount -= 1;
							}
							pc--;
						}

						if (pc < 0) {
							pc = Int32.MinValue;
						}
					}
					break;
			}
			pc++;
		}

		public char[] mutate() {
			var choice = Program.rnd.Next(1,4);
			int index;
			switch (choice) {
				case 1:
					//replace instruction
					if (imem.Count > 0) {
						index = Program.rnd.Next(0, imem.Count - 1);
						imem[index] = GetRandomOperator();
					}
					break;
				case 2:
					//delete instruction
					if (imem.Count > 0) {
						index = Program.rnd.Next(0, imem.Count - 1);
						imem.RemoveAt(index);
					}
					break;
				case 3:
					//add instruction
					if (imem.Count > 0) {
						index = Program.rnd.Next(0, imem.Count - 1);
						imem.Insert(index, GetRandomOperator());
					}
					break;
			}

			return imem.ToArray();
		}

		public BrainfuckProgram clone() {
			return new BrainfuckProgram(imem);
		}

		static char GetRandomOperator() {
			var choice = Program.rnd.Next(0, 7);
			return usefulInstructions[choice];
		}

		public static char[] RandomProgram(int length) {
			var program = new char[length];
			for (var i = 0; i < length; i++) {
				program[i] = GetRandomOperator();
			}

			return program;
		}
	}
}