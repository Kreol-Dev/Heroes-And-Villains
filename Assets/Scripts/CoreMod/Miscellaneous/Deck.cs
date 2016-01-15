using UnityEngine;
using System.Collections;

public class Deck<T>
{
	public T[] values { get; internal set; }

	System.Random random;

	public Deck (System.Random random, params T[] values)
	{
		this.values = values;
		this.random = random;
	}

	public void Shuffle ()
	{  
		int n = values.Length;  
		while (n > 1) {  
			n--;  
			int k = random.Next (n + 1);  
			T value = values [k];  
			values [k] = values [n];  
			values [n] = value;  
		}  
	}
}

