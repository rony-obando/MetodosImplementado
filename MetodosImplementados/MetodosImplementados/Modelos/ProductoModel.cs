using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Enums;
using Newtonsoft.Json;

namespace MetodosImplementados.Modelos
{
    public class ProductoModel
    {
        Producto[] Productos;
        Producto product;
        public void Add(Producto P)
        {
            //La palabra ref hace que se iguale el arreglo Productos al arreglo del metodo
            //osea lo que pase con el arreglo de el metodo afecta directamente con este arreglo
            Add(P, ref Productos);
        }
        public void Add(Producto P,ref Producto[] pds)
        {
            if (P == null)
            {
                throw new ArgumentException("Error, el producto no puede ser nulo");
            }
            if (pds == null)
            {
                pds = new Producto[1];
                pds[0] = P;
                return;
            }
            //creo un arreglo mas grande(un 1 mas grande) que el arreglo original
                Producto[] tmp = new Producto[pds.Length + 1];
            //copio lo que esta en el arreglo pds al tmp(temporal) hasta el numero indicado
                Array.Copy(pds, tmp, pds.Length);
            //copio el producto P ala ultima posicion del arreglo tmp ya que este esta vacio(la posicion)
                tmp[tmp.Length - 1] = P;
            //igualo los dos arreglos(el arreglo pds obtiene el mismo tamaño que el de tmp con sus mismos productos)
                pds = tmp;
        }
        //Este metodo obtiene el indice del producto por medio del ID
        public int GetIndexProductos(Producto P)
        {
            if (P == null)
            {
                throw new ArgumentException("El producto es null");
            }
            if (Productos == null)
            {
                throw new ArgumentException("No hay Productos");
            }
            int Index = int.MinValue;
            int i = 0;
            foreach(Producto ps in Productos)
            {
                if (ps.ID == P.ID)
                {
                    Index = i;
                }
                i++;
            }
            return Index;
        }
        public void Delete(Producto P)
        {
            int i = GetIndexProductos(P);
            if (i < 0)
            {
                throw new ArgumentException($"No se ha encontrado un producto con el ID: {P.ID}");
            }
            //si i es diferente que el ultimo indice(de el ultimo producto) entonces que haga una copia del ultimo
            //producto en el producto del indice "i" para borrar el ultimo producto ya que este estaría repetido
            if(i != Productos.Length - 1)
            {
                Productos[i] = Productos[Productos.Length - 1];
            }
            //se crea un arreglo mas pequeño que el arreglo original
            Producto[] tmp = new Producto[Productos.Length-1];
            //se copia lo que esta en el arreglo original al arreglo temporal hasta el tamaño de este
            Array.Copy(Productos, tmp, tmp.Length);
            //se igualan los dos arreglos
            Productos = tmp;
        }
        //Este metodo es para actualizar el producto
        public void Update(Producto P)
        {
            int i = GetIndexProductos(P);
            if (i < 0)
            {
                throw new ArgumentException($"No se ha encontrado un producto con el ID: {P.ID}");
            }
            Productos[i] = P;
        }
        public Producto[] GetProductoByUnidadMedida(UnidadMedida um)
        {
            Producto[] Pum = null;
            if (Productos == null)
            {
                throw new ArgumentException("No hay Productos");
            }
            foreach (Producto P in Productos)
            {
                if (um == P.unidadMedida)
                {
                    Add(P, ref Pum);
                }
            }
            if (Pum == null)
            {
                throw new ArgumentException("No se ha encontrado un producto con esa unidad de medida");
            }
            return Pum;
        }
        public Producto[] GetProductoByVencimiento(DateTime dt)
        {
            Producto[] Pdt = null;
            if (Productos == null)
            {
                throw new ArgumentException("No hay Productos");
            }
            foreach (Producto P in Productos)
            {
                if (dt == P.Vencimiento)
                {
                    Add(P, ref Pdt);
                }
            }
            if (Pdt == null)
            {
                throw new ArgumentException("No se ha encontrado un producto con esa fecha de vencimiento");
            }
            return Pdt;
        }
        public Producto[] GetProductoByRango(decimal r1,decimal r2)
        {
            Producto[] Pr = null;
            if (Productos == null)
            {
                throw new ArgumentException("No hay Productos");
            }
            foreach (Producto P in Productos)
            {
                if (P.Precio>=r1||P.Precio<=r2)
                {
                    Add(P, ref Pr);
                }
            }
            if (Pr == null)
            {
                throw new ArgumentException("No se ha encontrado un producto con esa fecha de vencimiento");
            }
            return Pr;
        }
        public string ConvertJson()
        {
            if (Productos== null)
            {
                throw new ArgumentException("No hay Productos");
            }
            return JsonConvert.SerializeObject(Productos);
        }
        public Producto[] OrdenarByPrecio()
        {
            //Ordena el arreglo "Productos"
             Array.Sort(Productos, new Producto.Preciocompare());
            return Productos;
        }
        public Producto GetProductoById(int id)
        {
            //Ordena el arreglo original
            Array.Sort(Productos, new Producto.Idcompare());
            Producto P = new Producto { ID = id };
            //Busca en un arreglo ordenado de forma binaria el objeto indicado y la propiedad
            //especificada por la intefaz comparer, devolviendo el indice del producto
            int i=Array.BinarySearch(Productos,P,new Producto.Idcompare());
            if (i < 0)
            {
                throw new ArgumentException("No se ha encontrado el producto");
            }
            return Productos[i];
        }
        //***************************************************************************************
        //               Aqui estan los metodos implementados en el formulario
        //***************************************************************************************

