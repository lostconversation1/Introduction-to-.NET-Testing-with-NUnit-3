using Loans.Domain.Applications;
using NUnit.Framework;
using System;

namespace Loans.Tests
{
    [TestFixture]
    public class LoanTermShould
    {
        [Test]
        public void ReturnTermInMonths()
        {
            var sut = new LoanTerm(1);
            Assert.That(sut.ToMonths(),Is.EqualTo(12));
        }
    }
}
