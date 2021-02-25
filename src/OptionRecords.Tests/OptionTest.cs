using System;
using Xunit;
using OptionRecords;

namespace OptionRecords.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void SomeTest()
        {
            Assert.Equal(new Some<int>(42), Option.Some(42));
        }

        [Fact]
        public void NoneTest()
        {
            Assert.Equal(new None<int>(), Option.None<int>());
        }

        [Fact]
        public void EqualityTest()
        {
            Assert.True(new None<int>() == new None<int>());
            Assert.True(new None<int>() != new Some<int>(42));
            Assert.True(new Some<int>(42) == new Some<int>(42));
            Assert.True(new Some<int>(42) != new Some<int>(99));
        }

        [Fact]
        public void BindTest()
        {
            Option<int> TryParse(string input)
            {
                int v = 0;
                if (Int32.TryParse(input, out v))
                    return new Some<int>(v);
                return new None<int>();
            }

            Assert.Equal(new None<int>(), Option.Bind(new None<string>(), TryParse));
            Assert.Equal(new Some<int>(42), Option.Bind(new Some<string>("42"), TryParse));
            Assert.Equal(new None<int>(), Option.Bind(new Some<string>("Forty-two"), TryParse));
        }

        [Fact]
        public void ContainsTest()
        {
            Assert.False(Option.Contains(new None<int>(), 99));
            Assert.True(Option.Contains(new Some<int>(99), 99));
            Assert.False(Option.Contains(new Some<int>(100), 99));
        }

        [Fact]
        public void CountTest()
        {
            Assert.Equal(0, Option.Count(new None<int>()));
            Assert.Equal(1, Option.Count(new Some<int>(99)));
        }

        [Fact]
        public void DefaultValueTest()
        {
            Assert.Equal(99, Option.DefaultValue(new None<int>(), 99));
            Assert.Equal(42, Option.DefaultValue(new Some<int>(42), 99));
        }

        [Fact]
        public void DefaultWithTest()
        {
            Assert.Equal(99, Option.DefaultWith(new None<int>(), () => 99));
            Assert.Equal(42, Option.DefaultWith(new Some<int>(42), () => 99));
        }

        [Fact]
        public void ExistsTest()
        {
            Assert.False(Option.Exists(new None<int>(), x => x >= 5));
            Assert.True(Option.Exists(new Some<int>(42), x => x >= 5));
            Assert.False(Option.Exists(new Some<int>(4), x => x >= 5));
        }

        [Fact]
        public void FilterTest()
        {
            Assert.Equal(new None<int>(), Option.Filter(new None<int>(), x => x >= 5));
            Assert.Equal(new Some<int>(42), Option.Filter(new Some<int>(42), x => x >= 5));
            Assert.Equal(new None<int>(), Option.Filter(new Some<int>(4), x => x >= 5));
        }

        [Fact]
        public void FlattenTest()
        {
            Assert.Equal(new None<int>(), Option.Flatten(new None<Option<int>>()));
            Assert.Equal(new None<int>(), Option.Flatten(new Some<Option<int>>(new None<int>())));
            Assert.Equal(new Some<int>(42), Option.Flatten(new Some<Option<int>>(new Some<int>(42))));
        }

        [Fact]
        public void FoldTest()
        {
            Assert.Equal(0, Option.Fold(new None<int>(), (accum, x) => accum + x * 2, 0));
            Assert.Equal(2, Option.Fold(new Some<int>(1), (accum, x) => accum + x * 2, 0));
            Assert.Equal(12, Option.Fold(new Some<int>(1), (accum, x) => accum + x * 2, 10));
        }

        [Fact]
        public void FoldBackTest()
        {
            Assert.Equal(0, Option.Fold(new None<int>(), (accum, x) => accum + x * 2, 0));
            Assert.Equal(2, Option.Fold(new Some<int>(1), (accum, x) => accum + x * 2, 0));
            Assert.Equal(12, Option.Fold(new Some<int>(1), (accum, x) => accum + x * 2, 10));
        }

        [Fact]
        public void ForAllTest()
        {
            Assert.True(Option.ForAll(new None<int>(), x => x >= 5));
            Assert.True(Option.ForAll(new Some<int>(42), x => x >= 5));
            Assert.False(Option.ForAll(new Some<int>(4), x => x >= 5));
        }

        [Fact]
        public void GetTest()
        {
            Assert.Equal(42, Option.Get(new Some<int>(42)));
            Assert.Throws<ArgumentException>(() => Option.Get(new None<int>()));
            Assert.Equal(42, Option.Get(new Some<int>(42), () => new Exception()));
            Assert.Throws<Exception>(() => Option.Get(new None<int>(), () => new Exception()));
        }

        [Fact]
        public void IsNoneTest()
        {
            Assert.True(Option.IsNone(new None<int>()));
            Assert.False(Option.IsNone(new Some<int>(42)));
        }

        [Fact]
        public void IsSomeTest()
        {
            Assert.False(Option.IsSome(new None<int>()));
            Assert.True(Option.IsSome(new Some<int>(42)));
        }

        [Fact]
        public void IterTest()
        {
            Option.Iter(new None<int>(), i => throw new Exception());
            Option.Iter(new Some<int>(42), i => Assert.Equal(42, i));
        }

        [Fact]
        public void MapTest()
        {
            Assert.Equal(new None<int>(), Option.Map(new None<int>(), x => x * 2));
            Assert.Equal(new Some<int>(84), Option.Map(new Some<int>(42), x => x * 2));
        }

        [Fact]
        public void Map2Test()
        {
            Assert.Equal(new None<int>(), Option.Map2(new None<int>(), new None<int>(), (x, y) => x + y));
            Assert.Equal(new None<int>(), Option.Map2(new Some<int>(5), new None<int>(), (x, y) => x + y));
            Assert.Equal(new None<int>(), Option.Map2(new None<int>(), new Some<int>(10), (x, y) => x + y));
            Assert.Equal(new Some<int>(15), Option.Map2(new Some<int>(5), new Some<int>(10), (x, y) => x + y));
        }

        [Fact]
        public void Map3Test()
        {
            Assert.Equal(new None<int>(), Option.Map3(new None<int>(), new None<int>(), new None<int>(), (x, y, z) => x + y + z));
            Assert.Equal(new None<int>(), Option.Map3(new Some<int>(100), new None<int>(), new None<int>(), (x, y, z) => x + y + z));
            Assert.Equal(new None<int>(), Option.Map3(new None<int>(), new Some<int>(100), new None<int>(), (x, y, z) => x + y + z));
            Assert.Equal(new None<int>(), Option.Map3(new None<int>(), new None<int>(), new Some<int>(100), (x, y, z) => x + y + z));
            Assert.Equal(new Some<int>(115), Option.Map3(new Some<int>(5), new Some<int>(100), new Some<int>(10), (x, y, z) => x + y + z));
        }

        [Fact]
        public void ofNullableTest()
        {
            Assert.Equal(new None<int>(), Option.ofNullable(new Nullable<int>()));
            Assert.Equal(new Some<int>(42), Option.ofNullable(new Nullable<int>(42)));
        }

        [Fact]
        public void OfObjTest()
        {
            Assert.Equal(new None<string>(), Option.OfObj<string>(null));
            Assert.Equal(new Some<string>("not a null string"), Option.OfObj("not a null string"));
        }

        [Fact]
        public void OrElseTest()
        {
            Assert.Equal(new None<int>(), Option.OrElse(new None<int>(), new None<int>()));
            Assert.Equal(new Some<int>(99), Option.OrElse(new None<int>(), new Some<int>(99)));
            Assert.Equal(new Some<int>(42), Option.OrElse(new Some<int>(42), new None<int>()));
            Assert.Equal(new Some<int>(42), Option.OrElse(new Some<int>(42), new Some<int>(99)));
        }

        [Fact]
        public void OrElseWithTest()
        {
            Assert.Equal(new None<int>(), Option.OrElseWith(new None<int>(), () => new None<int>()));
            Assert.Equal(new Some<int>(99), Option.OrElseWith(new None<int>(), () => new Some<int>(99)));
            Assert.Equal(new Some<int>(42), Option.OrElseWith(new Some<int>(42), () => new None<int>()));
            Assert.Equal(new Some<int>(42), Option.OrElseWith(new Some<int>(42), () => new Some<int>(99)));
        }

        [Fact]
        public void ToArrayTest()
        {
            Assert.Empty(Option.ToArray(new None<int>()));
            var arr = Option.ToArray(new Some<int>(42));
            Assert.Single(arr);
            Assert.Collection(arr, i => Assert.Equal(42, i));
        }

        [Fact]
        public void ToEnumerableTest()
        {
            Assert.Empty(Option.ToEnumerable(new None<int>()));
            var enumerable = Option.ToEnumerable(new Some<int>(42));
            Assert.Single(enumerable);
            Assert.Collection(enumerable, i => Assert.Equal(42, i));
        }

        [Fact]
        public void ToListTest()
        {
            Assert.Empty(Option.ToList(new None<int>()));
            var list = Option.ToList(new Some<int>(42));
            Assert.Single(list);
            Assert.Collection(list, i => Assert.Equal(42, i));
        }

        [Fact]
        public void ToNullableTest()
        {
            Assert.Equal(new Nullable<int>(), Option.ToNullable(new None<int>()));
            Assert.Equal(new Nullable<int>(42), Option.ToNullable(new Some<int>(42)));
        }

        [Fact]
        public void ToObjTest()
        {
            Assert.Null(Option.ToObj(new None<string>()));
            Assert.Equal("not a null string", Option.ToObj(new Some<string>("not a null string")));
        }
    }
}
