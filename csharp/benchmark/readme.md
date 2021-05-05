BenchmarkDotNet=v0.10.14, OS=Windows 8.1 (6.3.9600.0)  
Intel Core i7-4790 CPU 3.60GHz (Haswell), 1 CPU, 8 logical and 4 physical cores  
Frequency=3507497 Hz, Resolution=285.1036 ns, Timer=TSC  
  [Host]     : .NET Framework 4.6.1 (CLR 4.0.30319.42000), 64bit RyuJIT-v4.7.3190.0  
  DefaultJob : .NET Framework 4.6.1 (CLR 4.0.30319.42000), 64bit RyuJIT-v4.7.3190.0


|               Method |       N |  M | Seed |     Mean |     Error |    StdDev |
|--------------------- |-------- |--- |----- |---------:|----------:|----------:|
|             Smallest | 1000000 | 20 |   42 | 6.721 ms | 0.0324 ms | 0.0287 ms |
|      SmallestIndices | 1000000 | 20 |   42 | 6.762 ms | 0.0123 ms | 0.0103 ms |
|              Largest | 1000000 | 20 |   42 | 6.243 ms | 0.0191 ms | 0.0169 ms |
|        SmallestArray | 1000000 | 20 |   42 | 6.748 ms | 0.0137 ms | 0.0129 ms |
| SmallestIndicesArray | 1000000 | 20 |   42 | 6.707 ms | 0.0170 ms | 0.0150 ms |
|         LargestArray | 1000000 | 20 |   42 | 6.674 ms | 0.0186 ms | 0.0174 ms |
|        SmallestIList | 1000000 | 20 |   42 | 8.918 ms | 0.0292 ms | 0.0273 ms |
| SmallestIndicesIList | 1000000 | 20 |   42 | 9.746 ms | 0.0293 ms | 0.0244 ms |
|         LargestIList | 1000000 | 20 |   42 | 8.929 ms | 0.0252 ms | 0.0235 ms |


BenchmarkDotNet=v0.10.14, OS=Windows 8.1 (6.3.9600.0)  
Intel Core i7-4790 CPU 3.60GHz (Haswell), 1 CPU, 8 logical and 4 physical cores  
Frequency=3507497 Hz, Resolution=285.1036 ns, Timer=TSC  
.NET Core SDK=2.1.4  
  [Host]     : .NET Core 2.0.5 (CoreCLR 4.6.26020.03, CoreFX 4.6.26018.01), 64bit RyuJIT  
  DefaultJob : .NET Core 2.0.5 (CoreCLR 4.6.26020.03, CoreFX 4.6.26018.01), 64bit RyuJIT


|               Method |       N |  M | Seed |     Mean |     Error |    StdDev |
|--------------------- |-------- |--- |----- |---------:|----------:|----------:|
|             Smallest | 1000000 | 20 |   42 | 5.965 ms | 0.0232 ms | 0.0206 ms |
|      SmallestIndices | 1000000 | 20 |   42 | 5.935 ms | 0.0215 ms | 0.0190 ms |
|              Largest | 1000000 | 20 |   42 | 5.932 ms | 0.0129 ms | 0.0114 ms |
|        SmallestArray | 1000000 | 20 |   42 | 5.685 ms | 0.0122 ms | 0.0108 ms |
| SmallestIndicesArray | 1000000 | 20 |   42 | 5.947 ms | 0.0228 ms | 0.0213 ms |
|         LargestArray | 1000000 | 20 |   42 | 5.951 ms | 0.0238 ms | 0.0223 ms |
|        SmallestIList | 1000000 | 20 |   42 | 8.394 ms | 0.0303 ms | 0.0269 ms |
| SmallestIndicesIList | 1000000 | 20 |   42 | 8.655 ms | 0.0313 ms | 0.0292 ms |
|         LargestIList | 1000000 | 20 |   42 | 8.402 ms | 0.0309 ms | 0.0289 ms |