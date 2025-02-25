SHA256BloomFilter<int> test = new SHA256BloomFilter<int>(2, 1);
test.AddToBloomFilter(5);
Console.WriteLine(test.GetErrorProneInSet(6));
Console.WriteLine(test.GetErrorProneInSet(5));