using Loans.Domain.Applications;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Loans.Tests
{
    [TestFixture]
    public class LoanApplicationProcessorShould
    {
        [Test]
        public void DeclineLowSalary()
        {
            LoanProduct product = new LoanProduct(99, "Loan", 5.25m);
            LoanAmount amount = new LoanAmount("USD", 200_000);
            var application = new LoanApplication(42, product, amount, "Sarah", 25, "133 some city,some drive", 64_999);

            var mockIdentityVerifier = new Mock<IIdentityVerifier>();
            var mockCreditScore = new Mock<ICreditScorer>();

            var sut = new LoanApplicationProcessor(mockIdentityVerifier.Object, mockCreditScore.Object);

            sut.Process(application);

            Assert.That(application.GetIsAccepted(), Is.False);
        }

        delegate void ValidateCallback(string applicantName,
            int applicantAge,
            string applicantAddress,
            ref IdentityVerificationStatus status);

        [Test]
        public void Accept()
        {
            LoanProduct product = new LoanProduct(99, "Loan", 5.25m);
            LoanAmount amount = new LoanAmount("USD", 200_000);
            var application = new LoanApplication(42, product, amount, "Sarah", 25, "133 some city,some drive", 65_000);

            var mockIdentityVerifier = new Mock<IIdentityVerifier>();

            ////- parameters must be the same
            ////+ it' s more realiable, when sth brokes in identityverifier
            mockIdentityVerifier.Setup(x => x.Validate("Sarah",
                        25,
                        "133 some city,some drive"))
                .Returns(true);
            ////- when sth brokes in identityverifier, this will not show this
            ////mockIdentityVerifier.Setup(x => x.Validate(It.IsAny<string>(),
            ////           It.IsAny<int>(),
            ////           It.IsAny<string>()))
            ////   .Returns(true);

            //out param in method
            //bool isValidOutValue = true;
            //mockIdentityVerifier.Setup(x => x.Validate("Sarah",
            //          25,
            //          "133 some city,some drive",
            //          out isValidOutValue));


            //ref param in method
            //mockIdentityVerifier
            //    .Setup(x => x.Validate("Sarah",
            //          25,
            //          "133 some city,some drive",
            //          ref It.Ref<IdentityVerificationStatus>.IsAny))
            //    .Callback(new ValidateCallback(
            //          (string applicantName,
            //          int applicantAge,
            //          string applicantAddress,
            //          ref IdentityVerificationStatus status)=>
            //                 status = new IdentityVerificationStatus(true)));



            var mockCreditScore = new Mock<ICreditScorer>();
            mockCreditScore.Setup(x => x.Score).Returns(300);

            var sut = new LoanApplicationProcessor(mockIdentityVerifier.Object, mockCreditScore.Object);

            sut.Process(application);

            Assert.That(application.GetIsAccepted(), Is.True);
        }
    }
}
