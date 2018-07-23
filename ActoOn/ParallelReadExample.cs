using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActonTranslation
{
    public class ParallelReadExamples
    {
        public void ParaRead_1()
        {
            var inputLines = new BlockingCollection<string>();
            ConcurrentDictionary<int, int> catalog = new ConcurrentDictionary<int, int>();

            var readLines = Task.Factory.StartNew(() =>
            {
                foreach (var line in File.ReadLines("file.txt"))
                    inputLines.Add(line);

                inputLines.CompleteAdding();
            });

            var processLines = Task.Factory.StartNew(() =>
            {
                Parallel.ForEach(inputLines.GetConsumingEnumerable(), line =>
                {
                    string[] lineFields = line.Split('\t');
                    int genomicId = int.Parse(lineFields[3]);
                    int taxId = int.Parse(lineFields[0]);
                    catalog.TryAdd(genomicId, taxId);
                });
            });

            Task.WaitAll(readLines, processLines);
        }

        public void ParaRead_2()
        {
            Parallel.ForEach(File.ReadLines("file.txt"), (line, _, lineNumber) =>
            {
                // your code here
            });

            Parallel.ForEach(File.ReadLines("file.txt"), line =>
            {

            });
        }

        public void ParaRead_3()
        {
            string filename = "C:\\TEST\\TEST.DATA";
            int n = 5;

            foreach (var line in File.ReadLines(filename).AsParallel().WithDegreeOfParallelism(n))
            {
                // Process line.
            }
        }

    }
}