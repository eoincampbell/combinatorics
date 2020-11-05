using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Order;
using BenchmarkDotNet.Running;

namespace Benchmarks
{
    public class PermutationBenchmarks
    {
        private List<int> _source;
        private List<int> _largeSource;
        private List<int> _veryLargeSource;
        private BigInteger _veryLargeSourceCount;

        [GlobalSetup]
        public void GlobalSetup()
        {
            _source = Enumerable.Range(0, 9).ToList();
            _largeSource = Enumerable.Range(0, 19).ToList();
            _veryLargeSource = Enumerable.Range(0, 21).ToList();
            _veryLargeSourceCount = BigInteger.Parse("51090942171709440000");
        }

        [BenchmarkCategory("Enumerate"), Benchmark(Baseline = true)]
        public void EnumerateOld()
        {
            var permutations = new Combinatorics.Collections.Permutations<int>(_source);
            foreach (var p in permutations)
                ;
        }

        [BenchmarkCategory("Enumerate"), Benchmark]
        public void EnumerateNew()
        {
            var permutations = new Nito.Combinatorics.Permutations<int>(_source);
            foreach (var p in permutations)
                ;
        }

        [BenchmarkCategory("Count0"), Benchmark(Baseline = true)]
        public void Count0Old()
        {
            var permutations = new Combinatorics.Collections.Permutations<int>(_source);
            if (permutations.Count != 362880)
                throw new InvalidOperationException($"Expected 362880 but got {permutations.Count}.");
        }

        [BenchmarkCategory("Count0"), Benchmark]
        public void Count0New()
        {
            var permutations = new Nito.Combinatorics.Permutations<int>(_source);
            if (permutations.Count != 362880)
                throw new InvalidOperationException($"Expected 362880 but got {permutations.Count}.");
        }

        [BenchmarkCategory("Count1"), Benchmark(Baseline = true)]
        public void Count1Old()
        {
            var permutations = new Combinatorics.Collections.Permutations<int>(_largeSource);
            if (permutations.Count != 121645100408832000)
                throw new InvalidOperationException($"Expected 121645100408832000 but got {permutations.Count}.");
        }

        [BenchmarkCategory("Count1"), Benchmark]
        public void Count1New()
        {
            var permutations = new Nito.Combinatorics.Permutations<int>(_largeSource);
            if (permutations.Count != 121645100408832000)
                throw new InvalidOperationException($"Expected 121645100408832000 but got {permutations.Count}.");
        }

        [BenchmarkCategory("Count2"), Benchmark(Baseline = true)]
        public void Count2Old()
        {
            var permutations = new Combinatorics.Collections.Permutations<int>(_veryLargeSource);
            if (permutations.Count != _veryLargeSourceCount)
                throw new InvalidOperationException($"Expected 51090942171709440000 but got {permutations.Count}.");
        }

        [BenchmarkCategory("Count2"), Benchmark]
        public void Count2New()
        {
            var permutations = new Nito.Combinatorics.Permutations<int>(_veryLargeSource);
            if (permutations.Count != _veryLargeSourceCount)
                throw new InvalidOperationException($"Expected 51090942171709440000 but got {permutations.Count}.");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run(typeof(Program).Assembly,
                DefaultConfig.Instance
                    .WithOptions(ConfigOptions.JoinSummary | ConfigOptions.DisableOptimizationsValidator)
                    .AddDiagnoser(MemoryDiagnoser.Default)
                    .WithOrderer(new DefaultOrderer(SummaryOrderPolicy.Method))
                    .AddLogicalGroupRules(BenchmarkLogicalGroupRule.ByCategory)
                );
        }
    }
}
