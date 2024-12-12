from django.shortcuts import get_object_or_404, render, redirect
from .forms import *
from .models import *
from django.contrib.auth import login
from django.http import JsonResponse
from django.contrib import messages
from django.contrib.auth.decorators import login_required
from .context_processors import total_carrito
from xhtml2pdf import pisa
from bs4 import BeautifulSoup
from django.template.loader import get_template
from io import BytesIO
from django.http import HttpResponse, HttpResponseRedirect
from PyPDF2 import PdfWriter, PdfReader
from reportlab.pdfgen import canvas
from reportlab.lib.pagesizes import letter
# imports API
from django.contrib.auth.models import User
from django.contrib.auth.hashers import make_password
from django.contrib.auth import authenticate
from django.views.generic import ListView
from django.db.models import Sum
# Create your views here.

def sugerencias_producto(request):
    query = request.GET.get('term', '')
    productos = Producto.objects.filter(nombre_producto__icontains=query)[:10]
    resultados = list(productos.values('id_producto', 'nombre_producto'))
    return JsonResponse(resultados, safe=False)

def a(request):

    return render(request, 'app/404.html')

def index(request):
    productos = Producto.objects.all()

    ofertas = Ofertas.objects.filter(
        fecha_inicio__lte=datetime.now().date(),
        fecha_fin__gte=datetime.now().date()
    )

    # Usamos lote.producto.id_producto para acceder al id_producto
    ofertas_dict = {oferta.lote.producto.id_producto: oferta.descuento.descuento for oferta in ofertas}

    productos_lotes = []
    for prod in productos:
        lote = Lotes.objects.filter(producto=prod).order_by('fecha_entrada').first()
        
        if lote:
            prod.lote = lote
            prod.fecha_entrada_lote = lote.fecha_entrada
        else:
            prod.fecha_entrada_lote = None  # Si no tiene lote, asignar None

        # Obtener el descuento para el producto
        prod.descuento = ofertas_dict.get(prod.id_producto, None)
        if prod.descuento:
            nuevo_precio = prod.precio_unitario * (1 - (prod.descuento / 100))
            prod.nuevo_precio = round(nuevo_precio)
        else:
            prod.nuevo_precio = prod.precio_unitario

        # Obtener el stock disponible
        inventario = Inventario.objects.filter(producto=prod).aggregate(total_stock=models.Sum('cantidad_disponible'))
        prod.stock = inventario.get('total_stock', 0)

        productos_lotes.append(prod)

    productos_lotes.sort(key=lambda p: p.fecha_entrada_lote or datetime.max)

    datos = {
        'productos': productos_lotes
    }
    return render(request, 'app/index.html', datos)




def registro(request): 
    if request.method == 'POST':
        form = RegisterForm(request.POST)
        if form.is_valid():
            user = form.save()
            login(request, user)
            return redirect('index')
    else:
        form = RegisterForm()
    return render(request,'registration/registro.html', {'form': form})

@login_required
def perfil(request):
    if request.user.is_authenticated:
        ordenes = Orden.objects.filter(cliente=request.user)

        ordenamiento = request.GET.get('ordenamiento', 'recientes')
        
        if ordenamiento == 'antiguos':
            ordenes = ordenes.order_by('boleta__fecha')
        else:
            ordenes = ordenes.order_by('-boleta__fecha') 

        historial_compras = []

        for orden in ordenes:
            boleta = orden.boleta 
            historial_compras.append({
                'orden': orden,
                'boleta': boleta,
            })

        datos = {
            'historial_compras': historial_compras,
            'ordenamiento': ordenamiento
        }
    else:
        datos = {} 
    return render(request, 'app/perfil.html', datos)

def render_to_pdf(html):
    result = BytesIO()
    pdf = pisa.pisaDocument(BytesIO(html.encode('UTF-8')), result)
    if not pdf.err:
        return result.getvalue()
    return None

