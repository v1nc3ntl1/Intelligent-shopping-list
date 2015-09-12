using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser
{
  public class RssParser<T, TN> : IParser<T, TN>
    where T : class
    where TN : class
  {
    private IParser<T, TN> _parser;

    public RssParser() { }

    public RssParser(IParser<T, TN> parser)
    {
      _parser = parser;
    }

    public IEnumerable<T> Parse(TN input)
    {
      //Do parsing
      if (_parser != null)
      {
        var result = _parser.Parse(input);
        return result;
      }
      return null;
    }
  }
}
