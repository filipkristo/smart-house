using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHouse.Lib
{
    public interface ITVService
    {
        Task<Result> Power();
        Task<Result> Stop();
        Task<Result> Play();
        Task<Result> Pause();
        Task<Result> Record();
        Task<Result> Rewind();
        Task<Result> Forward();
        Task<Result> Netflix();
        Task<Result> Source();
        Task<Result> Info();
        Task<Result> Option();
        Task<Result> Back();
        Task<Result> Home();
        Task<Result> Ok();
        Task<Result> Up();
        Task<Result> Left();
        Task<Result> Right();
        Task<Result> Down();
        Task<Result> KEY_1();
        Task<Result> KEY_2();
        Task<Result> KEY_3();
        Task<Result> KEY_4();
        Task<Result> KEY_5();
        Task<Result> KEY_6();
        Task<Result> KEY_7();
        Task<Result> KEY_8();
        Task<Result> KEY_9();
        Task<Result> KEY_0();
    }
}
