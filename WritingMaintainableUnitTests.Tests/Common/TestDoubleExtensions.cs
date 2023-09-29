using System.Linq;
using NSubstitute;

namespace WritingMaintainableUnitTests.Tests.Common
{
    public static class TestDoubleExtensions
    {
        public static void Should_have_received<TReceiver, TIndirectOutput>(
            this TReceiver receiver,  
            TIndirectOutput indirectOutput) where TReceiver : class
        {
            var dispatchedCall = receiver.ReceivedCalls().SingleOrDefault();
            dispatchedCall.Should_exist();

            if(dispatchedCall == null) 
                return;
            
            var receivedInstance = (TIndirectOutput) dispatchedCall.GetArguments().First();
            receivedInstance.Should_be_deep_equal_to(indirectOutput);
        }
    }
}