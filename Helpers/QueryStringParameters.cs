using System;
using System.Linq;
using System.Net;
using System.Collections.Generic;

namespace Jppol.Auth.LoginExample.Helpers
{
    public class QueryStringParameters 
    {
	private List<Tuple<string, string>> _parameters = new List<Tuple<string,string>>();

	public void Add(string key, string value) 
	{
		_parameters.Add(Tuple.Create(key, value));
	}

	public string GetQueryString()
	{
		return string.Join("&", _parameters
				.Select(p => $"{p.Item1}={WebUtility.UrlEncode(p.Item2)}"));
	}
    }
}
