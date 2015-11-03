
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LuaTableEntries;

public interface ILuaTabled
{
	void InitFrom(Table table);
	void LoadFrom(Table table);
	Table ToTable();
}




