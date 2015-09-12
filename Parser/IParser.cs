using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser
{
  public interface IParser<T, TN>
    where T : class 
    where TN : class
  {
    IEnumerable<T> Parse(TN input); 
  }
}
