using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Data.SqlClient;

namespace CONTPAQ_API.Services
{
    public class ProductoServices : ContpaqItem
    {
        public bool create(ProductoJSON producto)
        {
            int lastProductId;
            StringBuilder aValor = new StringBuilder();
            SDK.tProducto lProducto = returnProductoStruct(producto);
            lastProductId = 0;

            errorCode = SDK.fAltaProducto(ref lastProductId, ref lProducto);

            if (errorCode != 0)
            {
                errorMessage = errorCode + ": " + SDK.rError(errorCode);
                createErrorLog();
                return false;
            }

            // TODO: I don't know if your logic is like this 

            string connString = DatabaseServices.GetConnString();

            using (SqlConnection sqlConnection = new SqlConnection(connString))
            {
                string query = @"UPDATE [adpruebas_de_timbrado].[dbo].[admProductos] SET CCLAVESAT = '01010101' WHERE CCODIGOPRODUCTO = @codProd;";
                SqlCommand cmd = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                cmd.Parameters.Add("@codProd", SqlDbType.NVarChar);
                cmd.Parameters["@codProd"].Value = producto.cCodigoProducto;
                cmd.ExecuteNonQuery();
            }
            return true;
        }

        public List<Producto> ReturnProducts(int pageNumber, int rows)
        {
            List<Producto> lProductos = new List<Producto>();
            string query =
                "SELECT CIDPRODUCTO, CCODIGOPRODUCTO, CNOMBREPRODUCTO, CPRECIO1, CPRECIO2, CPRECIO3, CPRECIO4, " +
                "CPRECIO5, CPRECIO6,CPRECIO7, CPRECIO8, CPRECIO9, CPRECIO10, CCLAVESAT, CTIPOPRODUCTO " +
                "FROM [adpruebas_de_timbrado].[dbo].[admProductos] " +
                "ORDER BY CIDPRODUCTO DESC " +
                "OFFSET (@PageNumber-1)*@RowsOfPage ROWS " +
                "FETCH NEXT @RowsOfPage ROWS ONLY;";
            
            string connString = DatabaseServices.GetConnString();

            using (SqlConnection connection = new SqlConnection(connString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.Add("@PageNumber", SqlDbType.Int).Value = pageNumber;
                command.Parameters.Add("@RowsOfPage", SqlDbType.Int).Value = rows;
                
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        List<double> lPrecios = new List<double>();

                        for (int i = 3; i < 13; i++)
                        {
                            double precio = reader.GetDouble(i);
                            if (precio == 0)
                            {
                                break;
                            }
                            
                            lPrecios.Add(precio);
                        }

                        Producto producto;

                        string codigoProducto = reader.GetString(1);
                        string nombreProducto = reader.GetString(2);
                        string claveSAT = reader.GetString(13);
                        int tipoProducto = reader.GetInt32(14);
                        string tipoProductoString = string.Empty;

                        if (tipoProducto == 1)
                        {
                            tipoProductoString = "Productos";
                        }
                        else if (tipoProducto == 2)
                        {
                            tipoProductoString = "Paquete";
                        }
                        else if (tipoProducto == 3)
                        {
                            tipoProductoString = "Servicio";
                        }
                        

                        if (lPrecios.Count > 0)
                        {
                            producto = new Producto(codigoProducto, nombreProducto, claveSAT.ToString(), tipoProductoString, lPrecios);
                        }
                        else
                        {
                            producto = new Producto(codigoProducto, nombreProducto, claveSAT.ToString(), tipoProductoString);
                        }

                        lProductos.Add(producto);
                    }
                }

                return lProductos;
            }
        }

        private SDK.tProducto returnProductoStruct(ProductoJSON producto)
        {
            return new SDK.tProducto()
            {
                cCodigoProducto = producto.cCodigoProducto,
                cNombreProducto = producto.cNombreProducto,
                cDescripcionProducto = producto.cDescripcionProducto,
                cTipoProducto = producto.cTipoProducto,
                cFechaAltaProducto = producto.cFechaAltaProducto,
                cFechaBaja = producto.cFechaBaja,
                cStatusProducto = producto.cStatusProducto,
                cControlExistencia = producto.cControlExistencia,
                cMetodoCosteo = producto.cMetodoCosteo,
                cCodigoUnidadBase = producto.cCodigoUnidadBase,
                cCodigoUnidadNoConvertible = producto.cCodigoUnidadNoConvertible,
                cPrecio1 = producto.cPrecio1,
                cPrecio2 = producto.cPrecio2,
                cPrecio3 = producto.cPrecio3,
                cPrecio4 = producto.cPrecio4,
                cPrecio5 = producto.cPrecio5,
                cPrecio6 = producto.cPrecio6,
                cPrecio7 = producto.cPrecio7,
                cPrecio8 = producto.cPrecio8,
                cPrecio9 = producto.cPrecio9,
                cPrecio10 = producto.cPrecio10,
                cImpuesto1 = producto.cImpuesto1,
                cImpuesto2 = producto.cImpuesto2,
                cImpuesto3 = producto.cImpuesto3,
                cRetencion1 = producto.cRetencion1,
                cRetencion2 = producto.cRetencion2,
                cNombreCaracteristica1 = producto.cNombreCaracteristica1,
                cNombreCaracteristica2 = producto.cNombreCaracteristica2,
                cNombreCaracteristica3 = producto.cNombreCaracteristica3,
                cCodigoValorClasificacion1 = producto.cCodigoValorClasificacion1,
                cCodigoValorClasificacion2 = producto.cCodigoValorClasificacion2,
                cCodigoValorClasificacion3 = producto.cCodigoValorClasificacion3,
                cCodigoValorClasificacion4 = producto.cCodigoValorClasificacion4,
                cCodigoValorClasificacion5 = producto.cCodigoValorClasificacion5,
                cCodigoValorClasificacion6 = producto.cCodigoValorClasificacion6,
                cTextoExtra1 = producto.cTextoExtra1,
                cTextoExtra2 = producto.cTextoExtra2,
                cTextoExtra3 = producto.cTextoExtra3,
                cImporteExtra1 = producto.cImporteExtra1,
                cImporteExtra2 = producto.cImporteExtra2,
                cImporteExtra3 = producto.cImporteExtra3,
                cImporteExtra4 = producto.cImporteExtra4
            };
        }
    }
}