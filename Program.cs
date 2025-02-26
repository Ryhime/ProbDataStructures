IProbMembership<int> test = new BloomFilter<int>(2, 100);
test.AddToSet(5);
Console.WriteLine(test.ObjectInSet(6));
Console.WriteLine(test.ObjectInSet(5));