def add_watermark(input_pdf, watermark_text):
    packet = BytesIO()
    can = canvas.Canvas(packet, pagesize=letter)
    can.setFont("Helvetica", 100)
    can.setFillColorRGB(255.9, 0.9, 0.9, alpha=0.8)  # Light gray color
    can.saveState()
    can.translate(300, 500)
    can.rotate(45)
    can.drawCentredString(0, 0, watermark_text)
    can.restoreState()
    can.save()

    packet.seek(0)
    watermark = PdfReader(packet)
    existing_pdf = PdfReader(BytesIO(input_pdf))
    output = PdfWriter()

    for i in range(len(existing_pdf.pages)):
        page = existing_pdf.pages[i]
        page.merge_page(watermark.pages[0])
        output.add_page(page)

    output_stream = BytesIO()
    output.write(output_stream)
    return output_stream.getvalue()


@login_required
def detalle_boleta(request, id_boleta):
    boleta = get_object_or_404(Boleta, id_boleta=id_boleta)
    productos_boleta = ProductoBoleta.objects.filter(boleta=boleta)
    orden = Orden.objects.filter(boleta=boleta)

    # Obtener el estado "Anulada" y "Rechazado"
    estado_anulada = EstadoBoleta.objects.get(estado_boleta="Anulada")
    estado_rechazado = EstadoEntrega.objects.get(estado_entrega="Rechazado")

    datos = {
        'boleta': boleta,
        'productos_boleta': productos_boleta,
        'orden': orden,
    }

    if request.method == 'POST':

        if boleta.estado_boleta.estado_boleta.lower() == "entregado":
            return HttpResponse("No se puede cancelar una boleta que ya ha sido entregada.", status=403)

        boleta.estado_boleta = estado_anulada
        boleta.save()

        for ord in orden:
            ord.estado_entrega = estado_rechazado
            ord.save()

        return redirect('detalle_boleta', id_boleta=id_boleta)

    if request.GET.get('download') == 'pdf':
        template = get_template('app/detalle_boleta.html')
        html = template.render(datos)

        soup = BeautifulSoup(html, 'html.parser')
        pdf_content = soup.find(class_='pdf-content')
        html = str(pdf_content) if pdf_content else html

        pdf = render_to_pdf(html)
        if pdf:
            if boleta.estado_boleta.estado_boleta == "Anulada":
                pdf = add_watermark(pdf, "ANULADA")

            response = HttpResponse(pdf, content_type='application/pdf')
            response['Content-Disposition'] = f'attachment; filename="detalle_boleta_{id_boleta}.pdf"'
            return response

        return HttpResponse('Tuvimos algunos errores <pre>' + html + '</pre>')

    return render(request, 'app/detalle_boleta.html', datos)

def productos(request):
    productos = Producto.objects.all()

    categorias = Categoria.objects.all()

    categoria_selec = request.GET.get('categoria')
    if categoria_selec:
        productos = productos.filter(categoria__nombre_categoria=categoria_selec)

    ofertas = Ofertas.objects.filter(
        fecha_inicio__lte=datetime.now().date(),
        fecha_fin__gte=datetime.now().date()
    )

    ofertas_dict = {oferta.lote.producto.id_producto: oferta.descuento.descuento for oferta in ofertas}
    
    productos_lotes = []
    for prod in productos:
        lote = Lotes.objects.filter(producto=prod).order_by('fecha_entrada').first()
        
        if lote:
            prod.lote = lote
            prod.fecha_entrada_lote = lote.fecha_entrada
        else:
            prod.fecha_entrada_lote = None  # Si no tiene lote, asignar None

        # Obtener el descuento para el producto
        prod.descuento = ofertas_dict.get(prod.id_producto, None)
        if prod.descuento:
            nuevo_precio = prod.precio_unitario * (1 - (prod.descuento / 100))
            prod.nuevo_precio = round(nuevo_precio)
        else:
            prod.nuevo_precio = prod.precio_unitario

        # Obtener el stock disponible
        inventario = Inventario.objects.filter(producto=prod).aggregate(total_stock=models.Sum('cantidad_disponible'))
        prod.stock = inventario.get('total_stock', 0)

        productos_lotes.append(prod)

    productos_lotes.sort(key=lambda p: p.fecha_entrada_lote or datetime.max)

    datos = {
        'productos': productos_lotes,
        'categorias': categorias
    }
    return render(request, 'app/productos.html', datos)


