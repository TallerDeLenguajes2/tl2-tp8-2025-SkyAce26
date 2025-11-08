namespace Repositories;

using Models;

public class PresupuestoDetalle{
    private Producto _producto;
    private int _cantidad;
    public Producto Producto
    {
        get { return _producto; }
        set{ _producto = value; }
    }

    public int Cantidad
    {
        get { return _cantidad; }
        set { _cantidad = value; }
    }

    public PresupuestoDetalle(){}
    public PresupuestoDetalle(int cantidad, Producto producto)
    {
        this._producto = producto;
        this._cantidad = cantidad;
    }
}