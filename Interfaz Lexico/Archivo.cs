using System;
using System.IO;
using System.Text.Json; 

public delegate void Operacion();
public delegate void NotificarCreacionEventHandler(Operacion miOperacion);

internal class Archivo<Tipo>
{
    public event NotificarCreacionEventHandler NotificarCreacion;

    private string _strNombreArchivo;
    public string NombreArchivo
    {
        get { return _strNombreArchivo; }
        set { _strNombreArchivo = value; }
    }

    private FileStream Flujo;
    private StreamWriter Escritor;
    private StreamReader Lector;

    // Actualizado para funcionar correctamente con la lectura de líneas de texto
    public bool FinArchivo
    {
        get
        {
            if (Lector != null)
                return Lector.EndOfStream;

            if (Flujo != null)
                return Flujo.Position >= Flujo.Length;

            return true;
        }
    }

    private void Crear()
    {
        this.Flujo = new FileStream(NombreArchivo, FileMode.Create, FileAccess.ReadWrite);
    }

    public Archivo(string strArchivo)
    {
        this.NombreArchivo = strArchivo;
    }

    ~Archivo()
    {
        CerrarArchivo();
    }

    public void HacerModoEscritura()
    {
        if (File.Exists(NombreArchivo))
        {
            Flujo = new FileStream(NombreArchivo, FileMode.Append, FileAccess.Write);
        }
        else
        {
            Crear();
        }
        // Inicializamos el escritor de texto
        Escritor = new StreamWriter(Flujo);
        Escritor.AutoFlush = true; // Importante para que los datos se guarden al instante
    }

    public void HacerModoLectura()
    {
        if (File.Exists(NombreArchivo))
        {
            Flujo = new FileStream(NombreArchivo, FileMode.Open, FileAccess.Read);
        }
        else
        {
            // this.NotificarCreacion(Crear); // Mantenido de tu lógica original
            Crear();
        }
        // Inicializamos el lector de texto
        Lector = new StreamReader(Flujo);
    }

    public void AgregarObjeto(Tipo Objeto)
    {
        if (Escritor == null) throw new InvalidOperationException("El archivo no está en modo escritura.");

        // Serializa el objeto a texto JSON en una sola línea
        string json = JsonSerializer.Serialize(Objeto);
        Escritor.WriteLine(json);
    }

    public Tipo LeerObjeto()
    {
        if (Lector == null) throw new InvalidOperationException("El archivo no está en modo lectura.");
        if (FinArchivo) return default(Tipo);

        // Lee la línea y la deserializa de regreso al objeto original
        string json = Lector.ReadLine();

        if (string.IsNullOrWhiteSpace(json)) return default(Tipo);

        return JsonSerializer.Deserialize<Tipo>(json);
    }

    public void CerrarArchivo()
    {
        // Es importante cerrar los Writers/Readers, ellos se encargarán de cerrar el Flujo base
        Lector?.Close();
        Escritor?.Close();
        Flujo?.Close();
    }

    public void EliminarArchivo()
    {
        CerrarArchivo(); // Siempre cierra antes de eliminar para liberar el archivo en Windows

        if (File.Exists(NombreArchivo))
        {
            File.Delete(NombreArchivo);
        }
        else
        {
            throw new Exception("No se puede eliminar porque el archivo no existe.");
        }
    }
}
