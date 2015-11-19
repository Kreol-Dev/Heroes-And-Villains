using UnityEngine;
using System.Collections;
namespace Demiurg.Core.Extensions
{
    public interface ITable
    {
        object Get (string id);
        object Get (int id);

    }
}