def detalle_prod(request, id_producto):
    producto = Producto.objects.get(id_producto=id_producto)

    lotes = Lotes.objects.filter(producto=producto).order_by('fecha_entrada')

    lote_antiguo = None
    fecha_vencimiento = None
    if lotes.exists():
        lote_antiguo = lotes.first()
        fecha_vencimiento = lote_antiguo.fecha_vencimiento

    oferta = Ofertas.objects.filter(
        lote=lote_antiguo,
        fecha_inicio__lte=datetime.now().date(),
        fecha_fin__gte=datetime.now().date()
    ).first()

    precio_con_descuento = producto.precio_unitario
    if oferta:
        politica_descuento = PoliticaDescuentos.objects.get(id=oferta.descuento_id)
        descuento = politica_descuento.descuento
        precio_con_descuento = producto.precio_unitario - (producto.precio_unitario * descuento / 100)
        precio_con_descuento = round(precio_con_descuento)

    inventario = Inventario.objects.filter(producto=producto).aggregate(total_stock=models.Sum('cantidad_disponible'))
    stock = inventario.get('total_stock', 0)

    datos = {
        'producto': [producto],
        'precio_con_descuento': precio_con_descuento,
        'fecha_vencimiento': fecha_vencimiento,
        'lote_antiguo': lote_antiguo,
        'stock': stock
    }

    return render(request, 'app/detalle_prod.html', datos)
@login_required
def carro(request):
    if request.user.is_authenticated:
        # Obtén los productos del carrito
        carrito_productos = CarroProducto.objects.filter(user=request.user)
        productos_con_stock = []

        for item in carrito_productos:
            producto = item.producto
            
            # Sumar el stock total de todos los lotes del producto
            inventario = Inventario.objects.filter(producto=producto).aggregate(
                total_stock=Sum('cantidad_disponible')
            )
            cantidad_disponible = inventario.get('total_stock') or 0
            
            productos_con_stock.append({
                'producto': producto,
                'cantidad': item.cantidad,
                'precio': producto.precio_unitario,
                'stock_disponible': cantidad_disponible
            })

        if request.method == 'POST':
            if carrito_productos.exists():
                # Calcular totales
                context = total_carrito(request)
                subtotal = context['subtotal']
                iva = context['iva']
                total = context['total']
                descuentos = context['descuento_total']

                # Crear la boleta
                estado_boleta = EstadoBoleta.objects.get(estado_boleta='Creada')

                boleta = Boleta.objects.create(
                    subtotal=subtotal,
                    iva=iva,
                    total=total,
                    descuentos=descuentos,
                    estado_boleta=estado_boleta  
                )

                # Asociar productos del carrito a la boleta
                for prod in carrito_productos:
                    ProductoBoleta.objects.create(
                        cantidad=prod.cantidad,
                        precio_unitario=prod.producto.precio_unitario,
                        precio_total=prod.producto.precio_unitario * prod.cantidad,
                        boleta=boleta,
                        id_producto=prod.producto
                    )

                # Redirigir después de finalizar compra
                return redirect('finalizar_compra', id_boleta=boleta.id_boleta)
            else:
                return redirect('carro')
    else:
        carrito_productos = []

    # Datos para la plantilla
    datos = {
        'carrito': carrito_productos,
        'stock': productos_con_stock
    }
    return render(request, 'app/carro.html', datos)


