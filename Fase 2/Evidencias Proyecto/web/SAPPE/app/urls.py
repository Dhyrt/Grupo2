from django.urls import path
from .views import *

urlpatterns = [
    path('', index, name="index"),
    path('a', a, name="a"),
    path('productos', productos, name="productos"),
    path('detalle_prod/<id_producto>/', detalle_prod, name="detalle_prod"),
    path('carro', carro, name="carro"),
    path('registro/', registro, name="registro"),
    path('perfil/', perfil, name="perfil"),
    path('detalle_boleta/int:<id_boleta>/', detalle_boleta, name="detalle_boleta"),
    path('finalizar_compra/int:<id_boleta>/', finalizar_compra, name="finalizar_compra"),
    path('compra_exito/int:<id_orden>/', compra_exito, name="compra_exito"),
    path('sugerencias/', sugerencias_producto, name='sugerencias_producto'),
    path('agregar_prod/<id_producto>/', agregar_prod, name='Add'),
    path('eliminar/<id_producto>/', eliminar, name='Del'),
    path('restar/<id_producto>/', restar, name='Sub'),
    path('sumar/<id_producto>/', sumar, name='Sum'),
    # API
    path('mapa/', mapa_view, name='mapa'),
]