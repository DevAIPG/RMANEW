using Amphenol.RMA.Models;
using Amphenol.RMA.Models.Models500;

using Amphenol.RMA.Models.ViewModels100;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
namespace Amphenol.RMA.Utilidades
{
    public class StoreProcedures
    {
        #region CheckLists W/O Inspection
        public static string AddCheckListBody(string question, string username, int fkheader)
        {
            string msg = string.Empty;
            using (var conn = new SqlConnection(ConnectionM10.Connection))
            {
                conn.Open();
                SqlTransaction transaction = null;
                using (var cmd = new SqlCommand("CS_AddCheckListBody", conn))
                {
                    try
                    {
                        transaction = conn.BeginTransaction("addquestion");
                        cmd.Transaction = transaction;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@question", question);
                        cmd.Parameters.AddWithValue("@fkheaderid", fkheader);
                        cmd.Parameters.AddWithValue("username", username);
                        var result = cmd.ExecuteNonQuery();
                        if (result > 0)
                        {
                            transaction.Commit();
                            msg = "ok";
                        }
                        else
                        {
                            transaction.Rollback();
                            msg = "no";
                        }
                    }
                    catch (Exception ex)
                    {
                        if (!string.IsNullOrEmpty(ex.Message))
                        {
                            transaction.Rollback();
                            msg = ex.Message;
                        }
                    }
                }
            }
            return msg;
        }

        public static int LastCheckHeaderID()
        {
            int id = 0;
            using (var conn = new SqlConnection(ConnectionM10.Connection))
            {
                conn.Open();
                using (var cmd = new SqlCommand("CS_LastCheckHeader", conn))
                {
                    try
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        var rd = cmd.ExecuteReader();
                        if (rd.Read())
                        {
                            id = (int)rd["id"];
                        }
                    }
                    catch (Exception ex)
                    {
                        if (!string.IsNullOrEmpty(ex.Message))
                        {
                            id = 0;
                        }
                    }
                }
            }
            return id;
        }


        #endregion

        #region Reportes SP



        #endregion



        public static List<ShipViewModel> Ships(string customer)
        {

            var ships = new List<ShipViewModel>();
            using (var conn = new SqlConnection(ConnectionM10.Connection100))
            {
                conn.Open();
                using (var cmd = new SqlCommand("CS_SHIPS_BY_CUSTOMER", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@cusno", customer);
                    var rd = cmd.ExecuteReader();
                    while (rd.Read())
                    {
                        ShipViewModel ship = new ShipViewModel();
                        ship.ID = (int)rd["ID"];
                        ship.cus_alt_adr_cd = rd["cus_alt_adr_cd"] as string;
                        ship.user_def_fld_1 = rd["user_def_fld_1"] as string;
                        ship.cmp_e_mail = rd["cmp_e_mail"] as string;
                        ship.email_address = rd["email_address"] as string;
                        ship.phone_ext = rd["phone_ext"] as string;
                        ship.fax_no = rd["fax_no"] as string;
                        ship.phone_no = rd["phone_no"] as string;
                        ship.contact_1 = rd["contact_1"] as string;
                        ships.Add(ship);

                    }
                }
            }
            return ships;
        }


        public static List<SquenceViewModel> GetSecuencias(string factura)
        {
            List<SquenceViewModel> secuencias = new List<SquenceViewModel>();
            using (var conn = new SqlConnection(ConnectionM10.Connection100))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    try
                    {
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@factura", factura);
                        cmd.CommandText = @"CS_Sequences";
                        var rd = cmd.ExecuteReader();
                        while (rd.Read())
                        {
                            SquenceViewModel seq = new SquenceViewModel();
                            seq.ID = (decimal)rd["ID"];
                            seq.ord_type = rd["ord_type"] as string;
                            seq.ord_no = rd["ord_no"] as string;
                            seq.inv_no = rd["inv_no"] as string;
                            seq.line_seq_no = (Int16)rd["line_seq_no"];
                            seq.item_no = rd["item_no"] as string;
                            secuencias.Add(seq);
                        }
                    }
                    catch (Exception ex)
                    {

                        throw;
                    }
                }
            }
            return secuencias;
        }
        public static List<ItemVM> GetItems()
        {
            List<ItemVM> items = new List<ItemVM>();
            using (SqlConnection con = new SqlConnection(ConnectionM10.Connection100))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    try
                    {
                        cmd.Connection = con;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = @"CS_AllItems";
                        var rd = cmd.ExecuteReader();
                        while (rd.Read())
                        {
                            var newItem = new ItemVM();
                            newItem.ID = Int32.Parse(rd["ID"].ToString());
                            newItem.item_no = rd["item_no"] as string;
                            newItem.item_desc_1 = rd["item_desc_1"] as string;
                            items.Add(newItem);
                        }
                    }
                    catch (SqlException ex)
                    {
                        return null;
                    }
                }
            }
            return items;
        }

        public static csexsw_tarea TaskById(int id)
        {
            csexsw_tarea employee = new csexsw_tarea();
            using (var conn = new SqlConnection(ConnectionM10.Connection))
            {
                conn.Open();
                var state = conn.State;
                using (var cmd = new SqlCommand("CS_TaskById", conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", id);
                    var rd = cmd.ExecuteReader();
                    if (rd.Read())
                    {
                        employee = new csexsw_tarea();
                        employee.Id = (int)rd["Id"];
                        employee.Encargado = rd["Encargado"] as string;
                        employee.Proyecto = rd["Proyecto"] as string;
                        employee.Version = rd["Version"] as string;
                        employee.Fecha = rd["Fecha"] as string;
                        employee.Status = (bool)rd["Status"];
                        employee.Statusaprobado = (bool)rd["Statusaprobado"];
                        employee.Comment = rd["Comment"] as string;
                        employee.Statusrechazado = (bool)rd["Statusrechazado"];
                        employee.Category = rd["Category"] as string;
                    }
                }
            }
            return employee;
        }
    }
}
