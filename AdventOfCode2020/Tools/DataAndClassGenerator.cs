using AdventOfCode2020.Solver;
using System.Text;

namespace AdventOfCode2020.Tools
{
    public static class DataAndClassGenerator
    {
        public static bool CreateLevel()
        {
            // Check if fully solved
            if (!AppDomain.CurrentDomain.GetAssemblies()
                 .SelectMany(s => s.GetTypes())
                 .Any(p => typeof(BaseSolver).IsAssignableFrom(p) && !p.IsAbstract && p.Name == "DayXX"))
            {
                return false;
            }

            // Get last day with solver
            int dayToSolve = 1;
            try
            {
                Type lastSolver = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(s => s.GetTypes())
                    .Where(p => typeof(BaseSolver).IsAssignableFrom(p) && !p.IsAbstract && p.Name != "DayXX")
                    .OrderByDescending(p => p.Name)
                    .First();
                dayToSolve = int.Parse(lastSolver.Name[3..]) + 1;
            }
            catch
            {
                return false;
            }

            // Check if higher than 25
            if (dayToSolve > 25)
            {
                return false;
            }

            // Ask user to confirm
            Console.Write($"Create level for Day {dayToSolve:0} ? [y/n]: ");
            if (!(Console.ReadLine() ?? "").Equals("y", StringComparison.InvariantCultureIgnoreCase))
            {
                return false;
            }

            // Ask for title
            Console.WriteLine("");
            Console.WriteLine("");
            Console.Write($"Please input level title: ");
            string title = Console.ReadLine() ?? "";
            if (string.IsNullOrEmpty(title))
            {
                return false;
            }

            // Copy Data DayXX to Data Day{dayToSolve:00}
            string newDataFolder = @$"..\..\..\Data\Day{dayToSolve:00}\";
            string sampleDataFolder = @$"..\..\..\Data\DayXX\";
            if (!Directory.Exists(newDataFolder)) Directory.CreateDirectory(newDataFolder);
            if (Directory.Exists(sampleDataFolder))
            {
                string[] files = Directory.GetFiles(sampleDataFolder);
                foreach (string file in files)
                {
                    File.Copy(file, file.Replace("XX", dayToSolve.ToString("00")), true);
                }
            }

            // Create new solver Day{dayToSolve:00}
            string newSolverFile = @$"..\..\..\Solver\Day{dayToSolve:00}.cs";
            string sampleSolverFile = @$"..\..\..\Solver\DayXX.cs";

            // Read and fix
            List<string> solverFile = [.. File.ReadAllLines(sampleSolverFile)];
            for (int i = 0; i < solverFile.Count; i++)
            {
                if (solverFile[i].Contains("DayXX"))
                {
                    solverFile[i] = solverFile[i].Replace("DayXX", $"Day{dayToSolve:00}");
                }
                if (solverFile[i].Contains("\"XXX\""))
                {
                    solverFile[i] = solverFile[i].Replace("\"XXX\"", $"\"{title}\"");
                }
            }

            // Write
            File.WriteAllLines(newSolverFile, solverFile, Encoding.UTF8);

            // Inform user
            Console.WriteLine("");
            Console.WriteLine("New solver created. Press any key to exit.");
            Console.ReadKey();

            // Done
            return true;
        }
    }
}