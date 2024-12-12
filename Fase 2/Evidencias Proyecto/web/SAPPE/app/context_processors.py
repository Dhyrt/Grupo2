from .models import *

def barra_busqueda(request):
    search_query = request.GET.get('search', '')
    search_results = Producto.objects.filter(nombre_producto__icontains=search_query) if search_query else []
    return {
        'search_query': search_query,
        'search_results': search_results,
    }

def total_carrito(request):
    subtotal = 0
    iva = 0
    total = 0
    descuento_total = 0
    total_desc = 0
    IVA_PORCENTAJE = 0.19

    if request.user.is_authenticated:
        # Obtiene los productos del carrito de la base de datos
        carrito_productos = CarroProducto.objects.filter(user=request.user)

        for carro_producto in carrito_productos:
            precio_normal = carro_producto.producto.precio_unitario
            cantidad = carro_producto.cantidad
            
            # Obtener el descuento asociado al producto, si existe
            oferta = Ofertas.objects.filter(lote__producto=carro_producto.producto).first()
            # Si existe una oferta, aplica el descuento
            if oferta:
                # Asumimos que el descuento es un porcentaje
                politica_descuento = PoliticaDescuentos.objects.get(id_descuento=oferta.descuento_id)
                descuento = politica_descuento.descuento  # Obtener el porcentaje de descuento
                precio_descuento = precio_normal * (1 - descuento / 100)  # Aplica el descuento al precio normal
            else:
                precio_descuento = precio_normal
            
            # Calcular el ahorro total por producto
            ahorro_por_producto = round((precio_normal - precio_descuento) * cantidad)
            descuento_total += ahorro_por_producto  # Sumar al total de descuentos
            
            # Calcular el subtotal (precio_descuento * cantidad)
            subtotal += round(precio_descuento * cantidad)  # Usar el precio de descuento para el subtotal

        # Total después de aplicar los descuentos
        total_desc = subtotal  # El subtotal ya tiene en cuenta los precios con descuento

        # Calcular el IVA y el total con IVA
        iva = round(total_desc * IVA_PORCENTAJE)
        total = round(total_desc + iva)

    return {
        "subtotal": subtotal,
        "iva": iva,
        "total": total,
        "descuento_total": descuento_total,
        "total_desc": total_desc,
    }
def total_productos_carrito(request):
    carrito = Carro(request)
    total_tipos_productos = carrito.contar_productos()
    return {'total_productos_carrito': total_tipos_productos}
