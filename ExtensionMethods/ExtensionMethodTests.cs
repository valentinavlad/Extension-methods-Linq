using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
//using System.Linq;
using Xunit;

namespace ExtensionMethods
{
    public class ExtensionMethodTests
    {
        [Fact]
        public void Aggregate()
        {
            int[] ints = { 4, 8, 8, 3, 9, 0, 7, 8, 2 };

            // Count the even numbers in the array, using a seed value of 0.
            int numEven = ints.Aggregate(0, (total, next) =>
                                                next % 2 == 0 ? total + 1 : total);

            Assert.Equal(6, numEven);
        }

        [Fact]
        public void All()
        {
            int[] array = { 10, 20, 30 };
            bool a = array.All(element => element >= 10);
            bool b = array.All(element => element >= 20);
            bool c = array.All(element => element < 40);

            Assert.True(a);
            Assert.True(c);
            Assert.False(b);
        }

        [Fact]
        public void Any()
        {
            int[] array = { 1, 2, 3 };

            bool a = array.Any(item => item % 2 == 0);
            bool b = array.Any(item => item > 3);
            bool c = array.Any(item => item == 2);

            Assert.True(a);
            Assert.True(c);
            Assert.False(b);
        }

        [Fact]
        public void Distinct()
        {
            int[] array1 = { 1, 2, 2, 3, 4, 4 };
            //1 2 3 4

            var result = array1.Distinct(EqualityComparer<int>.Default);
            int[] s = { 1, 2, 3, 4 };
            Assert.Equal(s, result);
        }

        [Fact]
        public void Except()
        {
            // Contains four values.
            int[] values1 = { 1, 2, 3, 4 };

            // Contains three values (1 and 2 also found in values1).
            int[] values2 = { 1, 2, 5 };

            // Remove all values2 from values1.
            var query = values1.Except(values2, EqualityComparer<int>.Default);
            int[] result = { 3, 4 };
            Assert.Equal(result, query);
        }

        [Fact]
        public void Except2()
        {
            // Contains four values.
            int[] values1 = { 1, 2, 3, 3, 4 };

            // Contains three values (1 and 2 also found in values1).
            int[] values2 = { 1, 2, 2, 5 };

            // Remove all values2 from values1.
            var query = values1.Except(values2, EqualityComparer<int>.Default);
            int[] result = { 3, 3, 4 };
            Assert.Equal(result, query);
        }

        [Fact]
        public void First()
        {
            int[] numbers = { 5, 9 };
            int first = numbers.First(number => number > 8);

            Assert.Equal(9, first);
        }

        [Fact]
        public void GroupBy()
        {
            string[] source = { "abc", "hello", "def", "there", "four" };

            var groups = source.GroupBy(x => x.Length,
                                        x => x[0],
                                        (key, values) => key + ":" + string.Join(";", values),
                                        EqualityComparer<int>.Default);
            string[] result = { "3:a;d", "5:h;t", "4:f" };
            Assert.Equal(result, groups);
        }

        [Fact]
        public void Intersect()
        {
            List<int> dataSource1 = new List<int>() { 1, 2, 3, 4, 5, 6 };
            List<int> dataSource2 = new List<int>() { 1, 3, 5, 8, 9, 10 };
            var query = dataSource1.Intersect(dataSource2, EqualityComparer<int>.Default);

        }

        [Fact]
        public void Join()
        {
            var ints1 = new int[3];
            ints1[0] = 4;
            ints1[1] = 3;
            ints1[2] = 0;

            var ints2 = new int[3];
            ints2[0] = 5;
            ints2[1] = 4;
            ints2[2] = 2;

            var result = ints1.Join<int, int, int, int>(ints2,
                x => x + 1,
                y => y,
                (x, y) => x);

            Assert.Equal(4, result.First(e => e == 4));
        }

