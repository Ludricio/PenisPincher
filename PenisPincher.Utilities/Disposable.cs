using System;
using System.Collections.Generic;
using System.Text;

namespace PenisPincher.Utilities
{
    public class Disposable : IDisposable
    {
        protected bool Disposed { get; private set; }

        public void Dispose()
        {
            if(Disposed) return;

            Dispose(true);
            GC.SuppressFinalize(this);
            Disposed = true;
        }

        ~Disposable()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
        }
    }
}
