using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISingletonSettings
{
    bool ShouldNotDestroyOnLoad { get; }
}
