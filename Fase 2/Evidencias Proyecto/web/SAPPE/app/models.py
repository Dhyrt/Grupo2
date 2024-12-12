# Create your models here.
from django.db import models
from datetime import datetime, date
from django.contrib.auth.models import User
import random, string
from django.db.models.signals import post_migrate
from django.dispatch import receiver
import requests
from django.utils import timezone

class TipoUsuario(models.Model):
    id_tipo_usuario = models.AutoField(primary_key=True)
    tipo_usuario = models.CharField(max_length=50)

    def __str__(self):
        return self.tipo_usuario
    
    class Meta():
        db_table = 'TipoUsuario'

class Region(models.Model):
    id_region = models.AutoField(primary_key=True)
    nombre_region = models.CharField(max_length=50)

    def __str__(self):
        return self.nombre_region
    
    class Meta():
        db_table = 'Region'

class Comunas(models.Model):
    id_comuna = models.AutoField(primary_key=True)
    id_region = models.ForeignKey(Region, on_delete=models.CASCADE)
    nombre_comuna = models.CharField(max_length=50)

    def __str__(self):
        return self.nombre_comuna
    
    class Meta():
        db_table = 'Comunas'

class PerfilUsuario(models.Model):
    id_perfil = models.AutoField(primary_key=True)
    user = models.OneToOneField(User, on_delete=models.CASCADE)
    tipo_usuario = models.ForeignKey(TipoUsuario, on_delete=models.CASCADE)

    def __str__(self):
        return self.user.username
    
    class Meta():
        db_table = 'PerfilUsuario'

class Administrador(models.Model):
    id_usuario = models.AutoField(primary_key=True)
    rut = models.CharField(max_length=12)
    perfil_usuario = models.ForeignKey(PerfilUsuario, on_delete=models.CASCADE)

    def __str__(self):
        return self.nombre_usuario
    
    class Meta():
        db_table = 'Administrador'

class Tienda(models.Model):
    id_tienda = models.AutoField(primary_key=True)
    nombre_tienda = models.CharField(max_length=50)
    frase_footer = models.CharField(max_length=150, null=True)
    direccion = models.CharField(max_length=50)
    comuna = models.ForeignKey(Comunas, on_delete=models.CASCADE)
    logo = models.ImageField(upload_to='tienda', null=True)
    color = models.CharField(max_length=50, null=True)
    color_secundario = models.CharField(max_length=50, null=True)
    img_login = models.ImageField(upload_to='tienda', null=True)
    img_registro = models.ImageField(upload_to='tienda', null=True)

    def __str__(self):
        return self.nombre_tienda
    
    class Meta():
        db_table = 'Tienda'

class Anuncios(models.Model):
    id_anuncio = models.AutoField(primary_key=True)
    titulo = models.CharField(max_length=50)
    descripcion = models.CharField(max_length=500)
    fecha_inicio = models.DateField()
    fecha_fin = models.DateField()
    img = models.ImageField(upload_to='anuncios', null=True)
    tienda = models.ForeignKey(Tienda, on_delete=models.CASCADE)

    def __str__(self):
        return self.titulo
    
    class Meta():
        db_table = 'Anuncios'

class PerecibilidadProducto(models.Model):
    id_perecibilidad = models.AutoField(primary_key=True)
    perecibilidad = models.CharField(max_length=50)

    def __str__(self):
        return self.perecibilidad
    
    class Meta():
        db_table = 'PerecibilidadProducto'
    
class Categoria(models.Model):
    id_categoria = models.AutoField(primary_key=True)
    nombre_categoria = models.CharField(max_length=50)

    def __str__(self):
        return self.nombre_categoria
    
    class Meta():
        db_table = 'Categoria'

class Marca(models.Model):
    id_marca = models.AutoField(primary_key=True)
    nombre_marca = models.CharField(max_length=50)

    def __str__(self):
        return self.nombre_marca
    
    class Meta():
        db_table = 'Marca'

class UnidadMedida(models.Model):
    id_unidad_medida = models.AutoField(primary_key=True)
    unidad_medida = models.CharField(max_length=50)
    peso_gramos = models.IntegerField()

    def __str__(self):
        return self.unidad_medida
    
    class Meta():
        db_table = 'Unidad_Medida'

class Proveedor(models.Model):
    id_proveedor = models.AutoField(primary_key=True)
    nombre_proveedor = models.CharField(max_length=50)
    telefono = models.IntegerField()
    correo = models.CharField(max_length=50)
    direccion = models.CharField(max_length=50)
    comuna = models.ForeignKey(Comunas, on_delete=models.CASCADE)

    def __str__(self):
        return self.nombre_proveedor
    
    class Meta():
        db_table = 'Proveedor'