@login_required
def finalizar_compra(request, id_boleta):
    boleta = Boleta.objects.get(id_boleta=id_boleta)
    if request.method == 'POST':
        form = OrdenForm(request.POST)
        estado_entrega = EstadoEntrega.objects.get(estado_entrega='Por entregar')
        metodo_pago_nombre = request.POST.get('metodo_pago')
        metodo_pago = MetodoPago.objects.get(nombre_metodo=metodo_pago_nombre)
        if form.is_valid():
            orden = form.save(commit=False)
            orden.boleta = boleta
            orden.cliente = request.user
            orden.estado_entrega = estado_entrega
            orden.metodo_pago = metodo_pago
            orden.save()

            productos_boleta = ProductoBoleta.objects.filter(boleta=boleta)
            for producto_boleta in productos_boleta:
                producto = producto_boleta.id_producto
                
                # Obtener la cantidad disponible en el inventario
                inventario = Inventario.objects.filter(producto=producto).aggregate(total_stock=models.Sum('cantidad_disponible'))
                cantidad_disponible = inventario.get('total_stock', 0)
                
                if producto_boleta.cantidad > cantidad_disponible:
                    messages.error(request, f"No hay suficiente stock de {producto.nombre_producto}.")
                    return redirect('finalizar_compra', id_boleta=boleta.id_boleta)
                
                # Actualizar el stock en el inventario
                inventarios = Inventario.objects.filter(producto=producto).order_by('id_inventario')
                cantidad_a_reducir = producto_boleta.cantidad
                for inv in inventarios:
                    if cantidad_a_reducir <= 0:
                        break
                    if inv.cantidad_disponible >= cantidad_a_reducir:
                        inv.cantidad_disponible -= cantidad_a_reducir
                        inv.save()
                        cantidad_a_reducir = 0
                    else:
                        cantidad_a_reducir -= inv.cantidad_disponible
                        inv.cantidad_disponible = 0
                        inv.save()

            carrito = Carro(request)
            carrito.limpiar()

            return redirect('compra_exito', id_orden=orden.id_orden)

    else:
        form = OrdenForm()

    datos = {
        'boleta': boleta,
        'form': form
    }
    return render(request, 'app/finalizar_compra.html', datos)

@login_required
def compra_exito(request, id_orden):
    orden = Orden.objects.get(id_orden=id_orden)
    boleta = orden.boleta
    datos = {
        'boleta': boleta,
        'orden': orden
    }
    return render(request, 'app/compra_exito.html', datos)
    
def agregar_prod(request, id_producto):
    carrito = Carro(request)
    producto = Producto.objects.get(id_producto=id_producto)
    carrito.agregar_prod(producto)
    return HttpResponseRedirect(request.META.get('HTTP_REFERER', '/'))

def eliminar(request, id_producto):
    carrito = Carro(request)
    producto = Producto.objects.get(id_producto=id_producto)
    carrito.eliminar(producto)
    return redirect(to="carro")

def restar(request, id_producto):
    if request.user.is_authenticated:
        carrito = Carro(request)
        producto = Producto.objects.get(id_producto=id_producto)
        if carrito.carrito.get(str(producto.id_producto), {}).get("cant", 0) < 1:
            carrito.restar(producto)
    return redirect(to="carro")

def sumar(request, id_producto):
    if request.user.is_authenticated:
        carrito = Carro(request)
        producto = Producto.objects.get(id_producto=id_producto)
        
        # Obtener la cantidad disponible en el inventario sumando todos los lotes
        inventario = Inventario.objects.filter(producto=producto).aggregate(total_stock=models.Sum('cantidad_disponible'))
        cantidad_disponible = inventario.get('total_stock', 0)
        
        # Obtener la cantidad actual en el carrito
        cantidad_en_carrito = carrito.carrito.get(str(producto.id_producto), {}).get("cantidad", 0)
        
        # Verificar si la cantidad en el carrito más uno es menor o igual que la cantidad disponible en el inventario
        if cantidad_en_carrito + 1 <= cantidad_disponible:
            carrito.sumar(producto)
        else:
            messages.warning(request, f"No hay suficiente stock de {producto.nombre_producto}.")
    return redirect(to="carro")

def limpiar(request):
    carrito = Carro(request)
    carrito.limpiar()
    return redirect(to="carro")

def render_to_pdf(html):
    result = BytesIO()
    pdf = pisa.pisaDocument(BytesIO(html.encode('UTF-8')), result)
    if not pdf.err:
        return result.getvalue()
    return None

# API
def mapa_view(request):
    tienda = Tienda.objects.first()
    direccion_tienda = tienda.direccion if tienda else "No disponible"
    comuna_tienda = tienda.comuna.nombre_comuna if tienda and tienda.comuna else "No disponible"

    datos = {
        'direccion_tienda' : direccion_tienda,
        'comuna_tienda' : comuna_tienda
    }
    return render(request, 'app/mapa.html', datos)

