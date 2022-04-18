using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.UseCases.CreateCandlestick
{
    public interface ICreateCandleStickUseCase
    {
        public Task ExecuteAsync();
    }
}