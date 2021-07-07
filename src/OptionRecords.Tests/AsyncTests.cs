using System;
using Xunit;
using OptionRecords;
using System.Threading.Tasks;

namespace OptionRecords.Tests
{
    public class AsyncTest
    {
        [Fact]
        public async Task BindTest()
        {
            async Task<Option<int>> TryParse(string input)
            {
                int v = 0;
                if (Int32.TryParse(input, out v))
                    return await Task.FromResult(new Some<int>(v));
                return await Task.FromResult(new None<int>());
            }

            Assert.Equal(new None<int>(), await Option.Bind(new None<string>(), TryParse));
            Assert.Equal(new Some<int>(42), await Option.Bind(new Some<string>("42"), TryParse));
            Assert.Equal(new None<int>(), await Option.Bind(new Some<string>("Forty-two"), TryParse));
        }

        [Fact]
        public async Task MapTest()
        {
            Assert.Equal(new None<int>(), await Option.Map(new None<int>(), async x => await Task.FromResult(x * 2)));
            Assert.Equal(new Some<int>(84), await Option.Map(new Some<int>(42), async x => await Task.FromResult(x * 2)));
        }
    }

    public class AsyncOptionTest
    {
        [Fact]
        public async Task BindTest()
        {
            async Task<Option<int>> TryParse(string input)
            {
                int v = 0;
                if (Int32.TryParse(input, out v))
                    return await Task.FromResult(new Some<int>(v));
                return await Task.FromResult(new None<int>());
            }

            var option1 = Task.FromResult<Option<string>>(new None<string>());
            var option2 = Task.FromResult<Option<string>>(new Some<string>("42"));
            var option3 = Task.FromResult<Option<string>>(new Some<string>("Forty-two"));

            Assert.Equal(new None<int>(), await Option.Bind(option1, TryParse));
            Assert.Equal(new Some<int>(42), await Option.Bind(option2, TryParse));
            Assert.Equal(new None<int>(), await Option.Bind(option3, TryParse));
        }

        [Fact]
        public async Task ContainsTest()
        {
            var option1 = Task.FromResult<Option<int>>(new None<int>());
            var option2 = Task.FromResult<Option<int>>(new Some<int>(99));
            var option3 = Task.FromResult<Option<int>>(new Some<int>(100));

            Assert.False(await Option.Contains(option1, 99));
            Assert.True(await Option.Contains(option2, 99));
            Assert.False(await Option.Contains(option3, 99));
        }

        [Fact]
        public async Task CountTest()
        {
            var option1 = Task.FromResult<Option<int>>(new None<int>());
            var option2 = Task.FromResult<Option<int>>(new Some<int>(99));

            Assert.Equal(0, await Option.Count(option1));
            Assert.Equal(1, await Option.Count(option2));
        }

        [Fact]
        public async Task DefaultValueTest()
        {
            var option1 = Task.FromResult<Option<int>>(new None<int>());
            var option2 = Task.FromResult<Option<int>>(new Some<int>(42));

            Assert.Equal(99, await Option.DefaultValue(option1, 99));
            Assert.Equal(42, await Option.DefaultValue(option2, 99));
        }

        [Fact]
        public async Task DefaultWithTest()
        {
            var option1 = Task.FromResult<Option<int>>(new None<int>());
            var option2 = Task.FromResult<Option<int>>(new Some<int>(42));

            Assert.Equal(99, await Option.DefaultWith(option1, () => 99));
            Assert.Equal(42, await Option.DefaultWith(option2, () => 99));
        }

        [Fact]
        public async Task IterTest()
        {
            var option1 = Task.FromResult<Option<int>>(new None<int>());
            var option2 = Task.FromResult<Option<int>>(new Some<int>(42));

            await Option.Iter(option1, i => throw new Exception());
            await Option.Iter(option2, i => Assert.Equal(42, i));
        }

        [Fact]
        public async Task MapTest()
        {
            var option1 = Task.FromResult<Option<int>>(new None<int>());
            var option2 = Task.FromResult<Option<int>>(new Some<int>(42));


            Assert.Equal(new None<int>(), await Option.Map(option1, async x => await Task.FromResult(x * 2)));
            Assert.Equal(new Some<int>(84), await Option.Map(option2, async x => await Task.FromResult(x * 2)));
        }
    }
}