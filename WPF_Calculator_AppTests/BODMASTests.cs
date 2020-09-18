using NUnit.Framework;
using WPF_Calculator_App;
using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography.X509Certificates;

namespace WPF_Calculator_App.Tests
{
    [TestFixture]
    public class BODMASTests
    {
        [TestCase("2+5/2-4", 0.5)]
        [TestCase("5/2", 2.5)]
        [TestCase("5+2", 7.0)]
        [TestCase("5-2", 3.0)]
        [TestCase("5*2", 10.0)]
        [TestCase("212+52/4*22-78", 420.0)]
        [TestCase("1234+5256*8568/854521", 1286.7)]
        [TestCase("1+2+3+4", 10.0)]
        [TestCase("1+2/4+3", 4.5)]
        [TestCase("-1+3", 2)]
        [TestCase("5+-3", 2)]
        [TestCase("5+-4/3", 3.7)]
        [TestCase("((1-3)+5)", 3)]
        [TestCase("(2*(1-3))", -4)]
        [TestCase("(0-5)+5/2*(6-3)", 2.5)]
        public void CalculateTest(string calc, double expected)
        {
            Calculator bodmas = new Calculator(calc);
            bodmas.Calculate();
            Assert.AreEqual(expected, Math.Round(bodmas.Result, 1));
        }
    }
}