class PoliticaDescuentos(models.Model):
    id_descuento = models.AutoField(primary_key=True)
    dias_para_vencimiento = models.IntegerField()
    descuento = models.IntegerField()

    def __str__(self):
        return str(self.id_descuento)
    
    class Meta():
        db_table = 'PoliticaDescuentos'
    
class Producto(models.Model):
    id_producto = models.AutoField(primary_key=True)
    nombre_producto = models.CharField(max_length=50)
    precio_unitario = models.IntegerField()
    descripcion = models.CharField(max_length=500)
    img = models.ImageField(upload_to='productos', null=True)
    categoria = models.ForeignKey(Categoria, on_delete=models.CASCADE)
    marca = models.ForeignKey(Marca, on_delete=models.CASCADE)
    perecible = models.ForeignKey(PerecibilidadProducto, on_delete=models.CASCADE)  # Perecible o no perecible
    unidad_medida = models.ForeignKey(UnidadMedida, on_delete=models.CASCADE)
    volumen = models.IntegerField(null=True)

    def __str__(self):
        return self.nombre_producto
    
    class Meta():
        db_table = 'Producto'

class EstadoOferta(models.Model):
    id_estado_oferta = models.AutoField(primary_key=True)
    estado_oferta = models.CharField(max_length=50)

    def __str__(self):
        return self.estado_oferta
    
    class Meta():
        db_table = 'EstadoOferata'

class TipoMovimiento(models.Model):
    id_tipo_movimiento = models.AutoField(primary_key=True)
    tipo_movimiento = models.CharField(max_length=50)

    def __str__(self):
        return self.tipo_movimiento
    
    class Meta():
        db_table = 'Tipo_Movimiento'

class SeccionBodega(models.Model):
    id_seccion = models.AutoField(primary_key=True)
    nombre_seccion = models.CharField(max_length=50)

    def __str__(self):
        return str(self.id_seccion)
    
    class Meta():
        db_table = 'Seccion_Bodega'

class Bodega(models.Model):
    id_bodega = models.AutoField(primary_key=True)
    nombre = models.CharField(max_length=50)
    capacidad_maxima = models.IntegerField()

    def __str__(self):
        return str(self.id_bodega)
    
    class Meta():
        db_table = 'Bodega'

class Lotes(models.Model):
    id_lote = models.CharField(max_length=10, primary_key=True, editable=False)
    cantidad = models.IntegerField()
    fecha_entrada = models.DateField(auto_now_add=True)
    fecha_vencimiento = models.DateField(null=True)
    producto = models.ForeignKey(Producto, on_delete=models.CASCADE)
    proveedor = models.ForeignKey(Proveedor, on_delete=models.CASCADE)
    seccion_bodega = models.ForeignKey(SeccionBodega, on_delete=models.CASCADE)
    bodega = models.ForeignKey(Bodega, on_delete=models.CASCADE)

    def save(self, *args, **kwargs):
        if not self.id_lote:
            self.id_lote = self.generate_unique_id()
        super().save(*args, **kwargs)

    def generate_unique_id(self):
        length = 7
        characters = string.ascii_uppercase + string.digits
        while True:
            unique_id = ''.join(random.choice(characters) for _ in range(length))
            if not Lotes.objects.filter(id_lote=unique_id).exists():
                return unique_id

    def __str__(self):
        return str(self.id_lote)
    
    class Meta():
        db_table = 'Lotes'

class Ofertas(models.Model):
    id_oferta = models.AutoField(primary_key=True)
    fecha_inicio = models.DateField()
    fecha_fin = models.DateField()
    lote = models.ForeignKey(Lotes, on_delete=models.CASCADE)
    descuento = models.ForeignKey(PoliticaDescuentos, on_delete=models.CASCADE)
    estado_oferta = models.ForeignKey(EstadoOferta, on_delete=models.CASCADE)

    def __str__(self):
        return str(self.id_oferta)
    
    class Meta():
        db_table = 'Ofertas'

class Movimiento(models.Model):
    id_movimiento = models.AutoField(primary_key=True)
    cantidad = models.IntegerField()
    fecha = models.DateField(auto_now_add=True)
    tipo_movimiento = models.ForeignKey(TipoMovimiento, on_delete=models.CASCADE)
    lote = models.ForeignKey(Lotes, on_delete=models.CASCADE)
    bodega = models.ForeignKey(Bodega, on_delete=models.CASCADE)

    def __str__(self):
        return str(self.id_movimiento)
    
    class Meta():
        db_table = 'Movimiento_Bodega'