        //pds seria el arreglo "Producto(el que tiene los datos)"
        public void Mostrar(Producto[] pds)
        {
            string mostrar = "";
            foreach (Producto p in pds)
            {
                mostrar = $@"ID: {p.ID}{Environment.NewLine} Nombre: {p.Nombre}{Environment.NewLine}
Descripción: {p.Descripcion}{Environment.NewLine} Precio: {p.Precio}{Environment.NewLine}
Fecha de vencimiento: {p.Vencimiento}{Environment.NewLine} Unidad de medida: {p.unidadMedida}{Environment.NewLine}{Environment.NewLine}"
+ mostrar;
            }
            //obviamente el nombre del richtexbox
            richtextbox.Text = "";
            richtextbox.Text = mostrar;
        }
        //Se sobrecarga ya que hay metodos que tiran un objeto solamente
        public void Mostrar(Producto p)
        {
            string mostrar = "";
            mostrar = $@"ID: {p.ID}{Environment.NewLine} Nombre: {p.Nombre}{Environment.NewLine}
Descripción: {p.Descripcion}{Environment.NewLine} Precio: {p.Precio}{Environment.NewLine}
Fecha de vencimiento: {p.Vencimiento}{Environment.NewLine} Unidad de medida: {p.unidadMedida}{Environment.NewLine}{Environment.NewLine}"
+ mostrar;
            //obviamente el nombre del richtexbox
            richtextbox.Text = "";
            richtextbox.Text = mostrar;
        }
        //Esto se hace para mostrar lo que esta en el enum al combobox
        //se realiza en el evento load del formulario
        //****************************************************************************************
        // combobox.Items.AddRange(Enum.GetValues(typeof(UnidadMedida)).Cast<object>().ToArray());
        //****************************************************************************************
        public void validateDate()
        {
            string a = "0";
            //verifica si es null o una cadena vacia (string = "")
            if (string.IsNullOrEmpty(a))//Fino
            {
                throw new ArgumentException("Error,todos los datos son requeridos");
            }
            if (a=="")//Version Rony xd
            {
                throw new ArgumentException("Error,todos los datos son requeridos");
            }
            //Por si acaso
            if (!decimal.TryParse(a, out decimal b))
            {
                throw new ArgumentException($"Error, esto: {a}, no es un decimal");
            }
        }

        //Nota: inicializar el objeto con el que se llama a las clases de infraestructura en el metodo generado automaticamente initilizeform
        //Ejemplo:
        /*private ProductoModel productoModel;
        public FrmProductos()
        {
            productoModel = new ProductoModel();
            InitializeComponent();
        }*/
        //Form2 nombre = new Form2();
        //Nota: tengo que pasar el objeto con el que he estado llamando los modelos al segundo formulario para que se realizen lo deseado
        //Ejemplo:
        //nombre= new Form2(objeto);
        //nombre.objetodeForm2 = objetodeFom1; //aqui se igualan los dos objetos antes de que se abra el Form2
        //nombre.ShowDialog; //es para abrir el formulario
        //Dispose(); //es un metodo para cerrar el formulario en el que se escribe o se utiliza
        //************************************************************
        //int Num=0;
        /*public Form2(ClaseModelo P, int num)
        {
            PModel = new ProductoModel();
            this.Num = num;
            InitializeComponent();
            this.PModel = P;
        }*/
        //este es un ejemplo en el Form2 de un metodo sobrecargado de su inicializacion y extrae datos del Form1
        //dependiendo de lo que haya sucedido en este
    }
}
