using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public abstract class AbstractCompositeFlockBehavior : AbstractFlockBehavior
{
    public abstract WeightedBehavior[] Behaviors { get;}
}