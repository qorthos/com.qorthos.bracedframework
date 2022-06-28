﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Events;

namespace BracedFramework
{
    public interface IModalPanel : IShowable
    {
        UnityEvent OnFinished { get; }

        void CustomSet(object parameters);
    }
}