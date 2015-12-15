using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace Demiurg.Core.Extensions
{
    public interface ITable
    {
        string Name { get; set; }

        IEnumerable<object> GetKeys ();

        object Get (object id);

        void Set (object id, object o);
    }
}
