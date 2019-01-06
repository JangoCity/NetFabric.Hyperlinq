﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace NetFabric.Hyperlinq
{
    public static partial class Enumerable
    {
        public static SelectEnumerable<TSource, TResult> Select<TEnumerable, TSource, TResult>(this TEnumerable source, Func<TSource, TResult> selector) where TEnumerable : IEnumerable<TSource> =>
            new SelectEnumerable<TSource, TResult>(source, selector);

        public struct SelectEnumerable<TSource, TResult> : IEnumerable<TResult>
        {
            readonly IEnumerable<TSource> source;
            readonly Func<TSource, TResult> selector;

            public SelectEnumerable(IEnumerable<TSource> source, Func<TSource, TResult> selector)
            {
                this.source = source;
                this.selector = selector;
            }

            public Enumerator GetEnumerator() => new Enumerator(ref this);
            IEnumerator<TResult> IEnumerable<TResult>.GetEnumerator() => new Enumerator(ref this);
            IEnumerator IEnumerable.GetEnumerator() => new Enumerator(ref this);

            public struct Enumerator : IEnumerator<TResult>
            {
                readonly IEnumerator<TSource> enumerator;
                readonly Func<TSource, TResult> selector;

                public Enumerator(ref SelectEnumerable<TSource, TResult> enumerable)
                {
                    enumerator = enumerable.source.GetEnumerator();
                    selector = enumerable.selector;
                }

                public TResult Current => selector(enumerator.Current);
                object IEnumerator.Current => selector(enumerator.Current);

                public bool MoveNext() => enumerator.MoveNext();

                public void Reset() => throw new NotSupportedException();

                public void Dispose() => enumerator.Dispose();
            }
        }
    }
}
