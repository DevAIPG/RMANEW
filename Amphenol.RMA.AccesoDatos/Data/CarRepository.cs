using Amphenol.RMA.AccesoDatos.Data.Repository;
using Amphenol.RMA.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Globalization;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.Extensions.Options;

namespace Amphenol.RMA.AccesoDatos.Data
{
    public class CarRepository : Repository<csexsw_car>, ICarRepository
    {
        private readonly DbContextM10 _db;

        public CarRepository(DbContextM10 db) : base(db)
        {
            _db = db;
        }
        public void Delayed(int car)
        {
            var cars = _db.csexsw_car.FirstOrDefault(s => s.Id == car);


            if (cars.Status != "Closed")
            {
                cars.Status = "Delayed";




                _db.SaveChanges();
            }
        }
        public void Delayedupdate(int car, string internalduedate)
        {
            var cars = _db.csexsw_car.FirstOrDefault(s => s.Id == car);
            DateTime fecha = Convert.ToDateTime(Convert.ToDateTime(internalduedate, CultureInfo.InvariantCulture));

            if (cars.Internalduedate == fecha)
            {
                if (cars.Status != "Closed")
                {
                    cars.Status = "Delayed";




                    _db.SaveChanges();
                }
            }
        }
        public async Task<bool> crearcar(csexsw_car objDesdeDbt, int idCAR)
        {

            _db.csexsw_car.Add(objDesdeDbt);

            _db.SaveChanges();


            Create(idCAR);
            _db.SaveChanges();
            await Task.Delay(10000);

            return true;
        }
        public void Create(int idCAR)
        {
            var objDesdeDbcar = _db.csexsw_car.FirstOrDefault(s => s.Id == idCAR);

            var objDesdeDb = new csexsw_whys();
            objDesdeDb.CarId = idCAR;
            if (objDesdeDbcar.Discrepancy != null)
            {
                objDesdeDb.problem = objDesdeDbcar.Discrepancy;
            }
            _db.csexsw_whys.Add(objDesdeDb);
            _db.SaveChanges();

            var objDesdeDb8 = new  csexsw_d8encabezado();
            objDesdeDb8.CarId = idCAR;
            if (objDesdeDbcar.Discrepancy != null)
            {
                objDesdeDb8.Detallesd2 = objDesdeDbcar.Discrepancy;
            }
            objDesdeDb8.Detallesd3 = "";
            _db. csexsw_d8encabezado.Add(objDesdeDb8);

            _db.SaveChanges();

            DateTime fechaUno = Convert.ToDateTime(objDesdeDbcar.Issue_date);
            DateTime fechaDos = Convert.ToDateTime(objDesdeDbcar.Internalduedate);
            TimeSpan difFechas = fechaDos - fechaUno;
            int dias = difFechas.Days;
            BackgroundJob.Schedule(() => Delayed(idCAR), TimeSpan.FromDays(dias));





        }
        public void Update(int idcars,
          string part,
       string depa,
         string ownercar,
         string category,
        string issuedate,
        string internalduedate,
        string responsabledate,
         string answeraccepted,
        string status,
         string po,
          string clientsnombre,
           string customerpart,
          string code,
          string assigneto,
          string contact,
          string customerpn,
          string notes,
          string rma,
          string discrepancy,
           string customercar,
          string customercode,
           string sumbit)
        {

            var car = _db.csexsw_car.FirstOrDefault(s => s.Id == idcars);
            if (car.Internalduedate != Convert.ToDateTime(internalduedate, CultureInfo.InvariantCulture))
            {
                DateTime fechaUno = Convert.ToDateTime(Convert.ToDateTime(issuedate, CultureInfo.InvariantCulture));
                DateTime fechaDos = Convert.ToDateTime(Convert.ToDateTime(internalduedate, CultureInfo.InvariantCulture));
                TimeSpan difFechas = fechaDos - fechaUno;
                int dias = difFechas.Days;
                BackgroundJob.Schedule(() => Delayedupdate(idcars, internalduedate), TimeSpan.FromDays(dias));


            }
            car.Answeraccepted = answeraccepted;
            if (sumbit == "Sumbit")
            {
                car.sumbit = true;
            }
            else
            {
                car.sumbit = false;
            }
            car.Assignedto = assigneto;
            car.Category = category;
            car.Contact = contact;
            car.customername = clientsnombre;
            car.customerpart = customerpart;
            car.customerpn = customerpn;
            car.Defectcode = code;
            car.Department = depa;
            car.Discrepancy = discrepancy;

            if (car.sumbit)
            {
                car.approval_date = DateTime.Now;
            }


            car.Internalduedate = Convert.ToDateTime(internalduedate, CultureInfo.InvariantCulture);
            car.Issue_date = Convert.ToDateTime(issuedate, CultureInfo.InvariantCulture);
            car.Notes = notes;
            car.Owner = ownercar;
            if (!string.IsNullOrEmpty(car.Owner))
            {
                car.owner_name = _db.humres.Where(s => s.res_id.ToString().Trim() == car.Owner.ToString().Trim()).FirstOrDefault().fullname;
            }
           
            car.Partnumber = part;
            car.po = po;
            car.Responsabledate = Convert.ToDateTime(responsabledate, CultureInfo.InvariantCulture);
            car.Rmanumber = rma;
            car.Status = status;
            car.customercar = customercar;
            car.customercode = customercode;


            _db.SaveChanges();
        }

    }
}
