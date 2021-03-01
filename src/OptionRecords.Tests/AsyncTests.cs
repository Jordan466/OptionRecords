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
    } 
}