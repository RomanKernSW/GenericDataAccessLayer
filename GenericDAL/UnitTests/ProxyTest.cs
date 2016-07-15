﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Reflection;
using NProxy.Core;
using GenericDataAccessLayer.LazyDal;

namespace UnitTests
{
    [TestClass()]
    public class ProxyTest
    {
        private int counts = int.MaxValue / 64;
        public class Proxy : IInterceptor
        {
            public void Intercept(IInvocation invocation)
            {
            }
        }

        public class NProxy : IInvocationHandler
        {
            /// <summary>Processes an invocation on a target.</summary>
            /// <param name="target">The target object.</param>
            /// <param name="methodInfo">The method information.</param>
            /// <param name="parameters">The parameter values.</param>
            /// <returns>The return value.</returns>
            public object Invoke(object target, MethodInfo methodInfo, object[] parameters)
            {
                return null;
            }
        }

        [TestMethod]
        public void MyTestMethod()
        {
            var w = Stopwatch.StartNew();
            var generator = new Castle.DynamicProxy.ProxyGenerator();
            var proxy = generator.CreateInterfaceProxyWithoutTarget<Repository.StoredProcedure.ExecutionTest>(new Proxy());
            w.Stop();
            Console.WriteLine(w.Elapsed);
            w.Reset();
            int counter = counts;
            w.Start();
            while (true)
            {
                proxy.UpdateSomeEntity(1, "sds");
                if (counter-- == 0)
                {
                    break;
                }
            }
            w.Stop();
            Console.WriteLine(w.Elapsed);
        }

        [TestMethod]
        public void MyTestMethod2()
        {
            var w = Stopwatch.StartNew();
            var proxy = new ProxyFactory().CreateProxy<Repository.StoredProcedure.ExecutionTest>(Type.EmptyTypes, new NProxy());
            w.Stop();
            Console.WriteLine(w.Elapsed);
            w.Reset();
            int counter = counts;
            w.Start();
            while (true)
            {
                proxy.UpdateSomeEntity(1, "sds");
                if (counter-- == 0)
                {
                    break;
                }
            }
            w.Stop();
            Console.WriteLine(w.Elapsed);
        }
    }
}