class Inventario(models.Model):
    id_inventario = models.AutoField(primary_key=True)
    producto = models.ForeignKey(Producto, on_delete=models.CASCADE)
    cantidad_disponible = models.IntegerField()
    seccion_bodega = models.ForeignKey(SeccionBodega, on_delete=models.CASCADE)
    bodega = models.ForeignKey(Bodega, on_delete=models.CASCADE)

    def __str__(self):
        return str(self.id_inventario)
    
    class Meta():
        db_table = 'Inventario'

class MetodoPago(models.Model):
    id_metodo_pago = models.AutoField(primary_key=True)
    nombre_metodo = models.CharField(max_length=50)

    def __str__(self):
        return self.nombre_metodo
    
    class Meta():
        db_table = 'MetodoPago'

class CarroProducto(models.Model):
    user = models.ForeignKey(User, on_delete=models.CASCADE)
    producto = models.ForeignKey(Producto, on_delete=models.CASCADE)
    cantidad = models.PositiveIntegerField(default=1)
    precio = models.IntegerField()
    fecha_agregado = models.DateTimeField(auto_now_add=True)

    def __str__(self):
        return f"{self.producto.nombre_producto} - {self.cantidad} unidades"
    
class Carro:
    def __init__(self, request):
        self.request = request
        self.user = request.user if request.user.is_authenticated else None
        self.session = request.session
        carrito = self.session.get("carrito")
        if not carrito:
            carrito = self.session["carrito"] = {}
        self.carrito = carrito

    def obtener_descuento(self, producto):
        lotes = Lotes.objects.filter(producto=producto, cantidad__gt=0)
        descuento_aplicado = 0

        for lote in lotes:
            if lote.fecha_vencimiento:
                ofertas = Ofertas.objects.filter(
                    lote=lote,
                    fecha_inicio__lte=timezone.now(),
                    fecha_fin__gte=timezone.now()
                )
                for oferta in ofertas:
                    politica_descuento = PoliticaDescuentos.objects.get(id_descuento=oferta.descuento_id)
                    descuento_aplicado = max(descuento_aplicado, politica_descuento.descuento)

        return round(producto.precio_unitario - (producto.precio_unitario * descuento_aplicado / 100))

    def agregar_prod(self, producto):
        if self.user:
            # Obtener el precio con descuento, si aplica
            precio_con_descuento = self.obtener_descuento(producto)

            # Guardar el producto en el carrito
            carro_producto, created = CarroProducto.objects.get_or_create(
                user=self.user,
                producto=producto,
                defaults={
                    'cantidad': 1,
                    'precio': precio_con_descuento,
                }
            )

            if not created:
                # Si el producto ya estaba en el carrito, incrementar la cantidad
                carro_producto.cantidad += 1
                carro_producto.precio = precio_con_descuento  # Actualizar el precio si es necesario
                carro_producto.save()

    def sumar(self, producto):
        if self.user:
            carro_producto = CarroProducto.objects.filter(user=self.user, producto=producto).first()
            if carro_producto:
                # Obtener la cantidad disponible en el inventario sumando todos los lotes
                inventario = Inventario.objects.filter(producto=producto).aggregate(total_stock=models.Sum('cantidad_disponible'))
                cantidad_disponible = inventario.get('total_stock', 0)
                
                # Verificar si la cantidad en el carrito mÃ¡s uno es menor o igual que la cantidad disponible en el inventario
                if carro_producto.cantidad + 1 <= cantidad_disponible:
                    carro_producto.cantidad += 1
                    carro_producto.save()

    def restar(self, producto):
        if self.user:
            carro_producto = CarroProducto.objects.filter(user=self.user, producto=producto).first()
            if carro_producto and carro_producto.cantidad > 1:
                carro_producto.cantidad -= 1
                carro_producto.save()

    def eliminar(self, producto):
        if self.user:
            CarroProducto.objects.filter(user=self.user, producto=producto).delete()

    def limpiar(self):
        if self.user:
            CarroProducto.objects.filter(user=self.user).delete()

    def contar_productos(self):
        if self.user:
            return CarroProducto.objects.filter(user=self.user).count()

    def obtener_total(self):
        if self.user:
            # Calcular el total basado en los precios y cantidades de los productos
            return sum(cp.precio * cp.cantidad for cp in CarroProducto.objects.filter(user=self.user))

