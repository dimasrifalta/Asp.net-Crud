using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DatatableCRUD.Models
{
    [MetadataType(typeof(ProdukMetadata))]
    public partial class produk
    {

    }

    public class ProdukMetadata
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please provide first name")]
        public string nama_produk { get; set; }


    }

}