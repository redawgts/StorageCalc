﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace StorageCalc
{
    public interface IMessageBoxHelper
    {
        void Show(string message);
    }

    public class MessageBoxHelper : IMessageBoxHelper
    {
        public void Show(string message)
        {
            MessageBox.Show(message);
        }
    }
}
