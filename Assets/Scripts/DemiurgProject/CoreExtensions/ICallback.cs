using UnityEngine;
using System.Collections;

namespace Demiurg.Core.Extensions
{
    public interface ICallback
    {
        object Call (params object[] args);
    }

}