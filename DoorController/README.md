# Controlador de Puertas (Blazor Server .NET 8) – V2

Correcciones: sin clase extra en App.razor, sin archivos MVC (_Layout/_ViewStart), perfil de inicio correcto.

## Pasos rápidos
1. Establece **DoorController** como *Startup Project* (clic derecho sobre el proyecto → Set as Startup Project).
2. En el combo de perfiles de VS, elige **DoorController** (no "Executable").
3. Presiona **Ctrl+Shift+B** para *Build*; luego **F5** para ejecutar.

Si el build falla, abre **Error List**: cualquier error de C# impedirá que se genere la salida y VS mostrará el mensaje de "no se puede encontrar el archivo".
