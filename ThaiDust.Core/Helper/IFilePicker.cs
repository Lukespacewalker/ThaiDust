using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ThaiDust.Core.Helper
{
    public interface IFilePicker
    {
        Task<Stream> CreateFile(string filename = null);
    }
}
