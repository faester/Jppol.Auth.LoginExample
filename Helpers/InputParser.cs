using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Jppol.Auth.LoginExample.Helpers
{ 
    class InputParser
    {
	  Regex re = new Regex(@"\<input ([^>])*/>");

	  public IEnumerable<HtmlInput> Parse(string htmlDom)
	  {
		foreach(Match match in re.Matches(htmlDom))
		{
			HtmlInput input = new HtmlInput(match.Value);
			yield return input;
		}
	  }
    }
}