class EstadoBoleta(models.Model):
    id_estado_boleta = models.AutoField(primary_key=True)
    estado_boleta = models.CharField(max_length=50)

    def __str__(self):
        return self.estado_boleta
    
    class Meta():
        db_table = 'EstadoBoleta'

class Boleta(models.Model):
    id_boleta = models.CharField(max_length=10, primary_key=True, editable=False)
    fecha = models.DateTimeField(auto_now_add=True)
    subtotal = models.IntegerField()
    iva = models.IntegerField()
    total = models.IntegerField()
    descuentos = models.IntegerField()
    estado_boleta = models.ForeignKey(EstadoBoleta, on_delete=models.CASCADE)

    def save(self, *args, **kwargs):
        if not self.id_boleta:
            self.id_boleta = self.generate_unique_id()
        super().save(*args, **kwargs)

    def generate_unique_id(self):
        length = 7  # Longitud del ID
        while True:
            unique_id = ''.join(random.choices('0123456789', k=length))
            if not Boleta.objects.filter(id_boleta=unique_id).exists():
                return unique_id

    def __str__(self):
        return str(self.id_boleta)
    
    class Meta():
        db_table = 'Boleta'

class ProductoBoleta(models.Model):
    id_producto_boleta = models.AutoField(primary_key=True)
    cantidad = models.IntegerField()
    precio_unitario = models.IntegerField()
    precio_total = models.IntegerField()
    boleta = models.ForeignKey(Boleta, on_delete=models.CASCADE)
    id_producto = models.ForeignKey(Producto, on_delete=models.CASCADE)

    def __str__(self):
        return str(self.id_producto_boleta)
    
    class Meta():
        db_table = 'ProductoBoleta'

class EstadoEntrega(models.Model):
    id_estado_entrega = models.AutoField(primary_key=True)
    estado_entrega = models.CharField(max_length=50)

    def __str__(self):
        return self.estado_entrega
    
    class Meta():
        db_table = 'EstadoEntrega'

class Orden(models.Model):
    id_orden = models.CharField(max_length=10, primary_key=True, editable=False)
    direccion = models.CharField(max_length=50)
    telefono = models.IntegerField()
    codigo_postal = models.CharField(max_length=7)
    rut = models.CharField(max_length=12)
    boleta = models.ForeignKey(Boleta, on_delete=models.CASCADE)
    estado_entrega = models.ForeignKey(EstadoEntrega, on_delete=models.CASCADE)
    metodo_pago = models.ForeignKey(MetodoPago, on_delete=models.CASCADE)
    cliente = models.ForeignKey(User, on_delete=models.CASCADE)

    def save(self, *args, **kwargs):
        if not self.id_orden:
            self.id_orden = self.generate_unique_id()
        super().save(*args, **kwargs)

    def generate_unique_id(self):
        length = 7
        characters = string.ascii_uppercase + string.digits
        while True:
            unique_id = ''.join(random.choice(characters) for _ in range(length))
            if not Orden.objects.filter(id_orden=unique_id).exists():
                return unique_id

    def __str__(self):
        return str(self.id_orden)
    
    class Meta():
        db_table = 'Orden'

class Alertas(models.Model):
    id_alerta = models.AutoField(primary_key=True)
    titulo = models.CharField(max_length=50)
    descripcion = models.CharField(max_length=150)

    def __str__(self):
        return str(self.id_alerta)
    
    class Meta():
        db_table = 'Alertas'

class HistorialAlertas(models.Model):
    id_historial_alertas = models.AutoField(primary_key=True)
    fecha = models.DateField()
    alerta = models.ForeignKey(Alertas, on_delete=models.CASCADE)
    id_producto = models.ForeignKey(Producto, on_delete=models.CASCADE)

    def __str__(self):
        return str(self.id_historial_alertas)
    
    class Meta():
        db_table = 'HistorialAlertas'
    
class Notificaciones(models.Model):
    id_notificacion = models.AutoField(primary_key=True)
    titulo = models.CharField(max_length=50)
    descripcion = models.CharField(max_length=150)

    def __str__(self):
        return str(self.id_notificacion)
    
    class Meta():
        db_table = 'Notificaciones'

class HistorialNotificaciones(models.Model):
    id_historial_notificaciones = models.AutoField(primary_key=True)
    fecha = models.DateField()
    notificacion = models.ForeignKey(Notificaciones, on_delete=models.CASCADE)
    user = models.ForeignKey(User, on_delete=models.CASCADE)

    def __str__(self):
        return str(self.id_historial_notificaciones)
    
    class Meta():
        db_table = 'HistorialNotificaciones'


