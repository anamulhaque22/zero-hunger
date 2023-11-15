using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using zero_hunger.EF;

namespace zero_hunger.DTO
{
    public class CollectRequestDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Food item is required!")]
        public string FoodItem { get; set; }

        [Required(ErrorMessage ="Restaurant ID is Required!")]
        public int Restaurant_ID { get; set; }
       
        public DateTime PreservedTime { get; set; }

    }
}