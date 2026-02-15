global using static CsharpTags.Core.Types.Prelude;
global using CsharpTags.Core.Types;

using LanguageExt;
using static LanguageExt.Prelude;

using BenchmarkDotNet.Attributes;
using CsharpTags.Core.Interface;
using CsCheck;
using System.Reflection;


namespace CsharpTags.Benchmarks
{
    [RankColumn]
    public class TagBenchmarks
    {
        [Params(100, 1000, 10000, 100000, 1000000, 10000000)]
        public int N;

        HtmlElement root = Div;

        [GlobalSetup]
        public void Setup()
        {
            // var fields = Seq<FieldInfo>(typeof(Core.Types.Prelude).GetFields());
            // fields.Filter(x => x.FieldType == typeof(Tag) && x.IsStatic)
            //     .Map(x => (Tag)x.GetValue(null)!);
            // Gen.OneOfConst()
        }

    }
}
