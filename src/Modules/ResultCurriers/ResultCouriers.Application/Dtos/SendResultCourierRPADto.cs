using SharedKernel.Abstractions.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResultCouriers.Application.Dtos
{
    public class SendResultCourierRPADto {

        public int Id { get; set; }
        public decimal DeclaredValueCop { get; set; }

       
        public double WeightKg { get; set; }

      
        public string DestinationPostalCode { get; set; }

        public double dimLength { get; set; }

        public double dimWidth { get; set; }

        public double dimHeight { get; set; }

        public string destinationCountry { get; set; }

        public string destinationCity { get; set; }

        public string destinationAddress { get; set; }

        public SendResultCourierRPADto(
            decimal declaredValueCop,
            double weightKg,
            string destinationPostalCode,int Idresult,double dimLength, double dimWidth, double dimHeight, string destinationCountry, string destinationCity, string destinationAddress)
        {
            this.Id =Idresult ;
            this.DeclaredValueCop = declaredValueCop;
            this.WeightKg = weightKg;
            this.DestinationPostalCode = destinationPostalCode;
            this.dimLength = dimLength;
            this.dimWidth = dimWidth;
            this.dimHeight = dimHeight;
            this.destinationCountry = destinationCountry;
            this.destinationCity = destinationCity;
            this.destinationAddress = destinationAddress;
        }

    }
}