        [Fact]
        public void JoinDifferentSourceTypes()
        {
            int[] outer = { 5, 3, 7 };
            string[] inner = { "bee", "giraffe", "tiger", "badger", "ox", "cat", "dog" };
            var query = from x in outer
                        join y in inner on x equals y.Length
                        select x + ":" + y;

            string[] s = { "5:tiger", "3:bee", "3:cat", "3:dog", "7:giraffe" };
            Assert.Equal(s, query);
        }

        [Fact]
        public void OrderBy()
        {
            List<int> intList = new List<int>() { 10, 45, 35, 29 };

            // Order alphabetically by reversed strings.
            var result = intList.OrderBy(num => num, Comparer<int>.Default);

            int[] output = { 10, 29, 35, 45 };
            Assert.Equal(result, output);
        }

        [Fact]
        public void OrderBy2()
        {
            var source = new[]
              {
                    new { Value = 1, Key = 10 },
                    new { Value = 2, Key = 11 },
                    new { Value = 3, Key = 11 },
                    new { Value = 4, Key = 10 },
                };
            var query = source.OrderBy(x => x.Key, Comparer<int>.Default).Select(x => x.Value);
            int[] result = { 1, 4, 2, 3 };
            Assert.Equal(result, query);
        }

        [Fact]
        public void Select()
        {
            string[] array = { "cat", "dog", "mouse" };
            var result = array.Select(element => element.ToUpper());
            string s = result.First(e => e == "CAT");

            Assert.Equal("CAT", s);
        }

        [Fact]
        public void SelectMany()
        {
            string[] array = { "cat", "dog", "mouse" };
            var result = array.SelectMany(element => element.ToCharArray());
            char s = result.First(e => e == 'c');

            Assert.Equal('c', s);
        }

        [Fact]
        public void ToDictionary()
        {
            int[] values = new int[] { 1, 3, 5, 7 };
            Dictionary<int, bool> dictionary =
                values.ToDictionary(v => v, v => true);

            Assert.True(dictionary.ContainsKey(1));
        }

        [Fact]
        public void Union()
        {
            List<int> dataSource1 = new List<int>() { 1, 2, 3, 4, 5, 6 };
            List<int> dataSource2 = new List<int>() { 1, 3, 5, 8, 9, 10 };
            var query = dataSource1.Union(dataSource2, EqualityComparer<int>.Default);
            int[] result = { 1, 2, 3, 4, 5, 6, 8, 9, 10 };
            Assert.Equal(result, query);
        }

        [Fact]
        public void Where()
        {
            var fruits =
            new List<string> { "apple", "passionfruit", "banana", "mango",
                    "orange", "blueberry", "grape", "strawberry" };

            IEnumerable<string> query = fruits.Where(fruit => fruit.Length < 6);
            Assert.Equal("apple", query.First(e => e == "apple"));
        }

        [Fact]
        public void Zip()
        {
            var array1 = new int[] { 1, 2, 3, 4, 5, 9 };
            var array2 = new int[] { 6, 7, 8, 9, 10 };

            var zip = array1.Zip(array2, (a, b) => (a + b));
            Assert.Equal(7, zip.First(e => e == 7));
        }

        [Fact]
        public void ThenBy()
        {
            string[] fruits = { "grape", "passionfruit", "banana", "mango",
                      "orange", "raspberry", "apple", "blueberry" };

            var query =
                fruits.OrderBy(fruit => fruit.Length, Comparer<int>.Default).ThenBy(fruit => fruit, StringComparer.Ordinal);

            Assert.Equal(new[] { "apple", "grape", "mango", "banana", "orange", "blueberry", "raspberry", "passionfruit" }, query);
        }

        [Fact]
        public void ThenByOnString()
        {
           
            string[] names = new string[] { "Andrei", "andrei", "Ana", "ana" };
         
            var sortedArray = names.OrderBy(i => i.Length, Comparer<int>.Default)
                                     .ThenBy(i => i, StringComparer.Ordinal);
  
            Assert.Equal(new[] { "Ana", "ana", "Andrei", "andrei" }, sortedArray);
        }
    }
}
