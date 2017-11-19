using System;
using Xunit;
using FluentAssertions;
using ChequePrint.Services;

namespace ChequePrint.Tests
{
    public class PrinterTests
    {
        private IPrinter printer = new Printer();

        [Theory]
        [InlineData(0)]
        [InlineData(7)]
        [InlineData(13)]
        [InlineData(19)]
        public void TestLess20Number(decimal number)
        {
            string printedNumber = printer.PrintChequeAmount(number);

            string result = "";
            switch (number)
            {
                case 0:
                    result = "ZERO DOLLARS";
                    break;
                case 7:
                    result = "SEVEN DOLLARS";
                    break;
                case 13:
                    result = "THIRTEEN DOLLARS";
                    break;
                case 19:
                    result = "NINETEEN DOLLARS";
                    break;
                default:
                    break;
            }

            printedNumber.Should().Be(result);
        }

        [Theory]
        [InlineData(20)]
        [InlineData(47)]
        [InlineData(59)]
        [InlineData(91)]
        public void TestTenNumber(decimal number)
        {
            string printedNumber = printer.PrintChequeAmount(number);

            string result = "";
            switch (number)
            {
                case 20:
                    result = "TWENTY DOLLARS";
                    break;
                case 47:
                    result = "FORTY-SEVEN DOLLARS";
                    break;
                case 59:
                    result = "FIFTY-NINE DOLLARS";
                    break;
                case 91:
                    result = "NINETY-ONE DOLLARS";
                    break;
                default:
                    break;
            }

            printedNumber.Should().Be(result);
        }

        [Theory]
        [InlineData(100)]
        [InlineData(307)]
        [InlineData(782)]
        [InlineData(880)]
        public void TestHundredNumber(decimal number)
        {
            string printedNumber = printer.PrintChequeAmount(number);

            string result = "";
            switch (number)
            {
                case 100:
                    result = "ONE HUNDRED DOLLARS";
                    break;
                case 307:
                    result = "THREE HUNDRED AND SEVEN DOLLARS";
                    break;
                case 782:
                    result = "SEVEN HUNDRED AND EIGHTY-TWO DOLLARS";
                    break;
                case 880:
                    result = "EIGHT HUNDRED AND EIGHTY DOLLARS";
                    break;
                default:
                    break;
            }

            printedNumber.Should().Be(result);
        }

        [Theory]
        [InlineData(2000)]
        [InlineData(3005)]
        [InlineData(3087)]
        [InlineData(5921)]
        [InlineData(7703)]
        public void TestThousandNumber(decimal number)
        {
            string printedNumber = printer.PrintChequeAmount(number);

            string result = "";
            switch (number)
            {
                case 2000:
                    result = "TWO THOUSAND DOLLARS";
                    break;
                case 3005:
                    result = "THREE THOUSAND FIVE DOLLARS";
                    break;
                case 3087:
                    result = "THREE THOUSAND EIGHTY-SEVEN DOLLARS";
                    break;
                case 5921:
                    result = "FIVE THOUSAND NINE HUNDRED AND TWENTY-ONE DOLLARS";
                    break;
                case 7703:
                    result = "SEVEN THOUSAND SEVEN HUNDRED AND THREE DOLLARS";
                    break;
                default:
                    break;
            }

            printedNumber.Should().Be(result);
        }

        [Theory]
        [InlineData(12000)]
        [InlineData(73494)]
        [InlineData(647000)]
        [InlineData(100300)]
        [InlineData(9038293)]
        [InlineData(10000000)]
        [InlineData(703403403)]
        [InlineData(49810928910)]
        [InlineData(999999999999)]
        public void TestWholeNumber(decimal number)
        {
            string printedNumber = printer.PrintChequeAmount(number);

            string result = "";
            switch (number)
            {
                case 12000:
                    result = "TWELVE THOUSAND DOLLARS";
                    break;
                case 73494:
                    result = "SEVENTY-THREE THOUSAND FOUR HUNDRED AND NINETY-FOUR DOLLARS";
                    break;
                case 647000:
                    result = "SIX HUNDRED AND FORTY-SEVEN THOUSAND DOLLARS";
                    break;
                case 100300:
                    result = "ONE HUNDRED THOUSAND THREE HUNDRED DOLLARS";
                    break;
                case 9038293:
                    result = "NINE MILLION THIRTY-EIGHT THOUSAND TWO HUNDRED AND NINETY-THREE DOLLARS";
                    break;
                case 10000000:
                    result = "TEN MILLION DOLLARS";
                    break;
                case 703403403:
                    result = "SEVEN HUNDRED AND THREE MILLION FOUR HUNDRED AND THREE THOUSAND FOUR HUNDRED AND THREE DOLLARS";
                    break;
                case 49810928910:
                    result = "FORTY-NINE BILLION EIGHT HUNDRED AND TEN MILLION NINE HUNDRED AND TWENTY-EIGHT THOUSAND NINE HUNDRED AND TEN DOLLARS";
                    break;
                case 999999999999:
                    result = "NINE HUNDRED AND NINETY-NINE BILLION NINE HUNDRED AND NINETY-NINE MILLION NINE HUNDRED AND NINETY-NINE THOUSAND NINE HUNDRED AND NINETY-NINE DOLLARS";
                    break;
                default:
                    break;
            }

            printedNumber.Should().Be(result);
        }

        [Theory]
        [InlineData(0.99)]
        [InlineData(20.04)]
        [InlineData(83748392.103)]
        [InlineData(54344.00)]
        public void TestAmountWithCents(decimal amount)
        {
            string printAmount = printer.PrintChequeAmount(amount);

            string result = "";
            switch (amount)
            {
                case 0.99m:
                    result = "ZERO DOLLARS AND NINETY-NINE CENTS";
                    break;
                case 20.04m:
                    result = "TWENTY DOLLARS AND FOUR CENTS";
                    break;
                case 83748392.103m:
                    result = "EIGHTY-THREE MILLION SEVEN HUNDRED AND FORTY-EIGHT THOUSAND THREE HUNDRED AND NINETY-TWO DOLLARS AND TEN CENTS";
                    break;
                case 54344.00m:
                    result = "FIFTY-FOUR THOUSAND THREE HUNDRED AND FORTY-FOUR DOLLARS";
                    break;
                default:
                    break;
            }

            printAmount.Should().Be(result);
        }

        [Theory]
        [InlineData(1000000000000)]
        [InlineData(78374283472834)]
        public void TestNumberTooBig(decimal number)
        { 
            printer.Invoking(p => p.PrintChequeAmount(number))
            .ShouldThrow<ApplicationException>()
            .WithMessage("Only amout up to maximum 999,999,999,999.99 is allowed");
        }
    }
}
