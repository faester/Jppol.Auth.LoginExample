using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Generic;
using System;

namespace Jppol.Auth.LoginExample.Helpers
{ 
    public class HtmlInput 
    {
	private static Regex re = new Regex("\\w+=\"[^\"]*\"");
	public string Name { get; set; }
	public string Id { get; set; }
	public string Value { get; set; }
	public string Class { get; set; }

	public HtmlInput() { }

	public HtmlInput(string htmlRepresentation) 
	{
		var match = re.Matches(htmlRepresentation);
		if (match == null || match.Count == 0)
		{
			throw new ArgumentException($"Could not parse as html input: {htmlRepresentation}");
		}
		foreach(Match m in match)
		{
			var components = m.Value.Split("=", 2);
			switch(components[0])
			{
				case "name":
					Name = UnQuote(components[1]);
					break;
				case "id":
					Id = UnQuote(components[1]);
					break;
				case "class":
					Class = UnQuote(components[1]);
					break;
				case "value":
					Value = UnQuote(components[1]);
					break;
				default: 
					break;
			}
		}
	}

	private string UnQuote(string input)
	{
		return input.Length > 1
			&& ((input.StartsWith("'") && input.EndsWith("'"))
			||(input.StartsWith("\"") && input.EndsWith("\"")))
			? input.Substring(1, input.Length - 2) : input;
	}
		
	public override string ToString()
	{
		return $"INPUT Name = {Name}, Id = {Id}, Value = {Value}, Class = {Class}";
	}
    }
}
