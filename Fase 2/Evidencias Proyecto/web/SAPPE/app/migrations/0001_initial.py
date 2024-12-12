# Generated by Django 4.2.16 on 2024-12-12 12:07

from django.conf import settings
from django.db import migrations, models
import django.db.models.deletion


class Migration(migrations.Migration):

    initial = True

    dependencies = [
        migrations.swappable_dependency(settings.AUTH_USER_MODEL),
    ]

    operations = [
        migrations.CreateModel(
            name='Alertas',
            fields=[
                ('id_alerta', models.AutoField(primary_key=True, serialize=False)),
                ('titulo', models.CharField(max_length=50)),
                ('descripcion', models.CharField(max_length=150)),
            ],
            options={
                'db_table': 'Alertas',
            },
        ),
        migrations.CreateModel(
            name='Bodega',
            fields=[
                ('id_bodega', models.AutoField(primary_key=True, serialize=False)),
                ('nombre', models.CharField(max_length=50)),
                ('capacidad_maxima', models.IntegerField()),
            ],
            options={
                'db_table': 'Bodega',
            },
        ),
        migrations.CreateModel(
            name='Boleta',
            fields=[
                ('id_boleta', models.CharField(editable=False, max_length=10, primary_key=True, serialize=False)),
                ('fecha', models.DateTimeField(auto_now_add=True)),
                ('subtotal', models.IntegerField()),
                ('iva', models.IntegerField()),
                ('total', models.IntegerField()),
                ('descuentos', models.IntegerField()),
            ],
            options={
                'db_table': 'Boleta',
            },
        ),
        migrations.CreateModel(
            name='Categoria',
            fields=[
                ('id_categoria', models.AutoField(primary_key=True, serialize=False)),
                ('nombre_categoria', models.CharField(max_length=50)),
            ],
            options={
                'db_table': 'Categoria',
            },
        ),
        migrations.CreateModel(
            name='Comunas',
            fields=[
                ('id_comuna', models.AutoField(primary_key=True, serialize=False)),
                ('nombre_comuna', models.CharField(max_length=50)),
            ],
            options={
                'db_table': 'Comunas',
            },
        ),
        migrations.CreateModel(
            name='EstadoBoleta',
            fields=[
                ('id_estado_boleta', models.AutoField(primary_key=True, serialize=False)),
                ('estado_boleta', models.CharField(max_length=50)),
            ],
            options={
                'db_table': 'EstadoBoleta',
            },
        ),
        migrations.CreateModel(
            name='EstadoEntrega',
            fields=[
                ('id_estado_entrega', models.AutoField(primary_key=True, serialize=False)),
                ('estado_entrega', models.CharField(max_length=50)),
            ],
            options={
                'db_table': 'EstadoEntrega',
            },
        ),
        migrations.CreateModel(
            name='EstadoOferta',
            fields=[
                ('id_estado_oferta', models.AutoField(primary_key=True, serialize=False)),
                ('estado_oferta', models.CharField(max_length=50)),
            ],
            options={
                'db_table': 'EstadoOferata',
            },
        ),
        migrations.CreateModel(
            name='Lotes',
            fields=[
                ('id_lote', models.CharField(editable=False, max_length=10, primary_key=True, serialize=False)),
                ('cantidad', models.IntegerField()),
                ('fecha_entrada', models.DateField(auto_now_add=True)),
                ('fecha_vencimiento', models.DateField(null=True)),
                ('bodega', models.ForeignKey(on_delete=django.db.models.deletion.CASCADE, to='app.bodega')),
            ],
            options={
                'db_table': 'Lotes',
            },
        ),
        migrations.CreateModel(
            name='Marca',
            fields=[
                ('id_marca', models.AutoField(primary_key=True, serialize=False)),
                ('nombre_marca', models.CharField(max_length=50)),
            ],
            options={
                'db_table': 'Marca',
            },
        ),
        migrations.CreateModel(
            name='MetodoPago',
            fields=[
                ('id_metodo_pago', models.AutoField(primary_key=True, serialize=False)),
                ('nombre_metodo', models.CharField(max_length=50)),
            ],
            options={
                'db_table': 'MetodoPago',
            },
        ),
        migrations.CreateModel(
            name='Notificaciones',
            fields=[
                ('id_notificacion', models.AutoField(primary_key=True, serialize=False)),
                ('titulo', models.CharField(max_length=50)),
                ('descripcion', models.CharField(max_length=150)),
            ],
            options={
                'db_table': 'Notificaciones',
            },
        ),
        migrations.CreateModel(
            name='PerecibilidadProducto',
            fields=[
                ('id_perecibilidad', models.AutoField(primary_key=True, serialize=False)),
                ('perecibilidad', models.CharField(max_length=50)),
            ],
            options={
                'db_table': 'PerecibilidadProducto',
            },
        ),
        migrations.CreateModel(
            name='PoliticaDescuentos',
            fields=[
                ('id_descuento', models.AutoField(primary_key=True, serialize=False)),
                ('dias_para_vencimiento', models.IntegerField()),
                ('descuento', models.IntegerField()),
            ],
            options={
                'db_table': 'PoliticaDescuentos',
            },
        ),
        migrations.CreateModel(
            name='Producto',
            fields=[
                ('id_producto', models.AutoField(primary_key=True, serialize=False)),
                ('nombre_producto', models.CharField(max_length=50)),
                ('precio_unitario', models.IntegerField()),
                ('descripcion', models.CharField(max_length=500)),
                ('img', models.ImageField(null=True, upload_to='productos')),
                ('volumen', models.IntegerField(null=True)),
                ('categoria', models.ForeignKey(on_delete=django.db.models.deletion.CASCADE, to='app.categoria')),
                ('marca', models.ForeignKey(on_delete=django.db.models.deletion.CASCADE, to='app.marca')),
                ('perecible', models.ForeignKey(on_delete=django.db.models.deletion.CASCADE, to='app.perecibilidadproducto')),
            ],
            options={
                'db_table': 'Producto',
            },
        ),
        migrations.CreateModel(
            name='Region',
            fields=[
                ('id_region', models.AutoField(primary_key=True, serialize=False)),
                ('nombre_region', models.CharField(max_length=50)),
            ],
            options={
                'db_table': 'Region',
            },
        ),
        migrations.CreateModel(
            name='SeccionBodega',
            fields=[
                ('id_seccion', models.AutoField(primary_key=True, serialize=False)),
                ('nombre_seccion', models.CharField(max_length=50)),
            ],
            options={
                'db_table': 'Seccion_Bodega',
            },
        ),
        migrations.CreateModel(
            name='TipoMovimiento',
            fields=[
                ('id_tipo_movimiento', models.AutoField(primary_key=True, serialize=False)),
                ('tipo_movimiento', models.CharField(max_length=50)),
            ],
            options={
                'db_table': 'Tipo_Movimiento',
            },
        ),
        migrations.CreateModel(
            name='TipoUsuario',
            fields=[
                ('id_tipo_usuario', models.AutoField(primary_key=True, serialize=False)),
                ('tipo_usuario', models.CharField(max_length=50)),
            ],
            options={
                'db_table': 'TipoUsuario',
            },
        ),
        migrations.CreateModel(
            name='UnidadMedida',
            fields=[
                ('id_unidad_medida', models.AutoField(primary_key=True, serialize=False)),
                ('unidad_medida', models.CharField(max_length=50)),
                ('peso_gramos', models.IntegerField()),
            ],
            options={
                'db_table': 'Unidad_Medida',
            },
        ),
        migrations.CreateModel(
            name='Tienda',
            fields=[
                ('id_tienda', models.AutoField(primary_key=True, serialize=False)),
                ('nombre_tienda', models.CharField(max_length=50)),
                ('frase_footer', models.CharField(max_length=150, null=True)),
                ('direccion', models.CharField(max_length=50)),
                ('logo', models.ImageField(null=True, upload_to='tienda')),
                ('color', models.CharField(max_length=50, null=True)),
                ('color_secundario', models.CharField(max_length=50, null=True)),
                ('img_login', models.ImageField(null=True, upload_to='tienda')),
                ('img_registro', models.ImageField(null=True, upload_to='tienda')),
                ('comuna', models.ForeignKey(on_delete=django.db.models.deletion.CASCADE, to='app.comunas')),
            ],
            options={
                'db_table': 'Tienda',
            },
        ),
        migrations.CreateModel(
            name='Proveedor',
            fields=[
                ('id_proveedor', models.AutoField(primary_key=True, serialize=False)),
                ('nombre_proveedor', models.CharField(max_length=50)),
                ('telefono', models.IntegerField()),
                ('correo', models.CharField(max_length=50)),
                ('direccion', models.CharField(max_length=50)),
                ('comuna', models.ForeignKey(on_delete=django.db.models.deletion.CASCADE, to='app.comunas')),
            ],
            options={
                'db_table': 'Proveedor',
            },
        ),
        migrations.CreateModel(
            name='ProductoBoleta',
            fields=[
                ('id_producto_boleta', models.AutoField(primary_key=True, serialize=False)),
                ('cantidad', models.IntegerField()),
                ('precio_unitario', models.IntegerField()),
                ('precio_total', models.IntegerField()),
                ('boleta', models.ForeignKey(on_delete=django.db.models.deletion.CASCADE, to='app.boleta')),
                ('id_producto', models.ForeignKey(on_delete=django.db.models.deletion.CASCADE, to='app.producto')),
            ],
            options={
                'db_table': 'ProductoBoleta',
            },
        ),
        migrations.AddField(
            model_name='producto',
            name='unidad_medida',
            field=models.ForeignKey(on_delete=django.db.models.deletion.CASCADE, to='app.unidadmedida'),
        ),
        migrations.CreateModel(
            name='PerfilUsuario',
            fields=[
                ('id_perfil', models.AutoField(primary_key=True, serialize=False)),
                ('tipo_usuario', models.ForeignKey(on_delete=django.db.models.deletion.CASCADE, to='app.tipousuario')),
                ('user', models.OneToOneField(on_delete=django.db.models.deletion.CASCADE, to=settings.AUTH_USER_MODEL)),
            ],
            options={
                'db_table': 'PerfilUsuario',
            },
        ),
        migrations.CreateModel(
            name='Orden',
            fields=[
                ('id_orden', models.CharField(editable=False, max_length=10, primary_key=True, serialize=False)),
                ('direccion', models.CharField(max_length=50)),
                ('telefono', models.IntegerField()),
                ('codigo_postal', models.CharField(max_length=7)),
                ('rut', models.CharField(max_length=12)),
                ('boleta', models.ForeignKey(on_delete=django.db.models.deletion.CASCADE, to='app.boleta')),
                ('cliente', models.ForeignKey(on_delete=django.db.models.deletion.CASCADE, to=settings.AUTH_USER_MODEL)),
                ('estado_entrega', models.ForeignKey(on_delete=django.db.models.deletion.CASCADE, to='app.estadoentrega')),
                ('metodo_pago', models.ForeignKey(on_delete=django.db.models.deletion.CASCADE, to='app.metodopago')),
            ],
            options={
                'db_table': 'Orden',
            },
        ),
        migrations.CreateModel(
            name='Ofertas',
            fields=[
                ('id_oferta', models.AutoField(primary_key=True, serialize=False)),
                ('fecha_inicio', models.DateField()),
                ('fecha_fin', models.DateField()),
                ('descuento', models.ForeignKey(on_delete=django.db.models.deletion.CASCADE, to='app.politicadescuentos')),
                ('estado_oferta', models.ForeignKey(on_delete=django.db.models.deletion.CASCADE, to='app.estadooferta')),
                ('lote', models.ForeignKey(on_delete=django.db.models.deletion.CASCADE, to='app.lotes')),
            ],
            options={
                'db_table': 'Ofertas',
            },
        ),
        migrations.CreateModel(
            name='Movimiento',
            fields=[
                ('id_movimiento', models.AutoField(primary_key=True, serialize=False)),
                ('cantidad', models.IntegerField()),
                ('fecha', models.DateField(auto_now_add=True)),
                ('bodega', models.ForeignKey(on_delete=django.db.models.deletion.CASCADE, to='app.bodega')),
                ('lote', models.ForeignKey(on_delete=django.db.models.deletion.CASCADE, to='app.lotes')),
                ('tipo_movimiento', models.ForeignKey(on_delete=django.db.models.deletion.CASCADE, to='app.tipomovimiento')),
            ],
            options={
                'db_table': 'Movimiento_Bodega',
            },
        ),
        migrations.AddField(
            model_name='lotes',
            name='producto',
            field=models.ForeignKey(on_delete=django.db.models.deletion.CASCADE, to='app.producto'),
        ),
        migrations.AddField(
            model_name='lotes',
            name='proveedor',
            field=models.ForeignKey(on_delete=django.db.models.deletion.CASCADE, to='app.proveedor'),
        ),
        migrations.AddField(
            model_name='lotes',
            name='seccion_bodega',
            field=models.ForeignKey(on_delete=django.db.models.deletion.CASCADE, to='app.seccionbodega'),
        ),
        migrations.CreateModel(
            name='Inventario',
            fields=[
                ('id_inventario', models.AutoField(primary_key=True, serialize=False)),
                ('cantidad_disponible', models.IntegerField()),
                ('bodega', models.ForeignKey(on_delete=django.db.models.deletion.CASCADE, to='app.bodega')),
                ('producto', models.ForeignKey(on_delete=django.db.models.deletion.CASCADE, to='app.producto')),
                ('seccion_bodega', models.ForeignKey(on_delete=django.db.models.deletion.CASCADE, to='app.seccionbodega')),
            ],
            options={
                'db_table': 'Inventario',
            },
        ),
        migrations.CreateModel(
            name='HistorialNotificaciones',
            fields=[
                ('id_historial_notificaciones', models.AutoField(primary_key=True, serialize=False)),
                ('fecha', models.DateField()),
                ('notificacion', models.ForeignKey(on_delete=django.db.models.deletion.CASCADE, to='app.notificaciones')),
                ('user', models.ForeignKey(on_delete=django.db.models.deletion.CASCADE, to=settings.AUTH_USER_MODEL)),
            ],
            options={
                'db_table': 'HistorialNotificaciones',
            },
        ),
        migrations.CreateModel(
            name='HistorialAlertas',
            fields=[
                ('id_historial_alertas', models.AutoField(primary_key=True, serialize=False)),
                ('fecha', models.DateField()),
                ('alerta', models.ForeignKey(on_delete=django.db.models.deletion.CASCADE, to='app.alertas')),
                ('id_producto', models.ForeignKey(on_delete=django.db.models.deletion.CASCADE, to='app.producto')),
            ],
            options={
                'db_table': 'HistorialAlertas',
            },
        ),
        migrations.AddField(
            model_name='comunas',
            name='id_region',
            field=models.ForeignKey(on_delete=django.db.models.deletion.CASCADE, to='app.region'),
        ),
        migrations.CreateModel(
            name='CarroProducto',
            fields=[
                ('id', models.BigAutoField(auto_created=True, primary_key=True, serialize=False, verbose_name='ID')),
                ('cantidad', models.PositiveIntegerField(default=1)),
                ('precio', models.IntegerField()),
                ('fecha_agregado', models.DateTimeField(auto_now_add=True)),
                ('producto', models.ForeignKey(on_delete=django.db.models.deletion.CASCADE, to='app.producto')),
                ('user', models.ForeignKey(on_delete=django.db.models.deletion.CASCADE, to=settings.AUTH_USER_MODEL)),
            ],
        ),
        migrations.AddField(
            model_name='boleta',
            name='estado_boleta',
            field=models.ForeignKey(on_delete=django.db.models.deletion.CASCADE, to='app.estadoboleta'),
        ),
        migrations.CreateModel(
            name='Anuncios',
            fields=[
                ('id_anuncio', models.AutoField(primary_key=True, serialize=False)),
                ('titulo', models.CharField(max_length=50)),
                ('descripcion', models.CharField(max_length=500)),
                ('fecha_inicio', models.DateField()),
                ('fecha_fin', models.DateField()),
                ('img', models.ImageField(null=True, upload_to='anuncios')),
                ('tienda', models.ForeignKey(on_delete=django.db.models.deletion.CASCADE, to='app.tienda')),
            ],
            options={
                'db_table': 'Anuncios',
            },
        ),
        migrations.CreateModel(
            name='Administrador',
            fields=[
                ('id_usuario', models.AutoField(primary_key=True, serialize=False)),
                ('rut', models.CharField(max_length=12)),
                ('perfil_usuario', models.ForeignKey(on_delete=django.db.models.deletion.CASCADE, to='app.perfilusuario')),
            ],
            options={
                'db_table': 'Administrador',
            },
        ),
    ]