# Crear los estados iniciales
def create_initial_estados(sender, **kwargs):
    EstadoBoleta.objects.get_or_create(estado_boleta='Creada')
    EstadoBoleta.objects.get_or_create(estado_boleta='Anulada')
    EstadoEntrega.objects.get_or_create(estado_entrega='Por entregar')
    EstadoEntrega.objects.get_or_create(estado_entrega='En camino')
    EstadoEntrega.objects.get_or_create(estado_entrega='Entregado')
    EstadoEntrega.objects.get_or_create(estado_entrega='Rechazado')
    TipoUsuario.objects.get_or_create(tipo_usuario='Cliente')
    TipoUsuario.objects.get_or_create(tipo_usuario='Administrador')
    MetodoPago.objects.get_or_create(nombre_metodo='Efectivo')
    MetodoPago.objects.get_or_create(nombre_metodo='Paypal')
    EstadoOferta.objects.get_or_create(estado_oferta='Activa')
    EstadoOferta.objects.get_or_create(estado_oferta='Inactiva')
    TipoMovimiento.objects.get_or_create(tipo_movimiento='Entrada')
    TipoMovimiento.objects.get_or_create(tipo_movimiento='Salida')
    TipoMovimiento.objects.get_or_create(tipo_movimiento='Ajuste')
    PerecibilidadProducto.objects.get_or_create(perecibilidad='Perecible')
    PerecibilidadProducto.objects.get_or_create(perecibilidad='No perecible')
    UnidadMedida.objects.get_or_create(unidad_medida='Kilogramo', peso_gramos=1000)
    UnidadMedida.objects.get_or_create(unidad_medida='Gramo', peso_gramos=1)
    UnidadMedida.objects.get_or_create(unidad_medida='Litro', peso_gramos=1000)
    UnidadMedida.objects.get_or_create(unidad_medida='Mililitro', peso_gramos=1)

# Crear las regiones y comunas iniciales
BASE_URL_REGIONES = "https://apis.digital.gob.cl/dpa/regiones"
BASE_URL_COMUNAS = "https://apis.digital.gob.cl/dpa/regiones/{codigo}/comunas"

# Encabezados para la solicitud
HEADERS = {
    "User-Agent": "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36"
}

# Obtener regiones
def obtener_regiones():
    try:
        response = requests.get(BASE_URL_REGIONES, headers=HEADERS, timeout=10)
        response.raise_for_status()
        return response.json()  # Lista de regiones
    except requests.exceptions.RequestException as e:
        print(f"Error al obtener regiones: {e}")
        return []

# Obtener comunas por región
def obtener_comunas_por_region(codigo_region):
    try:
        url_comunas = BASE_URL_COMUNAS.format(codigo=codigo_region)
        response = requests.get(url_comunas, headers=HEADERS, timeout=10)
        response.raise_for_status()
        return response.json()  # Lista de comunas
    except requests.exceptions.RequestException as e:
        print(f"Error al obtener comunas para la región {codigo_region}: {e}")
        return []

# Guardar las regiones y sus comunas en la base de datos
def guardar_regiones_y_comunas(sender, **kwargs):
    regiones = obtener_regiones()
    for region in regiones:
        # Guardar la región en la base de datos
        nombre_region = region["nombre"]
        region_obj, created = Region.objects.get_or_create(
            nombre_region=nombre_region
        )
        print(f"Guardada región: {region_obj.nombre_region}")

        # Obtener las comunas de la región y guardarlas
        comunas = obtener_comunas_por_region(region["codigo"])
        for comuna in comunas:
            nombre_comuna = comuna["nombre"]
            # Guardar la comuna en la base de datos asociada a la región
            Comunas.objects.get_or_create(
                nombre_comuna=nombre_comuna,
                id_region=region_obj
            )
            print(f"  - Guardada comuna: {nombre_comuna} en la región {nombre_region}")

@receiver(post_migrate)
def create_initial_data(sender, **kwargs):
    create_initial_estados(sender, **kwargs)
    guardar_regiones_y_comunas(sender, **kwargs)


def actualizar_ofertas_vencidas():
    estado_inactiva = EstadoOferta.objects.get(estado_oferta='Inactiva')
    ofertas_vencidas = Ofertas.objects.filter(fecha_fin__lt=date.today(), estado_oferta__estado_oferta='Activa')
    for oferta in ofertas_vencidas:
        oferta.estado_oferta = estado_inactiva
        oferta.save()