using System;
using System.ComponentModel;

public class disposed 
{
    // Pointer to an external unmanaged resource.
    private IntPtr handle;
    private Component component = new Component();
    private bool _disposed = false;
    public disposed(IntPtr handle)
    {
        this.handle = handle;
    }

    public disposed() { }

    // Run IDisposable.
    // Do not do this virtual method.
    // The derived class must not be able to override this method.
    public void Dispose()
    {
        Dispose(true);
        // This object will be cleaned up using the Dispose method.
        // So you should call GC.SupressFinalize
        // get this object out of the queue checkout
        // and prevent termination code for this object
        // to run a second time.
        GC.SuppressFinalize(this);
    }

    // This object will be cleaned up using the Dispose method.
    // So you should call GC.SupressFinalize
    // get this object out of the queue checkout
    // and prevent termination code for this object
    // to run a second time.
    protected virtual void Dispose(bool disposing)
    {
        if (!this._disposed)
        {
            // If deletion equals true, delete all managed
            // resources and unmanaged.
            if (disposing)
            {
                component.Dispose();
            }

            CloseHandle(handle);
            handle = IntPtr.Zero;
            _disposed = true;

        }
    }

    // Use interoperability to call the required method
    // to clear the unmanaged resource.
    [System.Runtime.InteropServices.DllImport("Kernel32")]
    private extern static Boolean CloseHandle(IntPtr handle);

    // Use C # destructor syntax for the finalization code.
    // This destructor will only be executed if the Dispose method
    // are not called.
    // He gives his base class the opportunity to finish.
    // Do not provide destructors on types derived from this class.
    ~disposed()
    {
        // Não recriar Eliminar código de limpeza aqui.  
        // Chamando Dispose (false) é o ideal em termos de  
        // legibilidade e facilidade de manutenção. 
        Dispose(false);
    }

}

