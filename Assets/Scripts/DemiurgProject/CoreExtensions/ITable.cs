using UnityEngine;
using System.Collections;
namespace Demiurg.Core.Extensions
{
    public interface ITable
    {
        string Name { get; set; }
        object Get (string id);
        object Get (int id);
        void Set (string id, object o);
        void Set (int id, object o);
    }
}
