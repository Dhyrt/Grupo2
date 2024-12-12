from django import forms
from django.contrib.auth.forms import UserCreationForm
from django.contrib.auth.forms import AuthenticationForm
from django.contrib.auth.models import User
from .models import *

class RegisterForm(UserCreationForm):
    email = forms.EmailField()
    class Meta:
        model = User 
        fields = ['username','email', 'password1', 'password2']
    def __init__(self, *args, **kwargs):
        super().__init__(*args, **kwargs)
        self.fields['username'].help_text= None
        self.fields['password1'].help_text=None
        self.fields['password2'].help_text=None

        self.fields['username'].widget.attrs['placeholder'] = 'Nombre de usuario'
        self.fields['email'].widget.attrs['placeholder'] = 'Correo electrónico'
        self.fields['password1'].widget.attrs['placeholder'] = 'Contraseña'
        self.fields['password2'].widget.attrs['placeholder'] = 'Confirma tu contraseña'  

        self.fields['username'].label = ""
        self.fields['email'].label = ""
        self.fields['password1'].label = ""
        self.fields['password2'].label = ""
    
class OrdenForm(forms.ModelForm):
    direccion = forms.CharField(min_length=4, max_length=50)
    telefono = forms.CharField(min_length=8, max_length=12)
    codigo_postal = forms.CharField(min_length=7, max_length=7)
    rut = forms.CharField(min_length=8, max_length=12)
        
    class Meta:
        model = Orden
        fields = 'direccion','telefono','codigo_postal','rut'