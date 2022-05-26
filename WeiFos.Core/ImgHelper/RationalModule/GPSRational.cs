using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeiFos.Core.ImgHelper.RationalModule
{

    internal sealed class GPSRational
    {
        private Rational _hours;
        private Rational _minutes;
        private Rational _seconds;

        public Rational Hours
        {
            get
            {
                return _hours;
            }
            set
            {
                _hours = value;
            }
        }
        public Rational Minutes
        {
            get
            {
                return _minutes;
            }
            set
            {
                _minutes = value;
            }
        }
        public Rational Seconds
        {
            get
            {
                return _seconds;
            }
            set
            {
                _seconds = value;
            }
        }

        public GPSRational(byte[] bytes)
        {
            byte[] h = new byte[8]; byte[] m = new byte[8]; byte[] s = new byte[8];

            Array.Copy(bytes, 0, h, 0, 8); Array.Copy(bytes, 8, m, 0, 8); Array.Copy(bytes, 16, s, 0, 8);

            _hours = new Rational(h);
            _minutes = new Rational(m);
            _seconds = new Rational(s);
        }

        public override string ToString()
        {
            return _hours.ToDouble() + "° "
                + _minutes.ToDouble() + "\' "
                + _seconds.ToDouble() + "\"";
        }

        public string ToString(string separator)
        {
            return _hours.ToDouble() + separator
                + _minutes.ToDouble() + separator +
                _seconds.ToDouble();
        }
    }

}
