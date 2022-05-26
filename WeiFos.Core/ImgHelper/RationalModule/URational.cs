﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeiFos.Core.ImgHelper.RationalModule
{
    internal sealed class URational
    {
        private UInt32 _num;
        private UInt32 _denom;

        public URational(byte[] bytes)
        {
            byte[] n = new byte[4];
            byte[] d = new byte[4];
            Array.Copy(bytes, 0, n, 0, 4);
            Array.Copy(bytes, 4, d, 0, 4);
            _num = BitConverter.ToUInt32(n, 0);
            _denom = BitConverter.ToUInt32(d, 0);
        }

        public double ToDouble()
        {
            return Math.Round(Convert.ToDouble(_num) / Convert.ToDouble(_denom), 2);
        }

        public override string ToString()
        {
            return this.ToString("/");
        }

        public string ToString(string separator)
        {
            return _num.ToString() + separator + _denom.ToString();
        }
    }
}
