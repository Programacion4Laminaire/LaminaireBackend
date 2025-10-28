using ResultCouriers.Application.Dtos;
using ResultCouriers.Application.Interfaces;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResultCouriers.Application.UseCases
{
    public class SendResultCourierCommandHandler:ICommandHandler<SendResultCourierCommand, bool>
    {
        private readonly ResultCouriers.Application.Interfaces.IResultCourierRepository _repository;
        private readonly ResultCouriers.Application.Interfaces.IResultCourierRPA _resultCourierRPA;

        public SendResultCourierCommandHandler(IResultCourierRepository repository, IResultCourierRPA resultCourierRPA)
        {
            _repository = repository;
            _resultCourierRPA = resultCourierRPA;
        }

        public async Task<BaseResponse<bool>> Handle(SendResultCourierCommand command, CancellationToken cancellationToken)
        {
           
         var items= await _repository.GetPendingItemsAsync();
        
            var listado = items.Select(i=> i.Id).ToList();

            var listado2 =_repository.UpdateStatusToProcessingAsync(listado);

         //   Console.WriteLine(listado2);

            var tasks = items.Select(item =>
            {
                // Mapear el item de la DB a un Command/Payload para el servicio externo
                var singleCommand = new SendResultCourierRPADto(item.CourierJob.MerchandiseValueInCop, item.CourierJob.WeightInKg, item.CourierJob.Zipcode,item.Id,item.CourierJob.LengthInCm,item.CourierJob.WidthInCm,item.CourierJob.HeightInCm,item.CourierJob.destinationCountry.Name,item.CourierJob.destinationCity.Name,item.CourierJob.Address);
                return _resultCourierRPA.SendCallAsync(singleCommand);
            }).ToList();
            try
            {
             var respuesta=   await Task.WhenAll(tasks);

                var items2 = await _repository.UpdateStatusToProcessingFinalAsync(respuesta);

            }
            catch (AggregateException ex)
            {
               
                Console.WriteLine("Una o más tareas fallaron:");

                foreach (var innerEx in ex.InnerExceptions)
                {
                   
                    Console.WriteLine($"Error de Tarea: {innerEx.GetType().Name} - Mensaje: {innerEx.Message}");

                   
                }

             
                throw;
            }
            catch (Exception ex)
            {
                // Esto captura otros errores que no sean AggregateException
                Console.WriteLine($"Error inesperado: {ex.Message}");
                throw;
            }
            throw new NotImplementedException();
        }
    }
}
