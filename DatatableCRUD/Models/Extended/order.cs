using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DatatableCRUD.Models
{
    [MetadataType(typeof(ordermetadata))]
    public partial class order
    {

    }

    public class ordermetadata
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please provide IdProduk")]
        public int Idproduk { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please provide EmployeeID")]
        public int EmployeeId { get; set; }


    }
}