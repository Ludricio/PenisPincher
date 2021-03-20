using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PenisPincher.Utilities
{
    public abstract class AsyncDisposable : Disposable, IAsyncDisposable
    {
        protected async virtual ValueTask DisposableAsyncCore()
        {
        }

        public async ValueTask DisposeAsync()
        {
            await DisposableAsyncCore();

            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
