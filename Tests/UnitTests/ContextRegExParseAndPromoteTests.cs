using System;
using BizTalkComponents.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Winterdom.BizTalk.PipelineTesting;
using ContextProperty = BizTalkComponents.Utils.ContextProperty;

namespace BizTalkComponents.PipelineComponents.ContextRegExParseAndPromote.Tests.UnitTests
{
    [TestClass]
    public class ContextRegExParseAndPromoteTests
    {
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestNonExistingSourceProperty()
        {
            var pipeline = PipelineFactory.CreateEmptyReceivePipeline();

            var component = new ContextRegExParseAndPromote
            {
                PropertyToParse = "http://tempuri.org#Source",
                DestinationProperty = "http://tempuri.org#Destination",
                RegExPattern = "Test"
            };

            pipeline.AddComponent(component, PipelineStage.Decode);

            var message = MessageHelper.Create("<test></test>");
            
            var output = pipeline.Execute(message);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestNoMatchThrow()
        {
            var pipeline = PipelineFactory.CreateEmptyReceivePipeline();

            var component = new ContextRegExParseAndPromote
            {
                PropertyToParse = "http://tempuri.org#Source",
                DestinationProperty = "http://tempuri.org#Destination",
                RegExPattern = "TestPattern",
                ThrowIfNoMatch = true
            };

            pipeline.AddComponent(component, PipelineStage.Decode);

            var message = MessageHelper.Create("<test></test>");
            message.Context.Promote(new ContextProperty("http://tempuri.org#Source"),"Test");


            var output = pipeline.Execute(message);
        }


        [TestMethod]
        public void TestNoMatchNoThrow()
        {
            var pipeline = PipelineFactory.CreateEmptyReceivePipeline();

            var component = new ContextRegExParseAndPromote
            {
                PropertyToParse = "http://tempuri.org#Source",
                DestinationProperty = "http://tempuri.org#Destination",
                RegExPattern = "TestPattern"
            };

            pipeline.AddComponent(component, PipelineStage.Decode);

            var message = MessageHelper.Create("<test></test>");
            var source = new ContextProperty("http://tempuri.org#Source");
            message.Context.Promote(source, "Test");


            var output = pipeline.Execute(message);
            string val;
            var destination = new ContextProperty("http://tempuri.org#Destination");
            Assert.IsFalse(output[0].Context.TryRead(destination, out val));
        }

        [TestMethod]
        public void TestMatch()
        {
            var pipeline = PipelineFactory.CreateEmptyReceivePipeline();

            var component = new ContextRegExParseAndPromote
            {
                PropertyToParse = "http://tempuri.org#Source",
                DestinationProperty = "http://tempuri.org#Destination",
                RegExPattern = @"(?<=application/vnd.vendor.)(.*?)(?=\+)"
            };

            pipeline.AddComponent(component, PipelineStage.Decode);

            var message = MessageHelper.Create("<test></test>");
            var source = new ContextProperty("http://tempuri.org#Source");
            message.Context.Promote(source, "application/vnd.vendor.v1+xml");


            var output = pipeline.Execute(message);
            string val;
            var destination = new ContextProperty("http://tempuri.org#Destination");
            Assert.IsTrue(output[0].Context.TryRead(destination, out val));
            Assert.AreEqual("v1",val);
        }
    }
}
