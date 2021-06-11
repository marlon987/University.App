using System;
using System.IO;

namespace University.BL.Helpers
{
    //Metodos genericos que se pueden reuticilar
    public class Utils
    {
        public static void SaveFile(string path,
            string base64BinaryStr)
        {
            //Si el archivo existe se elimina para agregar uno nuevo
            if (File.Exists(path))
                File.Delete(path);

            //Se toma archivo en base64 y se convierte a byte y se guarda como archivo.
            byte[] tempBytes = Convert.FromBase64String(base64BinaryStr);
            FileStream file = File.Create(path);
            //Se escriben los bytes que se acabaron de convertir.
            file.Write(tempBytes, 0, tempBytes.Length);
            //Teniendo el archivo lo escribe y luego lo guarda.
            file.Close();
        }

        public static bool IsExistFile(string path)
        {
            //Si el archivo existe en el servidor retornar un true , de que el id de la persona si existe
            if (File.Exists(path))
                return true;

            //De lo contrario retornamos un false y no se muestra nada ya que el campo viene vacio
            return false;
        }

    }
}
