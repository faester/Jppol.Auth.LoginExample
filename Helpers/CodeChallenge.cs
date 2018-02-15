
using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Jppol.Auth.LoginExample.Helpers
{
    public class CodeChallenge
    {
	public string Method => "S256";
	private readonly string _codeVerifier;
	private readonly string _codeChallenge;

	public string Verifier => _codeChallenge;
	public string Challenge => _codeChallenge;

	public CodeChallenge()
	{
		var codeVerifierInput = new byte[32];
		Array.Copy(Guid.NewGuid().ToByteArray(), 0, codeVerifierInput, 0, 16);
		Array.Copy(Guid.NewGuid().ToByteArray(), 0, codeVerifierInput, 16, 16);
		_codeVerifier = Base64UrlEncode(codeVerifierInput);
		_codeChallenge = Base64UrlEncode(Sha256(ASCII(_codeVerifier)));
	}


	private string Base64UrlEncode(byte[] x)
	{
	       return Convert.ToBase64String(x)
	      .Replace('+', '-')
	      .Replace('/', '_')
	      .Replace("=", "");
	}

	private byte[] Sha256(byte[] x)
	{
		return System.Security.Cryptography.SHA256Managed.Create().ComputeHash(x);
	}

	private byte[] ASCII(string x) 
	{
		return System.Text.Encoding.UTF8.GetBytes(x);
	}
 
    }
}
